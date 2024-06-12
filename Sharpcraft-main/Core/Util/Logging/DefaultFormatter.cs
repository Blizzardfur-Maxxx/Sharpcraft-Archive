using System;
using System.Text;

namespace SharpCraft.Core.Util.Logging
{
    public class DefaultFormatter : ILoggingFormatter
    {
        public string Format(LogRecord record)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(record.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));
            LogLevel level = record.Level;
            switch (level)
            {
                case LogLevel.DEBUG: sb.Append(" [DEBUG] "); break;
                case LogLevel.INFO: sb.Append(" [INFO] "); break;
                case LogLevel.WARNING: sb.Append(" [WARNING] "); break;
                case LogLevel.ERROR: sb.Append(" [ERROR] "); break;
            }
            sb.Append(record.Message);
            sb.Append('\n');

            Exception ex = record.Exception;
            if (ex != null)
            {
                sb.Append(ex.Message).Append(" :\n").Append(ex.StackTrace);
            }

            return sb.ToString();
        }
    }
}
