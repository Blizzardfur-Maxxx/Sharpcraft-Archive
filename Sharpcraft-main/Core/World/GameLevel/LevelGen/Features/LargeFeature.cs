using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class LargeFeature
    {
        protected int range = 8;
        protected JRandom rand = new JRandom();
        public virtual void Apply(IChunkSource cs, Level level, int x, int z, byte[] blocks)
        {
            int i6 = this.range;
            this.rand.SetSeed(level.GetRandomSeed());
            long j7 = this.rand.NextLong() / 2 * 2 + 1;
            long j9 = this.rand.NextLong() / 2 * 2 + 1;
            for (int i11 = x - i6; i11 <= x + i6; ++i11)
            {
                for (int i12 = z - i6; i12 <= z + i6; ++i12)
                {
                    this.rand.SetSeed(i11 * j7 + i12 * j9 ^ level.GetRandomSeed());
                    this.AddFeature(level, i11, i12, x, z, blocks);
                }
            }
        }

        protected virtual void AddFeature(Level world1, int i2, int i3, int i4, int i5, byte[] b6)
        {
        }
    }
}