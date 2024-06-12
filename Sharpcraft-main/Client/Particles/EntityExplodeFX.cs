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
    public class EntityExplodeFX : Particle
    {
        public EntityExplodeFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1, d2, d4, d6, d8, d10, d12)
        {
            this.motionX = d8 + (float)(Mth.Random() * 2 - 1) * 0.05F;
            this.motionY = d10 + (float)(Mth.Random() * 2 - 1) * 0.05F;
            this.motionZ = d12 + (float)(Mth.Random() * 2 - 1) * 0.05F;
            this.particleRed = this.particleGreen = this.particleBlue = this.rand.NextFloat() * 0.3F + 0.7F;
            this.particleScale = this.rand.NextFloat() * this.rand.NextFloat() * 6F + 1F;
            this.particleMaxAge = (int)(16 / (this.rand.NextFloat() * 0.8 + 0.2)) + 2;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
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
            this.motionY += 0.004;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.9F;
            this.motionY *= 0.9F;
            this.motionZ *= 0.9F;
            if (this.onGround)
            {
                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }
        }
    }
}