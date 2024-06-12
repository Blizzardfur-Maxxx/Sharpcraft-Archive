using System;

namespace SharpCraft.Core
{
    public struct Pos
    {
        public int x;
        public int y;
        public int z;

        public Pos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Pos(Pos orig)
        {
            x = orig.x;
            y = orig.y;
            z = orig.z;
        }

        public double GetSqDistanceTo(int x, int y, int z)
        {
            int xDiff = this.x - x;
            int yDiff = this.y - y;
            int zDiff = this.z - z;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);
        }

        public static bool operator ==(Pos first, Pos second)
        {
            return first.x == second.x &&
                first.y == second.y &&
                first.z == second.z;
        }

        public static bool operator !=(Pos first, Pos second)
        {
            return first.x != second.x &&
                first.y != second.y &&
                first.z != second.z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pos)) return false;
            Pos pos = (Pos)obj;
            return pos == this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return x + z << 8 + y << 16;
            }
        }
    }
}
