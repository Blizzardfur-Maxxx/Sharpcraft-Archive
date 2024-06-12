using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderEntity : Render
    {
        public override void DoRender(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            RenderOffsetAABB(entity1.boundingBox, d2 - entity1.lastTickPosX, d4 - entity1.lastTickPosY, d6 - entity1.lastTickPosZ);
            GL11.glPopMatrix();
        }
    }
}
