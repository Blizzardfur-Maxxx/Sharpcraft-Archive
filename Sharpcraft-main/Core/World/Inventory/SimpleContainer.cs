using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Inventory
{
    public class SimpleContainer : IContainer
    {
        private string name;
        private int slotCount;
        private ItemInstance[] items;

        public SimpleContainer(string name, int slotCount)
        {
            this.name = name;
            this.slotCount = slotCount;
            this.items = new ItemInstance[slotCount];
        }

        public virtual ItemInstance GetItem(int i1)
        {
            return this.items[i1];
        }

        public virtual ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.items[i1] != null)
            {
                ItemInstance itemStack3;
                if (this.items[i1].stackSize <= i2)
                {
                    itemStack3 = this.items[i1];
                    this.items[i1] = null;
                    this.SetChanged();
                    return itemStack3;
                }
                else
                {
                    itemStack3 = this.items[i1].SplitStack(i2);
                    if (this.items[i1].stackSize == 0)
                    {
                        this.items[i1] = null;
                    }

                    this.SetChanged();
                    return itemStack3;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual void SetItem(int i1, ItemInstance itemStack2)
        {
            this.items[i1] = itemStack2;
            if (itemStack2 != null && itemStack2.stackSize > this.GetMaxStackSize())
            {
                itemStack2.stackSize = this.GetMaxStackSize();
            }

            this.SetChanged();
        }

        public virtual int GetContainerSize()
        {
            return this.slotCount;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        public virtual int GetMaxStackSize()
        {
            return 64;
        }

        public virtual void SetChanged()
        {
        }

        public virtual bool StillValid(Player entityPlayer1)
        {
            return true;
        }
    }
}