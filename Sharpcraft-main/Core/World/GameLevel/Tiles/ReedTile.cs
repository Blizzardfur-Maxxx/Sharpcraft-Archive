using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ReedTile : Tile
    {
        public ReedTile(int i1, int i2) : base(i1, Material.plant)
        {
            this.texture = i2;
            float f3 = 0.375F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, 1F, 0.5F + f3);
            this.SetTicking(true);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.IsAirBlock(i2, i3 + 1, i4))
            {
                int i6;
                for (i6 = 1; world1.GetTile(i2, i3 - i6, i4) == this.id; ++i6)
                {
                }

                if (i6 < 3)
                {
                    int i7 = world1.GetData(i2, i3, i4);
                    if (i7 == 15)
                    {
                        world1.SetTile(i2, i3 + 1, i4, this.id);
                        world1.SetData(i2, i3, i4, 0);
                    }
                    else
                    {
                        world1.SetData(i2, i3, i4, i7 + 1);
                    }
                }
            }
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetTile(i2, i3 - 1, i4);
            return i5 == this.id ? true : (i5 != Tile.grass.id && i5 != Tile.dirt.id ? false : (world1.GetMaterial(i2 - 1, i3 - 1, i4) == Material.water ? true : (world1.GetMaterial(i2 + 1, i3 - 1, i4) == Material.water ? true : (world1.GetMaterial(i2, i3 - 1, i4 - 1) == Material.water ? true : world1.GetMaterial(i2, i3 - 1, i4 + 1) == Material.water))));
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            this.CheckBlockCoordValid(world1, i2, i3, i4);
        }

        protected void CheckBlockCoordValid(Level world1, int i2, int i3, int i4)
        {
            if (!this.CanBlockStay(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            return this.CanPlaceBlockAt(world1, i2, i3, i4);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.reed.id;
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
            return RenderShape.CROSS;
        }
    }
}