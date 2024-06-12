using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Commands
{
    public class Command
    {
        public readonly string command;
        public readonly ICommandSource source;

        public Command(string command, ICommandSource source)
        {
            this.command = command;
            this.source = source;
        }
    }
}
