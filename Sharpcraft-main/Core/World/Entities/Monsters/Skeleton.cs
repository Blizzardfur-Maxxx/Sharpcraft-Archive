using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using System;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Skeleton : Monster
    {
        private static readonly ItemInstance defaultHeldItem = new ItemInstance(Item.bow, 1);
        public Skeleton(Level world1) : base(world1)
        {
            this.texture = "/mob/skeleton.png";
        }

        protected override string GetLivingSound()
        {
            return "mob.skeleton";
        }

        protected override string GetHurtSound()
        {
            return "mob.skeletonhurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.skeletonhurt";
        }

        public override void OnLivingUpdate()
        {
            if (this.worldObj.IsDaytime())
            {
                float f1 = this.GetEntityBrightness(1F);
                if (f1 > 0.5F && this.worldObj.CanCockSeeTheSky(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)) && this.rand.NextFloat() * 30F < (f1 - 0.4F) * 2F)
                {
                    this.fire = 300;
                }
            }

            base.OnLivingUpdate();
        }

        protected override void AttackEntity(Entity entity1, float f2)
        {
            if (f2 < 10F)
            {
                double d3 = entity1.x - this.x;
                double d5 = entity1.z - this.z;
                if (this.attackTime == 0)
                {
                    Arrow entityArrow7 = new Arrow(this.worldObj, this);
                    ++entityArrow7.y;
                    double d8 = entity1.y + entity1.GetEyeHeight() - 0.2F - entityArrow7.y;
                    float f10 = Mth.Sqrt(d3 * d3 + d5 * d5) * 0.2F;
                    this.worldObj.PlaySound(this, "random.bow", 1F, 1F / (this.rand.NextFloat() * 0.4F + 0.8F));
                    this.worldObj.AddEntity(entityArrow7);
                    entityArrow7.Shoot(d3, d8 + f10, d5, 0.6F, 12F);
                    this.attackTime = 30;
                }

                this.yaw = (float)(Math.Atan2(d5, d3) * 180 / Mth.PI) - 90F;
                this.hasAttacked = true;
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
        }

        protected override int GetDropItemId()
        {
            return Item.arrow.id;
        }

        protected override void DropFewItems()
        {
            int i1 = this.rand.NextInt(3);
            int i2;
            for (i2 = 0; i2 < i1; ++i2)
            {
                this.DropItem(Item.arrow.id, 1);
            }

            i1 = this.rand.NextInt(3);
            for (i2 = 0; i2 < i1; ++i2)
            {
                this.DropItem(Item.bone.id, 1);
            }
        }

        public override ItemInstance GetHeldItem()
        {
            return defaultHeldItem;
        }
    }
}