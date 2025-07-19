using CohesiveWizardry.Common.Configuration;
using CohesiveWizardry.Common.Exceptions;

namespace CohesiveWizardry.Common.Diagnostics
{
    /// <summary>
    /// Handle all logging within the software.
    /// </summary>
    public static class LoggingManager
    {
        // ********************************************************************
        //                            Nested
        // ********************************************************************
        public enum LogVerbosity
        {
            Critical = 0,
            Error = 1,
            Warning = 2,
            Info = 3,
            Verbose = 4// aka debug
        }

        // ********************************************************************
        //                            Constants
        // ********************************************************************
        public const string DEFAULT_LOG_FILE_RELATIVE_PATH = "CohesiveWizardry-logs.json";
        public static readonly LogVerbosity LogFileVerbosity;

        static LoggingManager()
        {
            var config = CommonConfigurationManager.ReloadConfig();
            LogFileVerbosity = config.LogVerbosity;
        }

        // ********************************************************************
        //                            Public
        // ********************************************************************
        /// <summary>
        /// Format message and then log it to specified or default file
        /// </summary>
        public static void LogToFile(string logUID, string logContent, Exception exception = null, LogVerbosity logVerbosity = LogVerbosity.Critical, string logFilePath = DEFAULT_LOG_FILE_RELATIVE_PATH)
        {
            if (logVerbosity > LogFileVerbosity)
                return;

            string message = $"Message=[{logContent}]{Environment.NewLine}";

            if (exception != null)
                message += $"Exception=[{Environment.NewLine}{ExceptionUtils.BuildExceptionAndInnerExceptionsMessage(exception)}]{Environment.NewLine}";

            // Format message to add useful information
            message = $"{DateTime.UtcNow:yyyy-MM-dd HH.mm.ss.fff} - [{logUID}] {message}";

            int retryDecrementor = 100;
            while (retryDecrementor > 0)
            {
                try
                {
                    File.AppendAllText(logFilePath, message);
                    return;
                } catch (Exception)
                {
                    Thread.Sleep(50);
                }

                --retryDecrementor;
            }
        }
    }
}
