//#define TEXTURES_MIPMAP

using LWCSGL.OpenGL;
using LWCSGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using SharpCraft.Client.Texturepack;
using SharpCraft.Client.Renderer.Ptexture;
using SharpCraft.Core.Util;
using System.Windows.Forms;
using SharpCraft.Core;
using System.Runtime.InteropServices;

namespace SharpCraft.Client.Renderer
{
    public class Textures //i suffered for 3 hours in this class - genericpnp
    {
        private NullDictionary<string, uint> idMap = new NullDictionary<string, uint>();
        private NullDictionary<string, int[]> pixelsMap = new NullDictionary<string, int[]>();
        private NullDictionary<uint, Bitmap> loadedImages = new NullDictionary<uint, Bitmap>();
        private readonly uint[] ib = new uint[1];
        private readonly byte[] pixels = new byte[1048576];
        private nint pixaddr;
        private IList<DynamicTexture> dynamicTextures = new List<DynamicTexture>();
        private NullDictionary<string, MemTexture> memTextures = new NullDictionary<string, MemTexture>();
        private Options options;
        private bool clamp = false;
        private bool blur = false;
        private TexturePackRepository texturePacks;
        private static Bitmap missingNo = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
        private static List<FileStream> toCleanupStreams = new List<FileStream>();

        static Textures()
        {
            Graphics g = Graphics.FromImage(missingNo);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, 64, 64));
            g.DrawString("missingtex", SystemFonts.DialogFont, Brushes.Black, 1, 10);
            g.Dispose();
        }

        public Textures(TexturePackRepository texturePacks, Options options)
        {
            this.texturePacks = texturePacks;
            this.options = options;
            GCHandle handle = GCHandle.Alloc(this.pixels, GCHandleType.Pinned);
            pixaddr = handle.AddrOfPinnedObject();
        }

        public static void CleanupStreams()
        {
            // Janky way to not leak memory
            // Basically (should) get the job done
            //Console.WriteLine("Cleaning up default texture pack file streams");
            foreach (FileStream stream in toCleanupStreams)
            {
                stream.Close();
                stream.Dispose();
            }
        }

        public static Stream GetAssetsStream(string assetPath)
        {
            string path = Path.Combine(SharedConstants.ASSETS_CLIENT_PATH, assetPath.Substring(1));
            try
            {
                FileStream stream = File.OpenRead(path);
                toCleanupStreams.Add(stream);
                return stream;
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
                MessageBox.Show($"Failed to load the asset: {assetPath}", "Fatal Error");
                Environment.Exit(1);
                return null;
            }
        }

        public static Bitmap GetAssetsBitmap(string assetPath)
        {
            return new Bitmap(Image.FromStream(GetAssetsStream(assetPath)));
        }

        public virtual int[] LoadTexturePixels(string resourceName)
        {
            AbstractTexturePack texturePackBase2 = this.texturePacks.selectedTexturePack;
            int[] pixels = pixelsMap[resourceName];

            if (pixels != null)
            {
                return pixels;
            }
            else
            {
                try
                {
                    if (resourceName.StartsWith("##"))
                    {
                        pixels = this.LoadTexturePixels(this.UnwrapImageByColumns(this.ReadImage(texturePackBase2.GetResourceAsStream(resourceName.Substring(2)))));
                    }
                    else if (resourceName.StartsWith("%clamp%"))
                    {
                        this.clamp = true;
                        pixels = this.LoadTexturePixels(this.ReadImage(texturePackBase2.GetResourceAsStream(resourceName.Substring(7))));
                        this.clamp = false;
                    }
                    else if (resourceName.StartsWith("%blur%"))
                    {
                        this.blur = true;
                        pixels = this.LoadTexturePixels(this.ReadImage(texturePackBase2.GetResourceAsStream(resourceName.Substring(6))));
                        this.blur = false;
                    }
                    else
                    {
                        Stream inputStream7 = texturePackBase2.GetResourceAsStream(resourceName);
                        if (inputStream7 == null)
                        {
                            pixels = this.LoadTexturePixels(missingNo);
                        }
                        else
                        {
                            pixels = this.LoadTexturePixels(this.ReadImage(inputStream7));
                        }
                    }

                    this.pixelsMap[resourceName] = pixels;
                    return pixels;
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                    int[] missingPixels = this.LoadTexturePixels(missingNo);
                    this.pixelsMap[resourceName] = missingPixels;
                    return missingPixels;
                }
                finally 
                {
                    CleanupStreams();
                }
            }
        }

        private int[] LoadTexturePixels(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            int[] pixels = new int[width * height];
            image.GetARGB(0, 0, width, height, pixels, 0, width);
            return pixels;
        }

        private int[] LoadTexturePixels(Bitmap image, int[] pixels)
        {
            int width = image.Width;
            int height = image.Height;
            image.GetARGB(0, 0, width, height, pixels, 0, width);
            return pixels;
        }

        public virtual uint LoadTexture(string string1)
        {
            AbstractTexturePack texturePackBase2 = this.texturePacks.selectedTexturePack;
            
            if (this.idMap.TryGetValue(string1, out uint integer3))
            {
                return integer3;
            }
            else
            {
                try
                {
                    MemoryTracker.GenTextures(this.ib);
                    uint i6 = this.ib[0];
                    if (string1.StartsWith("##"))
                    {
                        this.LoadTexture(this.UnwrapImageByColumns(this.ReadImage(texturePackBase2.GetResourceAsStream(string1.Substring(2)))), i6);
                    }
                    else if (string1.StartsWith("%clamp%"))
                    {
                        this.clamp = true;
                        this.LoadTexture(this.ReadImage(texturePackBase2.GetResourceAsStream(string1.Substring(7))), i6);
                        this.clamp = false;
                    }
                    else if (string1.StartsWith("%blur%"))
                    {
                        this.blur = true;
                        this.LoadTexture(this.ReadImage(texturePackBase2.GetResourceAsStream(string1.Substring(6))), i6);
                        this.blur = false;
                    }
                    else
                    {
                        Stream inputStream7 = texturePackBase2.GetResourceAsStream(string1);
                        if (inputStream7 == null)
                        {
                            this.LoadTexture(missingNo, i6);
                        }
                        else
                        {
                            this.LoadTexture(this.ReadImage(inputStream7), i6);
                        }
                    }

                    this.idMap[string1] = i6;
                    return i6;
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                    MemoryTracker.GenTextures(this.ib);
                    uint i4 = this.ib[0];
                    this.LoadTexture(missingNo, i4);
                    this.idMap[string1] = i4;
                    return i4;
                }
                finally 
                {
                    CleanupStreams();
                }
            }
        }

        private Bitmap UnwrapImageByColumns(Bitmap bufferedImage1)
        {
            int i2 = bufferedImage1.Width / 16;
            Bitmap bufferedImage3 = new Bitmap(16, bufferedImage1.Height * i2, PixelFormat.Format32bppArgb);
            Graphics graphics4 = Graphics.FromImage(bufferedImage3);
            for (int i5 = 0; i5 < i2; ++i5)
            {
                graphics4.DrawImage(bufferedImage1, new Point(-i5 * 16, i5 * bufferedImage1.Height));
            }

            graphics4.Dispose();
            return bufferedImage3;
        }

        public virtual uint LoadTexture(Bitmap bufferedImage1)
        {
            MemoryTracker.GenTextures(this.ib);
            uint i2 = this.ib[0];
            this.LoadTexture(bufferedImage1, i2);
            this.loadedImages[i2] = bufferedImage1;
            return i2;
        }

        //this is more memory efficient since it does not allocate a new array
        private static void PutInt(byte[] buf, int index, int value, bool bigEndian)
        {
            if (bigEndian)
            {
                buf[index + 0] = (byte)((value >> 24) & 0x000000FF);
                buf[index + 1] = (byte)((value >> 16) & 0x000000FF);
                buf[index + 2] = (byte)((value >> 8) & 0x000000FF);
                buf[index + 3] = (byte)((value >> 0) & 0x000000FF);
            }
            else
            {
                buf[index + 0] = (byte)((value >> 0) & 0x000000FF);
                buf[index + 1] = (byte)((value >> 8) & 0x000000FF);
                buf[index + 2] = (byte)((value >> 16) & 0x000000FF);
                buf[index + 3] = (byte)((value >> 24) & 0x000000FF);
            }
        }


        private static int GetInt(byte[] buf, int index, bool bigEndian)
        {
            if (bigEndian)
                return (buf[index + 0] << 24) | (buf[index + 1] << 16) | (buf[index + 2] << 8) | (buf[index + 3] << 0);
            else
                return (buf[index + 0] << 0) | (buf[index + 1] << 8) | (buf[index + 2] << 16) | (buf[index + 3] << 24);
        }

        public virtual void LoadTexture(Bitmap bufferedImage1, uint i2)
        {
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i2);
#if TEXTURES_MIPMAP
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL12C.GL_TEXTURE_MAX_LEVEL, 4);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_NEAREST_MIPMAP_LINEAR);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_NEAREST);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL14C.GL_GENERATE_MIPMAP, (int)GL11C.GL_TRUE);
            GL30.glGenerateMipmap(GL11C.GL_TEXTURE_2D);
#else
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_NEAREST);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_NEAREST);
#endif

            if (this.blur)
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_LINEAR);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_LINEAR);
            }

            if (this.clamp)
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_S, (int)GL11C.GL_CLAMP);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_T, (int)GL11C.GL_CLAMP);
            }
            else
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_S, (int)GL11C.GL_REPEAT);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_T, (int)GL11C.GL_REPEAT);
            }

            int i3 = bufferedImage1.Width;
            int i4 = bufferedImage1.Height;
            int[] i5 = new int[i3 * i4];
            byte[] b6 = new byte[i3 * i4 * 4];
            bufferedImage1.GetARGB(0, 0, i3, i4, i5, 0, i3);
            int i7;
            int i8;
            int i9;
            int i10;
            int i11;
            int i12;
            int i13;
            int i14;
            for (i7 = 0; i7 < i5.Length; ++i7)
            {
                i8 = i5[i7] >> 24 & 255;
                i9 = i5[i7] >> 16 & 255;
                i10 = i5[i7] >> 8 & 255;
                i11 = i5[i7] & 255;
                if (this.options != null && this.options.anaglyph)
                {
                    i12 = (i9 * 30 + i10 * 59 + i11 * 11) / 100;
                    i13 = (i9 * 30 + i10 * 70) / 100;
                    i14 = (i9 * 30 + i11 * 70) / 100;
                    i9 = i12;
                    i10 = i13;
                    i11 = i14;
                }

                b6[i7 * 4 + 0] = (byte)i9;
                b6[i7 * 4 + 1] = (byte)i10;
                b6[i7 * 4 + 2] = (byte)i11;
                b6[i7 * 4 + 3] = (byte)i8;
            }

            Array.Copy(b6, pixels, b6.Length);
            GL11.glTexImage2D(GL11C.GL_TEXTURE_2D, 0, (int)GL11C.GL_RGBA, i3, i4, 0, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
#if TEXTURES_MIPMAP
            for (i7 = 1; i7 <= 4; ++i7)
            {
                i8 = i3 >> i7 - 1;
                i9 = i3 >> i7;
                i10 = i4 >> i7;
                for (i11 = 0; i11 < i9; ++i11)
                {
                    for (i12 = 0; i12 < i10; ++i12)
                    {
                        i13 = GetInt(this.pixels, (i11 * 2 + 0 + (i12 * 2 + 0) * i8) * 4, false);
                        i14 = GetInt(this.pixels, (i11 * 2 + 1 + (i12 * 2 + 0) * i8) * 4, false);
                        int i15 = GetInt(this.pixels, (i11 * 2 + 1 + (i12 * 2 + 1) * i8) * 4, false);
                        int i16 = GetInt(this.pixels, (i11 * 2 + 0 + (i12 * 2 + 1) * i8) * 4, false);
                        int i17 = CrispBlend(CrispBlend(i13, i14), CrispBlend(i15, i16));
                        PutInt(this.pixels, (i11 + i12 * i9) * 4, i17, false);
                    }
                }

                GL11.glTexImage2D(GL11C.GL_TEXTURE_2D, i7, (int)GL11C.GL_RGBA, i9, i10, 0, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
            }
#endif
        }

        public virtual void ReplaceTexture(int[] i1, int i2, int i3, uint i4)
        {
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i4);
#if TEXTURES_MIPMAP
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL12C.GL_TEXTURE_MAX_LEVEL, 4);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_NEAREST_MIPMAP_LINEAR);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_NEAREST);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL14C.GL_GENERATE_MIPMAP, (int)GL11C.GL_TRUE);
            GL30.glGenerateMipmap(GL11C.GL_TEXTURE_2D);

#else
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_NEAREST);
            GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_NEAREST);
#endif

            if (this.blur)
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MIN_FILTER, (int)GL11C.GL_LINEAR);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_MAG_FILTER, (int)GL11C.GL_LINEAR);
            }

            if (this.clamp)
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_S, (int)GL11C.GL_CLAMP);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_T, (int)GL11C.GL_CLAMP);
            }
            else
            {
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_S, (int)GL11C.GL_REPEAT);
                GL11.glTexParameteri(GL11C.GL_TEXTURE_2D, GL11C.GL_TEXTURE_WRAP_T, (int)GL11C.GL_REPEAT);
            }

            byte[] b5 = new byte[i2 * i3 * 4];
            for (int i6 = 0; i6 < i1.Length; ++i6)
            {
                int i7 = i1[i6] >> 24 & 255;
                int i8 = i1[i6] >> 16 & 255;
                int i9 = i1[i6] >> 8 & 255;
                int i10 = i1[i6] & 255;
                if (this.options != null && this.options.anaglyph)
                {
                    int i11 = (i8 * 30 + i9 * 59 + i10 * 11) / 100;
                    int i12 = (i8 * 30 + i9 * 70) / 100;
                    int i13 = (i8 * 30 + i10 * 70) / 100;
                    i8 = i11;
                    i9 = i12;
                    i10 = i13;
                }

                b5[i6 * 4 + 0] = (byte)i8;
                b5[i6 * 4 + 1] = (byte)i9;
                b5[i6 * 4 + 2] = (byte)i10;
                b5[i6 * 4 + 3] = (byte)i7;
            }

            Array.Copy(b5, pixels, b5.Length);
            GL11.glTexSubImage2D(GL11C.GL_TEXTURE_2D, 0, 0, 0, i2, i3, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
        }

        public virtual void ReleaseTexture(uint i1)
        {
            this.loadedImages.Remove(i1);
            int pos = 0;
            this.ib[pos++] = i1;
            //is this correct?
            GL11.glDeleteTextures(ib.Length - pos, ib);
        }

        public virtual uint LoadMemTexture(string url, string fallback)
        {
            MemTexture memTexture = url == null ? null : this.memTextures[url];
            if (memTexture != null && memTexture.image != null && !memTexture.textureSetupComplete)
            {
                if (memTexture.textureName == uint.MaxValue)
                {
                    memTexture.textureName = this.LoadTexture(memTexture.image);
                }
                else
                {
                    this.LoadTexture(memTexture.image, memTexture.textureName);
                }

                memTexture.textureSetupComplete = true;
            }

            return memTexture != null && memTexture.textureName != uint.MaxValue ? memTexture.textureName : (fallback == null ? uint.MaxValue : this.LoadTexture(fallback));
        }

        public virtual MemTexture AddMemTexture(string string1, IMemTextureProcessor imageBuffer2)
        {
            MemTexture threadDownloadImageData3 = this.memTextures[string1];
            if (threadDownloadImageData3 == null)
            {
                this.memTextures[string1] = new MemTexture(string1, imageBuffer2);
            }
            else
            {
                ++threadDownloadImageData3.referenceCount;
            }

            return threadDownloadImageData3;
        }

        public virtual void RemoveMemTexture(string string1)
        {
            MemTexture threadDownloadImageData2 = this.memTextures[string1];
            if (threadDownloadImageData2 != null)
            {
                --threadDownloadImageData2.referenceCount;
                if (threadDownloadImageData2.referenceCount == 0)
                {
                    if (threadDownloadImageData2.textureName >= 0)
                    {
                        this.ReleaseTexture(threadDownloadImageData2.textureName);
                    }

                    this.memTextures.Remove(string1);
                }
            }
        }

        public virtual void AddDynamicTexture(DynamicTexture textureFX1)
        {
            this.dynamicTextures.Add(textureFX1);
            textureFX1.OnTick();
        }

        public virtual void Tick()
        {
            int i1;
            DynamicTexture textureFX2;
            int i3;
            int i4;
#if TEXTURES_MIPMAP
            int i5, i6, i7, i8, i9, i10, i11, i12;
#endif
            for (i1 = 0; i1 < this.dynamicTextures.Count; ++i1)
            {
                textureFX2 = this.dynamicTextures[i1];
                textureFX2.anaglyphEnabled = this.options.anaglyph;
                textureFX2.OnTick();
                Array.Copy(textureFX2.imageData, pixels, textureFX2.imageData.Length);
                textureFX2.BindImage(this);
                for (i3 = 0; i3 < textureFX2.tileSize; ++i3)
                {
                    for (i4 = 0; i4 < textureFX2.tileSize; ++i4)
                    {
                        GL11.glTexSubImage2D(GL11C.GL_TEXTURE_2D, 0, textureFX2.iconIndex % 16 * 16 + i3 * 16, textureFX2.iconIndex / 16 * 16 + i4 * 16, 16, 16, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
#if TEXTURES_MIPMAP
                        for (i5 = 1; i5 <= 4; ++i5)
                        {
                            i6 = 16 >> i5 - 1;
                            i7 = 16 >> i5;
                            for (i8 = 0; i8 < i7; ++i8)
                            {
                                for (i9 = 0; i9 < i7; ++i9)
                                {
                                    i10 = GetInt(this.pixels, (i8 * 2 + 0 + (i9 * 2 + 0) * i6) * 4, false);
                                    i11 = GetInt(this.pixels, (i8 * 2 + 1 + (i9 * 2 + 0) * i6) * 4, false);
                                    i12 = GetInt(this.pixels, (i8 * 2 + 1 + (i9 * 2 + 1) * i6) * 4, false);
                                    int i13 = GetInt(this.pixels, (i8 * 2 + 0 + (i9 * 2 + 1) * i6) * 4, false);
                                    int i14 = AverageColor(AverageColor(i10, i11), AverageColor(i12, i13));
                                    PutInt(this.pixels, (i8 + i9 * i7) * 4, i14, false);
                                }
                            }

                            GL11.glTexSubImage2D(GL11C.GL_TEXTURE_2D, i5, textureFX2.iconIndex % 16 * i7, textureFX2.iconIndex / 16 * i7, i7, i7, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
                        }
#endif
                    }
                }
            }

            for (i1 = 0; i1 < this.dynamicTextures.Count; ++i1)
            {
                textureFX2 = this.dynamicTextures[i1];
                if (textureFX2.textureId > 0)
                {
                    //this.pixels.Clear();
                    //this.pixels.Put(textureFX2.imageData);
                    //this.pixels.Position(0).Limit(textureFX2.imageData.Length);
                    Array.Copy(textureFX2.imageData, pixels, textureFX2.imageData.Length);
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, textureFX2.textureId);
                    GL11.glTexSubImage2D(GL11C.GL_TEXTURE_2D, 0, 0, 0, 16, 16, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
#if TEXTURES_MIPMAP
                    for (i3 = 1; i3 <= 4; ++i3)
                    {
                        i4 = 16 >> i3 - 1;
                        i5 = 16 >> i3;
                        for (i6 = 0; i6 < i5; ++i6)
                        {
                            for (i7 = 0; i7 < i5; ++i7)
                            {

                                i8 = GetInt(this.pixels, (i6 * 2 + 0 + (i7 * 2 + 0) * i4) * 4, false);
                                i9 = GetInt(this.pixels, (i6 * 2 + 1 + (i7 * 2 + 0) * i4) * 4, false);
                                i10 = GetInt(this.pixels, (i6 * 2 + 1 + (i7 * 2 + 1) * i4) * 4, false);
                                i11 = GetInt(this.pixels, (i6 * 2 + 0 + (i7 * 2 + 1) * i4) * 4, false);
                                i12 = AverageColor(AverageColor(i8, i9), AverageColor(i10, i11));
                                PutInt(this.pixels, (i6 + i7 * i5) * 4, i12, false);
                            }
                        }

                        GL11.glTexSubImage2D(GL11C.GL_TEXTURE_2D, i3, 0, 0, i5, i5, GL11C.GL_RGBA, GL11C.GL_UNSIGNED_BYTE, this.pixaddr);
                    }
#endif
                }
            }
        }

        private static int AverageColor(int i1, int i2)
        {
            int i3 = (i1 & unchecked((int)0xFF000000)) >> 24 & 255; ;
            int i4 = (i2 & unchecked((int)0xFF000000)) >> 24 & 255; ;
            return (i3 + i4 >> 1 << 24) + ((i1 & 16711422) + (i2 & 16711422) >> 1);
        }

        private static int CrispBlend(int i1, int i2)
        {
            int i3 = (i1 & unchecked((int)0xFF000000)) >> 24 & 255;
            int i4 = (i2 & unchecked((int)0xFF000000)) >> 24 & 255; ;
            short s5 = 255;
            if (i3 + i4 == 0)
            {
                i3 = 1;
                i4 = 1;
                s5 = 0;
            }

            int i6 = (i1 >> 16 & 255) * i3;
            int i7 = (i1 >> 8 & 255) * i3;
            int i8 = (i1 & 255) * i3;
            int i9 = (i2 >> 16 & 255) * i4;
            int i10 = (i2 >> 8 & 255) * i4;
            int i11 = (i2 & 255) * i4;
            int i12 = (i6 + i9) / (i3 + i4);
            int i13 = (i7 + i10) / (i3 + i4);
            int i14 = (i8 + i11) / (i3 + i4);
            return s5 << 24 | i12 << 16 | i13 << 8 | i14;
        }

        public virtual void ReloadAll()
        {
            AbstractTexturePack texturePackBase1 = this.texturePacks.selectedTexturePack;
            IEnumerator<uint> iterator2 = this.loadedImages.Keys.GetEnumerator();
            Bitmap bufferedImage4;
            while (iterator2.MoveNext())
            {
                uint i3 = iterator2.Current;
                bufferedImage4 = this.loadedImages[i3];
                this.LoadTexture(bufferedImage4, i3);
            }

            MemTexture threadDownloadImageData8;
            for (IEnumerator<MemTexture> iterator2_0 = this.memTextures.Values.GetEnumerator(); iterator2_0.MoveNext(); threadDownloadImageData8.textureSetupComplete = false)
            {
                threadDownloadImageData8 = iterator2_0.Current;
            }

            IEnumerator<string> iterator2_1 = this.idMap.Keys.GetEnumerator();
            string string9;
            while (iterator2_1.MoveNext())
            {
                string9 = iterator2_1.Current;
                try
                {
                    if (string9.StartsWith("##"))
                    {
                        bufferedImage4 = this.UnwrapImageByColumns(this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(2))));
                    }
                    else if (string9.StartsWith("%clamp%"))
                    {
                        this.clamp = true;
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(7)));
                    }
                    else if (string9.StartsWith("%blur%"))
                    {
                        this.blur = true;
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(6)));
                    }
                    else
                    {
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9));
                    }

                    uint i5 = this.idMap[string9];
                    this.LoadTexture(bufferedImage4, i5);
                    this.blur = false;
                    this.clamp = false;
                }
                catch (IOException iOException7)
                {
                    iOException7.PrintStackTrace();
                }
            }

            IEnumerator<string> iterator2_2 = this.pixelsMap.Keys.GetEnumerator();
            while (iterator2_2.MoveNext())
            {
                string9 = iterator2_2.Current;
                try
                {
                    if (string9.StartsWith("##"))
                    {
                        bufferedImage4 = this.UnwrapImageByColumns(this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(2))));
                    }
                    else if (string9.StartsWith("%clamp%"))
                    {
                        this.clamp = true;
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(7)));
                    }
                    else if (string9.StartsWith("%blur%"))
                    {
                        this.blur = true;
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9.Substring(6)));
                    }
                    else
                    {
                        bufferedImage4 = this.ReadImage(texturePackBase1.GetResourceAsStream(string9));
                    }

                    this.LoadTexturePixels(bufferedImage4, this.pixelsMap[string9]);
                    this.blur = false;
                    this.clamp = false;
                }
                catch (IOException iOException6)
                {
                    iOException6.PrintStackTrace();
                }
            }

            CleanupStreams();
        }

        private Bitmap ReadImage(Stream inputStream1)
        {
            Bitmap bufferedImage2 = new Bitmap(Image.FromStream(inputStream1));
            inputStream1.Dispose();
            return bufferedImage2;
        }

        public virtual void Bind(uint texture)
        {
            if (texture >= 0)
            {
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, texture);
            }
        }

        public virtual void Bind(string resName)
        {
            this.Bind(this.LoadTexture(resName));
        }
    }
}
