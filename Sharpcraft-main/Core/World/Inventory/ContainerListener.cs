using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Inventory
{
    public interface IContainerListener
    {
        void UpdateCraftingInventory(AbstractContainerMenu container1, IList<ItemInstance> list2);
        void UpdateCraftingInventorySlot(AbstractContainerMenu container1, int i2, ItemInstance itemStack3);
        void UpdateCraftingInventoryInfo(AbstractContainerMenu container1, int i2, int i3);
    }
}