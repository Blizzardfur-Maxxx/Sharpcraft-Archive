using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Spider : Monster
    {
        public Spider(Level world1) : base(world1)
        {
            this.texture = "/mob/spider.png";
            this.SetSize(1.4F, 0.9F);
            this.moveSpeed = 0.8F;
        }

        public override double GetMountedYOffset()
        {
            return this.height * 0.75 - 0.5;
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        protected override Entity FindPlayerToAttack()
        {
            float f1 = this.GetEntityBrightness(1F);
            if (f1 < 0.5F)
            {
                double d2 = 16;
                return this.worldObj.GetClosestPlayerToEntity(this, d2);
            }
            else
            {
                return null;
            }
        }

        protected override string GetLivingSound()
        {
            return "mob.spider";
        }

        protected override string GetHurtSound()
        {
            return "mob.spider";
        }

        protected override string GetDeathSound()
        {
            return "mob.spiderdeath";
        }

        protected override void AttackEntity(Entity entity1, float f2)
        {
            float f3 = this.GetEntityBrightness(1F);
            if (f3 > 0.5F && this.rand.NextInt(100) == 0)
            {
                this.playerToAttack = null;
            }
            else
            {
                if (f2 > 2F && f2 < 6F && this.rand.NextInt(10) == 0)
                {
                    if (this.onGround)
                    {
                        double d4 = entity1.x - this.x;
                        double d6 = entity1.z - this.z;
                        float f8 = Mth.Sqrt(d4 * d4 + d6 * d6);
                        this.motionX = d4 / f8 * 0.5 * 0.8F + this.motionX * 0.2F;
                        this.motionZ = d6 / f8 * 0.5 * 0.8F + this.motionZ * 0.2F;
                        this.motionY = 0.4F;
                    }
                }
                else
                {
                    base.AttackEntity(entity1, f2);
                }
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
            return Item.silk.id;
        }

        public override bool IsOnLadder()
        {
            return this.isCollidedHorizontally;
        }
    }
}