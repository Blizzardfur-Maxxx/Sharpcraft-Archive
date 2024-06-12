using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Commands
{
    public interface ICommandSource
    {
        void Log(string msg);

        string GetUsername();
    }
}
