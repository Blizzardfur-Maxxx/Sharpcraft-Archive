using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class Slot
    {
        private readonly int slotIndex;
        private readonly IContainer inventory;
        public int id;
        public int xDisplayPosition;
        public int yDisplayPosition;
        public Slot(IContainer iInventory1, int i2, int i3, int i4)
        {
            this.inventory = iInventory1;
            this.slotIndex = i2;
            this.xDisplayPosition = i3;
            this.yDisplayPosition = i4;
        }

        public virtual void OnPickupFromSlot(ItemInstance itemStack1)
        {
            this.OnSlotChanged();
        }

        public virtual bool IsItemValid(ItemInstance itemStack1)
        {
            return true;
        }

        public virtual ItemInstance GetItem()
        {
            return this.inventory.GetItem(this.slotIndex);
        }

        public virtual bool HasItem()
        {
            return this.GetItem() != null;
        }

        public virtual void PutItem(ItemInstance itemStack1)
        {
            this.inventory.SetItem(this.slotIndex, itemStack1);
            this.OnSlotChanged();
        }

        public virtual void OnSlotChanged()
        {
            this.inventory.SetChanged();
        }

        public virtual int GetSlotStackLimit()
        {
            return this.inventory.GetMaxStackSize();
        }

        public virtual int GetBackgroundIconIndex()
        {
            return -1;
        }

        public virtual ItemInstance DecrStackSize(int i1)
        {
            return this.inventory.RemoveItem(this.slotIndex, i1);
        }

        public virtual bool IsHere(IContainer iInventory1, int i2)
        {
            return iInventory1 == this.inventory && i2 == this.slotIndex;
        }
    }
}