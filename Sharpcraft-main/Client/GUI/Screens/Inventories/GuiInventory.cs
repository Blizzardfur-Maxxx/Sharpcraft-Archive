using LWCSGL.OpenGL;
using SharpCraft.Client.GUI.Screens.Achievements;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public class GuiInventory : GuiContainer
    {
        private float xSize_lo;
        private float ySize_lo;
        public GuiInventory(Player entityPlayer1) : base(entityPlayer1.inventorySlots)
        {
            this.field_948_f = true;
            entityPlayer1.AddStat(AchievementList.openInventory, 1);
        }

        public override void InitGui()
        {
            this.buttons.Clear();
        }

        protected override void DrawGuiContainerForegroundLayer()
        {
            this.font.DrawString("Crafting", 86, 16, 4210752);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            base.DrawScreen(i1, i2, f3);
            this.xSize_lo = i1;
            this.ySize_lo = i2;
        }

        protected override void DrawGuiContainerBackgroundLayer(float f1)
        {
            uint i2 = this.mc.textures.LoadTexture("/gui/inventory.png");
            GL11.glColor4f(1F, 1F, 1F, 1F);
            this.mc.textures.Bind(i2);
            int i3 = (this.width - this.xSize) / 2;
            int i4 = (this.height - this.ySize) / 2;
            this.DrawTexturedModalRect(i3, i4, 0, 0, this.xSize, this.ySize);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            GL11.glPushMatrix();
            GL11.glTranslatef(i3 + 51, i4 + 75, 50F);
            float f5 = 30F;
            GL11.glScalef(-f5, f5, f5);
            GL11.glRotatef(180F, 0F, 0F, 1F);
            float f6 = this.mc.player.renderYawOffset;
            float f7 = this.mc.player.yaw;
            float f8 = this.mc.player.pitch;
            float f9 = i3 + 51 - this.xSize_lo;
            float f10 = i4 + 75 - 50 - this.ySize_lo;
            GL11.glRotatef(135F, 0F, 1F, 0F);
            Light.TurnOn();
            GL11.glRotatef(-135F, 0F, 1F, 0F);
            GL11.glRotatef(-((float)Math.Atan(f10 / 40F)) * 20F, 1F, 0F, 0F);
            this.mc.player.renderYawOffset = (float)Math.Atan(f9 / 40F) * 20F;
            this.mc.player.yaw = (float)Math.Atan(f9 / 40F) * 40F;
            this.mc.player.pitch = -((float)Math.Atan(f10 / 40F)) * 20F;
            this.mc.player.entityBrightness = 1F;
            GL11.glTranslatef(0F, this.mc.player.yOffset, 0F);
            //fear not pnp for i have come to liberate you from generic hell -dart
            EntityRenderDispatcher.instance.playerViewY = 180F;
            EntityRenderDispatcher.instance.RenderEntityWithPosYaw(this.mc.player, 0, 0, 0, 0F, 1F);
            this.mc.player.entityBrightness = 0F;
            this.mc.player.renderYawOffset = f6;
            this.mc.player.yaw = f7;
            this.mc.player.pitch = f8;
            GL11.glPopMatrix();
            Light.TurnOff();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
        }
    }
}
