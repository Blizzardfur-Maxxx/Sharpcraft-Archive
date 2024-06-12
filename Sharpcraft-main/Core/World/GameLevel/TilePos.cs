namespace SharpCraft.Core.World.GameLevel
{
    public struct TilePos
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;
        public TilePos(int i1, int i2, int i3)
        {
            this.x = i1;
            this.y = i2;
            this.z = i3;
        }

        public override bool Equals(object object1)
        {
            if (!(object1 is TilePos))
            {
                return false;
            }
            else
            {
                TilePos tpos = (TilePos)object1;
                return tpos == this;
            }
        }

        //notch keyboard walk
        public override int GetHashCode()
        {
            return this.x * 8976890 + this.y * 981131 + this.z;
        }

        public static bool operator ==(TilePos first, TilePos second)
        {
            return first.x == second.x &&
                first.y == second.y &&
                first.z == second.z;
        }

        public static bool operator !=(TilePos first, TilePos second)
        {
            return first.x != second.x &&
                first.y != second.y &&
                first.z != second.z;
        }
    }
}