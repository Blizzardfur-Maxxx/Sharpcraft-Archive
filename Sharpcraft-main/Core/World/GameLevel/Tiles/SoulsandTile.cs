using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SoulsandTile : Tile
    {
        public SoulsandTile(int i1, int i2) : base(i1, i2, Material.sand)
        {
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            float f5 = 0.125F;
            return AABB.Of(i2, i3, i4, i2 + 1, i3 + 1 - f5, i4 + 1);
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            entity5.motionX *= 0.4;
            entity5.motionZ *= 0.4;
        }
    }
}