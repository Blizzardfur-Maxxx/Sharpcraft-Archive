using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class PineFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            int i6 = random2.NextInt(5) + 7;
            int i7 = i6 - random2.NextInt(2) - 3;
            int i8 = i6 - i7;
            int i9 = 1 + random2.NextInt(i8 + 1);
            bool z10 = true;
            if (i4 >= 1 && i4 + i6 + 1 <= 128)
            {
                int i11;
                int i13;
                int i14;
                int i15;
                int i18;
                for (i11 = i4; i11 <= i4 + 1 + i6 && z10; ++i11)
                {
                    if (i11 - i4 < i7)
                    {
                        i18 = 0;
                    }
                    else
                    {
                        i18 = i9;
                    }

                    for (i13 = i3 - i18; i13 <= i3 + i18 && z10; ++i13)
                    {
                        for (i14 = i5 - i18; i14 <= i5 + i18 && z10; ++i14)
                        {
                            if (i11 >= 0 && i11 < 128)
                            {
                                i15 = world1.GetTile(i13, i11, i14);
                                if (i15 != 0 && i15 != Tile.leaves.id)
                                {
                                    z10 = false;
                                }
                            }
                            else
                            {
                                z10 = false;
                            }
                        }
                    }
                }

                if (!z10)
                {
                    return false;
                }
                else
                {
                    i11 = world1.GetTile(i3, i4 - 1, i5);
                    if ((i11 == Tile.grass.id || i11 == Tile.dirt.id) && i4 < 128 - i6 - 1)
                    {
                        world1.SetTileNoUpdate(i3, i4 - 1, i5, Tile.dirt.id);
                        i18 = 0;
                        for (i13 = i4 + i6; i13 >= i4 + i7; --i13)
                        {
                            for (i14 = i3 - i18; i14 <= i3 + i18; ++i14)
                            {
                                i15 = i14 - i3;
                                for (int i16 = i5 - i18; i16 <= i5 + i18; ++i16)
                                {
                                    int i17 = i16 - i5;
                                    if ((Math.Abs(i15) != i18 || Math.Abs(i17) != i18 || i18 <= 0) && !Tile.solid[world1.GetTile(i14, i13, i16)])
                                    {
                                        world1.SetTileAndDataNoUpdate(i14, i13, i16, Tile.leaves.id, 1);
                                    }
                                }
                            }

                            if (i18 >= 1 && i13 == i4 + i7 + 1)
                            {
                                --i18;
                            }
                            else if (i18 < i9)
                            {
                                ++i18;
                            }
                        }

                        for (i13 = 0; i13 < i6 - 1; ++i13)
                        {
                            i14 = world1.GetTile(i3, i4 + i13, i5);
                            if (i14 == 0 || i14 == Tile.leaves.id)
                            {
                                world1.SetTileAndDataNoUpdate(i3, i4 + i13, i5, Tile.treeTrunk.id, 1);
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}