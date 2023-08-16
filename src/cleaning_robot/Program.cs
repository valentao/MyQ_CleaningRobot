namespace cleaning_robot;
class Program
{
    public static void WriteBatteryStatus(int batteryStatus)
    {
        Console.WriteLine($"Battery: {batteryStatus}");
    }

    public static void Main(string[] args)
    {
        Console.WriteLine($"Number of arguments: {args.Length}");

        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"Argument {i}: {args[i]}");
        }

        if (args.Length == 2)
        {
            string inputFileArg = args[0];
            string outputFileArg = args[1];

            FileInfo inputFile = new FileInfo(inputFileArg);
            FileInfo outputFile = new FileInfo(outputFileArg);
            if (inputFile.Exists && inputFile.Extension == ".json")
            {
                Robot.GetRobot();
                if (Robot.LoadJson(inputFile))
                {
                    Robot.Start();
                }
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

        Console.ReadLine();
        Environment.Exit(0);
    }
}