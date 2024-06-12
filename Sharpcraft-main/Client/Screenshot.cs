#define USE_POINTER
using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class Screenshot
    {
        private static byte[] pixels;
#if USE_POINTER
        private static int[] rgb;
#endif
    
        public static string Save(JFile workDir, int width, int height)
        {
            try
            {
                JFile screenshotDir = new JFile(workDir, "screenshots");
                if (!screenshotDir.Exists())
                    screenshotDir.Mkdir();

                if (pixels == null || pixels.Length < width * height * 3) 
                {
                    pixels = new byte[width * height * 3];
#if USE_POINTER
                    rgb = new int[width * height];
#endif
                }
    
                GL11.glPixelStorei(GL11C.GL_PACK_ALIGNMENT, 1);
                GL11.glPixelStorei(GL11C.GL_UNPACK_ALIGNMENT, 1);
                GCHandle handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                GL11.glReadPixels(0, 0, width, height, GL11C.GL_RGB, GL11C.GL_UNSIGNED_BYTE, handle.AddrOfPinnedObject());
                handle.Free();
                string date = "" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
                JFile uniqueFile;
                for (int i = 1; (uniqueFile = new JFile(screenshotDir, date + (i == 1 ? "" : "_" + i) + ".png")).Exists(); ++i)
                {
                }
    
                Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        int offset = x + (height - y - 1) * width;
                        int r = pixels[offset * 3 + 0] & 255;
                        int g = pixels[offset * 3 + 1] & 255;
                        int b = pixels[offset * 3 + 2] & 255;
                        int color = unchecked((int)0xFF000000) | r << 16 | g << 8 | b;
#if USE_POINTER
                        rgb[x + y * width] = color;
#else
                        image.SetPixel(x, y, Color.FromArgb(color));
#endif
                    }
                }
#if USE_POINTER
                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                unsafe
                {
                    fixed (int* pixPtr = &rgb[0])
                    {
                        long l = rgb.LongLength * sizeof(int);
                        Buffer.MemoryCopy(pixPtr, (void*)data.Scan0, l, l);
                    }
                }
                image.UnlockBits(data);
#endif
                image.Save(uniqueFile.GetAbsolutePath(), ImageFormat.Png);
                image.Dispose();
    
                return "Saved screenshot as " + uniqueFile.GetName();
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
                return "Failed to save the screenshot: " + e.Message;
            }
        }
    }
}
