using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI
{
    public class GuiComponent
    {
        protected float zLevel = 0F;

        protected void Func_27100_a(int i1, int i2, int i3, int i4)
        {
            if (i2 < i1)
            {
                int i5 = i1;
                i1 = i2;
                i2 = i5;
            }

            DrawRect(i1, i3, i2 + 1, i3 + 1, i4);
        }

        protected void Func_27099_b(int i1, int i2, int i3, int i4)
        {
            if (i3 < i2)
            {
                int i5 = i2;
                i2 = i3;
                i3 = i5;
            }

            DrawRect(i1, i2 + 1, i1 + 1, i3, i4);
        }

        protected void DrawRect(int i1, int i2, int i3, int i4, int i5)
        {
            int i6;
            if (i1 < i3)
            {
                i6 = i1;
                i1 = i3;
                i3 = i6;
            }

            if (i2 < i4)
            {
                i6 = i2;
                i2 = i4;
                i4 = i6;
            }

            float f11 = (i5 >> 24 & 255) / 255F;
            float f7 = (i5 >> 16 & 255) / 255F;
            float f8 = (i5 >> 8 & 255) / 255F;
            float f9 = (i5 & 255) / 255F;
            Tessellator tessellator10 = Tessellator.Instance;
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glColor4f(f7, f8, f9, f11);
            tessellator10.Begin();
            tessellator10.Vertex(i1, i4, 0);
            tessellator10.Vertex(i3, i4, 0);
            tessellator10.Vertex(i3, i2, 0);
            tessellator10.Vertex(i1, i2, 0);
            tessellator10.End();
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            GL11.glDisable(GL11C.GL_BLEND);
        }

        protected void DrawGradientRect(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            float f7 = (i5 >> 24 & 255) / 255F;
            float f8 = (i5 >> 16 & 255) / 255F;
            float f9 = (i5 >> 8 & 255) / 255F;
            float f10 = (i5 & 255) / 255F;
            float f11 = (i6 >> 24 & 255) / 255F;
            float f12 = (i6 >> 16 & 255) / 255F;
            float f13 = (i6 >> 8 & 255) / 255F;
            float f14 = (i6 & 255) / 255F;
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glShadeModel(GL11C.GL_SMOOTH);
            Tessellator tessellator15 = Tessellator.Instance;
            tessellator15.Begin();
            tessellator15.Color(f8, f9, f10, f7);
            tessellator15.Vertex(i3, i2, 0);
            tessellator15.Vertex(i1, i2, 0);
            tessellator15.Color(f12, f13, f14, f11);
            tessellator15.Vertex(i1, i4, 0);
            tessellator15.Vertex(i3, i4, 0);
            tessellator15.End();
            GL11.glShadeModel(GL11C.GL_FLAT);
            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
        }

        public void DrawCenteredString(Font fontRenderer1, string string2, int i3, int i4, int i5)
        {
            fontRenderer1.DrawStringWithShadow(string2, i3 - fontRenderer1.GetStringWidth(string2) / 2, i4, (uint)i5);
        }

        public void DrawString(Font fontRenderer1, string string2, int i3, int i4, int i5)
        {
            fontRenderer1.DrawStringWithShadow(string2, i3, i4, (uint)i5);
        }

        public void DrawTexturedModalRect(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            float f7 = 0.00390625F;
            float f8 = 0.00390625F;
            Tessellator tessellator9 = Tessellator.Instance;
            tessellator9.Begin();
            tessellator9.VertexUV(i1 + 0, i2 + i6, this.zLevel, (i3 + 0) * f7, (i4 + i6) * f8);
            tessellator9.VertexUV(i1 + i5, i2 + i6, this.zLevel, (i3 + i5) * f7, (i4 + i6) * f8);
            tessellator9.VertexUV(i1 + i5, i2 + 0, this.zLevel, (i3 + i5) * f7, (i4 + 0) * f8);
            tessellator9.VertexUV(i1 + 0, i2 + 0, this.zLevel, (i3 + 0) * f7, (i4 + 0) * f8);
            tessellator9.End();
        }
    }
}
