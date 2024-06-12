using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public class GuiCrafting : GuiContainer
    {
        public GuiCrafting(Inventory inventoryPlayer1, Level world2, int i3, int i4, int i5)
            : base(new CraftingMenu(inventoryPlayer1, world2, i3, i4, i5))
        {
        }

        public override void OnGuiClosed()
        {
            base.OnGuiClosed();
            this.inventorySlots.Removed(this.mc.player);
        }

        protected override void DrawGuiContainerForegroundLayer()
        {
            this.font.DrawString("Crafting", 28, 6, 4210752);
            this.font.DrawString("Inventory", 8, this.ySize - 96 + 2, 4210752);
        }

        protected override void DrawGuiContainerBackgroundLayer(float f1)
        {
            uint i2 = this.mc.textures.LoadTexture("/gui/crafting.png");
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            this.mc.textures.Bind(i2);
            int i3 = (this.width - this.xSize) / 2;
            int i4 = (this.height - this.ySize) / 2;
            this.DrawTexturedModalRect(i3, i4, 0, 0, this.xSize, this.ySize);
        }
    }
}
