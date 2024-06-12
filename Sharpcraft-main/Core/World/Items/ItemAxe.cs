using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemAxe : ItemTool
    {
        private static Tile[] blocksEffectiveAgainst = new[]
        {
            Tile.wood,
            Tile.bookshelf,
            Tile.treeTrunk,
            Tile.chest
        };
        public ItemAxe(int i1, Tier enumToolMaterial2) : base(i1, 3, enumToolMaterial2, blocksEffectiveAgainst)
        {
        }
    }
}