using LWCSGL;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static LWCSGL.OpenGL.GL11_PTR;
using static LWCSGL.OpenGL.GL11C;

namespace SharpCraft.Client.GUI
{
    public class Font
    {
        private int[] charWidth = new int[256];
        public uint FontTextureId = 0;
        private uint fontDisplayLists;
        //TODO ptr indexing without external vars
        //i couldn't be arsed to convert all the flip, clear, remaining and whatever operations to pointer arithmetic so here we go...
        //i hate you notch!
        private readonly uint[] buffer = new uint[1024];
        private readonly int cap;
        private int pos, lim;

        public Font(Options options, string name, Textures textures)
        {
            cap = buffer.Length;
            lim = buffer.Length;
            pos = buffer.Length;
            Bitmap img = Textures.GetAssetsBitmap(name);

            int imgWidth = img.Width;
            int imgHeight = img.Height;
            int[] imgPixels = new int[imgWidth * imgHeight];
            img.GetRGB(0, 0, imgWidth, imgHeight, imgPixels, 0, imgWidth);

            int baseX;
            int baseY;
            int x;
            int xIndex;
            int yIndex;
            int pixel;
            for (int character = 0; character < 256; ++character)
            {
                baseX = character % 16;
                baseY = character / 16;
                for (x = 7; x >= 0; --x)
                {
                    xIndex = baseX * 8 + x;
                    bool emptyColumn = true;
                    for (int y = 0; y < 8 && emptyColumn; ++y)
                    {
                        yIndex = (baseY * 8 + y) * imgWidth;
                        pixel = imgPixels[xIndex + yIndex] & 255;
                        if (pixel > 0)
                        {
                            emptyColumn = false;
                        }
                    }

                    if (!emptyColumn)
                    {
                        break;
                    }
                }

                if (character == 32)
                {
                    x = 2;
                }

                charWidth[character] = x + 2;
            }

            FontTextureId = textures.LoadTexture(img);
            fontDisplayLists = glGenLists(288);
            Tessellator tessellator = Tessellator.Instance;
            for (baseX = 0; baseX < 256; ++baseX)
            {
                glNewList(fontDisplayLists + (uint) baseX, GL_COMPILE);
                tessellator.Begin();
                baseY = baseX % 16 * 8;
                x = baseX / 16 * 8;
                float f20 = 7.99F;
                float f21 = 0F;
                float f23 = 0F;
                tessellator.VertexUV(0, 0F + f20, 0, baseY / 128F + f21, (x + f20) / 128F + f23);
                tessellator.VertexUV(0F + f20, 0F + f20, 0, (baseY + f20) / 128F + f21, (x + f20) / 128F + f23);
                tessellator.VertexUV(0F + f20, 0, 0, (baseY + f20) / 128F + f21, x / 128F + f23);
                tessellator.VertexUV(0, 0, 0, baseY / 128F + f21, x / 128F + f23);
                tessellator.End();
                glTranslatef(charWidth[baseX], 0F, 0F);
                glEndList();
            }

            for (baseX = 0; baseX < 32; ++baseX)
            {
                baseY = (baseX >> 3 & 1) * 85;
                x = (baseX >> 2 & 1) * 170 + baseY;
                xIndex = (baseX >> 1 & 1) * 170 + baseY;
                int i22 = (baseX >> 0 & 1) * 170 + baseY;
                if (baseX == 6)
                {
                    x += 85;
                }

                bool z24 = baseX >= 16;
                if (options.anaglyph)
                {
                    yIndex = (x * 30 + xIndex * 59 + i22 * 11) / 100;
                    pixel = (x * 30 + xIndex * 70) / 100;
                    int i17 = (x * 30 + i22 * 70) / 100;
                    x = yIndex;
                    xIndex = pixel;
                    i22 = i17;
                }

                if (z24)
                {
                    x /= 4;
                    xIndex /= 4;
                    i22 /= 4;
                }

                glNewList(fontDisplayLists + 256 + (uint)baseX, GL_COMPILE);
                glColor3f(x / 255F, xIndex / 255F, i22 / 255F);
                glEndList();
            }
        }

        public virtual void DrawStringWithShadow(string str, int x, int y, uint color)
        {
            RenderString(str, x + 1, y + 1, color, true);
            DrawString(str, x, y, color);
        }

        public virtual void DrawString(string str, int x, int y, uint color)
        {
            RenderString(str, x, y, color, false);
        }

        private unsafe void RenderString(string text, int x, int y, uint color, bool dimColor)
        {
            if (string.IsNullOrEmpty(text)) return;
            fixed (uint* p = &buffer[0]) 
            {
                uint tempColor;
                if (dimColor)
                {
                    tempColor = color & 0xFF000000;
                    color = (color & 0xFCFCFC) >> 2;
                    color += tempColor;
                }

                glBindTexture(GL_TEXTURE_2D, this.FontTextureId);
                float r = (color >> 16 & 255) / 255F;
                float g = (color >> 8 & 255) / 255F;
                float b = (color & 255) / 255F;
                float a = (color >> 24 & 255) / 255F;
                if (a == 0F)
                {
                    a = 1F;
                }

                glColor4f(r, g, b, a);
                pos = 0;
                lim = cap;

                glPushMatrix();
                glTranslatef(x, y, 0F);
                for (int i = 0; i < text.Length; ++i)
                {
                    int colorCode;
                    for (; text.Length > i + 1 && text[i] == 167; i += 2)
                    {
                        colorCode = "0123456789abcdef".IndexOf(text.ToLower()[i + 1]);
                        if (colorCode < 0 || colorCode > 15)
                        {
                            colorCode = 15;
                        }

                        this.buffer[pos++] = this.fontDisplayLists + 256 + (uint)colorCode + (dimColor ? 16u : 0u);
                        if ((lim - pos) == 0)
                        {
                            lim = pos;
                            pos = 0;

                            glCallLists(lim, GL_UNSIGNED_INT, p);

                            pos = 0;
                            lim = cap;
                        }
                    }

                    if (i < text.Length)
                    {
                        colorCode = SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(text[i]);
                        if (colorCode >= 0)
                        {
                            this.buffer[pos++] = this.fontDisplayLists + (uint)colorCode + 32;
                        }
                    }

                    if ((lim - pos) == 0)
                    {
                        lim = pos;
                        pos = 0;

                        glCallLists(lim, GL_UNSIGNED_INT, p);

                        pos = 0;
                        lim = cap;
                    }
                }

                lim = pos;
                pos = 0;

                glCallLists(lim, GL_UNSIGNED_INT, p);

                glPopMatrix();
            }
        }

        public virtual int GetStringWidth(string string1)
        {
            if (string1 == null)
            {
                return 0;
            }
            else
            {
                int i2 = 0;
                for (int i3 = 0; i3 < string1.Length; ++i3)
                {
                    if (string1[i3] == 167)
                    {
                        ++i3;
                    }
                    else
                    {
                        int i4 = SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(string1[i3]);
                        if (i4 >= 0)
                        {
                            i2 += charWidth[i4 + 32];
                        }
                    }
                }

                return i2;
            }
        }

        public virtual void DrawMultiline(string str, int x, int y, int i4, uint color)
        {
            String[] string6 = str.Split("\n");
            if (string6.Length > 1)
            {
                for (int i11 = 0; i11 < string6.Length; ++i11)
                {
                    DrawMultiline(string6[i11], x, y, i4, color);
                    y += DrawMultiline(string6[i11], i4);
                }
            }
            else
            {
                String[] string7 = str.Split(" ");
                int i8 = 0;
                while (i8 < string7.Length)
                {
                    string string9;
                    for (string9 = string7[i8++] + " "; i8 < string7.Length && GetStringWidth(string9 + string7[i8]) < i4; string9 = string9 + string7[i8++] + " ")
                    {
                    }

                    int i10;
                    for (; GetStringWidth(string9) > i4; string9 = string9.Substring(i10))
                    {
                        for (i10 = 0; GetStringWidth(string9.Substring(0, i10 + 1)) <= i4; ++i10)
                        {
                        }

                        if (string9.Substring(0, i10).Trim().Length > 0)
                        {
                            DrawString(string9.Substring(0, i10), x, y, color);
                            y += 8;
                        }
                    }

                    if (string9.Trim().Length > 0)
                    {
                        DrawString(string9, x, y, color);
                        y += 8;
                    }
                }
            }
        }

        public virtual int DrawMultiline(string str, int i2)
        {
            String[] string3 = str.Split("\n");
            int i5;
            if (string3.Length > 1)
            {
                int i9 = 0;
                for (i5 = 0; i5 < string3.Length; ++i5)
                {
                    i9 += DrawMultiline(string3[i5], i2);
                }

                return i9;
            }
            else
            {
                String[] string4 = str.Split(" ");
                i5 = 0;
                int i6 = 0;
                while (i5 < string4.Length)
                {
                    string string7;
                    for (string7 = string4[i5++] + " "; i5 < string4.Length && GetStringWidth(string7 + string4[i5]) < i2; string7 = string7 + string4[i5++] + " ")
                    {
                    }

                    int i8;
                    for (; GetStringWidth(string7) > i2; string7 = string7.Substring(i8))
                    {
                        for (i8 = 0; GetStringWidth(string7.Substring(0, i8 + 1)) <= i2; ++i8)
                        {
                        }

                        if (string7.Substring(0, i8).Trim().Length > 0)
                        {
                            i6 += 8;
                        }
                    }

                    if (string7.Trim().Length > 0)
                    {
                        i6 += 8;
                    }
                }

                if (i6 < 8)
                {
                    i6 += 8;
                }

                return i6;
            }
        }
    }
}
