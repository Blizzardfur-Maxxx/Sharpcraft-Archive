using System;
using System.Collections.Generic;

namespace SharpCraft.Core.Util.Logging
{
    public class Logger
    {
        private static readonly NullDictionary<string, Logger> cache = new NullDictionary<string, Logger>();

        public static Logger GetLogger(string name)
        {
            Logger logger;
            if (!cache.TryGetValue(name, out logger))
            {
                logger = new Logger();
                cache[name] = logger;
                return logger;
            }
            else
            {
                return cache[name];
            }
        }

        public static void DisposeLogging()
        {
            foreach (Logger logger in cache.Values)
            {
                foreach (LoggingTarget target in logger.logTargets)
                {
                    target.Dispose();
                }
            }
        }

        private List<LoggingTarget> logTargets = new List<LoggingTarget>();

        protected Logger()
        {
        }

        private void LogInternal(LogRecord message)
        {
            foreach (LoggingTarget target in logTargets)
            {
                target.Write(message);
            }
        }

        public void AddLoggingTarget(LoggingTarget target)
        {
            logTargets.Add(target);
        }

        public void Log(LogLevel level, string message)
        {
            LogInternal(new LogRecord(message, level));
        }

        public void Log(LogLevel level, string message, Exception ex)
        {
            LogInternal(new LogRecord(message, level, ex));
        }

        public void Debug(string msg)
        {
            Log(LogLevel.DEBUG, msg);
        }

        public void Info(string msg)
        {
            Log(LogLevel.INFO, msg);
        }

        public void Warning(string msg)
        {
            Log(LogLevel.WARNING, msg);
        }

        public void Error(string msg)
        {
            Log(LogLevel.ERROR, msg);
        }

        public void Severe(string msg)
        {
            Error(msg);
        }


    }
}
