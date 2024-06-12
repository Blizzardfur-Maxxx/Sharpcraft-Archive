using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Client.Renderer.Entities;
using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace SharpCraft.Client.Particles
{
    public class EntityPickupFX : Particle
    {
        private Entity field_675_a;
        private Entity field_679_o;
        private int field_678_p = 0;
        private int field_677_q = 0;
        private float field_676_r;
        public EntityPickupFX(Level world1, Entity entity2, Entity entity3, float f4) : base(world1, entity2.x, entity2.y, entity2.z, entity2.motionX, entity2.motionY, entity2.motionZ)
        {
            this.field_675_a = entity2;
            this.field_679_o = entity3;
            this.field_677_q = 3;
            this.field_676_r = f4;
        }

        public override void RenderParticle(Tessellator tessellator1, float f2, float f3, float f4, float f5, float f6, float f7)
        {
            float f8 = (this.field_678_p + f2) / this.field_677_q;
            f8 *= f8;
            double d9 = this.field_675_a.x;
            double d11 = this.field_675_a.y;
            double d13 = this.field_675_a.z;
            double d15 = this.field_679_o.lastTickPosX + (this.field_679_o.x - this.field_679_o.lastTickPosX) * f2;
            double d17 = this.field_679_o.lastTickPosY + (this.field_679_o.y - this.field_679_o.lastTickPosY) * f2 + this.field_676_r;
            double d19 = this.field_679_o.lastTickPosZ + (this.field_679_o.z - this.field_679_o.lastTickPosZ) * f2;
            double d21 = d9 + (d15 - d9) * f8;
            double d23 = d11 + (d17 - d11) * f8;
            double d25 = d13 + (d19 - d13) * f8;
            int i27 = Mth.Floor(d21);
            int i28 = Mth.Floor(d23 + this.yOffset / 2F);
            int i29 = Mth.Floor(d25);
            float f30 = this.worldObj.GetBrightness(i27, i28, i29);
            d21 -= interpPosX;
            d23 -= interpPosY;
            d25 -= interpPosZ;
            GL11.glColor4f(f30, f30, f30, 1F);
            EntityRenderDispatcher.instance.RenderEntityWithPosYaw(this.field_675_a, ((float)d21), ((float)d23), ((float)d25), this.field_675_a.yaw, f2);
        }

        public override void OnUpdate()
        {
            ++this.field_678_p;
            if (this.field_678_p == this.field_677_q)
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