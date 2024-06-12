using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class BookshelfTile : Tile
    {
        public BookshelfTile(int i1, int i2) : base(i1, i2, Material.wood)
        {
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx <= TileFace.UP ? 4 : this.texture;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }
    }
}