using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemFood : Item
    {
        private int healAmount;
        private bool isWolfsFavoriteMeat;
        public ItemFood(int i1, int i2, bool z3) : base(i1)
        {
            this.healAmount = i2;
            this.isWolfsFavoriteMeat = z3;
            this.maxStackSize = 1;
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            --itemStack1.stackSize;
            entityPlayer3.Heal(this.healAmount);
            return itemStack1;
        }

        public virtual int GetHealAmount()
        {
            return this.healAmount;
        }

        public virtual bool IsWolfFavorite()
        {
            return this.isWolfsFavoriteMeat;
        }
    }
}