using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class ContainerMenu : AbstractContainerMenu
    {
        private IContainer parentContainer;
        private int size;
        public ContainerMenu(IContainer iInventory1, IContainer iInventory2)
        {
            this.parentContainer = iInventory2;
            this.size = iInventory2.GetContainerSize() / 9;
            int i3 = (this.size - 4) * 18;
            int i4;
            int i5;
            for (i4 = 0; i4 < this.size; ++i4)
            {
                for (i5 = 0; i5 < 9; ++i5)
                {
                    this.AddSlot(new Slot(iInventory2, i5 + i4 * 9, 8 + i5 * 18, 18 + i4 * 18));
                }
            }

            for (i4 = 0; i4 < 3; ++i4)
            {
                for (i5 = 0; i5 < 9; ++i5)
                {
                    this.AddSlot(new Slot(iInventory1, i5 + i4 * 9 + 9, 8 + i5 * 18, 103 + i4 * 18 + i3));
                }
            }

            for (i4 = 0; i4 < 9; ++i4)
            {
                this.AddSlot(new Slot(iInventory1, i4, 8 + i4 * 18, 161 + i3));
            }
        }

        public override bool StillValid(Player entityPlayer1)
        {
            return this.parentContainer.StillValid(entityPlayer1);
        }

        public override ItemInstance GetStackInSlot(int i1)
        {
            ItemInstance itemStack2 = null;
            Slot slot3 = this.slots[i1];
            if (slot3 != null && slot3.HasItem())
            {
                ItemInstance itemStack4 = slot3.GetItem();
                itemStack2 = itemStack4.Copy();
                if (i1 < this.size * 9)
                {
                    this.Func_28126(itemStack4, this.size * 9, this.slots.Count, true);
                }
                else
                {
                    this.Func_28126(itemStack4, 0, this.size * 9, false);
                }

                if (itemStack4.stackSize == 0)
                {
                    slot3.PutItem((ItemInstance)null);
                }
                else
                {
                    slot3.OnSlotChanged();
                }
            }

            return itemStack2;
        }
    }
}