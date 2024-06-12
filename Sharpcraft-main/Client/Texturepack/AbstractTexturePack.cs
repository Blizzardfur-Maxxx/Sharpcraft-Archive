using SharpCraft.Client.Renderer;
using SharpCraft.Core.World.Entities.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Texturepack
{
    public abstract class AbstractTexturePack
    {
        public string texturePackFileName;
        public string firstDescriptionLine;
        public string secondDescriptionLine;
        public string field_6488_d;
 
        public virtual void OpenTexturePackFile()
        {
        }

        public virtual void CloseTexturePackFile()
        {
        }

        public virtual void Prepare(Client instance)
        {
        }

        public virtual void UnbindThumbnailTexture(Client instance)
        {
        }

        public virtual void BindThumbnailTexture(Client instance)
        {
        }

        public virtual Stream GetResourceAsStream(string string1)
        {
            return Textures.GetAssetsStream(string1);
        }
    }
}
