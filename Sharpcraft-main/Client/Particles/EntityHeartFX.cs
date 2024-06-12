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
    public class EntityHeartFX : Particle
    {
        float field_25022_a;
        public EntityHeartFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : this(world1, d2, d4, d6, d8, d10, d12, 2F)
        {
        }

        public EntityHeartFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12, float f14) : base(world1, d2, d4, d6, 0, 0, 0)
        {
            this.motionX *= 0.01F;
            this.motionY *= 0.01F;
            this.motionZ *= 0.01F;
            this.motionY += 0.1;
            this.particleScale *= 0.75F;
            this.particleScale *= f14;
            this.field_25022_a = this.particleScale;
            this.particleMaxAge = 16;
            this.noClip = false;
            this.particleTextureIndex = 80;
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

            this.particleScale = this.field_25022_a * f8;
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

            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            if (this.y == this.prevY)
            {
                this.motionX *= 1.1;
                this.motionZ *= 1.1;
            }

            this.motionX *= 0.86F;
            this.motionY *= 0.86F;
            this.motionZ *= 0.86F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }
    }
}