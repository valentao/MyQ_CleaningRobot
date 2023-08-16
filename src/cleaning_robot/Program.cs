namespace cleaning_robot;
class Program
{
    public static void WriteBatteryStatus(int batteryStatus)
    {
        Console.WriteLine($"Battery: {batteryStatus}");
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

        if (args.Length == 2)
        {
            string inputFileArg = "test2.json";  // TODO: args[0];
            string outputFileArg = "test2_result.json"; // TODO: args[1];

            FileInfo inputFile = new FileInfo(inputFileArg);
            FileInfo outputFile = new FileInfo(outputFileArg);
            if (inputFile.Exists && inputFile.Extension == ".json")
            {
                //Robot robot = 
                Robot.GetRobot();
                Robot.LoadJson(inputFile);
                Robot.Start();
                //ReadFile(inputFile);
                //string json = Read(inputFile);
                //Console.WriteLine(json);
                //var x = JsonConvert.DeserializeObject<Input>(json);

                Robot.SaveJson(outputFile);
            }
            else
            {
                Console.WriteLine($"Argument {inputFileArg} is not valid .json file");
            }
        }
        else
        {
            Console.WriteLine("the application requires exactly 2 arguments");
        }
        
//        FileInfo jsonFile = new FileInfo("test0.json"); // TODO args[0]

        //Input? input = JsonConvert.DeserializeObject<Input>(Document.Read(jsonFile));

        //robot.Visited = new List<Cell>();
        //robot.Cleaned = new List<Cell>();

        //List<List<string>>? mapX = new List<List<string>>(); // TODO

        //if (input != null)
        //{
        //    mapX = input.map;
        //    robot.Battery = input.battery;
        //    if (input.start != null)
        //    {
        //        robot.Position = new Position(input.start.X, input.start.Y, input.start.facing);
        //    }

        //    if (input.commands != null)
        //    {
        //        Command[]? tmpCmd = new Command[input.commands.Length];

        //        for (int i = 0; i < input.commands.Length; i++)
        //        {
        //            tmpCmd[i] = Command.GetCommand(input.commands[i]);
        //        }

        //        robot.CommandsArray = tmpCmd;
        //    }
        //}

        //if (mapX != null)
        //{
        //    Map map = Map.GetMap(mapX);
        //}
        //bool test = Map.IsCellAccessible(robot.Position.X, robot.Position.Y);
        //if (test)
        //{
        //    robot.Visited.Add(new Cell(robot.Position.X, robot.Position.Y));
        //}      

        // set start position out of map
        //robot.Position = new Position(3, 1, robot.Position.facing);

        //Cell tmpPosition = new Cell(robot.Position.X, robot.Position.Y);
        //string? cellAccessibility = robot?.Map?[tmpPosition.X][tmpPosition.Y];

        //tmpPosition = new Cell(0, 5);
        //robot.Position = new Position(tmpPosition.X, tmpPosition.Y, robot.Position.facing);

        //int rowCount = robot.Map.Count; // X length
        //int colCount = robot.Map[0].Count(); // Y length

        //if (robot.Position.X < 0 || robot.Position.Y < 0)
        //{
            
        //}
        //else
        //{
        //    if (robot.Position.X > rowCount)
        //    {
        //        Console.WriteLine($"Position X:{robot.Position.X} is out of map. Max X value is {rowCount}");
        //    }
        //    else
        //    {
        //        if (robot.Position.Y > colCount)
        //        {
        //            Console.WriteLine($"Position Y:{robot.Position.Y} is out of map. Max Y value is {colCount}");
        //        }
        //    }
        //}
        
        //var row = robot.Map.Where(r => r[tmpPosition.X])

        //var element = (from row in robot.Map
        //               from col in row
        //               where col[tmpPosition.X] == tmpPosition.X
        //               select col).FirstOrDefault();

        //var element = (from sublist in userList
        //               from item in sublist
        //               where item.uniqueidentifier == someid
        //               select item).FirstOrDefault();

        //var t = robot.Map[0];

        //bool v = robot.Map.Exists(robot.Map[0]);

        //List<List<string>> tmpMap = new List<List<string>>();
        //tmpMap.Add();

        //robot.Map.Exists(tmpPosition.X, tmpPosition.Y);

        Console.ReadLine();
        Environment.Exit(0);
    }
}