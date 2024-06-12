using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.Stats;

namespace SharpCraft.Client.GUI.Screens.Achievements
{
    public class AchivementWidget : GuiComponent
    {
        private Client client;
        private int achievementWindowWidth;
        private int achievementWindowHeight;
        private string field_25085_d;
        private string field_25084_e;
        private Achievement theAchievement;
        private long field_25083_f;
        private RenderItem itemRender;
        private bool field_27103_i;

        public AchivementWidget(Client instance)
        {
            this.client = instance;
            this.itemRender = new RenderItem();
        }

        public void QueueTakenAchievement(Achievement achievement1)
        {
            this.field_25085_d = Locale.TranslateKey("achievement.get");
            this.field_25084_e = achievement1.statName;
            this.field_25083_f = TimeUtil.MilliTime;
            this.theAchievement = achievement1;
            this.field_27103_i = false;
        }

        public void QueueAchievementInformation(Achievement achievement1)
        {
            this.field_25085_d = achievement1.statName;
            this.field_25084_e = achievement1.GetDescription();
            this.field_25083_f = TimeUtil.MilliTime - 2500L;
            this.theAchievement = achievement1;
            this.field_27103_i = true;
        }

        private void UpdateAchievementWindowScale()
        {
            GL11.glViewport(0, 0, this.client.displayWidth, this.client.displayHeight);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            this.achievementWindowWidth = this.client.displayWidth;
            this.achievementWindowHeight = this.client.displayHeight;
            GuiScale scaledResolution1 = new GuiScale(this.client.options, this.client.displayWidth, this.client.displayHeight);
            this.achievementWindowWidth = scaledResolution1.GetWidth();
            this.achievementWindowHeight = scaledResolution1.GetHeight();
            GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            GL11.glOrtho(0.0D, this.achievementWindowWidth, this.achievementWindowHeight, 0.0D, 1000.0D, 3000.0D);
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            GL11.glTranslatef(0.0F, 0.0F, -2000.0F);
        }

        public void UpdateAchievementWindow()
        {
            if (Client.licenseCheckTime > 0L)
            {
                GL11.glDisable(GL11C.GL_DEPTH_TEST);
                GL11.glDepthMask(false);
                Light.TurnOff();
                this.UpdateAchievementWindowScale();
                string string1 = SharedConstants.VERSION_STRING + "   Unlicensed Copy :(";
                string string2 = "(Or logged in from another location)";
                string string3 = "Purchase at client.net";
                this.client.font.DrawStringWithShadow(string1, 2, 2, 0xFFFFFF);
                this.client.font.DrawStringWithShadow(string2, 2, 11, 0xFFFFFF);
                this.client.font.DrawStringWithShadow(string3, 2, 20, 0xFFFFFF);
                GL11.glDepthMask(true);
                GL11.glEnable(GL11C.GL_DEPTH_TEST);
            }

            if (this.theAchievement != null && this.field_25083_f != 0L)
            {
                double d8 = (TimeUtil.MilliTime - this.field_25083_f) / 3000.0D;
                if (this.field_27103_i || d8 >= 0.0D && d8 <= 1.0D)
                {
                    this.UpdateAchievementWindowScale();
                    GL11.glDisable(GL11C.GL_DEPTH_TEST);
                    GL11.glDepthMask(false);
                    double d9 = d8 * 2.0D;
                    if (d9 > 1.0D)
                    {
                        d9 = 2.0D - d9;
                    }

                    d9 *= 4.0D;
                    d9 = 1.0D - d9;
                    if (d9 < 0.0D)
                    {
                        d9 = 0.0D;
                    }

                    d9 *= d9;
                    d9 *= d9;
                    int i5 = this.achievementWindowWidth - 160;
                    int i6 = 0 - (int)(d9 * 36.0D);
                    uint i7 = this.client.textures.LoadTexture("/achievement/bg.png");
                    GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                    GL11.glEnable(GL11C.GL_TEXTURE_2D);
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i7);
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    this.DrawTexturedModalRect(i5, i6, 96, 202, 160, 32);
                    if (this.field_27103_i)
                    {
                        this.client.font.DrawMultiline(this.field_25084_e, i5 + 30, i6 + 7, 120, unchecked((uint)-1));
                    }
                    else
                    {
                        this.client.font.DrawString(this.field_25085_d, i5 + 30, i6 + 7, unchecked((uint)-256));
                        this.client.font.DrawString(this.field_25084_e, i5 + 30, i6 + 18, unchecked((uint)-1));
                    }

                    GL11.glPushMatrix();
                    GL11.glRotatef(180.0F, 1.0F, 0.0F, 0.0F);
                    Light.TurnOn();
                    GL11.glPopMatrix();
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                    GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    this.itemRender.RenderItemIntoGUI(this.client.font, this.client.textures, this.theAchievement.item, i5 + 8, i6 + 8);
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glDepthMask(true);
                    GL11.glEnable(GL11C.GL_DEPTH_TEST);
                }
                else
                {
                    this.field_25083_f = 0L;
                }
            }
        }
    }
}
