using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemSpade : ItemTool
    {
        private static Tile[] blocksEffectiveAgainst = new[]
        {
            Tile.grass,
            Tile.dirt,
            Tile.sand,
            Tile.gravel,
            Tile.topSnow,
            Tile.blockSnow,
            Tile.blockClay,
            Tile.farmland
        };
        public ItemSpade(int i1, Tier enumToolMaterial2) : base(i1, 1, enumToolMaterial2, blocksEffectiveAgainst)
        {
        }

        public override bool CanHarvestBlock(Tile block1)
        {
            return block1 == Tile.topSnow ? true : block1 == Tile.blockSnow;
        }
    }
}