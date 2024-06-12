using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SignTile : EntityTile
    {
        private Type signEntityClass;
        private bool isFreestanding;
        public SignTile(int i1, Type class2, bool z3) : base(i1, Material.wood)
        {
            this.isFreestanding = z3;
            this.texture = 4;
            this.signEntityClass = class2;
            float f4 = 0.25F;
            float f5 = 1F;
            this.SetShape(0.5F - f4, 0F, 0.5F - f4, 0.5F + f4, f5, 0.5F + f4);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.GetTileAABB(world1, i2, i3, i4);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            if (!this.isFreestanding)
            {
                int i5 = iBlockAccess1.GetData(i2, i3, i4);
                float f6 = 0.28125F;
                float f7 = 0.78125F;
                float f8 = 0F;
                float f9 = 1F;
                float f10 = 0.125F;
                this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
                if (i5 == 2)
                {
                    this.SetShape(f8, f6, 1F - f10, f9, f7, 1F);
                }

                if (i5 == 3)
                {
                    this.SetShape(f8, f6, 0F, f9, f7, f10);
                }

                if (i5 == 4)
                {
                    this.SetShape(1F - f10, f6, f8, 1F, f7, f9);
                }

                if (i5 == 5)
                {
                    this.SetShape(0F, f6, f8, f10, f7, f9);
                }
            }
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.NONE;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        protected override TileEntity NewTileEntity()
        {
            try
            {
                return (TileEntity)Activator.CreateInstance(signEntityClass);
            }
            catch (Exception exception2)
            {
                throw new Exception("", exception2);
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.sign.id;
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            bool z6 = false;
            if (this.isFreestanding)
            {
                if (!world1.GetMaterial(i2, i3 - 1, i4).IsSolid())
                {
                    z6 = true;
                }
            }
            else
            {
                int i7 = world1.GetData(i2, i3, i4);
                z6 = true;
                if (i7 == 2 && world1.GetMaterial(i2, i3, i4 + 1).IsSolid())
                {
                    z6 = false;
                }

                if (i7 == 3 && world1.GetMaterial(i2, i3, i4 - 1).IsSolid())
                {
                    z6 = false;
                }

                if (i7 == 4 && world1.GetMaterial(i2 + 1, i3, i4).IsSolid())
                {
                    z6 = false;
                }

                if (i7 == 5 && world1.GetMaterial(i2 - 1, i3, i4).IsSolid())
                {
                    z6 = false;
                }
            }

            if (z6)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }

            base.NeighborChanged(world1, i2, i3, i4, i5);
        }
    }
}