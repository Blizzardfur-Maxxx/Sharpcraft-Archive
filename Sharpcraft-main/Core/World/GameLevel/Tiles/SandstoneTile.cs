using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SandstoneTile : Tile
    {
        public SandstoneTile(int i1) : base(i1, 192, Material.stone)
        {
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture - 16 : (faceIdx == 0 ? this.texture + 16 : this.texture);
        }
    }
}