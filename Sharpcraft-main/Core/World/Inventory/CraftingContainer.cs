using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class CraftingContainer : IContainer
    {
        private ItemInstance[] stackList;
        private int field_21104_b;
        private AbstractContainerMenu eventHandler;
        public CraftingContainer(AbstractContainerMenu container1, int i2, int i3)
        {
            int i4 = i2 * i3;
            this.stackList = new ItemInstance[i4];
            this.eventHandler = container1;
            this.field_21104_b = i2;
        }

        public virtual int GetContainerSize()
        {
            return this.stackList.Length;
        }

        public virtual ItemInstance GetItem(int i1)
        {
            return i1 >= this.GetContainerSize() ? null : this.stackList[i1];
        }

        public virtual ItemInstance GetItemAt(int i1, int i2)
        {
            if (i1 >= 0 && i1 < this.field_21104_b)
            {
                int i3 = i1 + i2 * this.field_21104_b;
                return this.GetItem(i3);
            }
            else
            {
                return null;
            }
        }

        public virtual string GetName()
        {
            return "Crafting";
        }

        public virtual ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.stackList[i1] != null)
            {
                ItemInstance itemStack3;
                if (this.stackList[i1].stackSize <= i2)
                {
                    itemStack3 = this.stackList[i1];
                    this.stackList[i1] = null;
                    this.eventHandler.OnCraftMatrixChanged(this);
                    return itemStack3;
                }
                else
                {
                    itemStack3 = this.stackList[i1].SplitStack(i2);
                    if (this.stackList[i1].stackSize == 0)
                    {
                        this.stackList[i1] = null;
                    }

                    this.eventHandler.OnCraftMatrixChanged(this);
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
            this.stackList[i1] = itemStack2;
            this.eventHandler.OnCraftMatrixChanged(this);
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