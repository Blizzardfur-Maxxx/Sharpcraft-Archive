using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class MetalTile : Tile
    {
        public MetalTile(int i1, int i2) : base(i1, Material.metal)
        {
            this.texture = i2;
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.texture;
        }
    }
}