using System;

namespace AnalysisProjectDepenedcies
{
    public class Logger
    {
        private static int _MaxloggingLevel = (int) LogLevel.Low;

        public static void Log(string message, LogLevel logLevel = LogLevel.Low)
        {
            if ((int) logLevel <= _MaxloggingLevel)
            {
                Console.WriteLine(message);
            }
        }

        public static void SetLogLevel(LogLevel logLevel)
        {
            _MaxloggingLevel = (int) logLevel;
        }
    }

    public enum LogLevel
    {
        Low = 1,
        High = 2,
    }
}
