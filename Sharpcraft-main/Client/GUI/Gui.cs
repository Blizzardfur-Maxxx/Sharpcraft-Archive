using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SharpCraft.Client.GUI.Screens.Multiplayer;
using SharpCraft.Client.Renderer.Entities;
using Microsoft.VisualBasic.Devices;

namespace SharpCraft.Client.GUI
{
    public class Gui : GuiComponent
    {
        private static RenderItem itemRenderer = new RenderItem();
        private List<GuiMessage> chatMessageList = new List<GuiMessage>();
        private JRandom rand = new JRandom();
        private Client mc;
        public string ununsedTabPlayer = null;
        private int updateCounter = 0;
        private string recordPlaying = "";
        private int recordPlayingUpFor = 0;
        private bool field_22065_l = false;
        public float damageGuiPartialTime;
        float prevVignetteBrightness = 1F;
        private ComputerInfo computerInfo = new ComputerInfo();

        public Gui(Client instance)
        {
            this.mc = instance;
        }

        public virtual void RenderGameOverlay(float f1, bool z2, int i3, int i4)
        {
            GuiScale scaledResolution5 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
            int i6 = scaledResolution5.GetWidth();
            int i7 = scaledResolution5.GetHeight();
            Font font = this.mc.font;
            this.mc.entityRenderer.Func_905_b();
            GL11.glEnable(GL11C.GL_BLEND);
            if (Client.IsFancyGraphicsEnabled())
            {
                this.RenderVignette(this.mc.player.GetEntityBrightness(f1), i6, i7);
            }

            ItemInstance itemStack9 = this.mc.player.inventory.ArmorItemInSlot(3);
            if (!this.mc.options.thirdPersonView && itemStack9 != null && itemStack9.itemID == Tile.pumpkin.id)
            {
                this.RenderPumpkinBlur(i6, i7);
            }

            float f10 = this.mc.player.prevTimeInPortal + (this.mc.player.timeInPortal - this.mc.player.prevTimeInPortal) * f1;
            if (f10 > 0F)
            {
                this.RenderPortalOverlay(f10, i6, i7);
            }

            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/gui/gui.png"));
            Inventory inventoryPlayer11 = this.mc.player.inventory;
            this.zLevel = -90F;
            this.DrawTexturedModalRect(i6 / 2 - 91, i7 - 22, 0, 0, 182, 22);
            this.DrawTexturedModalRect(i6 / 2 - 91 - 1 + inventoryPlayer11.currentItem * 20, i7 - 22 - 1, 0, 22, 24, 22);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/gui/icons.png"));
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_ONE_MINUS_DST_COLOR, GL11C.GL_ONE_MINUS_SRC_COLOR);
            this.DrawTexturedModalRect(i6 / 2 - 7, i7 / 2 - 7, 0, 0, 16, 16);
            GL11.glDisable(GL11C.GL_BLEND);
            bool z12 = this.mc.player.heartsLife / 3 % 2 == 1;
            if (this.mc.player.heartsLife < 10)
            {
                z12 = false;
            }

            int i13 = this.mc.player.health;
            int i14 = this.mc.player.prevHealth;
            this.rand.SetSeed(this.updateCounter * 312871);
            int i15;
            int i16;
            int i17;
            if (this.mc.gameMode.ShouldDrawHUD())
            {
                i15 = this.mc.player.GetPlayerArmorValue();
                int i18;
                for (i16 = 0; i16 < 10; ++i16)
                {
                    i17 = i7 - 32;
                    if (i15 > 0)
                    {
                        i18 = i6 / 2 + 91 - i16 * 8 - 9;
                        if (i16 * 2 + 1 < i15)
                        {
                            this.DrawTexturedModalRect(i18, i17, 34, 9, 9, 9);
                        }

                        if (i16 * 2 + 1 == i15)
                        {
                            this.DrawTexturedModalRect(i18, i17, 25, 9, 9, 9);
                        }

                        if (i16 * 2 + 1 > i15)
                        {
                            this.DrawTexturedModalRect(i18, i17, 16, 9, 9, 9);
                        }
                    }

                    byte b28 = 0;
                    if (z12)
                    {
                        b28 = 1;
                    }

                    int i19 = i6 / 2 - 91 + i16 * 8;
                    if (i13 <= 4)
                    {
                        i17 += this.rand.NextInt(2);
                    }

                    this.DrawTexturedModalRect(i19, i17, 16 + b28 * 9, 0, 9, 9);
                    if (z12)
                    {
                        if (i16 * 2 + 1 < i14)
                        {
                            this.DrawTexturedModalRect(i19, i17, 70, 0, 9, 9);
                        }

                        if (i16 * 2 + 1 == i14)
                        {
                            this.DrawTexturedModalRect(i19, i17, 79, 0, 9, 9);
                        }
                    }

                    if (i16 * 2 + 1 < i13)
                    {
                        this.DrawTexturedModalRect(i19, i17, 52, 0, 9, 9);
                    }

                    if (i16 * 2 + 1 == i13)
                    {
                        this.DrawTexturedModalRect(i19, i17, 61, 0, 9, 9);
                    }
                }

                if (this.mc.player.IsInsideOfMaterial(Material.water))
                {
                    i16 = (int)Math.Ceiling((double)(this.mc.player.air - 2) * 10 / 300);
                    i17 = (int)Math.Ceiling((double)this.mc.player.air * 10 / 300) - i16;
                    for (i18 = 0; i18 < i16 + i17; ++i18)
                    {
                        if (i18 < i16)
                        {
                            this.DrawTexturedModalRect(i6 / 2 - 91 + i18 * 8, i7 - 32 - 9, 16, 18, 9, 9);
                        }
                        else
                        {
                            this.DrawTexturedModalRect(i6 / 2 - 91 + i18 * 8, i7 - 32 - 9, 25, 18, 9, 9);
                        }
                    }
                }
            }

            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPushMatrix();
            GL11.glRotatef(120F, 1F, 0F, 0F);
            Light.TurnOn();
            GL11.glPopMatrix();
            for (i15 = 0; i15 < 9; ++i15)
            {
                i16 = i6 / 2 - 90 + i15 * 20 + 2;
                i17 = i7 - 16 - 3;
                this.RenderInventorySlot(i15, i16, i17, f1);
            }

            Light.TurnOff();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            if (this.mc.player.Func_22060_M() > 0)
            {
                GL11.glDisable(GL11C.GL_DEPTH_TEST);
                GL11.glDisable(GL11C.GL_ALPHA_TEST);
                i15 = this.mc.player.Func_22060_M();
                float f27 = i15 / 100F;
                if (f27 > 1F)
                {
                    f27 = 1F - (i15 - 100) / 10F;
                }

                i17 = (int)(220F * f27) << 24 | 1052704;
                this.DrawRect(0, 0, i6, i7, i17);
                GL11.glEnable(GL11C.GL_ALPHA_TEST);
                GL11.glEnable(GL11C.GL_DEPTH_TEST);
            }

            string string23;
            if (mc.options.showDebugInfo)
            {
                GL11.glPushMatrix();
                if (Client.licenseCheckTime > 0L)
                    GL11.glTranslatef(0F, 32F, 0F);

                font.DrawStringWithShadow($"{SharedConstants.VERSION_STRING} ({mc.fpsString})", 2, 2, 0xFFFFFF);
                font.DrawStringWithShadow(mc.GetRenderStats(), 2, 12, 0xFFFFFF);
                font.DrawStringWithShadow(mc.GetEntityStats(), 2, 22, 0xFFFFFF);
                font.DrawStringWithShadow(mc.GetParticleStats(), 2, 32, 0xFFFFFF);
                font.DrawStringWithShadow(mc.GetChunkSourceStats(), 2, 42, 0xFFFFFF);
                long max = (long)computerInfo.AvailablePhysicalMemory;
                long used = GC.GetTotalMemory(false);
                //long allocated = ...;
                // There isn't such thing as "allocated" in C#, so allocated = used
                string23 = "Used memory: " + used * 100 / max + "% (" + used / 1024 / 1024 + "MB) of " + max / 1024 / 1024 + "MB";
                DrawString(font, string23, i6 - font.GetStringWidth(string23) - 2, 2, 14737632);
                string23 = "Allocated memory: " + used * 100 / max + "% (" + used / 1024 / 1024 + "MB)";
                DrawString(font, string23, i6 - font.GetStringWidth(string23) - 2, 12, 14737632);
                DrawString(font, "x: " + mc.player.x, 2, 64, 14737632);
                DrawString(font, "y: " + mc.player.y, 2, 72, 14737632);
                DrawString(font, "z: " + mc.player.z, 2, 80, 14737632);
                DrawString(font, "f: " + (Mth.Floor(mc.player.yaw * 4F / 360F + 0.5) & 3), 2, 88, 14737632);
                DrawString(font, "b: " + mc.level.GetBiomeSource().GetBiomeGenAt(Mth.Floor(mc.player.x), Mth.Floor(mc.player.z)).name, 2, 96, 14737632);
                DrawString(font, "Level seed: " + mc.level.GetRandomSeed(), 2, 104, 14737632);

                GL11.glPopMatrix();
            }

            if (this.recordPlayingUpFor > 0)
            {
                float f25 = this.recordPlayingUpFor - f1;
                i16 = (int)(f25 * 256F / 20F);
                if (i16 > 255)
                {
                    i16 = 255;
                }

                if (i16 > 0)
                {
                    GL11.glPushMatrix();
                    GL11.glTranslatef(i6 / 2, i7 - 48, 0F);
                    GL11.glEnable(GL11C.GL_BLEND);
                    GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                    i17 = 0xFFFFFF;
                    if (this.field_22065_l)
                    {
                        i17 = ColorExtensions.HSBtoRGB(f25 / 50F, 0.7F, 0.6F) & 0xFFFFFF;
                    }

                    font.DrawString(this.recordPlaying, -font.GetStringWidth(this.recordPlaying) / 2, -4, (uint)(i17 + (i16 << 24)));
                    GL11.glDisable(GL11C.GL_BLEND);
                    GL11.glPopMatrix();
                }
            }

            byte b26 = 10;
            bool z31 = false;
            if (this.mc.currentScreen is ChatScreen)
            {
                b26 = 20;
                z31 = true;
            }

            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            GL11.glPushMatrix();
            GL11.glTranslatef(0F, i7 - 48, 0F);
            for (i17 = 0; i17 < this.chatMessageList.Count && i17 < b26; ++i17)
            {
                if (this.chatMessageList[i17].updateCounter < 200 || z31)
                {
                    double d32 = this.chatMessageList[i17].updateCounter / 200;
                    d32 = 1 - d32;
                    d32 *= 10;
                    if (d32 < 0)
                    {
                        d32 = 0;
                    }

                    if (d32 > 1)
                    {
                        d32 = 1;
                    }

                    d32 *= d32;
                    int i20 = (int)(255 * d32);
                    if (z31)
                    {
                        i20 = 255;
                    }

                    if (i20 > 0)
                    {
                        byte b33 = 2;
                        int i22 = -i17 * 9;
                        string23 = this.chatMessageList[i17].message;
                        this.DrawRect(b33, i22 - 1, b33 + 320, i22 + 8, i20 / 2 << 24);
                        GL11.glEnable(GL11C.GL_BLEND);
                        font.DrawStringWithShadow(string23, b33, i22, (uint)(0xFFFFFF + (i20 << 24)));
                    }
                }
            }

            GL11.glPopMatrix();
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glDisable(GL11C.GL_BLEND);
        }

        private void RenderPumpkinBlur(int i1, int i2)
        {
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            GL11.glDepthMask(false);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("%blur%/misc/pumpkinblur.png"));
            Tessellator tessellator3 = Tessellator.Instance;
            tessellator3.Begin();
            tessellator3.VertexUV(0, i2, -90, 0, 1);
            tessellator3.VertexUV(i1, i2, -90, 1, 1);
            tessellator3.VertexUV(i1, 0, -90, 1, 0);
            tessellator3.VertexUV(0, 0, -90, 0, 0);
            tessellator3.End();
            GL11.glDepthMask(true);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glColor4f(1F, 1F, 1F, 1F);
        }

        private void RenderVignette(float f1, int i2, int i3)
        {
            f1 = 1F - f1;
            if (f1 < 0F)
            {
                f1 = 0F;
            }

            if (f1 > 1F)
            {
                f1 = 1F;
            }

            this.prevVignetteBrightness = (float)(this.prevVignetteBrightness + (f1 - this.prevVignetteBrightness) * 0.01);
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            GL11.glDepthMask(false);
            GL11.glBlendFunc(GL11C.GL_ZERO, GL11C.GL_ONE_MINUS_SRC_COLOR);
            GL11.glColor4f(this.prevVignetteBrightness, this.prevVignetteBrightness, this.prevVignetteBrightness, 1F);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("%blur%/misc/vignette.png"));
            Tessellator tessellator4 = Tessellator.Instance;
            tessellator4.Begin();
            tessellator4.VertexUV(0, i3, -90, 0, 1);
            tessellator4.VertexUV(i2, i3, -90, 1, 1);
            tessellator4.VertexUV(i2, 0, -90, 1, 0);
            tessellator4.VertexUV(0, 0, -90, 0, 0);
            tessellator4.End();
            GL11.glDepthMask(true);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
        }

        private void RenderPortalOverlay(float f1, int i2, int i3)
        {
            if (f1 < 1F)
            {
                f1 *= f1;
                f1 *= f1;
                f1 = f1 * 0.8F + 0.2F;
            }

            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            GL11.glDepthMask(false);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glColor4f(1F, 1F, 1F, f1);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/terrain.png"));
            float f4 = Tile.portal.texture % 16 / 16F;
            float f5 = Tile.portal.texture / 16 / 16F;
            float f6 = (Tile.portal.texture % 16 + 1) / 16F;
            float f7 = (Tile.portal.texture / 16 + 1) / 16F;
            Tessellator tessellator8 = Tessellator.Instance;
            tessellator8.Begin();
            tessellator8.VertexUV(0, i3, -90, f4, f7);
            tessellator8.VertexUV(i2, i3, -90, f6, f7);
            tessellator8.VertexUV(i2, 0, -90, f6, f5);
            tessellator8.VertexUV(0, 0, -90, f4, f5);
            tessellator8.End();
            GL11.glDepthMask(true);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glColor4f(1F, 1F, 1F, 1F);
        }

        private void RenderInventorySlot(int i1, int i2, int i3, float f4)
        {
            ItemInstance itemStack5 = this.mc.player.inventory.mainInventory[i1];
            if (itemStack5 != null)
            {
                float f6 = itemStack5.animationsToGo - f4;
                if (f6 > 0F)
                {
                    GL11.glPushMatrix();
                    float f7 = 1F + f6 / 5F;
                    GL11.glTranslatef(i2 + 8, i3 + 12, 0F);
                    GL11.glScalef(1F / f7, (f7 + 1F) / 2F, 1F);
                    GL11.glTranslatef((-(i2 + 8)), (-(i3 + 12)), 0F);
                }

                itemRenderer.RenderItemIntoGUI(this.mc.font, this.mc.textures, itemStack5, i2, i3);
                if (f6 > 0F)
                {
                    GL11.glPopMatrix();
                }

                itemRenderer.RenderItemOverlayIntoGUI(this.mc.font, this.mc.textures, itemStack5, i2, i3);
            }
        }

        public virtual void UpdateTick()
        {
            if (this.recordPlayingUpFor > 0)
            {
                --this.recordPlayingUpFor;
            }

            ++this.updateCounter;
            for (int i1 = 0; i1 < this.chatMessageList.Count; ++i1)
            {
                ++this.chatMessageList[i1].updateCounter;
            }
        }

        public virtual void ClearChatMessages()
        {
            this.chatMessageList.Clear();
        }

        public virtual void AddChatMessage(string string1)
        {
            while (this.mc.font.GetStringWidth(string1) > 320)
            {
                int i2;
                for (i2 = 1; i2 < string1.Length && this.mc.font.GetStringWidth(string1.Substring(0, i2 + 1)) <= 320; ++i2)
                {
                }

                this.AddChatMessage(string1.Substring(0, i2));
                string1 = string1.Substring(i2);
            }

            this.chatMessageList.Insert(0, new GuiMessage(string1));
            while (this.chatMessageList.Count > 50)
            {
                this.chatMessageList.RemoveAt(this.chatMessageList.Count - 1);
            }
        }

        public virtual void SetRecordPlayingMessage(string string1)
        {
            this.recordPlaying = "Now playing: " + string1;
            this.recordPlayingUpFor = 60;
            this.field_22065_l = true;
        }

        public virtual void AddChatMessageTranslate(string string1)
        {
            I18N stringTranslate2 = I18N.Instance;
            string string3 = stringTranslate2.TranslateKey(string1);
            this.AddChatMessage(string3);
        }
    }
}
