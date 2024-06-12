using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LWCSGL;

namespace SharpCraft.Client.Renderer
{
    public class MobSkinTextureProcessor : IMemTextureProcessor
    {
        private int[] imageData;
        private int imageWidth;
        private int imageHeight;

        public virtual Bitmap Process(Bitmap image)
        {
            if (image == null)
            {
                return null;
            }
            else
            {
                this.imageWidth = 64;
                this.imageHeight = 32;
                Bitmap bufferedImage2 = new Bitmap(this.imageWidth, this.imageHeight, PixelFormat.Format32bppArgb);
                Graphics graphics3 = Graphics.FromImage(bufferedImage2);
                graphics3.DrawImage(image, new Point(0, 0));
                graphics3.Dispose();
                this.imageData = new int[this.imageWidth * this.imageHeight];
                bufferedImage2.GetARGB(0, 0, imageWidth, imageHeight, imageData, 0, imageWidth);
                this.Func_884_b(0, 0, 32, 16);
                this.Func_885_a(32, 0, 64, 32);
                this.Func_884_b(0, 16, 64, 32);
                bool z4 = false;
                int i5;
                int i6;
                int i7;
                for (i5 = 32; i5 < 64; ++i5)
                {
                    for (i6 = 0; i6 < 16; ++i6)
                    {
                        i7 = this.imageData[i5 + i6 * 64];
                        if ((i7 >> 24 & 255) < 128)
                        {
                            z4 = true;
                        }
                    }
                }

                if (!z4)
                {
                    for (i5 = 32; i5 < 64; ++i5)
                    {
                        for (i6 = 0; i6 < 16; ++i6)
                        {
                            i7 = this.imageData[i5 + i6 * 64];
                            if ((i7 >> 24 & 255) < 128)
                            {
                                z4 = true;
                            }
                        }
                    }
                }

                return bufferedImage2;
            }
        }

        private void Func_885_a(int i1, int i2, int i3, int i4)
        {
            if (!this.Func_886_c(i1, i2, i3, i4))
            {
                for (int i5 = i1; i5 < i3; ++i5)
                {
                    for (int i6 = i2; i6 < i4; ++i6)
                    {
                        this.imageData[i5 + i6 * this.imageWidth] &= 0xFFFFFF;
                    }
                }
            }
        }

        private void Func_884_b(int i1, int i2, int i3, int i4)
        {
            for (int i5 = i1; i5 < i3; ++i5)
            {
                for (int i6 = i2; i6 < i4; ++i6)
                {
                    this.imageData[i5 + i6 * this.imageWidth] |= unchecked((int) 0xFF000000);
                }
            }
        }

        private bool Func_886_c(int i1, int i2, int i3, int i4)
        {
            for (int i5 = i1; i5 < i3; ++i5)
            {
                for (int i6 = i2; i6 < i4; ++i6)
                {
                    int i7 = this.imageData[i5 + i6 * this.imageWidth];
                    if ((i7 >> 24 & 255) < 128)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
