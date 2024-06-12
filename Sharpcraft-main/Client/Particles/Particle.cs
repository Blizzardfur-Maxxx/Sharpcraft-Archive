using SharpCraft.Client.Renderer;
using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Particles
{
    public class Particle : Entity
    {
        protected int particleTextureIndex;
        protected float particleTextureJitterX;
        protected float particleTextureJitterY;
        protected int particleAge = 0;
        protected int particleMaxAge = 0;
        protected float particleScale;
        protected float particleGravity;
        protected float particleRed;
        protected float particleGreen;
        protected float particleBlue;
        public static double interpPosX;
        public static double interpPosY;
        public static double interpPosZ;
        public Particle(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1)
        {
            this.SetSize(0.2F, 0.2F);
            this.yOffset = this.height / 2F;
            this.SetPosition(d2, d4, d6);
            this.particleRed = this.particleGreen = this.particleBlue = 1F;
            this.motionX = d8 + (float)(Mth.Random() * 2 - 1) * 0.4F;
            this.motionY = d10 + (float)(Mth.Random() * 2 - 1) * 0.4F;
            this.motionZ = d12 + (float)(Mth.Random() * 2 - 1) * 0.4F;
            float f14 = (float)(Mth.Random() + Mth.Random() + 1) * 0.15F;
            float f15 = Mth.Sqrt(this.motionX * this.motionX + this.motionY * this.motionY + this.motionZ * this.motionZ);
            this.motionX = this.motionX / f15 * f14 * 0.4F;
            this.motionY = this.motionY / f15 * f14 * 0.4F + 0.1F;
            this.motionZ = this.motionZ / f15 * f14 * 0.4F;
            this.particleTextureJitterX = this.rand.NextFloat() * 3F;
            this.particleTextureJitterY = this.rand.NextFloat() * 3F;
            this.particleScale = (this.rand.NextFloat() * 0.5F + 0.5F) * 2F;
            this.particleMaxAge = (int)(4F / (this.rand.NextFloat() * 0.9F + 0.1F));
            this.particleAge = 0;
        }

        public virtual Particle Func_407_b(float f1)
        {
            this.motionX *= f1;
            this.motionY = (this.motionY - 0.1F) * f1 + 0.1F;
            this.motionZ *= f1;
            return this;
        }

        public virtual Particle Func_405_d(float f1)
        {
            this.SetSize(0.2F * f1, 0.2F * f1);
            this.particleScale *= f1;
            return this;
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        protected override void EntityInit()
        {
        }

        public override void OnUpdate()
        {
            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            if (this.particleAge++ >= this.particleMaxAge)
            {
                this.SetEntityDead();
            }

            this.motionY -= 0.04 * this.particleGravity;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.98F;
            this.motionY *= 0.98F;
            this.motionZ *= 0.98F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }

        public virtual void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = this.particleTextureIndex % 16 / 16F;
            float f9 = f8 + 0.0624375F;
            float f10 = this.particleTextureIndex / 16 / 16F;
            float f11 = f10 + 0.0624375F;
            float f12 = 0.1F * this.particleScale;
            float f13 = (float)(this.prevX + (this.x - this.prevX) * f2 - interpPosX);
            float f14 = (float)(this.prevY + (this.y - this.prevY) * f2 - interpPosY);
            float f15 = (float)(this.prevZ + (this.z - this.prevZ) * f2 - interpPosZ);
            float f16 = this.GetEntityBrightness(f2);
            tessellator1.Color(this.particleRed * f16, this.particleGreen * f16, this.particleBlue * f16);
            tessellator1.VertexUV(f13 - f3 * f12 - f6 * f12, f14 - f4 * f12, f15 - f5 * f12 - f7 * f12, f9, f11);
            tessellator1.VertexUV(f13 - f3 * f12 + f6 * f12, f14 + f4 * f12, f15 - f5 * f12 + f7 * f12, f9, f10);
            tessellator1.VertexUV(f13 + f3 * f12 + f6 * f12, f14 + f4 * f12, f15 + f5 * f12 + f7 * f12, f8, f10);
            tessellator1.VertexUV(f13 + f3 * f12 - f6 * f12, f14 - f4 * f12, f15 + f5 * f12 - f7 * f12, f8, f11);
        }

        public virtual int GetFXLayer()
        {
            return 0;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
        }
    }
}
