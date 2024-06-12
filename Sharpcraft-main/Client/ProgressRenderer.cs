using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using System.Threading;

namespace SharpCraft.Client
{
    public class ProgressRenderer : IProgressListener
    {
        private string field_1004_a = "";
        private Client mc;
        private string field_1007_c = "";
        private long field_1006_d = TimeUtil.MilliTime;
        private bool field_1005_e = false;

        public ProgressRenderer(Client instance)
        {
            this.mc = instance;
        }

        public void PrintText(string string1)
        {
            this.field_1005_e = false;
            this.Func_597_c(string1);
        }

        public virtual void StartLoading(string string1)
        {
            this.field_1005_e = true;
            this.Func_597_c(this.field_1007_c);
        }

        public void Func_597_c(string string1)
        {
            if (!this.mc.running)
            {
                if (!this.field_1005_e)
                {
                    throw new StopGameException();
                }
            }
            else
            {
                this.field_1007_c = string1;
                GuiScale scaledResolution2 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
                GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
                GL11.glMatrixMode(GL11C.GL_PROJECTION);
                GL11.glLoadIdentity();
                GL11.glOrtho(0.0D, scaledResolution2.WidthScale, scaledResolution2.HeightScale, 0.0D, 100.0D, 300.0D);
                GL11.glMatrixMode(GL11C.GL_MODELVIEW);
                GL11.glLoadIdentity();
                GL11.glTranslatef(0.0F, 0.0F, -200.0F);
            }
        }

        public virtual void DisplayLoadingString(string string1)
        {
            if (!this.mc.running)
            {
                if (!this.field_1005_e)
                {
                    throw new StopGameException();
                }
            }
            else
            {
                this.field_1006_d = 0L;
                this.field_1004_a = string1;
                this.SetLoadingProgress(-1);
                this.field_1006_d = 0L;
            }
        }

        public virtual void SetLoadingProgress(int i1)
        {
            if (!this.mc.running)
            {
                if (!this.field_1005_e)
                {
                    throw new StopGameException();
                }
            }
            else
            {
                long j2 = TimeUtil.MilliTime;
                if (j2 - this.field_1006_d >= 20L)
                {
                    this.field_1006_d = j2;
                    GuiScale scaledResolution4 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
                    int i5 = scaledResolution4.GetWidth();
                    int i6 = scaledResolution4.GetHeight();
                    GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
                    GL11.glMatrixMode(GL11C.GL_PROJECTION);
                    GL11.glLoadIdentity();
                    GL11.glOrtho(0.0D, scaledResolution4.WidthScale, scaledResolution4.HeightScale, 0.0D, 100.0D, 300.0D);
                    GL11.glMatrixMode(GL11C.GL_MODELVIEW);
                    GL11.glLoadIdentity();
                    GL11.glTranslatef(0.0F, 0.0F, -200.0F);
                    GL11.glClear(GL11C.GL_COLOR_BUFFER_BIT | GL11C.GL_DEPTH_BUFFER_BIT);
                    Tessellator tessellator7 = Tessellator.Instance;
                    uint i8 = this.mc.textures.LoadTexture("/gui/background.png");
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i8);
                    float f9 = 32.0F;
                    tessellator7.Begin();
                    tessellator7.Color(4210752);
                    tessellator7.VertexUV(0.0D, i6, 0.0D, 0.0D, i6 / f9);
                    tessellator7.VertexUV(i5, i6, 0.0D, i5 / f9, i6 / f9);
                    tessellator7.VertexUV(i5, 0.0D, 0.0D, i5 / f9, 0.0D);
                    tessellator7.VertexUV(0.0D, 0.0D, 0.0D, 0.0D, 0.0D);
                    tessellator7.End();
                    if (i1 >= 0)
                    {
                        byte b10 = 100;
                        byte b11 = 2;
                        int i12 = i5 / 2 - b10 / 2;
                        int i13 = i6 / 2 + 16;
                        GL11.glDisable(GL11C.GL_TEXTURE_2D);
                        tessellator7.Begin();
                        tessellator7.Color(8421504);
                        tessellator7.Vertex(i12, i13, 0.0D);
                        tessellator7.Vertex(i12, i13 + b11, 0.0D);
                        tessellator7.Vertex(i12 + b10, i13 + b11, 0.0D);
                        tessellator7.Vertex(i12 + b10, i13, 0.0D);
                        tessellator7.Color(8454016);
                        tessellator7.Vertex(i12, i13, 0.0D);
                        tessellator7.Vertex(i12, i13 + b11, 0.0D);
                        tessellator7.Vertex(i12 + i1, i13 + b11, 0.0D);
                        tessellator7.Vertex(i12 + i1, i13, 0.0D);
                        tessellator7.End();
                        GL11.glEnable(GL11C.GL_TEXTURE_2D);
                    }

                    this.mc.font.DrawStringWithShadow(this.field_1007_c, (i5 - this.mc.font.GetStringWidth(this.field_1007_c)) / 2, i6 / 2 - 4 - 16, 0xFFFFFF);
                    this.mc.font.DrawStringWithShadow(this.field_1004_a, (i5 - this.mc.font.GetStringWidth(this.field_1004_a)) / 2, i6 / 2 - 4 + 8, 0xFFFFFF);
                    Display.Update();

                    Thread.Yield();
                }
            }
        }
    }
}