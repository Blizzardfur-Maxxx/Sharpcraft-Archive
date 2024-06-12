using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class Bush : Tile
    {
        public Bush(int i1, int i2) : base(i1, Material.plant)
        {
            this.texture = i2;
            this.SetTicking(true);
            float f3 = 0.2F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, f3 * 3F, 0.5F + f3);
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return base.CanPlaceBlockAt(world1, i2, i3, i4) && this.CanThisPlantGrowOnThisBlockID(world1.GetTile(i2, i3 - 1, i4));
        }

        protected virtual bool CanThisPlantGrowOnThisBlockID(int i1)
        {
            return i1 == Tile.grass.id || i1 == Tile.dirt.id || i1 == Tile.farmland.id;
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            base.NeighborChanged(world1, i2, i3, i4, i5);
            this.Fun_276(world1, i2, i3, i4);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            this.Fun_276(world1, i2, i3, i4);
        }

        protected void Fun_276(Level world1, int i2, int i3, int i4)
        {
            if (!this.CanBlockStay(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            return (world1.IsSkyLit(i2, i3, i4) >= 8 || world1.CanCockSeeTheSky(i2, i3, i4)) && this.CanThisPlantGrowOnThisBlockID(world1.GetTile(i2, i3 - 1, i4));
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
            return RenderShape.CROSS;
        }
    }
}