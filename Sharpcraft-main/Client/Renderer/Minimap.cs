using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.GameSavedData.maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.World.GameLevel.GameSavedData.maps.MapItemSavedData;

namespace SharpCraft.Client.Renderer
{
    public class Minimap
    {
        private int[] mapColors = new int[16384];
        private uint mapTextureHandle;
        private Options options;
        private GUI.Font font;

        public Minimap(GUI.Font font, Options options, Textures textures)
        {
            this.options = options;
            this.font = font;
            this.mapTextureHandle = textures.LoadTexture(new Bitmap(128, 128, PixelFormat.Format32bppArgb));

            for (int i = 0; i < this.mapColors.Length; ++i)
            {
                this.mapColors[i] = 0;
            }

        }

        public void Render(Player player, Textures textures, MapItemSavedData data)
        {
            for (int i = 0; i < this.mapColors.Length; ++i)
            {
                byte b5 = data.field_f[i];
                if (b5 / 4 == 0)
                {
                    this.mapColors[i] = (i + i / 128 & 1) * 8 + 16 << 24;
                }
                else
                {
                    int i6 = Core.World.GameLevel.Materials.Color.colors[b5 / 4].rgb;
                    int i7 = b5 & 3;
                    short s8 = 220;
                    if (i7 == 2)
                    {
                        s8 = 255;
                    }

                    if (i7 == 0)
                    {
                        s8 = 180;
                    }

                    int i9 = (i6 >> 16 & 255) * s8 / 255;
                    int i10 = (i6 >> 8 & 255) * s8 / 255;
                    int i11 = (i6 & 255) * s8 / 255;
                    if (this.options.anaglyph)
                    {
                        int i12 = (i9 * 30 + i10 * 59 + i11 * 11) / 100;
                        int i13 = (i9 * 30 + i10 * 70) / 100;
                        int i14 = (i9 * 30 + i11 * 70) / 100;
                        i9 = i12;
                        i10 = i13;
                        i11 = i14;
                    }

                    this.mapColors[i] = (unchecked((int)0xFF000000) | (i9 << 16) | (i10 << 8) | i11);
                }
            }

            textures.ReplaceTexture(this.mapColors, 128, 128, this.mapTextureHandle);
            byte b15 = 0;
            byte b16 = 0;
            Tessellator t = Tessellator.Instance;
            float f18 = 0.0F;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mapTextureHandle);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            t.Begin();
            t.VertexUV(b15 + 0 + f18, b16 + 128 - f18, -0.009999999776482582D, 0.0D, 1.0D);
            t.VertexUV(b15 + 128 - f18, b16 + 128 - f18, -0.009999999776482582D, 1.0D, 1.0D);
            t.VertexUV(b15 + 128 - f18, b16 + 0 + f18, -0.009999999776482582D, 1.0D, 0.0D);
            t.VertexUV(b15 + 0 + f18, b16 + 0 + f18, -0.009999999776482582D, 0.0D, 0.0D);
            t.End();
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glDisable(GL11C.GL_BLEND);
            textures.Bind(textures.LoadTexture("/misc/mapicons.png"));

            foreach (MapDecoration deco in data.mapDecorations)
            {
                GL11.glPushMatrix();
                GL11.glTranslatef(b15 + deco.x / 2.0F + 64.0F, b16 + deco.y / 2.0F + 64.0F, -0.02F);
                GL11.glRotatef(deco.rot * 360 / 16.0F, 0.0F, 0.0F, 1.0F);
                GL11.glScalef(4.0F, 4.0F, 3.0F);
                GL11.glTranslatef(-0.125F, 0.125F, 0.0F);
                float f21 = (deco.type % 4 + 0) / 4.0F;
                float f22 = (deco.type / 4 + 0) / 4.0F;
                float f23 = (deco.type % 4 + 1) / 4.0F;
                float f24 = (deco.type / 4 + 1) / 4.0F;
                t.Begin();
                t.VertexUV(-1.0D, 1.0D, 0.0D, f21, f22);
                t.VertexUV(1.0D, 1.0D, 0.0D, f23, f22);
                t.VertexUV(1.0D, -1.0D, 0.0D, f23, f24);
                t.VertexUV(-1.0D, -1.0D, 0.0D, f21, f24);
                t.End();
                GL11.glPopMatrix();
            }

            GL11.glPushMatrix();
            GL11.glTranslatef(0.0F, 0.0F, -0.04F);
            GL11.glScalef(1.0F, 1.0F, 1.0F);
            this.font.DrawString(data.name, b15, b16, 0xFF000000);
            GL11.glPopMatrix();
        }
    }
}
