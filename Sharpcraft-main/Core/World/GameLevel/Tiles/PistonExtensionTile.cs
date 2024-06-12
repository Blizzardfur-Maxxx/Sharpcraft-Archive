using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PistonExtensionTile : Tile
    {
        private int field_31053 = -1;
        public PistonExtensionTile(int i1, int i2) : base(i1, i2, Material.piston)
        {
            this.SetSoundType(SOUND_STONE);
            this.SetDestroyTime(0.5F);
        }

        public virtual void Func_31052_a_(int i1)
        {
            this.field_31053 = i1;
        }

        public virtual void Func_31051_a()
        {
            this.field_31053 = -1;
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            base.OnBlockRemoval(world1, i2, i3, i4);
            int i5 = world1.GetData(i2, i3, i4);
            TileFace i6 = Facing.opposite(Func_31050_c(i5));
            i2 += Facing.xOffset(i6);
            i3 += Facing.yOffset(i6);
            i4 += Facing.zOffset(i6);
            int i7 = world1.GetTile(i2, i3, i4);
            if (i7 == Tile.pistonBase.id || i7 == Tile.pistonStickyBase.id)
            {
                i5 = world1.GetData(i2, i3, i4);
                if (PistonBaseTile.IsPowered(i5))
                {
                    Tile.tiles[i7].DropBlockAsItem(world1, i2, i3, i4, i5);
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            TileFace i3 = Func_31050_c(i2);
            return faceIdx == i3 ? (this.field_31053 >= 0 ? this.field_31053 : ((i2 & 8) != 0 ? this.texture - 1 : this.texture)) : (faceIdx == Facing.opposite(i3) ? 107 : 108);
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.PISTON_EXTENSION;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return false;
        }

        public override bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return false;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override void AddAABBs(Level world1, int i2, int i3, int i4, AABB axisAlignedBB5, List<AABB> arrayList6)
        {
            int i7 = world1.GetData(i2, i3, i4);
            switch (Func_31050_c(i7))
            {
                case TileFace.DOWN:
                    this.SetShape(0F, 0F, 0F, 1F, 0.25F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0.375F, 0.25F, 0.375F, 0.625F, 1F, 0.625F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
                case TileFace.UP:
                    this.SetShape(0F, 0.75F, 0F, 1F, 1F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0.375F, 0F, 0.375F, 0.625F, 0.75F, 0.625F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
                case TileFace.NORTH:
                    this.SetShape(0F, 0F, 0F, 1F, 1F, 0.25F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0.25F, 0.375F, 0.25F, 0.75F, 0.625F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
                case TileFace.SOUTH:
                    this.SetShape(0F, 0F, 0.75F, 1F, 1F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0.25F, 0.375F, 0F, 0.75F, 0.625F, 0.75F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
                case TileFace.WEST:
                    this.SetShape(0F, 0F, 0F, 0.25F, 1F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0.375F, 0.25F, 0.25F, 0.625F, 0.75F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
                case TileFace.EAST:
                    this.SetShape(0.75F, 0F, 0F, 1F, 1F, 1F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    this.SetShape(0F, 0.375F, 0.25F, 0.75F, 0.625F, 0.75F);
                    base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                    break;
            }

            this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            switch (Func_31050_c(i5))
            {
                case TileFace.DOWN:
                    this.SetShape(0F, 0F, 0F, 1F, 0.25F, 1F);
                    break;
                case TileFace.UP:
                    this.SetShape(0F, 0.75F, 0F, 1F, 1F, 1F);
                    break;
                case TileFace.NORTH:
                    this.SetShape(0F, 0F, 0F, 1F, 1F, 0.25F);
                    break;
                case TileFace.SOUTH:
                    this.SetShape(0F, 0F, 0.75F, 1F, 1F, 1F);
                    break;
                case TileFace.WEST:
                    this.SetShape(0F, 0F, 0F, 0.25F, 1F, 1F);
                    break;
                case TileFace.EAST:
                    this.SetShape(0.75F, 0F, 0F, 1F, 1F, 1F);
                    break;
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            TileFace tf = Func_31050_c(world1.GetData(i2, i3, i4));
            int i7 = world1.GetTile(i2 - Facing.xOffset(tf), i3 - Facing.yOffset(tf), i4 - Facing.zOffset(tf));
            if (i7 != Tile.pistonBase.id && i7 != Tile.pistonStickyBase.id)
            {
                world1.SetTile(i2, i3, i4, 0);
            }
            else
            {
                Tile.tiles[i7].NeighborChanged(world1, i2 - Facing.xOffset(tf), i3 - Facing.yOffset(tf), i4 - Facing.zOffset(tf), i5);
            }
        }

        public static TileFace Func_31050_c(int i0)
        {
            return (TileFace)i0 & TileFace.MASK;
        }
    }
}