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
    public class EntityReddustFX : Particle
    {
        float field_673_a;
        public EntityReddustFX(Level world1, double d2, double d4, double d6, float f8, float f9, float f10) : this(world1, d2, d4, d6, 1F, f8, f9, f10)
        {
        }

        public EntityReddustFX(Level world1, double d2, double d4, double d6, float f8, float f9, float f10, float f11) : base(world1, d2, d4, d6, 0, 0, 0)
        {
            this.motionX *= 0.1F;
            this.motionY *= 0.1F;
            this.motionZ *= 0.1F;
            if (f9 == 0F)
            {
                f9 = 1F;
            }

            float f12 = (float)Mth.Random() * 0.4F + 0.6F;
            this.particleRed = ((float)(Mth.Random() * 0.2F) + 0.8F) * f9 * f12;
            this.particleGreen = ((float)(Mth.Random() * 0.2F) + 0.8F) * f10 * f12;
            this.particleBlue = ((float)(Mth.Random() * 0.2F) + 0.8F) * f11 * f12;
            this.particleScale *= 0.75F;
            this.particleScale *= f8;
            this.field_673_a = this.particleScale;
            this.particleMaxAge = (int)(8 / (Mth.Random() * 0.8 + 0.2));
            this.particleMaxAge = (int)(this.particleMaxAge * f8);
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

            this.particleScale = this.field_673_a * f8;
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
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            if (this.y == this.prevY)
            {
                this.motionX *= 1.1;
                this.motionZ *= 1.1;
            }

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