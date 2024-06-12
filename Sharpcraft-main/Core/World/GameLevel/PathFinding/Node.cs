using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.PathFinding
{
    public class Node
    {
        public readonly int xCoord;
        public readonly int yCoord;
        public readonly int zCoord;
        private readonly int hash;
        internal int index = -1;
        internal float totalPathDistance;
        internal float distanceToNext;
        internal float distanceToTarget;
        internal Node previous;
        public bool isFirst = false;

        public Node(int i1, int i2, int i3)
        {
            this.xCoord = i1;
            this.yCoord = i2;
            this.zCoord = i3;
            this.hash = CreateHash(i1, i2, i3);
        }

        public static int CreateHash(int x, int y, int z)
        {
            return y & 255 | (x & 32767) << 8 | (z & 32767) << 24 | (x < 0 ? int.MinValue : 0) | (z < 0 ? 32768 : 0);
        }

        public virtual float DistanceTo(Node pathPoint1)
        {
            float f2 = pathPoint1.xCoord - this.xCoord;
            float f3 = pathPoint1.yCoord - this.yCoord;
            float f4 = pathPoint1.zCoord - this.zCoord;
            return Mth.Sqrt(f2 * f2 + f3 * f3 + f4 * f4);
        }

        public override bool Equals(object object1)
        {
            if (!(object1 is Node))
            {
                return false;
            }
            else
            {
                Node pathPoint2 = (Node)object1;
                return this.hash == pathPoint2.hash && this.xCoord == pathPoint2.xCoord && this.yCoord == pathPoint2.yCoord && this.zCoord == pathPoint2.zCoord;
            }
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public bool InOpenSet()
        {
            return this.index >= 0;
        }

        public override string ToString()
        {
            return this.xCoord + ", " + this.yCoord + ", " + this.zCoord;
        }
    }
}