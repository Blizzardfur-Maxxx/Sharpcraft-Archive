using SharpCraft.Core.World.Inventory;

namespace SharpCraft.Core.World.Items.Crafting
{
    public interface IRecipe
    {
        bool Matches(CraftingContainer inventoryCrafting1);
        ItemInstance GetCraftingResult(CraftingContainer inventoryCrafting1);
        int GetRecipeSize();
        ItemInstance GetRecipeOutput();
    }
}