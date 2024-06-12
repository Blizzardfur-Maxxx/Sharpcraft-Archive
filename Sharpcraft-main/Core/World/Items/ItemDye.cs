using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemDye : Item
    {
        public static readonly String[] COLOR_DESCS = new[]
        {
            "black",
            "red",
            "green",
            "brown",
            "blue",
            "purple",
            "cyan",
            "silver",
            "gray",
            "pink",
            "lime",
            "yellow",
            "lightBlue",
            "magenta",
            "orange",
            "white"
        };
        public static readonly int[] COLOR_RGB = new[]
        {
            1973019,
            11743532,
            3887386,
            5320730,
            2437522,
            8073150,
            2651799,
            2651799,
            4408131,
            14188952,
            4312372,
            14602026,
            6719955,
            12801229,
            15435844,
            15790320
        };
        public ItemDye(int i1) : base(i1)
        {
            this.SetHasSubtypes(true);
            this.SetMaxDamage(0);
        }

        public override int GetIconFromDamage(int i1)
        {
            return this.iconIndex + i1 % 8 * 16 + i1 / 8;
        }

        public override string GetItemNameIS(ItemInstance itemStack1)
        {
            return base.GetItemName() + "." + COLOR_DESCS[itemStack1.GetItemDamage()];
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (itemStack1.GetItemDamage() == 15)
            {
                int i8 = world3.GetTile(i4, i5, i6);
                if (i8 == Tile.sapling.id)
                {
                    if (!world3.isRemote)
                    {
                        ((Sapling)Tile.sapling).GrowTree(world3, i4, i5, i6, world3.rand);
                        --itemStack1.stackSize;
                    }

                    return true;
                }

                if (i8 == Tile.crops.id)
                {
                    if (!world3.isRemote)
                    {
                        ((CropTile)Tile.crops).Fertilize(world3, i4, i5, i6);
                        --itemStack1.stackSize;
                    }

                    return true;
                }

                if (i8 == Tile.grass.id)
                {
                    if (!world3.isRemote)
                    {
                        --itemStack1.stackSize;
                        bool continue53 = false;
                    //label53:
                        for (int i9 = 0; i9 < 128; ++i9)
                        {
                            int i10 = i4;
                            int i11 = i5 + 1;
                            int i12 = i6;
                            for (int i13 = 0; i13 < i9 / 16; ++i13)
                            {
                                i10 += itemRand.NextInt(3) - 1;
                                i11 += (itemRand.NextInt(3) - 1) * itemRand.NextInt(3) / 2;
                                i12 += itemRand.NextInt(3) - 1;
                                if (world3.GetTile(i10, i11 - 1, i12) != Tile.grass.id || world3.IsSolidBlockingTile(i10, i11, i12))
                                {
                                    //continue label53;
                                    continue53 = true;
                                    break;
                                }
                            }
                            if (continue53) 
                            {
                                continue53 = false;
                                continue;
                            }

                            if (world3.GetTile(i10, i11, i12) == 0)
                            {
                                if (itemRand.NextInt(10) != 0)
                                {
                                    world3.SetTileAndData(i10, i11, i12, Tile.tallGrass.id, 1);
                                }
                                else if (itemRand.NextInt(3) != 0)
                                {
                                    world3.SetTile(i10, i11, i12, Tile.flower.id);
                                }
                                else
                                {
                                    world3.SetTile(i10, i11, i12, Tile.rose.id);
                                }
                            }
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override void SaddleEntity(ItemInstance itemStack1, Mob entityLiving2)
        {
            if (entityLiving2 is Sheep)
            {
                Sheep entitySheep3 = (Sheep)entityLiving2;
                int i4 = ClothTile.GetMetadataColor0(itemStack1.GetItemDamage());
                if (!entitySheep3.GetSheared() && entitySheep3.GetFleeceColor() != i4)
                {
                    entitySheep3.SetFleeceColor(i4);
                    --itemStack1.stackSize;
                }
            }
        }
    }
}