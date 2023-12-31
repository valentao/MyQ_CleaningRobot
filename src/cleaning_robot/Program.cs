﻿using CleaningRobotLibrary.Logic;
using CleaningRobotLibrary.Utils;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace cleaning_robot;
class Program
{
    public static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                .AddConsole();
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();
        //logger.LogInformation("Example log message");

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

            bool inputFileExists = Document.Exists(inputFile);
            bool inputFileIsJson = Document.IsJson(inputFile);
            bool outputFileIsJson = Document.IsJson(outputFile);

            if (inputFileExists && inputFileIsJson && outputFileIsJson)
            {
                Robot robot = Robot.GetRobot(logger);

                Log.Write(logger, $"Reading content from input file {inputFile}", Log.LogSeverity.Info);
                if (robot.LoadJson(Document.ReadAllText(inputFile)))
                {
                    robot.Start();
                    robot.SaveJson(outputFile);
                }
            }
            else
            {
                if (!inputFileExists)
                {
                    Log.Write(logger, $"Input file {inputFile} does not exist", Log.LogSeverity.Error);
                }

                if (!inputFileIsJson)
                {
                    Log.Write(logger, $"Input argument {inputFile} is not valid .json file.", Log.LogSeverity.Error);
                }

                if (!outputFileIsJson)
                {
                    Log.Write(logger, $"Output argument {outputFile} is not valid .json file.", Log.LogSeverity.Error);
                }
            }
        }
        else
        {
            Log.Write(logger, $"The application requires exactly 2 arguments. Number of arguments is {args.Length}.", Log.LogSeverity.Error);
        }

        Console.ReadLine();
        Environment.Exit(0);
    }
}