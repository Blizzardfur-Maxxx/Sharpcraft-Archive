using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderSpider : RenderLiving<Spider>
    {
        public RenderSpider() : base(new ModelSpider(), 1.0F)
        {
            this.SetRenderPassModel(new ModelSpider());
        }

        protected override float GetDeathMaxRotation(Spider entityLiving1)
        {
            return 180.0F;
        }

        protected override bool ShouldRenderPass(Spider entityLiving1, int i2, float f3)
        {
            if (i2 != 0)
            {
                return false;
            }
            else if (i2 != 0)
            {
                return false;
            }
            else
            {
                this.LoadTexture("/mob/spider_eyes.png");
                float f4 = (1.0F - entityLiving1.GetEntityBrightness(1.0F)) * 0.5F;
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glDisable(GL11C.GL_ALPHA_TEST);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                GL11.glColor4f(1.0F, 1.0F, 1.0F, f4);
                return true;
            }
        }
    }
}
