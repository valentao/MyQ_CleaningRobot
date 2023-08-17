namespace cleaning_robot;
class Program
{
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
            FileInfo logFile = new FileInfo(args[1].Replace(".json", ".txt"));

            Log.GetLog(logFile);
            Log.CleanLog();

            if (inputFile.Exists && inputFile.Extension == ".json")
            {
                Robot.GetRobot();
                if (Robot.LoadJson(inputFile))
                {
                    Robot.Start();
                    Robot.SaveJson(outputFile);
                }
            }
            else
            {
                Log.Write($"Argument {inputFileArg} is not valid .json file", Log.LogSeverity.Error);
            }
        }
        else
        {
            Log.Write($"The application requires exactly 2 arguments. Number of arguments is {args.Length}", Log.LogSeverity.Error);
        }

        Console.ReadLine();
        Environment.Exit(0);
    }
}