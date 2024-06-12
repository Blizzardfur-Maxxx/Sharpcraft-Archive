using SharpCraft.Core.NBT;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Cow : Animal
    {
        public Cow(Level world1) : base(world1)
        {
            this.texture = "/mob/cow.png";
            this.SetSize(0.9F, 1.3F);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
        }

        protected override string GetLivingSound()
        {
            return "mob.cow";
        }

        protected override string GetHurtSound()
        {
            return "mob.cowhurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.cowhurt";
        }

        protected override float GetSoundVolume()
        {
            return 0.4F;
        }

        protected override int GetDropItemId()
        {
            return Item.leather.id;
        }

        public override bool Interact(Player entityPlayer1)
        {
            ItemInstance itemStack2 = entityPlayer1.inventory.GetCurrentItem();
            if (itemStack2 != null && itemStack2.itemID == Item.bucketEmpty.id)
            {
                entityPlayer1.inventory.SetItem(entityPlayer1.inventory.currentItem, new ItemInstance(Item.bucketMilk));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}