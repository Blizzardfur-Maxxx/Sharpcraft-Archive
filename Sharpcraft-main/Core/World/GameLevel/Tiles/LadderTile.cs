using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LadderTile : Tile
    {
        public LadderTile(int i1, int i2) : base(i1, i2, Material.decoration)
        {
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            float f6 = 0.125F;
            if (i5 == 2)
            {
                this.SetShape(0F, 0F, 1F - f6, 1F, 1F, 1F);
            }

            if (i5 == 3)
            {
                this.SetShape(0F, 0F, 0F, 1F, 1F, f6);
            }

            if (i5 == 4)
            {
                this.SetShape(1F - f6, 0F, 0F, 1F, 1F, 1F);
            }

            if (i5 == 5)
            {
                this.SetShape(0F, 0F, 0F, f6, 1F, 1F);
            }

            return base.GetAABB(world1, i2, i3, i4);
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            float f6 = 0.125F;
            if (i5 == 2)
            {
                this.SetShape(0F, 0F, 1F - f6, 1F, 1F, 1F);
            }

            if (i5 == 3)
            {
                this.SetShape(0F, 0F, 0F, 1F, 1F, f6);
            }

            if (i5 == 4)
            {
                this.SetShape(1F - f6, 0F, 0F, 1F, 1F, 1F);
            }

            if (i5 == 5)
            {
                this.SetShape(0F, 0F, 0F, f6, 1F, 1F);
            }

            return base.GetTileAABB(world1, i2, i3, i4);
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
            return RenderShape.LADDER;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2 - 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : world1.IsSolidBlockingTile(i2, i3, i4 + 1)));
        }

        public override void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if ((i6 == 0 || i5 == TileFace.NORTH) && world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                i6 = 2;
            }

            if ((i6 == 0 || i5 == TileFace.SOUTH) && world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                i6 = 3;
            }

            if ((i6 == 0 || i5 == TileFace.WEST) && world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                i6 = 4;
            }

            if ((i6 == 0 || i5 == TileFace.EAST) && world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                i6 = 5;
            }

            world1.SetData(i2, i3, i4, i6);
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            bool z7 = false;
            if (i6 == 2 && world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                z7 = true;
            }

            if (i6 == 3 && world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                z7 = true;
            }

            if (i6 == 4 && world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                z7 = true;
            }

            if (i6 == 5 && world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                z7 = true;
            }

            if (!z7)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, i6);
                world1.SetTile(i2, i3, i4, 0);
            }

            base.NeighborChanged(world1, i2, i3, i4, i5);
        }

        public override int ResourceCount(JRandom random1)
        {
            return 1;
        }
    }
}