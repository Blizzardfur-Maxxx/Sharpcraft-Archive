using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class GlassTile : TransparentTile
    {
        public GlassTile(int i1, int i2, Material material3, bool z4) : base(i1, i2, material3, z4)
        {
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override RenderLayer GetRenderLayer()
        {
            return RenderLayer.RENDERLAYER_OPAQUE;
        }
    }
}