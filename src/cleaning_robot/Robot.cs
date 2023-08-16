using Newtonsoft.Json;
using static cleaning_robot.Movement;

namespace cleaning_robot;

public class Robot
{
    public enum Facing
    {
        N,
        E,
        S,
        W
    }

   public static readonly Facing[] facingArray = new[] {
    Facing.N,
    Facing.E,
    Facing.S,
    Facing.W
    };

    private static Robot? robot = null;

    private Robot()
    {
        Visited = new List<Cell>();
        Cleaned = new List<Cell>();
        HitObstacleCount = 0;
        IsStucked = false;
    }

    public int Battery { get; set; }

    public Position Position { get; set; }

    public Command[] CommandsArray { get; set; }

    public Map Map { get; set; }

    public List<Cell> Visited { get; set; }

    public List<Cell> Cleaned { get; set; }

    public int HitObstacleCount { get; set; }

    public bool IsStucked { get; set; }

    //Lock Object
    private static object lockThis = new object();
    public static Robot GetRobot()
    {
        lock (lockThis)
        {
            if (Robot.robot == null)
                robot = new Robot();
        }
        return robot;
    }

    public static void LoadJson(FileInfo file)
    {
        Input? input =  JsonConvert.DeserializeObject<Input>(Document.Read(file));

        //List<List<string>>? mapX = new List<List<string>>(); // TODO

        if (input != null && robot != null)
        {
            robot.Map = Map.GetMap(input.map);
            robot.Battery = input.battery;
            if (input.start != null)
            {
                robot.Position = new Position(input.start.X, input.start.Y, input.start.facing);
            }

            if (input.commands != null)
            {
                Command[]? tmpCmd = new Command[input.commands.Length];

                for (int i = 0; i < input.commands.Length; i++)
                {
                    tmpCmd[i] = Command.GetCommand(input.commands[i]);
                }

                robot.CommandsArray = tmpCmd;
            }
        }

        bool isCellAccessible = Map.IsCellAccessible(robot.Position.X, robot.Position.Y);
        if (isCellAccessible)
        {
            robot.Visited.Add(new Cell(robot.Position.X, robot.Position.Y));
        }
    }

    public static void SaveJson(FileInfo file)
    {
        Output output = new Output();
        output.battery = robot.Battery;
        output.final = robot.Position;

        // TODO - select distinct from 
        //Cell[] d = robot.Visited.ToArray();
        //Cell[] distinct = d.SelectMany(a => a).Distinct().ToArray();
        //var distinct = array.SelectMany(a => a).Distinct().ToArray();

        // TODO - sort Array by X
        //Cell[] x = robot.Visited.ToArray();
        //var sorted = x.OrderBy(y => y[0]).ThenBy(y => y[1]).ThenBy(y => y[2]);

        output.visited = robot.Visited.ToArray();
        output.cleaned = robot.Cleaned.ToArray();

        //Cell c1 = new Cell(1,0);
        //Cell c2 = new Cell(2, 0);
        //Cell c3 = new Cell(3, 0);
        //output.visited = new Cell[] { c1, c2, c3 };
        //output.cleaned = new Cell[] { c1, c2 };

        string outputJson = JsonConvert.SerializeObject(output);
        FileInfo fileJsonOutput = new FileInfo("test_output.json"); // TODO new FileInfo(args[1])
        Document.Write(fileJsonOutput, outputJson);
    }

    public static void Start()
    {
        foreach (Command cmd in robot.CommandsArray)
        {
            //if (robot.Battery >= cmd.Cost)
            if (IsBatteryEnough(cmd.Cost) && !robot.IsStucked)
            {
                Console.WriteLine($"Prepared move {cmd.Name}");

                Move(cmd);      
                //if (robot.HitObstacleCount >= 5)
                //{
                //    robot.IsStucked = 
                //}
            }
            else
            {
                if (robot.IsStucked)
                {
                    Console.WriteLine("Robot is stucked. End of program.");
                }
                else
                {
                    Console.WriteLine($"Command {cmd.Name} requires more battery ({cmd.Cost}) than is actual capacity {robot.Battery}.");
                }
                    //Document.WriteToJson(robot, new FileInfo(args[1]));
                break;
            }
        }
    }

    private static bool Move(Command command)
    {
        bool isMove = true;

        robot.Battery -= command.Cost;

        if ((command == Command.TurnLeft || command == Command.TurnRight) && command.Turn != 0)
        {
            robot.Position.facing = Turn(robot.Position.facing, command.Turn);
            Console.WriteLine($"{command.Name}");
        }

        if (command == Command.Advance)
        {
            Position position = Advance(robot.Position);

            if (Map.IsCellAccessible(position.X, position.Y))
            {
                robot.Position = position;
                robot.Visited.Add(new Cell(position.X, position.Y));
                Console.WriteLine($"Advanced to X:{position.X},Y:{position.Y}");
            }
            else
            {
                isMove = false;
                BackOff();
            }
        }

        if (command == Command.Back)
        {
            Position position = Back(robot.Position);

            if (Map.IsCellAccessible(position.X, position.Y))
            {
                robot.Position = position;
                robot.Visited.Add(new Cell(position.X, position.Y));
                Console.WriteLine($"Returned to X:{position.X},Y:{position.Y}");
            }
            else
            {
                isMove = false;
                BackOff();
            }
        }

        if (command == Command.Clean)
        {
            robot.Cleaned.Add(new Cell(robot.Position.X, robot.Position.Y));
            Console.WriteLine($"Clean X:{robot.Position.X},Y:{robot.Position.Y}");
        }

        return isMove;
    }

    private static void BackOff()
    {
        Console.WriteLine($"Back off sequence. Hit obstacle count: {robot.HitObstacleCount}.");

        if (robot.HitObstacleCount >= 5)
        {
            robot.IsStucked = true;
        }
        else
        {
            string[] backOffCommands = BackOffStrategy(robot.HitObstacleCount);

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

            if (!isMove)
            {
                BackOff();
            }
        }
    }

    public static bool IsBatteryEnough(int cost)
    {
        return robot?.Battery >= cost;
    }
}
