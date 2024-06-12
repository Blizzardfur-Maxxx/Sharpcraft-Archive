using System.IO;

namespace SharpCraft.Core.Util.Logging
{
    public class FileLoggingTarget : LoggingTarget
    {
        public FileLoggingTarget(string filePath, bool append) : base(new StreamWriter(filePath, append))
        {
        }
    }
}
