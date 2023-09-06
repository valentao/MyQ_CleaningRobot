using Microsoft.Extensions.Logging;

namespace CleaningRobotLibrary.Utils;

/// <summary>
/// Class representing log
/// </summary>
public class Log
{
    #region Enums

    public enum LogSeverity
    {
        Info,
        Warning,
        Error
    }

    #endregion


    #region Properties

    /// <summary>
    /// <see cref="Log"/> file
    /// </summary>
    FileInfo LogFile { get; }

    #endregion


    /// <summary>
    /// Log instance
    /// </summary>
    private static Log? log = null;

    /// <summary>
    /// Initialize new instance of <see cref="Log"/> class.
    /// </summary>
    private Log(FileInfo logFile)
    {
        LogFile = logFile;
    }

    //Lock Object
    private static object lockThis = new object();

    /// <summary>
    /// Return instance of <see cref="Log"/> class 
    /// </summary>
    /// <returns>Instance of Log object</returns>
    public static Log GetLog(FileInfo file)
    {
        lock (lockThis)
        {
            if (Log.log == null)
            {
                log = new Log(file);
            }

            return log;
        }
    }

    /// <summary>
    /// Clean content of existing log file
    /// </summary>
    public static void CleanLog()
    {
        if (log != null)
        {
            Document.WriteAllText(log.LogFile, String.Empty);
        }
    }

    /// <summary>
    /// Write message to log file and console
    /// </summary>
    /// <param name="message">message to log</param>
    public static void Write(ILogger logger, string message)
    {
        DateTime now = DateTime.Now;

        message = $"{now.ToString()}: {message}";

        logger.LogDebug(message);
        //WriteToConsole(message);
        WriteToLogFile(message);
    }

    /// <summary>
    /// Write message to log file and console with severity identificator
    /// </summary>
    /// <param name="message">message to log</param>
    /// <param name="severity">log severity identificator</param>
    public static void Write(ILogger logger, string message, LogSeverity severity)
    {
        DateTime now = DateTime.Now;

        message = $"{now.ToString()} ({severity}): {message}";

        //WriteToConsole(message);

        switch (severity)
        {
            case LogSeverity.Info:
                logger.LogInformation(message);
                break;
            case LogSeverity.Warning:
                logger.LogWarning(message);
                break;
            case LogSeverity.Error:
                logger.LogError(message);
                break;
            default:
                logger.LogDebug(message);
                break;
        }
        WriteToLogFile(message);
    }

    /// <summary>
    /// Write message to log file
    /// </summary>
    /// <param name="message"></param>
    public static void WriteToLogFile(string message)
    {
        if (log != null)
        {
            Document.AppendText(log.LogFile, message);
        }
    }

    /// <summary>
    /// Write message to console
    /// </summary>
    /// <param name="message"></param>
    public static void WriteToConsole(string message)
    {
        Console.WriteLine(message);
    }
}
