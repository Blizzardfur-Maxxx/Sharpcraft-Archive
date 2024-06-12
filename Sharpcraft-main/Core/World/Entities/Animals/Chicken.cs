using SharpCraft.Core.NBT;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Chicken : Animal
    {
        public bool field_753a = false;
        public float field_752b = 0F;
        public float destPos = 0F;
        public float field_757d;
        public float field_756e;
        public float field_755h = 1F;
        public int timeUntilNextEgg;
        public Chicken(Level world1) : base(world1)
        {
            this.texture = "/mob/chicken.png";
            this.SetSize(0.3F, 0.4F);
            this.health = 4;
            this.timeUntilNextEgg = this.rand.NextInt(6000) + 6000;
        }

        public override void OnLivingUpdate()
        {
            base.OnLivingUpdate();
            this.field_756e = this.field_752b;
            this.field_757d = this.destPos;
            this.destPos = (float)(this.destPos + (this.onGround ? -1 : 4) * 0.3);
            if (this.destPos < 0F)
            {
                this.destPos = 0F;
            }

            if (this.destPos > 1F)
            {
                this.destPos = 1F;
            }

            if (!this.onGround && this.field_755h < 1F)
            {
                this.field_755h = 1F;
            }

            this.field_755h = (float)(this.field_755h * 0.9);
            if (!this.onGround && this.motionY < 0)
            {
                this.motionY *= 0.6;
            }

            this.field_752b += this.field_755h * 2F;
            if (!this.worldObj.isRemote && --this.timeUntilNextEgg <= 0)
            {
                this.worldObj.PlaySound(this, "mob.chickenplop", 1F, (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                this.DropItem(Item.egg.id, 1);
                this.timeUntilNextEgg = this.rand.NextInt(6000) + 6000;
            }
        }

        protected override void Fall(float f1)
        {
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
            return "mob.chicken";
        }

        protected override string GetHurtSound()
        {
            return "mob.chickenhurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.chickenhurt";
        }

        protected override int GetDropItemId()
        {
            return Item.feather.id;
        }
    }
}