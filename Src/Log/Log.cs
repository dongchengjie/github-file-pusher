using System;

namespace gfp.Src.Log
{
    public class Log
    {
        public LogLevel LogLevel { get; set; }
        public object Message { get; set; }
        public ConsoleColor Color { get; set; }

        public Log(LogLevel logLevel, object message) : this(logLevel, message, Logger.primitiveColor) { }

        public Log(LogLevel logLevel, object message, ConsoleColor color)
        {
            LogLevel = logLevel;
            Message = string.Format("[{0}] [{1,-5}] {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logLevel, message);
            Color = color;
        }
    }
}
