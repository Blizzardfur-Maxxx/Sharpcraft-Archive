using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Weather;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Pig : Animal
    {
        public Pig(Level world1) : base(world1)
        {
            this.texture = "/mob/pig.png";
            this.SetSize(0.9F, 0.9F);
        }

        protected override void EntityInit()
        {
            this.dataWatcher.AddObject(16, (sbyte)0);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetBoolean("Saddle", this.GetSaddled());
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.SetSaddled(nBTTagCompound1.GetBoolean("Saddle"));
        }

        protected override string GetLivingSound()
        {
            return "mob.pig";
        }

        protected override string GetHurtSound()
        {
            return "mob.pig";
        }

        protected override string GetDeathSound()
        {
            return "mob.pigdeath";
        }

        public override bool Interact(Player entityPlayer1)
        {
            if (!this.GetSaddled() || this.worldObj.isRemote || this.riddenByEntity != null && this.riddenByEntity != entityPlayer1)
            {
                return false;
            }
            else
            {
                entityPlayer1.MountEntity(this);
                return true;
            }
        }

        protected override int GetDropItemId()
        {
            return this.fire > 0 ? Item.porkCooked.id : Item.porkRaw.id;
        }

        public virtual bool GetSaddled()
        {
            return (this.dataWatcher.GetWatchableObjectByte(16) & 1) != 0;
        }

        public virtual void SetSaddled(bool z1)
        {
            if (z1)
            {
                this.dataWatcher.UpdateObject(16, (sbyte)1);
            }
            else
            {
                this.dataWatcher.UpdateObject(16, (sbyte)0);
            }
        }

        public override void OnStruckByLightning(LightningBolt entityLightningBolt1)
        {
            if (!this.worldObj.isRemote)
            {
                PigZombie entityPigZombie2 = new PigZombie(this.worldObj);
                entityPigZombie2.SetLocationAndAngles(this.x, this.y, this.z, this.yaw, this.pitch);
                this.worldObj.AddEntity(entityPigZombie2);
                this.SetEntityDead();
            }
        }

        protected override void Fall(float f1)
        {
            base.Fall(f1);
            if (f1 > 5F && this.riddenByEntity is Player)
            {
                ((Player)this.riddenByEntity).TriggerAchievement(AchievementList.flyPig);
            }
        }
    }
}