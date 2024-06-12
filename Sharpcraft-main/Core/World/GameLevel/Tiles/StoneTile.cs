using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class StoneTile : Tile
    {
        public StoneTile(int i1, int i2) : base(i1, i2, Material.stone)
        {
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.stoneBrick.id;
        }
    }
}