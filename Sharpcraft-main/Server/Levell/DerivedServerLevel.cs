using SharpCraft.Core.World.GameLevel.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Levell
{
    public class DerivedServerLevel : ServerLevel
    {
        public DerivedServerLevel(Server srv, ILevelStorage storage, string levelname, int dim, long seed, ServerLevel baseLevel) 
            : base(srv, storage, levelname, dim, seed)
        {
            this.mapStorage = baseLevel.mapStorage;
        }
    }
}
