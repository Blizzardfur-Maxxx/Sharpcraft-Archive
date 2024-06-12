using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class RecipesFood
    {
        public virtual void AddRecipes(Recipes craftingManager1)
        {
            craftingManager1.AddRecipe(new ItemInstance(Item.bowlSoup), "Y", "X", "#", 'X', Tile.mushroom1, 'Y', Tile.mushroom2, '#', Item.bowlEmpty);
            craftingManager1.AddRecipe(new ItemInstance(Item.bowlSoup), "Y", "X", "#", 'X', Tile.mushroom2, 'Y', Tile.mushroom1, '#', Item.bowlEmpty);
            craftingManager1.AddRecipe(new ItemInstance(Item.cookie, 8), "#X#", 'X', new ItemInstance(Item.dyePowder, 1, 3), '#', Item.wheat);
        }
    }
}