using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Dimensions
{
    public class HellDimension : Dimension
    {
        protected override void RegisterWorldChunkManager()
        {
            this.biomeSource = new SingleBiomeSource(Biome.hell, 1, 0);
            this.isNether = true;
            this.isHellWorld = true;
            this.hasNoSky = true;
            this.dimension = -1;
        }

        public override Vec3 GetFogColor(float f1, float f2)
        {
            return Vec3.Of(0.2F, 0.03F, 0.03F);
        }

        protected override void GenerateLightBrightnessTable()
        {
            float f1 = 0.1F;
            for (int i2 = 0; i2 <= 15; ++i2)
            {
                float f3 = 1F - i2 / 15F;
                this.lightBrightnessTable[i2] = (1F - f3) / (f3 * 3F + 1F) * (1F - f1) + f1;
            }
        }

        public override IChunkSource CreateRandomLevelSource()
        {
            return new HellRandomLevelSource(this.level, this.level.GetRandomSeed());
        }

        public override bool CanCoordinateBeSpawn(int i1, int i2)
        {
            int i3 = this.level.GetFirstUncoveredBlock(i1, i2);
            return i3 == Tile.unbreakable.id ? false : (i3 == 0 ? false : Tile.solid[i3]);
        }

        public override float GetSunAngle(long j1, float f3)
        {
            return 0.5F;
        }

        public override bool CanRespawnHere()
        {
            return false;
        }
    }
}