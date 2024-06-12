using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LightGemTile : Tile
    {
        public LightGemTile(int i1, int i2, Material material3) : base(i1, i2, material3)
        {
        }

        public override int ResourceCount(JRandom random1)
        {
            return 2 + random1.NextInt(3);
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.lightStoneDust.id;
        }
    }
}