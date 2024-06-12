using System;
using System.IO;

namespace SharpCraft.Core.Util.Logging
{
    public abstract class LoggingTarget
    {
        private ILoggingFormatter formatter = new NoneFormatter();
        private IErrorHandler errorHandler = null;
        private TextWriter writer;

        protected LoggingTarget(TextWriter writer)
        {
            this.writer = writer;
        }

        public void SetErrorHandler(IErrorHandler handler)
        {
            this.errorHandler = handler;
        }

        public void SetFormatter(ILoggingFormatter formatter)
        {
            this.formatter = formatter;
        }

        public virtual void Write(LogRecord record)
        {
            string formatted;
            try
            {
                formatted = formatter.Format(record);
            }
            catch (Exception ex)
            {
                if (errorHandler != null)
                {
                    errorHandler.OnError(this, ex);
                }
                return;
            }
            try
            {
                writer.Write(formatted);
                writer.Flush();
            }
            catch (Exception ex)
            {
                if (errorHandler != null)
                {
                    errorHandler.OnError(this, ex);
                }
            }

        }

        public virtual void Flush()
        {
            writer.Flush();
        }

        public virtual void Dispose()
        {
            writer.Dispose();
        }

    }
}
