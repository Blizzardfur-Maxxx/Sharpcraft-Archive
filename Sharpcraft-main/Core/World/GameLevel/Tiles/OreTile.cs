using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class OreTile : Tile
    {
        public OreTile(int i1, int i2) : base(i1, i2, Material.stone)
        {
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return this.id == Tile.coalOre.id ? Item.coal.id : (this.id == Tile.oreDiamond.id ? Item.diamond.id : (this.id == Tile.lapisOre.id ? Item.dyePowder.id : this.id));
        }

        public override int ResourceCount(JRandom random1)
        {
            return this.id == Tile.lapisOre.id ? 4 + random1.NextInt(5) : 1;
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return this.id == Tile.lapisOre.id ? 4 : 0;
        }
    }
}