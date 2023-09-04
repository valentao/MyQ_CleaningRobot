using System.Text.Json;
using CleaningRobotLibrary.Models;
using CleaningRobotLibrary.Utils;

namespace CleaningRobotLibrary.Logic;

/// <summary>
/// Class representing robot
/// </summary>
public class Robot
{
    #region Properties

    /// <summary>
    /// Actual battery level
    /// </summary>
    public int Battery { get; private set; }

    /// <summary>
    /// Current <see cref="Postion"/>
    /// </summary>
    private Position Position { get; set; }

    /// <summary>
    /// Array of commands (<see cref="Command"/>) to perform
    /// </summary>
    private Command[] CommandsArray { get; set; }

    /// <summary>
    /// <see cref="Map"/> 
    /// </summary>
    private Map Map { get; set; }

    /// <summary>
    /// List of visited cells (<see cref="Cell"/>)
    /// </summary>
    private List<Cell> Visited { get; set; }

    /// <summary>
    /// List of cleaned cells (<see cref="Cell"/>)
    /// </summary>
    private List<Cell> Cleaned { get; set; }

    /// <summary>
    /// Hit obstacles counter
    /// </summary>
    private int HitObstacleCount { get; set; }

    /// <summary>
    /// Auxiliary flag that the robot cannot move
    /// </summary>
    public bool IsStucked { get; private set; }

    #endregion

    /// <summary>
    /// Robot instance
    /// </summary>
    private static Robot? robot = null;

    /// <summary>
    /// Initialize new instance of <see cref="Robot"/> class.
    /// </summary>
    private Robot()
    {
        Visited = new List<Cell>();
        Cleaned = new List<Cell>();
        HitObstacleCount = 0;
        IsStucked = false;
    }
    //Lock Object
    private static object lockThis = new object();

    /// <summary>
    /// Return instance of <see cref="Robot"/> class 
    /// </summary>
    /// <returns>Instance of Robot object</returns>
    public static Robot GetRobot()
    {
        lock (lockThis)
        {
            if (Robot.robot == null)
                robot = new Robot();
        }
        return robot;
    }

    /// <summary>
    /// Load input json file into robot instance
    /// </summary>
    /// <param name="inputJson">input json string</param>
    /// <returns>no input data is missing</returns>
    public bool LoadJson(string inputJson)
    {
        bool isPrepared = true;

        Input? input = new Input();

        try
        {
            input = JsonSerializer.Deserialize<Input>(inputJson);
        }
        catch (Exception ex)
        {
            Log.Write($"Invalid input: {ex.Message}", Log.LogSeverity.Error);
            input = null;
        }

        if (input != null && robot != null)
        {
            robot.Map = Map.GetMap(input.Map);
            robot.Battery = input.Battery;
            LogBatteryLevel(robot.Battery);
            if (input.Start != null)
            {
                robot.Position = new Position(input.Start.X, input.Start.Y, input.Start.Facing);
            }
            else
            {
                isPrepared = false;
                Log.Write("Starting position is not specified in input file.", Log.LogSeverity.Error);
            }

            if (input.Commands != null)
            {
                Command[]? tmpCmd = new Command[input.Commands.Length];

                for (int i = 0; i < input.Commands.Length; i++)
                {
                    tmpCmd[i] = Command.GetCommand(input.Commands[i]);
                }

                robot.CommandsArray = tmpCmd;
            }
            else
            {
                Log.Write("No commands are specifiend in input file.", Log.LogSeverity.Warning);
            }

            bool isCellAccessible = Map.IsCellAccessible(robot.Position.X, robot.Position.Y);
            if (isCellAccessible)
            {
                robot.Visited.Add(new Cell(robot.Position.X, robot.Position.Y));
            }
            else
            {
                isPrepared = false;
                Log.Write($"Starting position X:{robot.Position.X},Y:{robot.Position.Y} is not accessible.", Log.LogSeverity.Error);
            }
        }
        else
        {
            isPrepared = false;
            Log.Write("Input file does not contain valid input data", Log.LogSeverity.Error);
        }

        return isPrepared;
    }

    /// <summary>
    /// Save result of robot's work to file
    /// </summary>
    /// <param name="file">output file</param>
    public void SaveJson(FileInfo file)
    {
        Log.Write("Output generation started.", Log.LogSeverity.Info);

        Output output = new Output();
        output.Battery = robot.Battery;
        output.Final = robot.Position;
        output.Visited = robot.Visited.Select(visited => new { visited.X, visited.Y }) // select new anonymous type
            .Distinct() // select distinct elements
            .OrderBy(x => x.X) // first order by X
            .ThenBy(y => y.Y) // second order by Y
            .Select(cell => new Cell(cell.X, cell.Y)) // cast to Cell object
            .ToArray(); // convert to array
        output.Cleaned = robot.Cleaned.Select(cleaned => new { cleaned.X, cleaned.Y })
            .Distinct()
            .OrderBy(x => x.X)
            .ThenBy(y => y.Y)
            .Select(cell => new Cell(cell.X, cell.Y))
            .ToArray();

        string outputJson = JsonSerializer.Serialize<Output>(output);

        if (file.Exists)
        {
            Log.Write($"The output file {file} already exists and will be overwritten.", Log.LogSeverity.Warning);
        }
        Log.Write($"Writting content to ouput file {file}", Log.LogSeverity.Info);
        Document.WriteAllText(file, outputJson);
    }

    /// <summary>
    /// Proccessing loaded commands
    /// </summary>
    public void Start()
    {
        Log.Write("Commands processing started.", Log.LogSeverity.Info);
        foreach (Command cmd in robot.CommandsArray)
        {
            if (IsBatteryEnough(cmd.Cost) && !robot.IsStucked)
            {
                Move(cmd);
            }
            else
            {
                if (robot.IsStucked)
                {
                    Log.Write("Robot is stucked. End of program.", Log.LogSeverity.Warning);
                }
                else
                {
                    Log.Write($"Command {cmd.Name} requires more battery ({cmd.Cost}) than is actual capacity {robot.Battery}.", Log.LogSeverity.Warning);
                }
                Log.Write("Early termination of the program", Log.LogSeverity.Warning);
                break;
            }
        }
    }

    /// <summary>
    /// Evaluation and execution of one movements
    /// </summary>
    /// <param name="command">Actual movement <see cref="Command"/></param>
    /// <returns>movement was successful</returns>
    private bool Move(Command command)
    {
        Log.Write($"Preparation to command {command.Name}", Log.LogSeverity.Info);

        bool isMove = true;

        robot.Battery -= command.Cost;
        LogBatteryLevel(robot.Battery);

        if ((command == Command.TurnLeft || command == Command.TurnRight) && command.TurnDirection != 0)
        {
            robot.Position.Facing = Command.MoveTurn(robot.Position.Facing, command.TurnDirection);
            Log.Write($"{command.Name} to {robot.Position.Facing}", Log.LogSeverity.Info);
        }

        if (command == Command.Advance)
        {
            Position position = Command.MoveAdvance(robot.Position);

            if (Map.IsCellAccessible(position.X, position.Y))
            {
                robot.Position = position;
                robot.Visited.Add(new Cell(position.X, position.Y));
                Log.Write($"Advanced to X:{position.X},Y:{position.Y}", Log.LogSeverity.Info);
            }
            else
            {
                isMove = false;
                robot.HitObstacleCount += 1;
                BackOff();
            }
        }

        if (command == Command.Back)
        {
            Position position = Command.MoveBack(robot.Position);

            if (Map.IsCellAccessible(position.X, position.Y))
            {
                robot.Position = position;
                robot.Visited.Add(new Cell(position.X, position.Y));
                Log.Write($"Returned to X:{position.X},Y:{position.Y}", Log.LogSeverity.Info);
            }
            else
            {
                isMove = false;
                robot.HitObstacleCount += 1;
                BackOff();
            }
        }

        if (command == Command.Clean)
        {
            robot.Cleaned.Add(new Cell(robot.Position.X, robot.Position.Y));
            Log.Write($"Clean X:{robot.Position.X},Y:{robot.Position.Y}", Log.LogSeverity.Info);
        }

        return isMove;
    }

    /// <summary>
    /// Loading and executing a single back off sequence
    /// </summary>
    private void BackOff()
    {
        Log.Write($"Back off sequence [{robot.HitObstacleCount}].", Log.LogSeverity.Info);

        if (robot.HitObstacleCount > 5)
        {
            robot.IsStucked = true;
        }
        else
        {
            string[] backOffCommands = Command.BackOffStrategy(robot.HitObstacleCount);

            Command[]? tmpCmd = new Command[backOffCommands.Length];

            bool isMove = true;

            for (int i = 0; i < backOffCommands.Length; i++)
            {
                tmpCmd[i] = Command.GetCommand(backOffCommands[i]);

                isMove = Move(tmpCmd[i]);

                if (tmpCmd[i] == Command.Advance || tmpCmd[i] == Command.Back)
                {
                    if (isMove)
                    {
                        robot.HitObstacleCount = 0;
                    }
                    else
                    {
                        robot.HitObstacleCount += 1;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if robot has enough battery level for next move
    /// </summary>
    /// <param name="cost">Next move battery cost</param>
    /// <returns>robot has enough battery level for next move</returns>
    public bool IsBatteryEnough(int cost)
    {
        return robot?.Battery >= cost;
    }

    /// <summary>
    /// Write down current battery level
    /// </summary>
    /// <param name="batteryLevel">robot battery level</param>
    private void LogBatteryLevel(int batteryLevel)
    {
        Log.Write($"Battery level: {batteryLevel}", Log.LogSeverity.Info);
    }
}
