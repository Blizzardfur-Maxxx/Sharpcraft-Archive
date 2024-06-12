using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class FenceTile : Tile
    {
        public FenceTile(int i1, int i2) : base(i1, i2, Material.wood)
        {
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.GetTile(i2, i3 - 1, i4) == this.id ? true : (!world1.GetMaterial(i2, i3 - 1, i4).IsSolid() ? false : base.CanPlaceBlockAt(world1, i2, i3, i4));
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return AABB.Of(i2, i3, i4, i2 + 1, i3 + 1.5F, i4 + 1);
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
            return RenderShape.FENCE;
        }
    }
}