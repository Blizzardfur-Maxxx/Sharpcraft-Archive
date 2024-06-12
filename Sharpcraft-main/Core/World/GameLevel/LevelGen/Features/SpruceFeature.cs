using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class SpruceFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            int i6 = random2.NextInt(4) + 6;
            int i7 = 1 + random2.NextInt(2);
            int i8 = i6 - i7;
            int i9 = 2 + random2.NextInt(2);
            bool z10 = true;
            if (i4 >= 1 && i4 + i6 + 1 <= 128)
            {
                int i11;
                int i13;
                int i15;
                int i21;
                for (i11 = i4; i11 <= i4 + 1 + i6 && z10; ++i11)
                {
                    if (i11 - i4 < i7)
                    {
                        i21 = 0;
                    }
                    else
                    {
                        i21 = i9;
                    }

                    for (i13 = i3 - i21; i13 <= i3 + i21 && z10; ++i13)
                    {
                        for (int i14 = i5 - i21; i14 <= i5 + i21 && z10; ++i14)
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
                        i21 = random2.NextInt(2);
                        i13 = 1;
                        byte b22 = 0;
                        int i16;
                        int i17;
                        for (i15 = 0; i15 <= i8; ++i15)
                        {
                            i16 = i4 + i6 - i15;
                            for (i17 = i3 - i21; i17 <= i3 + i21; ++i17)
                            {
                                int i18 = i17 - i3;
                                for (int i19 = i5 - i21; i19 <= i5 + i21; ++i19)
                                {
                                    int i20 = i19 - i5;
                                    if ((Math.Abs(i18) != i21 || Math.Abs(i20) != i21 || i21 <= 0) && !Tile.solid[world1.GetTile(i17, i16, i19)])
                                    {
                                        world1.SetTileAndDataNoUpdate(i17, i16, i19, Tile.leaves.id, 1);
                                    }
                                }
                            }

                            if (i21 >= i13)
                            {
                                i21 = b22;
                                b22 = 1;
                                ++i13;
                                if (i13 > i9)
                                {
                                    i13 = i9;
                                }
                            }
                            else
                            {
                                ++i21;
                            }
                        }

                        i15 = random2.NextInt(3);
                        for (i16 = 0; i16 < i6 - i15; ++i16)
                        {
                            i17 = world1.GetTile(i3, i4 + i16, i5);
                            if (i17 == 0 || i17 == Tile.leaves.id)
                            {
                                world1.SetTileAndDataNoUpdate(i3, i4 + i16, i5, Tile.treeTrunk.id, 1);
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