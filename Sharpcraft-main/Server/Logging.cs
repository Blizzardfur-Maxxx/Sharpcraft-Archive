using SharpCraft.Core;
using SharpCraft.Core.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server
{
    public class Logging
    {
        private static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);

        public static void Init() 
        {
            DefaultFormatter formatter = new DefaultFormatter();
            LoggingTarget target = new ConsoleLoggingTarget();
            target.SetFormatter(formatter);
            logger.AddLoggingTarget(target);
            target = new FileLoggingTarget("./server.log", true);
            target.SetFormatter(formatter);
            logger.AddLoggingTarget(target);
        }
    }
}
