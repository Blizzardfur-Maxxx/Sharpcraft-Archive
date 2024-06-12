using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class ArmorRecipes
    {
        private String[][] recipePatterns = new[]
        {
            new[]
            {
                "XXX",
                "X X"
            },
            new[]
            {
                "X X",
                "XXX",
                "XXX"
            },
            new[]
            {
                "XXX",
                "X X",
                "X X"
            },
            new[]
            {
                "X X",
                "X X"
            }
        };
        private object[][] recipeItems = new[]
        {
            new[]
            {
                Item.leather,
                (object)Tile.fire,
                Item.ingotIron,
                Item.diamond,
                Item.ingotGold
            },
            new[]
            {
                Item.helmetLeather,
                Item.helmetChain,
                Item.helmetSteel,
                Item.helmetDiamond,
                Item.helmetGold
            },
            new[]
            {
                Item.plateLeather,
                Item.plateChain,
                Item.plateSteel,
                Item.plateDiamond,
                Item.plateGold
            },
            new[]
            {
                Item.legsLeather,
                Item.legsChain,
                Item.legsSteel,
                Item.legsDiamond,
                Item.legsGold
            },
            new[]
            {
                Item.bootsLeather,
                Item.bootsChain,
                Item.bootsSteel,
                Item.bootsDiamond,
                Item.bootsGold
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
                    craftingManager1.AddRecipe(new ItemInstance(item5), this.recipePatterns[i4], 'X', object3);
                }
            }
        }
    }
}