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
    public class EntityPortalFX : Particle
    {
        private float field_4083_a;
        private double field_4086_p;
        private double field_4085_q;
        private double field_4084_r;
        public EntityPortalFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1, d2, d4, d6, d8, d10, d12)
        {
            this.motionX = d8;
            this.motionY = d10;
            this.motionZ = d12;
            this.field_4086_p = this.x = d2;
            this.field_4085_q = this.y = d4;
            this.field_4084_r = this.z = d6;
            float f14 = this.rand.NextFloat() * 0.6F + 0.4F;
            this.field_4083_a = this.particleScale = this.rand.NextFloat() * 0.2F + 0.5F;
            this.particleRed = this.particleGreen = this.particleBlue = 1F * f14;
            this.particleGreen *= 0.3F;
            this.particleRed *= 0.9F;
            this.particleMaxAge = (int)(Mth.Random() * 10) + 40;
            this.noClip = true;
            this.particleTextureIndex = (int)(Mth.Random() * 8);
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleAge + f2) / this.particleMaxAge;
            f8 = 1F - f8;
            f8 *= f8;
            f8 = 1F - f8;
            this.particleScale = this.field_4083_a * f8;
            base.RenderParticle(tessellator1, f2, f3, f4, f5, f6, f7);
        }

        public override float GetEntityBrightness(float f1)
        {
            float f2 = base.GetEntityBrightness(f1);
            float f3 = (float)this.particleAge / (float)this.particleMaxAge;
            f3 *= f3;
            f3 *= f3;
            return f2 * (1F - f3) + f3;
        }

        public override void OnUpdate()
        {
            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            float f1 = (float)this.particleAge / (float)this.particleMaxAge;
            float f2 = f1;
            f1 = -f1 + f1 * f1 * 2F;
            f1 = 1F - f1;
            this.x = this.field_4086_p + this.motionX * f1;
            this.y = this.field_4085_q + this.motionY * f1 + (1F - f2);
            this.z = this.field_4084_r + this.motionZ * f1;
            if (this.particleAge++ >= this.particleMaxAge)
            {
                this.SetEntityDead();
            }
        }
    }
}