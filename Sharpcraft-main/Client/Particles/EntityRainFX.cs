using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntityRainFX : Particle
    {
        public EntityRainFX(Level world1, double d2, double d4, double d6) : base(world1, d2, d4, d6, 0, 0, 0)
        {
            this.motionX *= 0.3F;
            this.motionY = (float)Mth.Random() * 0.2F + 0.1F;
            this.motionZ *= 0.3F;
            this.particleRed = 1F;
            this.particleGreen = 1F;
            this.particleBlue = 1F;
            this.particleTextureIndex = 19 + this.rand.NextInt(4);
            this.SetSize(0.01F, 0.01F);
            this.particleGravity = 0.06F;
            this.particleMaxAge = (int)(8 / (Mth.Random() * 0.8 + 0.2));
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
            this.motionY -= this.particleGravity;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.98F;
            this.motionY *= 0.98F;
            this.motionZ *= 0.98F;
            if (this.particleMaxAge-- <= 0)
            {
                this.SetEntityDead();
            }

            if (this.onGround)
            {
                if (Mth.Random() < 0.5)
                {
                    this.SetEntityDead();
                }

                this.motionX *= 0.7F;
                this.motionZ *= 0.7F;
            }

            Material material1 = this.worldObj.GetMaterial(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z));
            if (material1.IsLiquid() || material1.IsSolid())
            {
                double d2 = Mth.Floor(this.y) + 1 - LiquidTile.GetHeight(this.worldObj.GetData(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)));
                if (this.y < d2)
                {
                    this.SetEntityDead();
                }
            }
        }
    }
}