using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ClayTile : Tile
    {
        public ClayTile(int i1, int i2) : base(i1, i2, Material.clay)
        {
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.clay.id;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 4;
        }
    }
}