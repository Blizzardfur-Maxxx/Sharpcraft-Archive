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
    public class GuiFurnace : GuiContainer
    {
        private TileEntityFurnace furnaceInventory;

        public GuiFurnace(Inventory inventoryPlayer1, TileEntityFurnace tileEntityFurnace2)
        : base(new FurnaceMenu(inventoryPlayer1, tileEntityFurnace2))
        {
            this.furnaceInventory = tileEntityFurnace2;
        }

        protected override void DrawGuiContainerForegroundLayer()
        {
            this.font.DrawString("Furnace", 60, 6, 4210752);
            this.font.DrawString("Inventory", 8, this.ySize - 96 + 2, 4210752);
        }

        protected override void DrawGuiContainerBackgroundLayer(float f1)
        {
            uint i2 = this.mc.textures.LoadTexture("/gui/furnace.png");
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            this.mc.textures.Bind(i2);
            int i3 = (this.width - this.xSize) / 2;
            int i4 = (this.height - this.ySize) / 2;
            this.DrawTexturedModalRect(i3, i4, 0, 0, this.xSize, this.ySize);
            int i5;
            if (this.furnaceInventory.IsBurning())
            {
                i5 = this.furnaceInventory.GetBurnTimeRemainingScaled(12);
                this.DrawTexturedModalRect(i3 + 56, i4 + 36 + 12 - i5, 176, 12 - i5, 14, i5 + 2);
            }

            i5 = this.furnaceInventory.GetCookProgressScaled(24);
            this.DrawTexturedModalRect(i3 + 79, i4 + 34, 176, 14, i5 + 1, 16);
        }
    }
}
