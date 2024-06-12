using SharpCraft.Core.World.Entities;
using System.Collections.Generic;

namespace SharpCraft.Client.Renderer
{
    public class EntitySorter : IComparer<Chunk>
    {
        private double x;
        private double y;
        private double z;

        public EntitySorter(Entity entity1)
        {
            this.x = -entity1.x;
            this.y = -entity1.y;
            this.z = -entity1.z;
        }

        public int Compare(Chunk c0, Chunk c1)
        {
            double d3 = c0.posXPlus + this.x;
            double d5 = c0.posYPlus + this.y;
            double d7 = c0.posZPlus + this.z;
            double d9 = c1.posXPlus + this.x;
            double d11 = c1.posYPlus + this.y;
            double d13 = c1.posZPlus + this.z;
            return (int)((d3 * d3 + d5 * d5 + d7 * d7 - (d9 * d9 + d11 * d11 + d13 * d13)) * 1024.0D);
        }
    }
}