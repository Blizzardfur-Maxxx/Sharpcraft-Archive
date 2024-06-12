using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PistonMovingPiece : EntityTile
    {
        public PistonMovingPiece(int i1) : base(i1, Material.piston)
        {
            this.SetDestroyTime(-1F);
        }

        protected override TileEntity NewTileEntity()
        {
            return null;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            TileEntity tileEntity5 = world1.GetTileEntity(i2, i3, i4);
            if (tileEntity5 != null && tileEntity5 is PistonPieceEntity)
            {
                ((PistonPieceEntity)tileEntity5).ClearPistonTileEntity();
            }
            else
            {
                base.OnBlockRemoval(world1, i2, i3, i4);
            }
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return false;
        }

        public override bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.NONE;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (!world1.isRemote && world1.GetTileEntity(i2, i3, i4) == null)
            {
                world1.SetTile(i2, i3, i4, 0);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return 0;
        }

        public override void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            if (!world1.isRemote)
            {
                PistonPieceEntity tileEntityPiston7 = this.GetTileEntityAtLocation(world1, i2, i3, i4);
                if (tileEntityPiston7 != null)
                {
                    Tile.tiles[tileEntityPiston7.GetStoredBlockID()].DropBlockAsItem(world1, i2, i3, i4, tileEntityPiston7.GetBlockMetadata());
                }
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote && world1.GetTileEntity(i2, i3, i4) == null)
            {
            }
        }

        public static TileEntity GetTileEntity(int i0, int i1, Facing.TileFace i2, bool z3, bool z4)
        {
            return new PistonPieceEntity(i0, i1, i2, z3, z4);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            PistonPieceEntity tileEntityPiston5 = this.GetTileEntityAtLocation(world1, i2, i3, i4);
            if (tileEntityPiston5 == null)
            {
                return null;
            }
            else
            {
                float f6 = tileEntityPiston5.Func_31008(0F);
                if (tileEntityPiston5.Func_31015())
                {
                    f6 = 1F - f6;
                }

                return this.Func_31035(world1, i2, i3, i4, tileEntityPiston5.GetStoredBlockID(), f6, tileEntityPiston5.Func_31009());
            }
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            PistonPieceEntity tileEntityPiston5 = this.GetTileEntityAtLocation(iBlockAccess1, i2, i3, i4);
            if (tileEntityPiston5 != null)
            {
                Tile block6 = Tile.tiles[tileEntityPiston5.GetStoredBlockID()];
                if (block6 == null || block6 == this)
                {
                    return;
                }

                block6.SetBlockBoundsBasedOnState(iBlockAccess1, i2, i3, i4);
                float f7 = tileEntityPiston5.Func_31008(0F);
                if (tileEntityPiston5.Func_31015())
                {
                    f7 = 1F - f7;
                }

                TileFace i8 = tileEntityPiston5.Func_31009();
                this.minX = block6.minX - Facing.xOffset(i8) * f7;
                this.minY = block6.minY - Facing.yOffset(i8) * f7;
                this.minZ = block6.minZ - Facing.zOffset(i8) * f7;
                this.maxX = block6.maxX - Facing.xOffset(i8) * f7;
                this.maxY = block6.maxY - Facing.yOffset(i8) * f7;
                this.maxZ = block6.maxZ - Facing.zOffset(i8) * f7;
            }
        }

        public virtual AABB Func_31035(Level world1, int i2, int i3, int i4, int i5, float f6, TileFace i7)
        {
            if (i5 != 0 && i5 != this.id)
            {
                AABB axisAlignedBB8 = Tile.tiles[i5].GetAABB(world1, i2, i3, i4);
                if (axisAlignedBB8 == null)
                {
                    return null;
                }
                else
                {
                    axisAlignedBB8.x0 -= Facing.xOffset(i7) * f6;
                    axisAlignedBB8.x1 -= Facing.xOffset(i7) * f6;
                    axisAlignedBB8.y0 -= Facing.yOffset(i7) * f6;
                    axisAlignedBB8.y1 -= Facing.yOffset(i7) * f6;
                    axisAlignedBB8.z0 -= Facing.zOffset(i7) * f6;
                    axisAlignedBB8.z1 -= Facing.zOffset(i7) * f6;
                    return axisAlignedBB8;
                }
            }
            else
            {
                return null;
            }
        }

        private PistonPieceEntity GetTileEntityAtLocation(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            TileEntity tileEntity5 = iBlockAccess1.GetTileEntity(i2, i3, i4);
            return tileEntity5 != null && tileEntity5 is PistonPieceEntity ? (PistonPieceEntity)tileEntity5 : null;
        }
    }
}