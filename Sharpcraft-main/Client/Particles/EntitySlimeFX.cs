using SharpCraft.Client.Renderer;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntitySlimeFX : Particle
    {
        public EntitySlimeFX(Level world1, double d2, double d4, double d6, Item item8) : base(world1, d2, d4, d6, 0, 0, 0)
        {
            this.particleTextureIndex = item8.GetIconFromDamage(0);
            this.particleRed = this.particleGreen = this.particleBlue = 1F;
            this.particleGravity = Tile.blockSnow.particleGravity;
            this.particleScale /= 2F;
        }

        public override int GetFXLayer()
        {
            return 2;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleTextureIndex % 16 + this.particleTextureJitterX / 4F) / 16F;
            float f9 = f8 + 0.015609375F;
            float f10 = (this.particleTextureIndex / 16 + this.particleTextureJitterY / 4F) / 16F;
            float f11 = f10 + 0.015609375F;
            float f12 = 0.1F * this.particleScale;
            float f13 = (float)(this.prevX + (this.x - this.prevX) * f2 - interpPosX);
            float f14 = (float)(this.prevY + (this.y - this.prevY) * f2 - interpPosY);
            float f15 = (float)(this.prevZ + (this.z - this.prevZ) * f2 - interpPosZ);
            float f16 = this.GetEntityBrightness(f2);
            tessellator1.Color(f16 * this.particleRed, f16 * this.particleGreen, f16 * this.particleBlue);
            tessellator1.VertexUV(f13 - f3 * f12 - f6 * f12, f14 - f4 * f12, f15 - f5 * f12 - f7 * f12, f8, f11);
            tessellator1.VertexUV(f13 - f3 * f12 + f6 * f12, f14 + f4 * f12, f15 - f5 * f12 + f7 * f12, f8, f10);
            tessellator1.VertexUV(f13 + f3 * f12 + f6 * f12, f14 + f4 * f12, f15 + f5 * f12 + f7 * f12, f9, f10);
            tessellator1.VertexUV(f13 + f3 * f12 - f6 * f12, f14 - f4 * f12, f15 + f5 * f12 - f7 * f12, f9, f11);
        }
    }
}