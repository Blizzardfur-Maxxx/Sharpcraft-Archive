using SharpCraft.Core.World.Inventory;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class ShapedRecipe : IRecipe
    {
        private int width;
        private int height;
        private ItemInstance[] items;
        private ItemInstance output;
        public readonly int outputId;
        public ShapedRecipe(int i1, int i2, ItemInstance[] itemStack3, ItemInstance itemStack4)
        {
            this.outputId = itemStack4.itemID;
            this.width = i1;
            this.height = i2;
            this.items = itemStack3;
            this.output = itemStack4;
        }

        public virtual ItemInstance GetRecipeOutput()
        {
            return this.output;
        }

        public virtual bool Matches(CraftingContainer inventoryCrafting1)
        {
            for (int i2 = 0; i2 <= 3 - this.width; ++i2)
            {
                for (int i3 = 0; i3 <= 3 - this.height; ++i3)
                {
                    if (this.RecipeMatches(inventoryCrafting1, i2, i3, true))
                    {
                        return true;
                    }

                    if (this.RecipeMatches(inventoryCrafting1, i2, i3, false))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool RecipeMatches(CraftingContainer inventoryCrafting1, int i2, int i3, bool z4)
        {
            for (int i5 = 0; i5 < 3; ++i5)
            {
                for (int i6 = 0; i6 < 3; ++i6)
                {
                    int i7 = i5 - i2;
                    int i8 = i6 - i3;
                    ItemInstance itemStack9 = null;
                    if (i7 >= 0 && i8 >= 0 && i7 < this.width && i8 < this.height)
                    {
                        if (z4)
                        {
                            itemStack9 = this.items[this.width - i7 - 1 + i8 * this.width];
                        }
                        else
                        {
                            itemStack9 = this.items[i7 + i8 * this.width];
                        }
                    }

                    ItemInstance itemStack10 = inventoryCrafting1.GetItemAt(i5, i6);
                    if (itemStack10 != null || itemStack9 != null)
                    {
                        if (itemStack10 == null && itemStack9 != null || itemStack10 != null && itemStack9 == null)
                        {
                            return false;
                        }

                        if (itemStack9.itemID != itemStack10.itemID)
                        {
                            return false;
                        }

                        if (itemStack9.GetItemDamage() != -1 && itemStack9.GetItemDamage() != itemStack10.GetItemDamage())
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public virtual ItemInstance GetCraftingResult(CraftingContainer inventoryCrafting1)
        {
            return new ItemInstance(this.output.itemID, this.output.stackSize, this.output.GetItemDamage());
        }

        public virtual int GetRecipeSize()
        {
            return this.width * this.height;
        }
    }
}