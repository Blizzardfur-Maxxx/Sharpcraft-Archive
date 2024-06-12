using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class BirchFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            int i6 = random2.NextInt(3) + 4;
            bool z7 = true;
            if (i4 >= 1 && i4 + i6 + 1 <= 128)
            {
                int i8;
                int i10;
                int i11;
                int i12;
                for (i8 = i4; i8 <= i4 + 1 + i6; ++i8)
                {
                    byte b9 = 1;
                    if (i8 == i4)
                    {
                        b9 = 0;
                    }

                    if (i8 >= i4 + 1 + i6 - 2)
                    {
                        b9 = 2;
                    }

                    for (i10 = i3 - b9; i10 <= i3 + b9 && z7; ++i10)
                    {
                        for (i11 = i5 - b9; i11 <= i5 + b9 && z7; ++i11)
                        {
                            if (i8 >= 0 && i8 < 128)
                            {
                                i12 = world1.GetTile(i10, i8, i11);
                                if (i12 != 0 && i12 != Tile.leaves.id)
                                {
                                    z7 = false;
                                }
                            }
                            else
                            {
                                z7 = false;
                            }
                        }
                    }
                }

                if (!z7)
                {
                    return false;
                }
                else
                {
                    i8 = world1.GetTile(i3, i4 - 1, i5);
                    if ((i8 == Tile.grass.id || i8 == Tile.dirt.id) && i4 < 128 - i6 - 1)
                    {
                        world1.SetTileNoUpdate(i3, i4 - 1, i5, Tile.dirt.id);
                        int i16;
                        for (i16 = i4 - 3 + i6; i16 <= i4 + i6; ++i16)
                        {
                            i10 = i16 - (i4 + i6);
                            i11 = 1 - i10 / 2;
                            for (i12 = i3 - i11; i12 <= i3 + i11; ++i12)
                            {
                                int i13 = i12 - i3;
                                for (int i14 = i5 - i11; i14 <= i5 + i11; ++i14)
                                {
                                    int i15 = i14 - i5;
                                    if ((Math.Abs(i13) != i11 || Math.Abs(i15) != i11 || random2.NextInt(2) != 0 && i10 != 0) && !Tile.solid[world1.GetTile(i12, i16, i14)])
                                    {
                                        world1.SetTileNoUpdate(i12, i16, i14, Tile.leaves.id);
                                    }
                                }
                            }
                        }

                        for (i16 = 0; i16 < i6; ++i16)
                        {
                            i10 = world1.GetTile(i3, i4 + i16, i5);
                            if (i10 == 0 || i10 == Tile.leaves.id)
                            {
                                world1.SetTileNoUpdate(i3, i4 + i16, i5, Tile.treeTrunk.id);
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