using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Texturepack
{
    public class DefaultTexturePack : AbstractTexturePack
    {
        private uint texturePackName = uint.MaxValue;
        private Bitmap texturePackThumbnail;

        public DefaultTexturePack()
        {
            this.texturePackFileName = "Default";
            this.firstDescriptionLine = "The default look of SharpCraft";

            try
            {
                this.texturePackThumbnail = Textures.GetAssetsBitmap("/pack.png");
            }
            catch (IOException iOException2)
            {
                iOException2.PrintStackTrace();
            }
        }

        public override void UnbindThumbnailTexture(Client instance)
        {
            if (this.texturePackThumbnail != null)
            {
                instance.textures.ReleaseTexture(this.texturePackName);
            }
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
    }
}
