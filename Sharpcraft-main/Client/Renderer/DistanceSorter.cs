using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpCraft.Client.Renderer
{
    public class DistanceSorter : IComparer<Chunk>
    {
        private Mob baseEntity;

        public DistanceSorter(Mob mob)
        {
            this.baseEntity = mob;
        }

        public int Compare(Chunk c0, Chunk c1)
        {
            bool z3 = c0.isInFrustum;
            bool z4 = c1.isInFrustum;
            if (z3 && !z4)
            {
                return 1;
            }
            else if (z4 && !z3)
            {
                return -1;
            }
            else
            {
                double d5 = c0.DistanceToEntitySquared(this.baseEntity);
                double d7 = c1.DistanceToEntitySquared(this.baseEntity);
                return d5 < d7 ? 1 : (d5 > d7 ? -1 : (c0.chunkIndex < c1.chunkIndex ? 1 : -1));
            }
        }
    }
}
