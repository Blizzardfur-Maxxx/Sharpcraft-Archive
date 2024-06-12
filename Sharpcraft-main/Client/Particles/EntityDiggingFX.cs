using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.Renderer;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Client.Particles
{
    public class EntityDiggingFX : Particle
    {
        private Tile field_4082_a;

        public EntityDiggingFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12, Tile block14, TileFace tf, int i16) 
            : base (world1, d2, d4, d6, d8, d10, d12)
        {
            this.field_4082_a = block14;
            this.particleTextureIndex = block14.GetTexture(0, i16);
            this.particleGravity = block14.particleGravity;
            this.particleRed = this.particleGreen = this.particleBlue = 0.6F;
            this.particleScale /= 2.0F;
            //unused:
            //this.field_32001_o = i15;
        }

        public EntityDiggingFX Func_4041_a(int i1, int i2, int i3)
        {
            if (this.field_4082_a == Tile.grass)
            {
                return this;
            }
            else
            {
                int i4 = this.field_4082_a.GetColor(this.worldObj, i1, i2, i3);
                this.particleRed *= (i4 >> 16 & 255) / 255.0F;
                this.particleGreen *= (i4 >> 8 & 255) / 255.0F;
                this.particleBlue *= (i4 & 255) / 255.0F;
                return this;
            }
        }

        public override int GetFXLayer()
        {
            return 1;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.particleTextureIndex % 16 + this.particleTextureJitterX / 4.0F) / 16.0F;
            float f9 = f8 + 0.015609375F;
            float f10 = (this.particleTextureIndex / 16 + this.particleTextureJitterY / 4.0F) / 16.0F;
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
