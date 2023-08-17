using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleaning_robot
{
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

        FileInfo LogFile { get; set; }

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
        /// Clean content of existing file
        /// </summary>
        public static void CleanLog()
        {
            if (log.LogFile.Exists)
            {
                Document.Write(log.LogFile, String.Empty);
            }
        }

        /// <summary>
        /// Write message to log file and console
        /// </summary>
        /// <param name="message">message to log</param>
        public static void Write (string message)
        {
            DateTime now = DateTime.Now;

            message = $"{now.ToString()}: {message}";

            WriteToConsole(message);
            WriteToLog(message);
        }

        /// <summary>
        /// Write message to log file and console with severity identificator
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="severity">log severity identificator</param>
        public static void Write(string message, LogSeverity severity)
        {
            DateTime now = DateTime.Now;

            message = $"{now.ToString()} ({severity}): {message}";

            WriteToConsole(message);
            WriteToLog(message);
        }

        /// <summary>
        /// Write message to log file
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToLog(string message)
        {
            if(log != null)
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
}
