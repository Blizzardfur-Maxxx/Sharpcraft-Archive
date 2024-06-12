using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class ResultContainer : IContainer
    {
        private ItemInstance[] stackResult = new ItemInstance[1];
        public virtual int GetContainerSize()
        {
            return 1;
        }

        public virtual ItemInstance GetItem(int i1)
        {
            return this.stackResult[i1];
        }

        public virtual string GetName()
        {
            return "Result";
        }

        public virtual ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.stackResult[i1] != null)
            {
                ItemInstance itemStack3 = this.stackResult[i1];
                this.stackResult[i1] = null;
                return itemStack3;
            }
            else
            {
                return null;
            }
        }

        public virtual void SetItem(int i1, ItemInstance itemStack2)
        {
            this.stackResult[i1] = itemStack2;
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