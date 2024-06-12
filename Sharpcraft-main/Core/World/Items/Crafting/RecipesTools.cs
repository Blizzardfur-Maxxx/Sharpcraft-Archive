using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class RecipesTools
    {
        private String[][] recipePatterns = new[]
        {
            new[]
            {
                "XXX",
                " # ",
                " # "
            },
            new[]
            {
                "X",
                "#",
                "#"
            },
            new[]
            {
                "XX",
                "X#",
                " #"
            },
            new[]
            {
                "XX",
                " #",
                " #"
            }
        };
        private object[][] recipeItems = new[]
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
                Item.pickaxeWood,
                Item.pickaxeStone,
                Item.pickaxeSteel,
                Item.pickaxeDiamond,
                Item.pickaxeGold
            },
            new[]
            {
                Item.shovelWood,
                Item.shovelStone,
                Item.shovelSteel,
                Item.shovelDiamond,
                Item.shovelGold
            },
            new[]
            {
                Item.axeWood,
                Item.axeStone,
                Item.axeSteel,
                Item.axeDiamond,
                Item.axeGold
            },
            new[]
            {
                Item.hoeWood,
                Item.hoeStone,
                Item.hoeSteel,
                Item.hoeDiamond,
                Item.hoeGold
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

            craftingManager1.AddRecipe(new ItemInstance(Item.shears), " #", "# ", '#', Item.ingotIron);
        }
    }
}