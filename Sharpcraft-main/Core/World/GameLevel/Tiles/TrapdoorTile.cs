using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TrapdoorTile : Tile
    {
        public TrapdoorTile(int i1, Material material2) : base(i1, material2)
        {
            this.texture = 84;
            if (material2 == Material.metal)
            {
                ++this.texture;
            }

            float f3 = 0.5F;
            float f4 = 1F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, f4, 0.5F + f3);
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
            return RenderShape.NORMAL;
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.GetTileAABB(world1, i2, i3, i4);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.GetAABB(world1, i2, i3, i4);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            this.SetCockBounds(iBlockAccess1.GetData(i2, i3, i4));
        }

        public override void SetBlockBoundsForItemRender()
        {
            float f1 = 0.1875F;
            this.SetShape(0F, 0.5F - f1 / 2F, 0F, 1F, 0.5F + f1 / 2F, 1F);
        }

        public virtual void SetCockBounds(int i1)
        {
            float f2 = 0.1875F;
            this.SetShape(0F, 0F, 0F, 1F, f2, 1F);
            if (IsTrapdoorOpen(i1))
            {
                if ((i1 & 3) == 0)
                {
                    this.SetShape(0F, 0F, 1F - f2, 1F, 1F, 1F);
                }

                if ((i1 & 3) == 1)
                {
                    this.SetShape(0F, 0F, 0F, 1F, 1F, f2);
                }

                if ((i1 & 3) == 2)
                {
                    this.SetShape(1F - f2, 0F, 0F, 1F, 1F, 1F);
                }

                if ((i1 & 3) == 3)
                {
                    this.SetShape(0F, 0F, 0F, f2, 1F, 1F);
                }
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (this.material == Material.metal)
            {
                return true;
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                world1.SetData(i2, i3, i4, i6 ^ 4);
                world1.LevelEvent(entityPlayer5, LevelEventType.DOOR, i2, i3, i4, 0);
                return true;
            }
        }

        public virtual void OnPoweredBlockChange(Level world1, int i2, int i3, int i4, bool z5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            bool z7 = (i6 & 4) > 0;
            if (z7 != z5)
            {
                world1.SetData(i2, i3, i4, i6 ^ 4);
                world1.LevelEvent((Player)null, LevelEventType.DOOR, i2, i3, i4, 0);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                int i7 = i2;
                int i8 = i4;
                if ((i6 & 3) == 0)
                {
                    i8 = i4 + 1;
                }

                if ((i6 & 3) == 1)
                {
                    --i8;
                }

                if ((i6 & 3) == 2)
                {
                    i7 = i2 + 1;
                }

                if ((i6 & 3) == 3)
                {
                    --i7;
                }

                if (!world1.IsSolidBlockingTile(i7, i3, i8))
                {
                    world1.SetTile(i2, i3, i4, 0);
                    this.DropBlockAsItem(world1, i2, i3, i4, i6);
                }

                if (i5 > 0 && Tile.tiles[i5].IsSignalSource())
                {
                    bool z9 = world1.IsBlockIndirectlyGettingPowered(i2, i3, i4);
                    this.OnPoweredBlockChange(world1, i2, i3, i4, z9);
                }
            }
        }

        public override HitResult Clip(Level world1, int i2, int i3, int i4, Vec3 vec3D5, Vec3 vec3D6)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.Clip(world1, i2, i3, i4, vec3D5, vec3D6);
        }

        public override void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            byte b6 = 0;
            if (i5 == TileFace.NORTH)
            {
                b6 = 0;
            }

            if (i5 == TileFace.SOUTH)
            {
                b6 = 1;
            }

            if (i5 == TileFace.WEST)
            {
                b6 = 2;
            }

            if (i5 == TileFace.EAST)
            {
                b6 = 3;
            }

            world1.SetData(i2, i3, i4, b6);
        }

        public override bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.DOWN)
            {
                return false;
            }
            else if (i5 == TileFace.UP)
            {
                return false;
            }
            else
            {
                if (i5 == TileFace.NORTH)
                {
                    ++i4;
                }

                if (i5 == TileFace.SOUTH)
                {
                    --i4;
                }

                if (i5 == TileFace.WEST)
                {
                    ++i2;
                }

                if (i5 == TileFace.EAST)
                {
                    --i2;
                }

                return world1.IsSolidBlockingTile(i2, i3, i4);
            }
        }

        public static bool IsTrapdoorOpen(int i0)
        {
            return (i0 & 4) != 0;
        }
    }
}