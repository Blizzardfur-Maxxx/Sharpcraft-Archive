using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.i18n;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Inventory;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public abstract class GuiContainer : Screen
    {
        private static RenderItem itemRenderer = new RenderItem();
        protected int xSize = 176;
        protected int ySize = 166;
        public AbstractContainerMenu inventorySlots;
        public GuiContainer(AbstractContainerMenu container1)
        {
            this.inventorySlots = container1;
        }

        public override void InitGui()
        {
            base.InitGui();
            this.mc.player.curCraftingInventory = this.inventorySlots;
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            int i4 = (this.width - this.xSize) / 2;
            int i5 = (this.height - this.ySize) / 2;
            this.DrawGuiContainerBackgroundLayer(f3);
            GL11.glPushMatrix();
            GL11.glRotatef(120F, 1F, 0F, 0F);
            Light.TurnOn();
            GL11.glPopMatrix();
            GL11.glPushMatrix();
            GL11.glTranslatef(i4, i5, 0F);
            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            Slot slot6 = null;
            int i9;
            int i10;
            for (int i7 = 0; i7 < this.inventorySlots.slots.Count; ++i7)
            {
                Slot slot8 = this.inventorySlots.slots[i7];
                this.DrawSlotInventory(slot8);
                if (this.GetIsMouseOverSlot(slot8, i1, i2))
                {
                    slot6 = slot8;
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glDisable(GL11C.GL_DEPTH_TEST);
                    i9 = slot8.xDisplayPosition;
                    i10 = slot8.yDisplayPosition;
                    this.DrawGradientRect(i9, i10, i9 + 16, i10 + 16, -2130706433, -2130706433);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    GL11.glEnable(GL11C.GL_DEPTH_TEST);
                }
            }

            Inventory inventoryPlayer12 = this.mc.player.inventory;
            if (inventoryPlayer12.GetItem() != null)
            {
                GL11.glTranslatef(0F, 0F, 32F);
                itemRenderer.RenderItemIntoGUI(this.font, this.mc.textures, inventoryPlayer12.GetItem(), i1 - i4 - 8, i2 - i5 - 8);
                itemRenderer.RenderItemOverlayIntoGUI(this.font, this.mc.textures, inventoryPlayer12.GetItem(), i1 - i4 - 8, i2 - i5 - 8);
            }

            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            Light.TurnOff();
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            this.DrawGuiContainerForegroundLayer();
            if (inventoryPlayer12.GetItem() == null && slot6 != null && slot6.HasItem())
            {
                string string13 = ("" + I18N.Instance.TranslateNamedKey(slot6.GetItem().GetItemName())).Trim();
                if (string13.Length > 0)
                {
                    i9 = i1 - i4 + 12;
                    i10 = i2 - i5 - 12;
                    int i11 = this.font.GetStringWidth(string13);
                    this.DrawGradientRect(i9 - 3, i10 - 3, i9 + i11 + 3, i10 + 8 + 3, -1073741824, -1073741824);
                    this.font.DrawStringWithShadow(string13, i9, i10, unchecked((uint)-1)); //too lazy
                }
            }

            GL11.glPopMatrix();
            base.DrawScreen(i1, i2, f3);
            GL11.glEnable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
        }

        protected virtual void DrawGuiContainerForegroundLayer()
        {
        }

        protected abstract void DrawGuiContainerBackgroundLayer(float f1);
        private void DrawSlotInventory(Slot slot1)
        {
            int i2 = slot1.xDisplayPosition;
            int i3 = slot1.yDisplayPosition;
            ItemInstance itemStack4 = slot1.GetItem();
            if (itemStack4 == null)
            {
                int i5 = slot1.GetBackgroundIconIndex();
                if (i5 >= 0)
                {
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    this.mc.textures.Bind(this.mc.textures.LoadTexture("/gui/items.png"));
                    this.DrawTexturedModalRect(i2, i3, i5 % 16 * 16, i5 / 16 * 16, 16, 16);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    return;
                }
            }

            itemRenderer.RenderItemIntoGUI(this.font, this.mc.textures, itemStack4, i2, i3);
            itemRenderer.RenderItemOverlayIntoGUI(this.font, this.mc.textures, itemStack4, i2, i3);
        }

        private Slot GetSlotAtPosition(int i1, int i2)
        {
            for (int i3 = 0; i3 < this.inventorySlots.slots.Count; ++i3)
            {
                Slot slot4 = this.inventorySlots.slots[i3];
                if (this.GetIsMouseOverSlot(slot4, i1, i2))
                {
                    return slot4;
                }
            }

            return null;
        }

        private bool GetIsMouseOverSlot(Slot slot1, int i2, int i3)
        {
            int i4 = (this.width - this.xSize) / 2;
            int i5 = (this.height - this.ySize) / 2;
            i2 -= i4;
            i3 -= i5;
            return i2 >= slot1.xDisplayPosition - 1 && i2 < slot1.xDisplayPosition + 16 + 1 && i3 >= slot1.yDisplayPosition - 1 && i3 < slot1.yDisplayPosition + 16 + 1;
        }

        protected override void MouseClicked(int i1, int i2, int i3)
        {
            base.MouseClicked(i1, i2, i3);
            if (i3 == 0 || i3 == 1)
            {
                Slot slot4 = this.GetSlotAtPosition(i1, i2);
                int i5 = (this.width - this.xSize) / 2;
                int i6 = (this.height - this.ySize) / 2;
                bool z7 = i1 < i5 || i2 < i6 || i1 >= i5 + this.xSize || i2 >= i6 + this.ySize;
                int i8 = -1;
                if (slot4 != null)
                {
                    i8 = slot4.id;
                }

                if (z7)
                {
                    i8 = -999;
                }

                if (i8 != -1)
                {
                    bool z9 = i8 != -999 && Keyboard.IsKeyDown(VirtualKey.ShiftKey);
                    this.mc.gameMode.Func_27174_a(this.inventorySlots.windowId, i8, i3, z9, this.mc.player);
                }
            }
        }

        protected override void MouseMovedOrUp(int i1, int i2, int i3)
        {
            if (i3 == 0)
            {
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            if (i2 == VirtualKey.Escape || i2 == this.mc.options.keyBindInventory.keyCode)
            {
                this.mc.player.AccessorCloseScreen();
            }
        }

        public override void OnGuiClosed()
        {
            if (this.mc.player != null)
            {
                this.mc.gameMode.Func_20086_a(this.inventorySlots.windowId, this.mc.player);
            }
        }

        public override bool DoesGuiPauseGame()
        {
            return false;
        }

        public override void UpdateScreen()
        {
            base.UpdateScreen();
            if (!this.mc.player.IsEntityAlive() || this.mc.player.isDead)
            {
                this.mc.player.AccessorCloseScreen();
            }
        }
    }
}
