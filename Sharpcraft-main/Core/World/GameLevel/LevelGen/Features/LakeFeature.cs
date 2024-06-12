using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class LakeFeature : Feature
    {
        private int field_15235_a;
        public LakeFeature(int i1)
        {
            this.field_15235_a = i1;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            i3 -= 8;
            for (i5 -= 8; i4 > 0 && world1.IsAirBlock(i3, i4, i5); --i4)
            {
            }

            i4 -= 4;
            bool[] z6 = new bool[2048];
            int i7 = random2.NextInt(4) + 4;
            int i8;
            for (i8 = 0; i8 < i7; ++i8)
            {
                double d9 = random2.NextDouble() * 6 + 3;
                double d11 = random2.NextDouble() * 4 + 2;
                double d13 = random2.NextDouble() * 6 + 3;
                double d15 = random2.NextDouble() * (16 - d9 - 2) + 1 + d9 / 2;
                double d17 = random2.NextDouble() * (8 - d11 - 4) + 2 + d11 / 2;
                double d19 = random2.NextDouble() * (16 - d13 - 2) + 1 + d13 / 2;
                for (int i21 = 1; i21 < 15; ++i21)
                {
                    for (int i22 = 1; i22 < 15; ++i22)
                    {
                        for (int i23 = 1; i23 < 7; ++i23)
                        {
                            double d24 = (i21 - d15) / (d9 / 2);
                            double d26 = (i23 - d17) / (d11 / 2);
                            double d28 = (i22 - d19) / (d13 / 2);
                            double d30 = d24 * d24 + d26 * d26 + d28 * d28;
                            if (d30 < 1)
                            {
                                z6[(i21 * 16 + i22) * 8 + i23] = true;
                            }
                        }
                    }
                }
            }

            int i10;
            int i32;
            bool z33;
            for (i8 = 0; i8 < 16; ++i8)
            {
                for (i32 = 0; i32 < 16; ++i32)
                {
                    for (i10 = 0; i10 < 8; ++i10)
                    {
                        z33 = !z6[(i8 * 16 + i32) * 8 + i10] && (i8 < 15 && z6[((i8 + 1) * 16 + i32) * 8 + i10] || i8 > 0 && z6[((i8 - 1) * 16 + i32) * 8 + i10] || i32 < 15 && z6[(i8 * 16 + i32 + 1) * 8 + i10] || i32 > 0 && z6[(i8 * 16 + (i32 - 1)) * 8 + i10] || i10 < 7 && z6[(i8 * 16 + i32) * 8 + i10 + 1] || i10 > 0 && z6[(i8 * 16 + i32) * 8 + (i10 - 1)]);
                        if (z33)
                        {
                            Material material12 = world1.GetMaterial(i3 + i8, i4 + i10, i5 + i32);
                            if (i10 >= 4 && material12.IsLiquid())
                            {
                                return false;
                            }

                            if (i10 < 4 && !material12.IsSolid() && world1.GetTile(i3 + i8, i4 + i10, i5 + i32) != this.field_15235_a)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            for (i8 = 0; i8 < 16; ++i8)
            {
                for (i32 = 0; i32 < 16; ++i32)
                {
                    for (i10 = 0; i10 < 8; ++i10)
                    {
                        if (z6[(i8 * 16 + i32) * 8 + i10])
                        {
                            world1.SetTileNoUpdate(i3 + i8, i4 + i10, i5 + i32, i10 >= 4 ? 0 : this.field_15235_a);
                        }
                    }
                }
            }

            for (i8 = 0; i8 < 16; ++i8)
            {
                for (i32 = 0; i32 < 16; ++i32)
                {
                    for (i10 = 4; i10 < 8; ++i10)
                    {
                        if (z6[(i8 * 16 + i32) * 8 + i10] && world1.GetTile(i3 + i8, i4 + i10 - 1, i5 + i32) == Tile.dirt.id && world1.GetSavedLightValue(LightLayer.Sky, i3 + i8, i4 + i10, i5 + i32) > 0)
                        {
                            world1.SetTileNoUpdate(i3 + i8, i4 + i10 - 1, i5 + i32, Tile.grass.id);
                        }
                    }
                }
            }

            if (Tile.tiles[this.field_15235_a].material == Material.lava)
            {
                for (i8 = 0; i8 < 16; ++i8)
                {
                    for (i32 = 0; i32 < 16; ++i32)
                    {
                        for (i10 = 0; i10 < 8; ++i10)
                        {
                            z33 = !z6[(i8 * 16 + i32) * 8 + i10] && (i8 < 15 && z6[((i8 + 1) * 16 + i32) * 8 + i10] || i8 > 0 && z6[((i8 - 1) * 16 + i32) * 8 + i10] || i32 < 15 && z6[(i8 * 16 + i32 + 1) * 8 + i10] || i32 > 0 && z6[(i8 * 16 + (i32 - 1)) * 8 + i10] || i10 < 7 && z6[(i8 * 16 + i32) * 8 + i10 + 1] || i10 > 0 && z6[(i8 * 16 + i32) * 8 + (i10 - 1)]);
                            if (z33 && (i10 < 4 || random2.NextInt(2) != 0) && world1.GetMaterial(i3 + i8, i4 + i10, i5 + i32).IsSolid())
                            {
                                world1.SetTileNoUpdate(i3 + i8, i4 + i10, i5 + i32, Tile.rock.id);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}