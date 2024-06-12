using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public class GuiDispenser : GuiContainer
    {
        public GuiDispenser(Inventory inventoryPlayer1, TileEntityDispenser tileEntityDispenser2)
        : base(new TrapMenu(inventoryPlayer1, tileEntityDispenser2))
        {
        }

        protected override void DrawGuiContainerForegroundLayer()
        {
            this.font.DrawString("Dispenser", 60, 6, 4210752);
            this.font.DrawString("Inventory", 8, this.ySize - 96 + 2, 4210752);
        }

        protected override void DrawGuiContainerBackgroundLayer(float f1)
        {
            uint i2 = this.mc.textures.LoadTexture("/gui/trap.png");
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            this.mc.textures.Bind(i2);
            int i3 = (this.width - this.xSize) / 2;
            int i4 = (this.height - this.ySize) / 2;
            this.DrawTexturedModalRect(i3, i4, 0, 0, this.xSize, this.ySize);
        }
    }
}
