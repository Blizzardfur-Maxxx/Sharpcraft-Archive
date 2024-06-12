using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Texturepack
{
    public class FileTexturePack : AbstractTexturePack
    {
        private JZipFile texturePackZipFile;
        private uint texturePackName = uint.MaxValue;
        private Bitmap texturePackThumbnail;
        private JFile texturePackFile;

        public FileTexturePack(JFile file1)
        {
            this.texturePackFileName = file1.GetName();
            this.texturePackFile = file1;
        }

        private string TruncateString(string string1)
        {
            if (string1 != null && string1.Length > 34)
            {
                string1 = string1.Substring(0, 34);
            }

            return string1;
        }

        public override void Prepare(Client instance)
        {
            JZipFile zipFile2 = null;
            Stream inputStream3 = null;
            try
            {
                zipFile2 = new JZipFile(this.texturePackFile);
                try
                {
                    inputStream3 = zipFile2.GetInputStream("pack.txt");
                    StreamReader bufferedReader4 = new StreamReader(inputStream3);
                    this.firstDescriptionLine = this.TruncateString(bufferedReader4.ReadLine());
                    this.secondDescriptionLine = this.TruncateString(bufferedReader4.ReadLine());
                    bufferedReader4.Dispose();
                    inputStream3.Dispose();
                }
                catch
                {
                }

                try
                {
                    inputStream3 = zipFile2.GetInputStream("pack.png");
                    this.texturePackThumbnail = new Bitmap(Image.FromStream(inputStream3));
                    inputStream3.Dispose();
                }
                catch
                {
                }

                zipFile2.Dispose();
            }
            catch (Exception exception21)
            {
                exception21.PrintStackTrace();
            }
            finally
            {
                try
                {
                    inputStream3.Dispose();
                }
                catch
                {
                }

                try
                {
                    zipFile2.Dispose();
                }
                catch
                {
                }
            }
        }

        public override void UnbindThumbnailTexture(Client instance)
        {
            if (this.texturePackThumbnail != null)
            {
                instance.textures.ReleaseTexture(this.texturePackName);
            }

            this.CloseTexturePackFile();
        }

        public override void BindThumbnailTexture(Client instance)
        {
            if (this.texturePackThumbnail != null && this.texturePackName == uint.MaxValue)
            {
                this.texturePackName = instance.textures.LoadTexture(this.texturePackThumbnail);
            }

            if (this.texturePackThumbnail != null)
            {
                instance.textures.Bind(this.texturePackName);
            }
            else
            {
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, instance.textures.LoadTexture("/gui/unknown_pack.png"));
            }
        }

        public override void OpenTexturePackFile()
        {
            try
            {
                this.texturePackZipFile = new JZipFile(this.texturePackFile);
            }
            catch
            {
            }
        }

        public override void CloseTexturePackFile()
        {
            try
            {
                this.texturePackZipFile.Dispose();
            }
            catch
            {
            }

            this.texturePackZipFile = null;
        }

        public override Stream GetResourceAsStream(string string1)
        {
            try
            {
                return this.texturePackZipFile.GetInputStream(string1.Substring(1));
            }
            catch
            {
            }

            return base.GetResourceAsStream(string1);
        }
    }
}
