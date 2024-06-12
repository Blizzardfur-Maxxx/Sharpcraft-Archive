using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class RecipesWeapons
    {
        private String[][] recipePatterns = new[]
        {
            new[]
            {
                "X",
                "X",
                "#"
            }
        };
        private Object[][] recipeItems = new[]
        {
            new[]
            {
                (object)Tile.wood,
                (object)Tile.stoneBrick,
                Item.ingotIron,
                Item.diamond,
                Item.ingotGold
            },
            new[]
            {
                Item.swordWood,
                Item.swordStone,
                Item.swordSteel,
                Item.swordDiamond,
                Item.swordGold
            }
        };
        public virtual void AddRecipes(Recipes craftingManager1)
        {
            for (int i2 = 0; i2 < this.recipeItems[0].Length; ++i2)
            {
                object object3 = this.recipeItems[0][i2];
                for (int i4 = 0; i4 < this.recipeItems.Length - 1; ++i4)
                {
                    Item item5 = (Item)this.recipeItems[i4 + 1][i2];
                    craftingManager1.AddRecipe(new ItemInstance(item5), this.recipePatterns[i4], '#', Item.stick, 'X', object3);
                }
            }

            craftingManager1.AddRecipe(new ItemInstance(Item.bow, 1), " #X", "# X", " #X", 'X', Item.silk, '#', Item.stick);
            craftingManager1.AddRecipe(new ItemInstance(Item.arrow, 4), "X", "#", "Y", 'Y', Item.feather, 'X', Item.flint, '#', Item.stick);
        }
    }
}