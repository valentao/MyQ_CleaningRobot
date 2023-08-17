namespace cleaning_robot;
class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            FileInfo inputFile = new FileInfo(args[0]);
            FileInfo outputFile = new FileInfo(args[1]);
            FileInfo logFile = new FileInfo(args[1].Replace(".json", ".txt"));

            // if output arg is not json file and replace above to create a txt log file does not work
            if (string.IsNullOrEmpty(logFile.Extension))
            {
                logFile = new FileInfo(args[1] + ".txt"); 
            }

            Log.GetLog(logFile);
            Log.CleanLog(); // clear content from log file

            if (inputFile.Exists && inputFile.Extension == ".json" && outputFile.Extension == ".json")
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
                if (inputFile.Extension != ".json")
                {
                    Log.Write($"Input argument {inputFile} is not valid .json file.", Log.LogSeverity.Error);
                }

                if (outputFile.Extension != ".json")
                {
                    Log.Write($"Output argument {outputFile} is not valid .json file.", Log.LogSeverity.Error);
                }
            }
        }
        else
        {
            Log.Write($"The application requires exactly 2 arguments. Number of arguments is {args.Length}.", Log.LogSeverity.Error);
        }

        Console.ReadLine();
        Environment.Exit(0);
    }
}