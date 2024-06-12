using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ChestTile : EntityTile
    {
        private JRandom random = new JRandom();
        public ChestTile(int i1) : base(i1, Material.wood)
        {
            this.texture = 26;
        }

        public override int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.UP)
            {
                return this.texture - 1;
            }
            else if (i5 == TileFace.DOWN)
            {
                return this.texture - 1;
            }
            else
            {
                int i6 = iBlockAccess1.GetTile(i2, i3, i4 - 1);
                int i7 = iBlockAccess1.GetTile(i2, i3, i4 + 1);
                int i8 = iBlockAccess1.GetTile(i2 - 1, i3, i4);
                int i9 = iBlockAccess1.GetTile(i2 + 1, i3, i4);
                int i10;
                int i11;
                int i12;
                TileFace b13;
                if (i6 != this.id && i7 != this.id)
                {
                    if (i8 != this.id && i9 != this.id)
                    {
                        TileFace b14 = TileFace.SOUTH;
                        if (Tile.solid[i6] && !Tile.solid[i7])
                        {
                            b14 = TileFace.SOUTH;
                        }

                        if (Tile.solid[i7] && !Tile.solid[i6])
                        {
                            b14 = TileFace.NORTH;
                        }

                        if (Tile.solid[i8] && !Tile.solid[i9])
                        {
                            b14 = TileFace.EAST;
                        }

                        if (Tile.solid[i9] && !Tile.solid[i8])
                        {
                            b14 = TileFace.WEST;
                        }

                        return i5 == b14 ? this.texture + 1 : this.texture;
                    }
                    else if (i5 != TileFace.WEST && i5 != TileFace.EAST)
                    {
                        i10 = 0;
                        if (i8 == this.id)
                        {
                            i10 = -1;
                        }

                        i11 = iBlockAccess1.GetTile(i8 == this.id ? i2 - 1 : i2 + 1, i3, i4 - 1);
                        i12 = iBlockAccess1.GetTile(i8 == this.id ? i2 - 1 : i2 + 1, i3, i4 + 1);
                        if (i5 == TileFace.SOUTH)
                        {
                            i10 = -1 - i10;
                        }

                        b13 = TileFace.SOUTH;
                        if ((Tile.solid[i6] || Tile.solid[i11]) && !Tile.solid[i7] && !Tile.solid[i12])
                        {
                            b13 = TileFace.SOUTH;
                        }

                        if ((Tile.solid[i7] || Tile.solid[i12]) && !Tile.solid[i6] && !Tile.solid[i11])
                        {
                            b13 = TileFace.NORTH;
                        }

                        return (i5 == b13 ? this.texture + 16 : this.texture + 32) + i10;
                    }
                    else
                    {
                        return this.texture;
                    }
                }
                else if (i5 != TileFace.NORTH && i5 != TileFace.SOUTH)
                {
                    i10 = 0;
                    if (i6 == this.id)
                    {
                        i10 = -1;
                    }

                    i11 = iBlockAccess1.GetTile(i2 - 1, i3, i6 == this.id ? i4 - 1 : i4 + 1);
                    i12 = iBlockAccess1.GetTile(i2 + 1, i3, i6 == this.id ? i4 - 1 : i4 + 1);
                    if (i5 == TileFace.WEST)
                    {
                        i10 = -1 - i10;
                    }

                    b13 = TileFace.EAST;
                    if ((Tile.solid[i8] || Tile.solid[i11]) && !Tile.solid[i9] && !Tile.solid[i12])
                    {
                        b13 = TileFace.EAST;
                    }

                    if ((Tile.solid[i9] || Tile.solid[i12]) && !Tile.solid[i8] && !Tile.solid[i11])
                    {
                        b13 = TileFace.WEST;
                    }

                    return (i5 == b13 ? this.texture + 16 : this.texture + 32) + i10;
                }
                else
                {
                    return this.texture;
                }
            }
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture - 1 : (faceIdx == 0 ? this.texture - 1 : (faceIdx == TileFace.SOUTH ? this.texture + 1 : this.texture));
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            int i5 = 0;
            if (world1.GetTile(i2 - 1, i3, i4) == this.id)
            {
                ++i5;
            }

            if (world1.GetTile(i2 + 1, i3, i4) == this.id)
            {
                ++i5;
            }

            if (world1.GetTile(i2, i3, i4 - 1) == this.id)
            {
                ++i5;
            }

            if (world1.GetTile(i2, i3, i4 + 1) == this.id)
            {
                ++i5;
            }

            return i5 > 1 ? false : (this.IsThereANeighborChest(world1, i2 - 1, i3, i4) ? false : (this.IsThereANeighborChest(world1, i2 + 1, i3, i4) ? false : (this.IsThereANeighborChest(world1, i2, i3, i4 - 1) ? false : !this.IsThereANeighborChest(world1, i2, i3, i4 + 1))));
        }

        private bool IsThereANeighborChest(Level world1, int i2, int i3, int i4)
        {
            return world1.GetTile(i2, i3, i4) != this.id ? false : (world1.GetTile(i2 - 1, i3, i4) == this.id ? true : (world1.GetTile(i2 + 1, i3, i4) == this.id ? true : (world1.GetTile(i2, i3, i4 - 1) == this.id ? true : world1.GetTile(i2, i3, i4 + 1) == this.id)));
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            TileEntityChest tileEntityChest5 = (TileEntityChest)world1.GetTileEntity(i2, i3, i4);
            for (int i6 = 0; i6 < tileEntityChest5.GetContainerSize(); ++i6)
            {
                ItemInstance itemStack7 = tileEntityChest5.GetItem(i6);
                if (itemStack7 != null)
                {
                    float f8 = (float)random.NextFloat() * 0.8F + 0.1F;
                    float f9 = (float)random.NextFloat() * 0.8F + 0.1F;
                    float f10 = (float)random.NextFloat() * 0.8F + 0.1F;
                    while (itemStack7.stackSize > 0)
                    {
                        int i11 = random.NextInt(21) + 10;
                        if (i11 > itemStack7.stackSize)
                        {
                            i11 = itemStack7.stackSize;
                        }

                        itemStack7.stackSize -= i11;
                        ItemEntity entityItem12 = new ItemEntity(world1, i2 + f8, i3 + f9, i4 + f10, new ItemInstance(itemStack7.itemID, i11, itemStack7.GetItemDamage()));
                        float f13 = 0.05F;
                        entityItem12.motionX = (float)random.NextGaussian() * f13;
                        entityItem12.motionY = (float)random.NextGaussian() * f13 + 0.2F;
                        entityItem12.motionZ = (float)random.NextGaussian() * f13;
                        world1.AddEntity(entityItem12);
                    }
                }
            }

            base.OnBlockRemoval(world1, i2, i3, i4);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            object object6 = world1.GetTileEntity(i2, i3, i4);
            if (world1.IsSolidBlockingTile(i2, i3 + 1, i4))
            {
                return true;
            }
            else if (world1.GetTile(i2 - 1, i3, i4) == this.id && world1.IsSolidBlockingTile(i2 - 1, i3 + 1, i4))
            {
                return true;
            }
            else if (world1.GetTile(i2 + 1, i3, i4) == this.id && world1.IsSolidBlockingTile(i2 + 1, i3 + 1, i4))
            {
                return true;
            }
            else if (world1.GetTile(i2, i3, i4 - 1) == this.id && world1.IsSolidBlockingTile(i2, i3 + 1, i4 - 1))
            {
                return true;
            }
            else if (world1.GetTile(i2, i3, i4 + 1) == this.id && world1.IsSolidBlockingTile(i2, i3 + 1, i4 + 1))
            {
                return true;
            }
            else
            {
                if (world1.GetTile(i2 - 1, i3, i4) == this.id)
                {
                    object6 = new CompoundContainer("Large chest", (TileEntityChest)world1.GetTileEntity(i2 - 1, i3, i4), (IContainer)object6);
                }

                if (world1.GetTile(i2 + 1, i3, i4) == this.id)
                {
                    object6 = new CompoundContainer("Large chest", (IContainer)object6, (TileEntityChest)world1.GetTileEntity(i2 + 1, i3, i4));
                }

                if (world1.GetTile(i2, i3, i4 - 1) == this.id)
                {
                    object6 = new CompoundContainer("Large chest", (TileEntityChest)world1.GetTileEntity(i2, i3, i4 - 1), (IContainer)object6);
                }

                if (world1.GetTile(i2, i3, i4 + 1) == this.id)
                {
                    object6 = new CompoundContainer("Large chest", (IContainer)object6, (TileEntityChest)world1.GetTileEntity(i2, i3, i4 + 1));
                }

                if (world1.isRemote)
                {
                    return true;
                }
                else
                {
                    entityPlayer5.DisplayGUIChest((IContainer)object6);
                    return true;
                }
            }
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityChest();
        }
    }
}