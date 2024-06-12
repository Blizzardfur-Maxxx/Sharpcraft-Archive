using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntityFootStepFX : Particle
    {
        private int field_27018_a = 0;
        private int field_27020_o = 0;
        private Textures field_27019_p;
        public EntityFootStepFX(Textures renderEngine1, Level world2, double d3, double d5, double d7) : base(world2, d3, d5, d7, 0, 0, 0)
        {
            this.field_27019_p = renderEngine1;
            this.motionX = this.motionY = this.motionZ = 0;
            this.field_27020_o = 200;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.field_27018_a + f2) / this.field_27020_o;
            f8 *= f8;
            float f9 = 2F - f8 * 2F;
            if (f9 > 1F)
            {
                f9 = 1F;
            }

            f9 *= 0.2F;
            GL11.glDisable(GL11C.GL_LIGHTING);
            float f10 = 0.125F;
            float f11 = (float)(this.x - interpPosX);
            float f12 = (float)(this.y - interpPosY);
            float f13 = (float)(this.z - interpPosZ);
            float f14 = this.worldObj.GetBrightness(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z));
            this.field_27019_p.Bind(this.field_27019_p.LoadTexture("/misc/footprint.png"));
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            tessellator1.Begin();
            tessellator1.Color(f14, f14, f14, f9);
            tessellator1.VertexUV(f11 - f10, f12, f13 + f10, 0, 1);
            tessellator1.VertexUV(f11 + f10, f12, f13 + f10, 1, 1);
            tessellator1.VertexUV(f11 + f10, f12, f13 - f10, 1, 0);
            tessellator1.VertexUV(f11 - f10, f12, f13 - f10, 0, 0);
            tessellator1.End();
            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glEnable(GL11C.GL_LIGHTING);
        }

        public override void OnUpdate()
        {
            ++this.field_27018_a;
            if (this.field_27018_a == this.field_27020_o)
            {
                this.SetEntityDead();
            }
        }

        public override int GetFXLayer()
        {
            return 3;
        }
    }
}