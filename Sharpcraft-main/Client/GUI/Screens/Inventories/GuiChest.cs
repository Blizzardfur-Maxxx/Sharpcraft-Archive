using LWCSGL.OpenGL;
using SharpCraft.Core.World;
using SharpCraft.Core.World.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public class GuiChest : GuiContainer
    {
        private IContainer upperChestInventory;
        private IContainer lowerChestInventory;
        private int inventoryRows = 0;
        public GuiChest(IContainer iInventory1, IContainer iInventory2) : base(new ContainerMenu(iInventory1, iInventory2))
        {
            this.upperChestInventory = iInventory1;
            this.lowerChestInventory = iInventory2;
            this.field_948_f = false;
            short s3 = 222;
            int i4 = s3 - 108;
            this.inventoryRows = iInventory2.GetContainerSize() / 9;
            this.ySize = i4 + this.inventoryRows * 18;
        }

        protected override void DrawGuiContainerForegroundLayer()
        {
            this.font.DrawString(this.lowerChestInventory.GetName(), 8, 6, 4210752);
            this.font.DrawString(this.upperChestInventory.GetName(), 8, this.ySize - 96 + 2, 4210752);
        }

        protected override void DrawGuiContainerBackgroundLayer(float f1)
        {
            uint i2 = this.mc.textures.LoadTexture("/gui/container.png");
            GL11.glColor4f(1F, 1F, 1F, 1F);
            this.mc.textures.Bind(i2);
            int i3 = (this.width - this.xSize) / 2;
            int i4 = (this.height - this.ySize) / 2;
            this.DrawTexturedModalRect(i3, i4, 0, 0, this.xSize, this.inventoryRows * 18 + 17);
            this.DrawTexturedModalRect(i3, i4 + this.inventoryRows * 18 + 17, 0, 126, this.xSize, 96);
        }
    }
}
