using System;

namespace SharpCraft.Core.Util.Logging
{
    public readonly struct LogRecord
    {
        public readonly string Message;
        public readonly LogLevel Level;
        public readonly Exception Exception;
        public readonly DateTime TimeStamp;
        public LogRecord(string message, LogLevel level)
        {
            Message = message;
            Level = level;
            Exception = null;
            TimeStamp = DateTime.Now;
        }
        public LogRecord(string message, LogLevel level, Exception exception)
        {
            Message = message;
            Level = level;
            Exception = exception;
            TimeStamp = DateTime.Now;
        }
    }
}
