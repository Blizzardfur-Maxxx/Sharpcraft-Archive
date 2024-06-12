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
    public class EntitySnowShovelFX : Particle
    {
        float field_27017_a;
        public EntitySnowShovelFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : this(world1, d2, d4, d6, d8, d10, d12, 1F)
        {
        }

        public EntitySnowShovelFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12, float f14) : base(world1, d2, d4, d6, d8, d10, d12)
        {
            this.motionX *= 0.1F;
            this.motionY *= 0.1F;
            this.motionZ *= 0.1F;
            this.motionX += d8;
            this.motionY += d10;
            this.motionZ += d12;
            this.particleRed = this.particleGreen = this.particleBlue = 1F - (float)(Mth.Random() * 0.3F);
            this.particleScale *= 0.75F;
            this.particleScale *= f14;
            this.field_27017_a = this.particleScale;
            this.particleMaxAge = (int)(8 / (Mth.Random() * 0.8 + 0.2));
            this.particleMaxAge = (int)(this.particleMaxAge * f14);
            this.noClip = false;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleAge + f2) / this.particleMaxAge * 32F;
            if (f8 < 0F)
            {
                f8 = 0F;
            }

            if (f8 > 1F)
            {
                f8 = 1F;
            }

            this.particleScale = this.field_27017_a * f8;
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

            this.particleTextureIndex = 7 - this.particleAge * 8 / this.particleMaxAge;
            this.motionY -= 0.03;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.99F;
            this.motionY *= 0.99F;
            this.motionZ *= 0.99F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }
    }
}