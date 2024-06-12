using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class MonsterRoomFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            byte b6 = 3;
            int i7 = random2.NextInt(2) + 2;
            int i8 = random2.NextInt(2) + 2;
            int i9 = 0;
            int i10;
            int i11;
            int i12;
            for (i10 = i3 - i7 - 1; i10 <= i3 + i7 + 1; ++i10)
            {
                for (i11 = i4 - 1; i11 <= i4 + b6 + 1; ++i11)
                {
                    for (i12 = i5 - i8 - 1; i12 <= i5 + i8 + 1; ++i12)
                    {
                        Material material13 = world1.GetMaterial(i10, i11, i12);
                        if (i11 == i4 - 1 && !material13.IsSolid())
                        {
                            return false;
                        }

                        if (i11 == i4 + b6 + 1 && !material13.IsSolid())
                        {
                            return false;
                        }

                        if ((i10 == i3 - i7 - 1 || i10 == i3 + i7 + 1 || i12 == i5 - i8 - 1 || i12 == i5 + i8 + 1) && i11 == i4 && world1.IsAirBlock(i10, i11, i12) && world1.IsAirBlock(i10, i11 + 1, i12))
                        {
                            ++i9;
                        }
                    }
                }
            }

            if (i9 >= 1 && i9 <= 5)
            {
                for (i10 = i3 - i7 - 1; i10 <= i3 + i7 + 1; ++i10)
                {
                    for (i11 = i4 + b6; i11 >= i4 - 1; --i11)
                    {
                        for (i12 = i5 - i8 - 1; i12 <= i5 + i8 + 1; ++i12)
                        {
                            if (i10 != i3 - i7 - 1 && i11 != i4 - 1 && i12 != i5 - i8 - 1 && i10 != i3 + i7 + 1 && i11 != i4 + b6 + 1 && i12 != i5 + i8 + 1)
                            {
                                world1.SetTile(i10, i11, i12, 0);
                            }
                            else if (i11 >= 0 && !world1.GetMaterial(i10, i11 - 1, i12).IsSolid())
                            {
                                world1.SetTile(i10, i11, i12, 0);
                            }
                            else if (world1.GetMaterial(i10, i11, i12).IsSolid())
                            {
                                if (i11 == i4 - 1 && random2.NextInt(4) != 0)
                                {
                                    world1.SetTile(i10, i11, i12, Tile.mossStone.id);
                                }
                                else
                                {
                                    world1.SetTile(i10, i11, i12, Tile.stoneBrick.id);
                                }
                            }
                        }
                    }
                }

                bool continue110 = false;
            //label110:
                for (i10 = 0; i10 < 2; ++i10)
                {
                    for (i11 = 0; i11 < 3; ++i11)
                    {
                        i12 = i3 + random2.NextInt(i7 * 2 + 1) - i7;
                        int i14 = i5 + random2.NextInt(i8 * 2 + 1) - i8;
                        if (world1.IsAirBlock(i12, i4, i14))
                        {
                            int i15 = 0;
                            if (world1.GetMaterial(i12 - 1, i4, i14).IsSolid())
                            {
                                ++i15;
                            }

                            if (world1.GetMaterial(i12 + 1, i4, i14).IsSolid())
                            {
                                ++i15;
                            }

                            if (world1.GetMaterial(i12, i4, i14 - 1).IsSolid())
                            {
                                ++i15;
                            }

                            if (world1.GetMaterial(i12, i4, i14 + 1).IsSolid())
                            {
                                ++i15;
                            }

                            if (i15 == 1)
                            {
                                world1.SetTile(i12, i4, i14, Tile.chest.id);
                                TileEntityChest tileEntityChest16 = (TileEntityChest)world1.GetTileEntity(i12, i4, i14);
                                int i17 = 0;
                                while (true)
                                {
                                    if (i17 >= 8)
                                    {
                                        //continue label110;
                                        continue110 = true;
                                        break;
                                    }

                                    ItemInstance itemStack18 = this.GetRandomItem(random2);
                                    if (itemStack18 != null)
                                    {
                                        tileEntityChest16.SetItem(random2.NextInt(tileEntityChest16.GetContainerSize()), itemStack18);
                                    }

                                    ++i17;
                                }
                                if (continue110) break;
                            }
                        }
                    }
                    if (continue110) 
                    {
                        continue110 = false;
                        continue;
                    }
                }

                world1.SetTile(i3, i4, i5, Tile.mobSpawner.id);
                TileEntityMobSpawner tileEntityMobSpawner19 = (TileEntityMobSpawner)world1.GetTileEntity(i3, i4, i5);
                tileEntityMobSpawner19.SetEntityId(this.GetRandomEntityId(random2));
                return true;
            }
            else
            {
                return false;
            }
        }

        private ItemInstance GetRandomItem(JRandom random1)
        {
            int i2 = random1.NextInt(11);
            return i2 == 0 ? new ItemInstance(Item.saddle) : (i2 == 1 ? new ItemInstance(Item.ingotIron, random1.NextInt(4) + 1) : (i2 == 2 ? new ItemInstance(Item.bread) : (i2 == 3 ? new ItemInstance(Item.wheat, random1.NextInt(4) + 1) : (i2 == 4 ? new ItemInstance(Item.gunpowder, random1.NextInt(4) + 1) : (i2 == 5 ? new ItemInstance(Item.silk, random1.NextInt(4) + 1) : (i2 == 6 ? new ItemInstance(Item.bucketEmpty) : (i2 == 7 && random1.NextInt(100) == 0 ? new ItemInstance(Item.appleGold) : (i2 == 8 && random1.NextInt(2) == 0 ? new ItemInstance(Item.redstone, random1.NextInt(4) + 1) : (i2 == 9 && random1.NextInt(10) == 0 ? new ItemInstance(Item.items[Item.record13.id + random1.NextInt(2)]) : (i2 == 10 ? new ItemInstance(Item.dyePowder, 1, 3) : null))))))))));
        }

        private string GetRandomEntityId(JRandom random1)
        {
            int i2 = random1.NextInt(4);
            return i2 == 0 ? "Skeleton" : (i2 == 1 ? "Zombie" : (i2 == 2 ? "Zombie" : (i2 == 3 ? "Spider" : "")));
        }
    }
}