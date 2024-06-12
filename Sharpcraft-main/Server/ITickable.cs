using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server
{
    //used by server gui to update the player list ui element
    public interface ITickable
    {
        void Tick();
    }
}
