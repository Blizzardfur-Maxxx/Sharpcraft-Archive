using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class RecipesIngots
    {
        private object[][] recipeItems = new[]
        {
            new[]
            {
                (object)Tile.goldBlock,
                new ItemInstance(Item.ingotGold, 9)
            },
            new[]
            {
                (object)Tile.ironBlock,
                new ItemInstance(Item.ingotIron, 9)
            },
            new[]
            {
                (object)Tile.blockDiamond,
                new ItemInstance(Item.diamond, 9)
            },
            new[]
            {
                (object)Tile.lapisBlock,
                new ItemInstance(Item.dyePowder, 9, 4)
            }
        };
        public virtual void AddRecipes(Recipes craftingManager1)
        {
            for (int i2 = 0; i2 < this.recipeItems.Length; ++i2)
            {
                Tile block3 = (Tile)this.recipeItems[i2][0];
                ItemInstance itemStack4 = (ItemInstance)this.recipeItems[i2][1];
                craftingManager1.AddRecipe(new ItemInstance(block3), "###", "###", "###", '#', itemStack4);
                craftingManager1.AddRecipe(itemStack4, "#", '#', block3);
            }
        }
    }
}