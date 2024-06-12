using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LeverTile : Tile
    {
        public LeverTile(int i1, int i2) : base(i1, i2, Material.decoration)
        {
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.LEVER;
        }

        public override bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return i5 == TileFace.UP && world1.IsSolidBlockingTile(i2, i3 - 1, i4) ? true : (i5 == TileFace.NORTH && world1.IsSolidBlockingTile(i2, i3, i4 + 1) ? true : (i5 == TileFace.SOUTH && world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : (i5 == TileFace.WEST && world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : i5 == TileFace.EAST && world1.IsSolidBlockingTile(i2 - 1, i3, i4))));
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2 - 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 + 1) ? true : world1.IsSolidBlockingTile(i2, i3 - 1, i4))));
        }

        public override void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = i6 & 8;
            i6 &= 7;
            i6 = -1;
            if (i5 == TileFace.UP && world1.IsSolidBlockingTile(i2, i3 - 1, i4))
            {
                i6 = 5 + world1.rand.NextInt(2);
            }

            if (i5 == TileFace.NORTH && world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                i6 = 4;
            }

            if (i5 == TileFace.SOUTH && world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                i6 = 3;
            }

            if (i5 == TileFace.WEST && world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                i6 = 2;
            }

            if (i5 == TileFace.EAST && world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                i6 = 1;
            }

            if (i6 == -1)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
            else
            {
                world1.SetData(i2, i3, i4, i6 + i7);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (this.CheckIfAttachedToBlock(world1, i2, i3, i4))
            {
                int i6 = world1.GetData(i2, i3, i4) & 7;
                bool z7 = false;
                if (!world1.IsSolidBlockingTile(i2 - 1, i3, i4) && i6 == 1)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2 + 1, i3, i4) && i6 == 2)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3, i4 - 1) && i6 == 3)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3, i4 + 1) && i6 == 4)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) && i6 == 5)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) && i6 == 6)
                {
                    z7 = true;
                }

                if (z7)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
        }

        private bool CheckIfAttachedToBlock(Level world1, int i2, int i3, int i4)
        {
            if (!this.CanPlaceBlockAt(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4) & 7;
            float f6 = 0.1875F;
            if (i5 == 1)
            {
                this.SetShape(0F, 0.2F, 0.5F - f6, f6 * 2F, 0.8F, 0.5F + f6);
            }
            else if (i5 == 2)
            {
                this.SetShape(1F - f6 * 2F, 0.2F, 0.5F - f6, 1F, 0.8F, 0.5F + f6);
            }
            else if (i5 == 3)
            {
                this.SetShape(0.5F - f6, 0.2F, 0F, 0.5F + f6, 0.8F, f6 * 2F);
            }
            else if (i5 == 4)
            {
                this.SetShape(0.5F - f6, 0.2F, 1F - f6 * 2F, 0.5F + f6, 0.8F, 1F);
            }
            else
            {
                f6 = 0.25F;
                this.SetShape(0.5F - f6, 0F, 0.5F - f6, 0.5F + f6, 0.6F, 0.5F + f6);
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                int i7 = i6 & 7;
                int i8 = 8 - (i6 & 8);
                world1.SetData(i2, i3, i4, i7 + i8);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "random.click", 0.3F, i8 > 0 ? 0.6F : 0.5F);
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                if (i7 == 1)
                {
                    world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
                }
                else if (i7 == 2)
                {
                    world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
                }
                else if (i7 == 3)
                {
                    world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
                }
                else if (i7 == 4)
                {
                    world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
                }
                else
                {
                    world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                }

                return true;
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            if ((i5 & 8) > 0)
            {
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                int i6 = i5 & 7;
                if (i6 == 1)
                {
                    world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
                }
                else if (i6 == 2)
                {
                    world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
                }
                else if (i6 == 3)
                {
                    world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
                }
                else if (i6 == 4)
                {
                    world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
                }
                else
                {
                    world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                }
            }

            base.OnBlockRemoval(world1, i2, i3, i4);
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return (iBlockAccess1.GetData(i2, i3, i4) & 8) > 0;
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if ((i6 & 8) == 0)
            {
                return false;
            }
            else
            {
                int i7 = i6 & 7;
                return i7 == 6 && i5 == 1 ? true : (i7 == 5 && i5 == 1 ? true : (i7 == 4 && i5 == 2 ? true : (i7 == 3 && i5 == 3 ? true : (i7 == 2 && i5 == 4 ? true : i7 == 1 && i5 == 5))));
            }
        }

        public override bool IsSignalSource()
        {
            return true;
        }
    }
}