using System;

namespace SharpCraft.Core.Util.Logging
{
    public class ConsoleLoggingTarget : LoggingTarget
    {
        public ConsoleLoggingTarget() : base(Console.Out)
        {
        }
    }
}
