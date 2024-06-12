using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class StructureRecipes
    {
        public virtual void AddRecipes(Recipes craftingManager1)
        {
            craftingManager1.AddRecipe(new ItemInstance(Tile.chest), "###", "# #", "###", '#', Tile.wood);
            craftingManager1.AddRecipe(new ItemInstance(Tile.furnace), "###", "# #", "###", '#', Tile.stoneBrick);
            craftingManager1.AddRecipe(new ItemInstance(Tile.workBench), "##", "##", '#', Tile.wood);
            craftingManager1.AddRecipe(new ItemInstance(Tile.sandStone), "##", "##", '#', Tile.sand);
        }
    }
}