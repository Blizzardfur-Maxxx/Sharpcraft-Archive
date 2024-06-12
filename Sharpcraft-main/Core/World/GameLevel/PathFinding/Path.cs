using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.PathFinding
{
    public class Path
    {
        private readonly Node[] nodes;
        public readonly int pathLength;
        private int pathIndex;
        public Path(Node[] pathPoint1)
        {
            this.nodes = pathPoint1;
            this.pathLength = pathPoint1.Length;
        }

        public virtual void Next()
        {
            ++this.pathIndex;
        }

        public virtual bool IsDone()
        {
            return this.pathIndex >= this.nodes.Length;
        }

        public virtual Node Last()
        {
            return this.pathLength > 0 ? this.nodes[this.pathLength - 1] : null;
        }

        public virtual Vec3 CurrentPos(Entity entity1)
        {
            double d2 = this.nodes[this.pathIndex].xCoord + ((int)(entity1.width + 1F)) * 0.5;
            double d4 = this.nodes[this.pathIndex].yCoord;
            double d6 = this.nodes[this.pathIndex].zCoord + ((int)(entity1.width + 1F)) * 0.5;
            return Vec3.Of(d2, d4, d6);
        }
    }
}