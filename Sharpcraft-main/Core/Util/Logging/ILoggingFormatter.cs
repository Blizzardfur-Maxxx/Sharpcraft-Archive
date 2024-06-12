namespace SharpCraft.Core.Util.Logging
{
    public interface ILoggingFormatter
    {
        string Format(LogRecord record);
    }
}
