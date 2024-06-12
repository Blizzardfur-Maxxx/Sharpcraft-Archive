using SharpCraft.Core.NBT;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class PigZombie : Zombie
    {
        private int angerLevel = 0;
        private int randomSoundDelay = 0;
        private static readonly ItemInstance defaultHeldItem = new ItemInstance(Item.swordGold, 1);
        public PigZombie(Level world1) : base(world1)
        {
            this.texture = "/mob/pigzombie.png";
            this.moveSpeed = 0.5F;
            this.attackStrength = 5;
            this.isImmuneToFire = true;
        }

        public override void OnUpdate()
        {
            this.moveSpeed = this.playerToAttack != null ? 0.95F : 0.5F;
            if (this.randomSoundDelay > 0 && --this.randomSoundDelay == 0)
            {
                this.worldObj.PlaySound(this, "mob.zombiepig.zpigangry", this.GetSoundVolume() * 2F, ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F) * 1.8F);
            }

            base.OnUpdate();
        }

        public override bool GetCanSpawnHere()
        {
            return this.worldObj.difficultySetting > 0 && this.worldObj.CheckIfAABBIsClear(this.boundingBox) && this.worldObj.GetCubes(this, this.boundingBox).Count == 0 && !this.worldObj.ContainsAnyLiquid(this.boundingBox);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetShort("Anger", (short)this.angerLevel);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.angerLevel = nBTTagCompound1.GetShort("Anger");
        }

        protected override Entity FindPlayerToAttack()
        {
            return this.angerLevel == 0 ? null : base.FindPlayerToAttack();
        }

        public override void OnLivingUpdate()
        {
            base.OnLivingUpdate();
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (entity1 is Player)
            {
                IList<Entity> list3 = this.worldObj.GetEntities(this, this.boundingBox.Expand(32, 32, 32));
                for (int i4 = 0; i4 < list3.Count; ++i4)
                {
                    Entity entity5 = list3[i4];
                    if (entity5 is PigZombie)
                    {
                        PigZombie entityPigZombie6 = (PigZombie)entity5;
                        entityPigZombie6.BecomeAngryAt(entity1);
                    }
                }

                this.BecomeAngryAt(entity1);
            }

            return base.AttackEntityFrom(entity1, i2);
        }

        private void BecomeAngryAt(Entity entity1)
        {
            this.playerToAttack = entity1;
            this.angerLevel = 400 + this.rand.NextInt(400);
            this.randomSoundDelay = this.rand.NextInt(40);
        }

        protected override string GetLivingSound()
        {
            return "mob.zombiepig.zpig";
        }

        protected override string GetHurtSound()
        {
            return "mob.zombiepig.zpighurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.zombiepig.zpigdeath";
        }

        protected override int GetDropItemId()
        {
            return Item.porkCooked.id;
        }

        public override ItemInstance GetHeldItem()
        {
            return defaultHeldItem;
        }
    }
}