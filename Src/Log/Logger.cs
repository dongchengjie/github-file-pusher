using System;
using System.Text;

namespace gfp.Src.Log
{
    public class Logger
    {
        public static readonly ConsoleColor primitiveColor = Console.ForegroundColor;

        public static LogLevel logLevel = LogLevel.trace;

        static Logger()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        private static void Print(object msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = primitiveColor;
        }

        public static void Log(LogLevel level, object message, ConsoleColor color)
        {
            if (level >= logLevel)
            {
                Print(message, color);
            }
        }

        public static void Trace(object msg) => Log(LogLevel.trace, msg, ConsoleColor.Gray);
        public static void Debug(object msg) => Log(LogLevel.debug, msg, ConsoleColor.Blue);
        public static void Info(object msg) => Log(LogLevel.info, msg, ConsoleColor.Green);
        public static void Warn(object msg) => Log(LogLevel.warn, msg, ConsoleColor.DarkYellow);
        public static void Error(object msg) => Log(LogLevel.error, msg, ConsoleColor.Red);
        public static void Fatal(object msg) => Log(LogLevel.fatal, msg, ConsoleColor.DarkRed);
    }
}
