using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Ptexture
{
    public class DynamicTexture
    {
        public byte[] imageData = new byte[16*16*4];
        public int iconIndex;
        public bool anaglyphEnabled = false;
        public uint textureId = 0;
        public int tileSize = 1;
        public int tileImage = 0;
        public DynamicTexture(int i1)
        {
            iconIndex = i1;
        }

        public virtual void OnTick()
        {
        }

        public virtual void BindImage(Textures renderEngine1)
        {
            if (tileImage == 0)
            {
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, renderEngine1.LoadTexture("/terrain.png"));
            }
            else if (tileImage == 1)
            {
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, renderEngine1.LoadTexture("/gui/items.png"));
            }
        }
    }
}
