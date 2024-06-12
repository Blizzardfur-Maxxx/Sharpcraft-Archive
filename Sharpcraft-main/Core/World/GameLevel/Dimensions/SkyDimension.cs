using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Dimensions
{
    public class SkyDimension : Dimension
    {
        protected override void RegisterWorldChunkManager()
        {
            this.biomeSource = new SingleBiomeSource(Biome.sky, 0.5, 0);
            this.dimension = 1;
        }

        public override IChunkSource CreateRandomLevelSource()
        {
            return new SkyRandomLevelSource(this.level, this.level.GetRandomSeed());
        }

        public override float GetSunAngle(long j1, float f3)
        {
            return 0F;
        }

        public override float[] CalcSunriseSunsetColors(float f1, float f2)
        {
            return null;
        }

        public override Vec3 GetFogColor(float f1, float f2)
        {
            int i3 = 8421536;
            float f4 = Mth.Cos(f1 * Mth.PI * 2F) * 2F + 0.5F;
            if (f4 < 0F)
            {
                f4 = 0F;
            }

            if (f4 > 1F)
            {
                f4 = 1F;
            }

            float f5 = (i3 >> 16 & 255) / 255F;
            float f6 = (i3 >> 8 & 255) / 255F;
            float f7 = (i3 & 255) / 255F;
            f5 *= f4 * 0.94F + 0.06F;
            f6 *= f4 * 0.94F + 0.06F;
            f7 *= f4 * 0.91F + 0.09F;
            return Vec3.Of(f5, f6, f7);
        }

        public override bool Func_28112_c()
        {
            return false;
        }

        public override float GetCloudHeight()
        {
            return 8F;
        }

        public override bool CanCoordinateBeSpawn(int i1, int i2)
        {
            int i3 = this.level.GetFirstUncoveredBlock(i1, i2);
            return i3 == 0 ? false : Tile.tiles[i3].material.BlocksMotion();
        }
    }
}