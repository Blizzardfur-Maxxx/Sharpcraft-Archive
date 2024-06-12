using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class OreFeature : Feature
    {
        private int minableBlockId;
        private int numberOfBlocks;
        public OreFeature(int i1, int i2)
        {
            this.minableBlockId = i1;
            this.numberOfBlocks = i2;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            float f6 = random2.NextFloat() * Mth.PI;
            double d7 = i3 + 8 + Mth.Sin(f6) * this.numberOfBlocks / 8F;
            double d9 = i3 + 8 - Mth.Sin(f6) * this.numberOfBlocks / 8F;
            double d11 = i5 + 8 + Mth.Cos(f6) * this.numberOfBlocks / 8F;
            double d13 = i5 + 8 - Mth.Cos(f6) * this.numberOfBlocks / 8F;
            double d15 = i4 + random2.NextInt(3) + 2;
            double d17 = i4 + random2.NextInt(3) + 2;
            for (int i19 = 0; i19 <= this.numberOfBlocks; ++i19)
            {
                double d20 = d7 + (d9 - d7) * i19 / this.numberOfBlocks;
                double d22 = d15 + (d17 - d15) * i19 / this.numberOfBlocks;
                double d24 = d11 + (d13 - d11) * i19 / this.numberOfBlocks;
                double d26 = random2.NextDouble() * this.numberOfBlocks / 16;
                double d28 = (Mth.Sin(i19 * Mth.PI / this.numberOfBlocks) + 1F) * d26 + 1;
                double d30 = (Mth.Sin(i19 * Mth.PI / this.numberOfBlocks) + 1F) * d26 + 1;
                int i32 = Mth.Floor(d20 - d28 / 2);
                int i33 = Mth.Floor(d22 - d30 / 2);
                int i34 = Mth.Floor(d24 - d28 / 2);
                int i35 = Mth.Floor(d20 + d28 / 2);
                int i36 = Mth.Floor(d22 + d30 / 2);
                int i37 = Mth.Floor(d24 + d28 / 2);
                for (int i38 = i32; i38 <= i35; ++i38)
                {
                    double d39 = (i38 + 0.5 - d20) / (d28 / 2);
                    if (d39 * d39 < 1)
                    {
                        for (int i41 = i33; i41 <= i36; ++i41)
                        {
                            double d42 = (i41 + 0.5 - d22) / (d30 / 2);
                            if (d39 * d39 + d42 * d42 < 1)
                            {
                                for (int i44 = i34; i44 <= i37; ++i44)
                                {
                                    double d45 = (i44 + 0.5 - d24) / (d28 / 2);
                                    if (d39 * d39 + d42 * d42 + d45 * d45 < 1 && world1.GetTile(i38, i41, i44) == Tile.rock.id)
                                    {
                                        world1.SetTileNoUpdate(i38, i41, i44, this.minableBlockId);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}