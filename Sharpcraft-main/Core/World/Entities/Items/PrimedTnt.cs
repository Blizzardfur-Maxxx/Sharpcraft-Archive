using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Entities.Items
{
    public class PrimedTnt : Entity
    {
        public int fuse;
        public PrimedTnt(Level world1) : base(world1)
        {
            this.fuse = 0;
            this.preventEntitySpawning = true;
            this.SetSize(0.98F, 0.98F);
            this.yOffset = this.height / 2F;
        }

        public PrimedTnt(Level world1, double d2, double d4, double d6) : this(world1)
        {
            this.SetPosition(d2, d4, d6);
            float f8 = (float)(Mth.Random() * Mth.PI * 2);
            this.motionX = -Mth.Sin(f8 * Mth.PI / 180F) * 0.02F;
            this.motionY = 0.2F;
            this.motionZ = -Mth.Cos(f8 * Mth.PI / 180F) * 0.02F;
            this.fuse = 80;
            this.prevX = d2;
            this.prevY = d4;
            this.prevZ = d6;
        }

        protected override void EntityInit()
        {
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        public override bool CanBeCollidedWith()
        {
            return !this.isDead;
        }

        public override void OnUpdate()
        {
            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            this.motionY -= 0.04F;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.98F;
            this.motionY *= 0.98F;
            this.motionZ *= 0.98F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
                this.motionY *= -0.5;
            }

            if (this.fuse-- <= 0)
            {
                if (!this.worldObj.isRemote)
                {
                    this.SetEntityDead();
                    this.Explode();
                }
                else
                {
                    this.SetEntityDead();
                }
            }
            else
            {
                this.worldObj.AddParticle("smoke", this.x, this.y + 0.5, this.z, 0, 0, 0);
            }
        }

        private void Explode()
        {
            float f1 = 4F;
            this.worldObj.Explode((Entity)null, this.x, this.y, this.z, f1);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetByte("Fuse", (byte)this.fuse);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.fuse = nBTTagCompound1.GetByte("Fuse");
        }

        public override float GetShadowSize()
        {
            return 0F;
        }
    }
}