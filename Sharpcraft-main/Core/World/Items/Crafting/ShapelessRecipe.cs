using SharpCraft.Core.World.Inventory;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class ShapelessRecipe : IRecipe
    {
        private readonly ItemInstance recipeOutput;
        private readonly IList<ItemInstance> recipeItems;
        public ShapelessRecipe(ItemInstance itemStack1, IList<ItemInstance> list2)
        {
            this.recipeOutput = itemStack1;
            this.recipeItems = list2;
        }

        public virtual ItemInstance GetRecipeOutput()
        {
            return this.recipeOutput;
        }

        public virtual bool Matches(CraftingContainer inventoryCrafting1)
        {
            List<ItemInstance> arrayList2 = new List<ItemInstance>(this.recipeItems);
            for (int i3 = 0; i3 < 3; ++i3)
            {
                for (int i4 = 0; i4 < 3; ++i4)
                {
                    ItemInstance itemStack5 = inventoryCrafting1.GetItemAt(i4, i3);
                    if (itemStack5 != null)
                    {
                        bool z6 = false;
                        foreach (ItemInstance itemStack8 in arrayList2)
                        {
                            if (itemStack5.itemID == itemStack8.itemID && (itemStack8.GetItemDamage() == -1 || itemStack5.GetItemDamage() == itemStack8.GetItemDamage()))
                            {
                                z6 = true;
                                arrayList2.Remove(itemStack8);
                                break;
                            }
                        }

                        if (!z6)
                        {
                            return false;
                        }
                    }
                }
            }

            return arrayList2.Count == 0;
        }

        public virtual ItemInstance GetCraftingResult(CraftingContainer inventoryCrafting1)
        {
            return this.recipeOutput.Copy();
        }

        public virtual int GetRecipeSize()
        {
            return this.recipeItems.Count;
        }
    }
}