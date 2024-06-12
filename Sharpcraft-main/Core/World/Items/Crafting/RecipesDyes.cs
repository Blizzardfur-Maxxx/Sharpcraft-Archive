using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class RecipesDyes
    {
        public virtual void AddRecipes(Recipes craftingManager1)
        {
            for (int i2 = 0; i2 < 16; ++i2)
            {
                craftingManager1.AddShapelessRecipe(new ItemInstance(Tile.cloth, 1, ClothTile.GetMetadataColor1(i2)), new ItemInstance(Item.dyePowder, 1, i2), new ItemInstance(Item.items[Tile.cloth.id], 1, 0));
            }

            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 11), Tile.flower);
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 1), Tile.rose);
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 3, 15), Item.bone);
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 9), new ItemInstance(Item.dyePowder, 1, 1), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 14), new ItemInstance(Item.dyePowder, 1, 1), new ItemInstance(Item.dyePowder, 1, 11));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 10), new ItemInstance(Item.dyePowder, 1, 2), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 8), new ItemInstance(Item.dyePowder, 1, 0), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 7), new ItemInstance(Item.dyePowder, 1, 8), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 3, 7), new ItemInstance(Item.dyePowder, 1, 0), new ItemInstance(Item.dyePowder, 1, 15), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 12), new ItemInstance(Item.dyePowder, 1, 4), new ItemInstance(Item.dyePowder, 1, 15));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 6), new ItemInstance(Item.dyePowder, 1, 4), new ItemInstance(Item.dyePowder, 1, 2));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 5), new ItemInstance(Item.dyePowder, 1, 4), new ItemInstance(Item.dyePowder, 1, 1));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 2, 13), new ItemInstance(Item.dyePowder, 1, 5), new ItemInstance(Item.dyePowder, 1, 9));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 3, 13), new ItemInstance(Item.dyePowder, 1, 4), new ItemInstance(Item.dyePowder, 1, 1), new ItemInstance(Item.dyePowder, 1, 9));
            craftingManager1.AddShapelessRecipe(new ItemInstance(Item.dyePowder, 4, 13), new ItemInstance(Item.dyePowder, 1, 4), new ItemInstance(Item.dyePowder, 1, 1), new ItemInstance(Item.dyePowder, 1, 1), new ItemInstance(Item.dyePowder, 1, 15));
        }
    }
}