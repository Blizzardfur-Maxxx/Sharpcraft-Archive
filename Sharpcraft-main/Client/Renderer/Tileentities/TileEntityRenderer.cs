using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.GUI;

namespace SharpCraft.Client.Renderer.Tileentities
{
    public abstract class TileEntityRenderer
    {
        protected TileEntityRenderDispatcher dispatcher;
        
        public abstract void RenderTileEntityAt(TileEntity tileEntity1, double d2, double d4, double d6, float f8);
        
        protected void BindTextureByName(string string1)
        {
            Textures renderEngine2 = this.dispatcher.renderEngine;
            renderEngine2.Bind(renderEngine2.LoadTexture(string1));
        }

        public void SetTileEntityRenderer(TileEntityRenderDispatcher tileEntityRenderer1)
        {
            this.dispatcher = tileEntityRenderer1;
        }

        public virtual void SetLevel(Level world1)
        {
        }

        public Font GetFontRenderer()
        {
            return this.dispatcher.GetFontRenderer();
        }
    }
    
    public abstract class TileEntityRenderer<T> : TileEntityRenderer where T : TileEntity
    {
        //fml
        public override void RenderTileEntityAt(TileEntity tileEntity1, double d2, double d4, double d6, float f8)
        {
            RenderTileEntityAt((T)tileEntity1, d2, d4, d6, f8);
        }

        public abstract void RenderTileEntityAt(T tileEntity1, double d2, double d4, double d6, float f8);
    }
}
