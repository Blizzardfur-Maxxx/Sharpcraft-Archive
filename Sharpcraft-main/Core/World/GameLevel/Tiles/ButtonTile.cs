using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ButtonTile : Tile
    {
        public ButtonTile(int i1, int i2) : base(i1, i2, Material.decoration)
        {
            this.SetTicking(true);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override int GetTickDelay()
        {
            return 20;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return i5 == TileFace.NORTH && world1.IsSolidBlockingTile(i2, i3, i4 + 1) ? true : (i5 == TileFace.SOUTH && world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : (i5 == TileFace.WEST && world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : i5 == TileFace.EAST && world1.IsSolidBlockingTile(i2 - 1, i3, i4)));
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2 - 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : world1.IsSolidBlockingTile(i2, i3, i4 + 1)));
        }

        public override void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = i6 & 8;
            i6 &= 7;
            if (i5 == TileFace.NORTH && world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                i6 = 4;
            }
            else if (i5 == TileFace.SOUTH && world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                i6 = 3;
            }
            else if (i5 == TileFace.WEST && world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                i6 = 2;
            }
            else if (i5 == TileFace.EAST && world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                i6 = 1;
            }
            else
            {
                i6 = this.GetOrientation(world1, i2, i3, i4);
            }

            world1.SetData(i2, i3, i4, i6 + i7);
        }

        private int GetOrientation(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2 - 1, i3, i4) ? 1 : (world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? 2 : (world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? 3 : (world1.IsSolidBlockingTile(i2, i3, i4 + 1) ? 4 : 1)));
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (this.PressButton(world1, i2, i3, i4))
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

                if (z7)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
        }

        private bool PressButton(Level world1, int i2, int i3, int i4)
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
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            int i6 = i5 & 7;
            bool z7 = (i5 & 8) > 0;
            float f8 = 0.375F;
            float f9 = 0.625F;
            float f10 = 0.1875F;
            float f11 = 0.125F;
            if (z7)
            {
                f11 = 0.0625F;
            }

            if (i6 == 1)
            {
                this.SetShape(0F, f8, 0.5F - f10, f11, f9, 0.5F + f10);
            }
            else if (i6 == 2)
            {
                this.SetShape(1F - f11, f8, 0.5F - f10, 1F, f9, 0.5F + f10);
            }
            else if (i6 == 3)
            {
                this.SetShape(0.5F - f10, f8, 0F, 0.5F + f10, f9, f11);
            }
            else if (i6 == 4)
            {
                this.SetShape(0.5F - f10, f8, 1F - f11, 0.5F + f10, f9, 1F);
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = i6 & 7;
            int i8 = 8 - (i6 & 8);
            if (i8 == 0)
            {
                return true;
            }
            else
            {
                world1.SetData(i2, i3, i4, i7 + i8);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "random.click", 0.3F, 0.6F);
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

                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
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
                return i7 == 5 && i5 == 1 ? true : (i7 == 4 && i5 == 2 ? true : (i7 == 3 && i5 == 3 ? true : (i7 == 2 && i5 == 4 ? true : i7 == 1 && i5 == 5)));
            }
        }

        public override bool IsSignalSource()
        {
            return true;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if ((i6 & 8) != 0)
                {
                    world1.SetData(i2, i3, i4, i6 & 7);
                    world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                    int i7 = i6 & 7;
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

                    world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "random.click", 0.3F, 0.5F);
                    world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                }
            }
        }

        public override void SetBlockBoundsForItemRender()
        {
            float f1 = 0.1875F;
            float f2 = 0.125F;
            float f3 = 0.125F;
            this.SetShape(0.5F - f1, 0.5F - f2, 0.5F - f3, 0.5F + f1, 0.5F + f2, 0.5F + f3);
        }
    }
}