using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class WebTile : Tile
    {
        public WebTile(int i1, int i2) : base(i1, i2, Material.web)
        {
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            entity5.isInWeb = true;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.CROSS;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.silk.id;
        }
    }
}