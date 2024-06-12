namespace SharpCraft.Core.Util.Logging
{
    public class NoneFormatter : ILoggingFormatter
    {
        public virtual string Format(LogRecord record)
        {
            return record.Message;
        }
    }
}
