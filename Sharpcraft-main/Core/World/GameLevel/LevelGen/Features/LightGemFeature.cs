using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class LightGemFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            if (!world1.IsAirBlock(i3, i4, i5))
            {
                return false;
            }
            else if (world1.GetTile(i3, i4 + 1, i5) != Tile.netherrack.id)
            {
                return false;
            }
            else
            {
                world1.SetTile(i3, i4, i5, Tile.lightgem.id);
                for (int i6 = 0; i6 < 1500; ++i6)
                {
                    int i7 = i3 + random2.NextInt(8) - random2.NextInt(8);
                    int i8 = i4 - random2.NextInt(12);
                    int i9 = i5 + random2.NextInt(8) - random2.NextInt(8);
                    if (world1.GetTile(i7, i8, i9) == 0)
                    {
                        int i10 = 0;
                        for (int i11 = 0; i11 < 6; ++i11)
                        {
                            int i12 = 0;
                            if (i11 == 0)
                            {
                                i12 = world1.GetTile(i7 - 1, i8, i9);
                            }

                            if (i11 == 1)
                            {
                                i12 = world1.GetTile(i7 + 1, i8, i9);
                            }

                            if (i11 == 2)
                            {
                                i12 = world1.GetTile(i7, i8 - 1, i9);
                            }

                            if (i11 == 3)
                            {
                                i12 = world1.GetTile(i7, i8 + 1, i9);
                            }

                            if (i11 == 4)
                            {
                                i12 = world1.GetTile(i7, i8, i9 - 1);
                            }

                            if (i11 == 5)
                            {
                                i12 = world1.GetTile(i7, i8, i9 + 1);
                            }

                            if (i12 == Tile.lightgem.id)
                            {
                                ++i10;
                            }
                        }

                        if (i10 == 1)
                        {
                            world1.SetTile(i7, i8, i9, Tile.lightgem.id);
                        }
                    }
                }

                return true;
            }
        }
    }
}