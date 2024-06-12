using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntityLavaFX : Particle
    {
        private float field_674_a;
        public EntityLavaFX(Level world1, double d2, double d4, double d6) : base(world1, d2, d4, d6, 0, 0, 0)
        {
            this.motionX *= 0.8F;
            this.motionY *= 0.8F;
            this.motionZ *= 0.8F;
            this.motionY = this.rand.NextFloat() * 0.4F + 0.05F;
            this.particleRed = this.particleGreen = this.particleBlue = 1F;
            this.particleScale *= this.rand.NextFloat() * 2F + 0.2F;
            this.field_674_a = this.particleScale;
            this.particleMaxAge = (int)(16 / (Mth.Random() * 0.8 + 0.2));
            this.noClip = false;
            this.particleTextureIndex = 49;
        }

        public override float GetEntityBrightness(float f1)
        {
            return 1F;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleAge + f2) / this.particleMaxAge;
            this.particleScale = this.field_674_a * (1F - f8 * f8);
            base.RenderParticle(tessellator1, f2, f3, f4, f5, f6, f7);
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

            float f1 = (float)this.particleAge / (float)this.particleMaxAge;
            if (this.rand.NextFloat() > f1)
            {
                this.worldObj.AddParticle("smoke", this.x, this.y, this.z, this.motionX, this.motionY, this.motionZ);
            }

            this.motionY -= 0.03;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.999F;
            this.motionY *= 0.999F;
            this.motionZ *= 0.999F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }
    }
}