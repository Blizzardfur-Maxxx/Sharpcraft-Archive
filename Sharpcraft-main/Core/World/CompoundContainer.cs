

using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World
{
    public class CompoundContainer : IContainer
    {
        private string name;
        private IContainer upperChest;
        private IContainer lowerChest;
        public CompoundContainer(string string1, IContainer iInventory2, IContainer iInventory3)
        {
            this.name = string1;
            this.upperChest = iInventory2;
            this.lowerChest = iInventory3;
        }

        public virtual int GetContainerSize()
        {
            return this.upperChest.GetContainerSize() + this.lowerChest.GetContainerSize();
        }

        public virtual string GetName()
        {
            return this.name;
        }

        public virtual ItemInstance GetItem(int i1)
        {
            return i1 >= this.upperChest.GetContainerSize() ? this.lowerChest.GetItem(i1 - this.upperChest.GetContainerSize()) : this.upperChest.GetItem(i1);
        }

        public virtual ItemInstance RemoveItem(int i1, int i2)
        {
            return i1 >= this.upperChest.GetContainerSize() ? this.lowerChest.RemoveItem(i1 - this.upperChest.GetContainerSize(), i2) : this.upperChest.RemoveItem(i1, i2);
        }

        public virtual void SetItem(int i1, ItemInstance itemStack2)
        {
            if (i1 >= this.upperChest.GetContainerSize())
            {
                this.lowerChest.SetItem(i1 - this.upperChest.GetContainerSize(), itemStack2);
            }
            else
            {
                this.upperChest.SetItem(i1, itemStack2);
            }
        }

        public virtual int GetMaxStackSize()
        {
            return this.upperChest.GetMaxStackSize();
        }

        public virtual void SetChanged()
        {
            this.upperChest.SetChanged();
            this.lowerChest.SetChanged();
        }

        public virtual bool StillValid(Player entityPlayer1)
        {
            return this.upperChest.StillValid(entityPlayer1) && this.lowerChest.StillValid(entityPlayer1);
        }
    }
}