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
    public class EntityFlameFX : Particle
    {
        private float field_672_a;
        public EntityFlameFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1, d2, d4, d6, d8, d10, d12)
        {
            this.motionX = this.motionX * 0.01F + d8;
            this.motionY = this.motionY * 0.01F + d10;
            this.motionZ = this.motionZ * 0.01F + d12;
            this.rand.NextFloat();
            this.rand.NextFloat();
            this.rand.NextFloat();
            this.rand.NextFloat();
            this.rand.NextFloat();
            this.rand.NextFloat();
            this.field_672_a = this.particleScale;
            this.particleRed = this.particleGreen = this.particleBlue = 1F;
            this.particleMaxAge = (int)(8 / (Mth.Random() * 0.8 + 0.2)) + 4;
            this.noClip = true;
            this.particleTextureIndex = 48;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleAge + f2) / this.particleMaxAge;
            this.particleScale = this.field_672_a * (1F - f8 * f8 * 0.5F);
            base.RenderParticle(tessellator1, f2, f3, f4, f5, f6, f7);
        }

        public override float GetEntityBrightness(float f1)
        {
            float f2 = (this.particleAge + f1) / this.particleMaxAge;
            if (f2 < 0F)
            {
                f2 = 0F;
            }

            if (f2 > 1F)
            {
                f2 = 1F;
            }

            float f3 = base.GetEntityBrightness(f1);
            return f3 * f2 + (1F - f2);
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

            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.96F;
            this.motionY *= 0.96F;
            this.motionZ *= 0.96F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }
    }
}