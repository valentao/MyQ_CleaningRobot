using Newtonsoft.Json;
using static cleaning_robot.Movement;

namespace cleaning_robot;
class Program
{
    public static void WriteBatteryStatus(int batteryStatus)
    {
        Console.WriteLine($"Battery: {batteryStatus}");
    }

    public static readonly string[] backOffCommands0 = new string[] { "TR", "A", "TL" };
    public static readonly string[] backOffCommands1 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands2 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands3 = new string[] { "TR", "B", "TR", "A" };
    public static readonly string[] backOffCommands4 = new string[] { "TL", "TL", "A" };


    /// <summary>
    /// Back off sequence when robot hits an obstacle
    /// </summary>
    /// <param name="hitObstacleCount">Number of attempts to back off</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static bool BackOff(int hitObstacleCount)
    {
        Console.WriteLine($"Hit obstacle count: {hitObstacleCount}");

        string[] backOffCommands;
        bool backOff = true;

        // all attempts failed
        if (hitObstacleCount == 5)
        {
            backOff = false;
            Console.WriteLine($"End of program.");
            return backOff;
        }
        else
        {   
            // back off sequence was successful
            if (hitObstacleCount == 2)
            {
                Console.WriteLine($"Back off sequence {hitObstacleCount} was successful");
                return true;
            }

            backOffCommands = hitObstacleCount switch
            {
                0 => backOffCommands0,
                1 => backOffCommands1,
                2 => backOffCommands2,
                3 => backOffCommands3,
                4 => backOffCommands4,
                _ => throw new Exception($"Unknown back off sequence")
            };

            Console.WriteLine($"Back off sequence {hitObstacleCount} commands {string.Join(", ", backOffCommands)}");

            hitObstacleCount += 1;
            return BackOff(hitObstacleCount);
        }
    }

    public static void Main(string[] args)
    {
        //bool backoff = BackOff(0);
        //Console.WriteLine($"Back off sequence result: {backoff}");

        Console.WriteLine($"Number of arguments: {args.Length}");

        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"Argument {i}: {args[i]}");
        }

        Robot robot = Robot.GetRobot();

        FileInfo jsonFile = new FileInfo("test0.json"); // args[0]

        Input? input = JsonConvert.DeserializeObject<Input>(ReadJson(jsonFile));

        robot.Visited = new List<Cell>();
        robot.Cleaned = new List<Cell>();

        if (input != null)
        {
            robot.Map = input.map;
            robot.Battery = input.battery;
            if (input.start != null)
            {
                robot.Position = new Position(input.start.X, input.start.Y, input.start.facing);
            }

            if (input.commands != null)
            {
                Command[] tmpCmd = new Command[input.commands.Length];

                for (int i = 0; i < input.commands.Length; i++)
                {
                    tmpCmd[i] = Command.GetCommand(input.commands[i]);
                }

                robot.CommandsArray = tmpCmd;
            }
        }

        // set start position out of map
        //robot.Position = new Position(3, 1, robot.Position.facing);

        Cell tmpPosition = new Cell(robot.Position.X, robot.Position.Y);
        string cellAccessibility = robot.Map[tmpPosition.X][tmpPosition.Y];

        // S - can be occupied and cleaned
        // C -  can’t be occupied or cleaned
        // null - empty cell
        if (cellAccessibility != null && cellAccessibility == "C")
        {
            robot.Visited.Add(tmpPosition);
        }
        else
        {
            Console.WriteLine($"Robot is out of map. Starting position X:{tmpPosition.X}, Y:{tmpPosition.Y} is not accessible.");
        }

        foreach (Command cmd in robot.CommandsArray)
        {
            if (robot.Battery >= cmd.Cost)
            {
                robot.Battery -= cmd.Cost;

                if ((cmd == Command.TurnLeft || cmd == Command.TurnRight) && cmd.Turn != 0)
                {
                    robot.Position.facing = Turn(robot.Position.facing, cmd.Turn);
                }

                if (cmd == Command.Advance)
                {
                    robot.Position = Advance(robot.Position);
                }

                if (cmd == Command.Back)
                {
                    robot.Position = Back(robot.Position);
                }

                if (cmd == Command.Clean)
                {
                }
            }
            else
            {
                Console.WriteLine($"Command {cmd.Name} requires more battery ({cmd.Cost}) than is actual capacity {robot.Battery}.");
                WriteOutput(robot, new FileInfo(args[1]));
                break;
            }
        }

        if (args.Length == 2)
        {
            string inputFileArg = args[0];
            string outputFileArg = args[1];

            FileInfo inputFile = new FileInfo(inputFileArg);
            FileInfo outputFile = new FileInfo(outputFileArg);
            if (inputFile.Exists && inputFile.Extension == ".json")
            {
                //ReadFile(inputFile);
                string json = ReadJson(inputFile);
                Console.WriteLine(json);
                var x = JsonConvert.DeserializeObject<Input>(json);

                ;
            }
            else
            {
                Console.WriteLine($"Argument {inputFileArg} is not valid .json file");
            }

            if (outputFile.Exists)
            {
                Console.WriteLine($"Output file {outputFileArg} already exists");
            }
            else
            {
                WriteFile(outputFile, inputFile);
            }
        }
        else
        {
            Console.WriteLine("the application requires exactly 2 arguments");
        }

        WriteOutput(robot, new FileInfo(args[1]));

        Console.ReadLine();
        Environment.Exit(0);
    }

    static string ReadJson(FileInfo file)
    {
        //StringBuilder sb = new();
        //File.ReadLines(file.FullName).ToList()
        //    .ForEach(line => sb.Append(line));

        //return sb.ToString().Trim();

        string example = File.ReadAllText(file.FullName);
        return System.String.Concat(example.Where(c => !Char.IsWhiteSpace(c)));
    }

    static void WriteOutput(Robot robot, FileInfo file)
    {
        //var start = input?.start;
        //var line = input?.map?[start.X];
        //var row = line[start.Y];
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
        FileInfo fileJsonOutput = new FileInfo("test_output.json"); // file
        File.WriteAllText(fileJsonOutput.FullName, outputJson);
    }


    static void ReadFile(FileInfo file)
    {
        File.ReadLines(file.FullName).ToList()
            .ForEach(line => Console.WriteLine(line));
    }

    static void WriteFile(FileInfo file, FileInfo inputFile)
    {
        List<string> lines = new List<string>();

        File.ReadLines(inputFile.FullName).ToList()
            .ForEach(line => lines.Add(line));

        string[] linesArray = lines.ToArray();

        File.WriteAllLines(file.FullName, linesArray);
    }
}