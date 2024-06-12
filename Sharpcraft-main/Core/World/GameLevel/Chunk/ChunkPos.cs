namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public struct ChunkPos
    {
        public readonly int x;
        public readonly int z;
        public ChunkPos(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public static int Hash(int x, int z)
        {
            return (x < 0 ? int.MinValue : 0) | (x & 32767) << 16 | (z < 0 ? 32768 : 0) | z & 32767;
        }

        public static long Lhash(int x, int z)
        {
            return ((long)x) & 4294967295L | (((long)z) & 4294967295L) << 32;
        }

        public override int GetHashCode()
        {
            return Hash(this.x, this.z);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ChunkPos)) return false;
            ChunkPos pos = (ChunkPos)obj;
            return pos == this;
        }

        public override string ToString()
        {
            return $"ChunkPos({x}, {z})";
        }

        public static bool operator ==(ChunkPos first, ChunkPos second)
        {
            return first.x == second.x && first.z == second.z;
        }

        public static bool operator !=(ChunkPos first, ChunkPos second)
        {
            return first.x != second.x && first.z != second.z;
        }
    }
}