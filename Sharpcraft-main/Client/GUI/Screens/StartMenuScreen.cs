using LWCSGL.OpenGL;
using SharpCraft.Client.GUI.Screens;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using SharpCraft.Client.GUI;
using LWCSGL.Input;
using SharpCraft.Client.GUI.Screens.Optionss;
using SharpCraft.Client.Gamemode;
using SharpCraft.Client.GUI.Screens.Worldselect;
using SharpCraft.Client.GUI.Screens.Multiplayer;

namespace SharpCraft.Client.GUI.Screens
{
    public class StartMenuScreen : Screen
    {
        private static readonly JRandom rand = new JRandom();
        private float updateCounter = 0F;
        private string splashText = "missingno";
        private Button multiplayerButton;

        public StartMenuScreen()
        {
            try
            {
                List<string> arrayList1 = new List<string>();
                StreamReader bufferedReader2 = new StreamReader($"{SharedConstants.ASSETS_CLIENT_PATH}/title/splashes.txt");
                string string3 = "";
                while ((string3 = bufferedReader2.ReadLine()) != null)
                {
                    string3 = string3.Trim();
                    if (string3.Length > 0)
                    {
                        arrayList1.Add(string3);
                    }
                }

                this.splashText = arrayList1[rand.NextInt(arrayList1.Count)];
            }
            catch (Exception)
            {
            }
        }

        public override void UpdateScreen()
        {
            ++this.updateCounter;
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }

        public override void InitGui()
        {
            if (DateTime.Now.Month == 11 && DateTime.Now.Day == 9)
            {
                this.splashText = "Happy birthday, ez!";
            } 
            else if (DateTime.Now.Month == 6 && DateTime.Now.Day == 1)
            {
                this.splashText = "Happy birthday, Nótch!";
            } 
            else if (DateTime.Now.Month == 12 && DateTime.Now.Day == 24)
            {
                this.splashText = "Merry X.com!";
            }  
            else if (DateTime.Now.Month == 1 && DateTime.Now.Day == 1)
            {
                this.splashText = "Shitty new year! Go fuck yourself!";
            }

            I18N stringTranslate2 = I18N.Instance;
            int i4 = this.height / 4 + 48;
            this.buttons.Add(new Button(1, this.width / 2 - 100, i4, stringTranslate2.TranslateKey("menu.singleplayer")));
            this.buttons.Add(this.multiplayerButton = new Button(2, this.width / 2 - 100, i4 + 24, stringTranslate2.TranslateKey("menu.multiplayer")));
            this.buttons.Add(new Button(3, this.width / 2 - 100, i4 + 48, stringTranslate2.TranslateKey("menu.mods")));
            if (this.mc.hideQuitButton)
            {
                this.buttons.Add(new Button(0, this.width / 2 - 100, i4 + 72, stringTranslate2.TranslateKey("menu.options")));
            }
            else
            {
                this.buttons.Add(new Button(0, this.width / 2 - 100, i4 + 72 + 12, 98, 20, stringTranslate2.TranslateKey("menu.options")));
                this.buttons.Add(new Button(4, this.width / 2 + 2, i4 + 72 + 12, 98, 20, stringTranslate2.TranslateKey("menu.quit")));
            }

            if (this.mc.user == null)
            {
                this.multiplayerButton.enabled = false;
            }
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 0)
            {
                this.mc.SetScreen(new OptionsScreen(this, this.mc.options));
            }

            if (guiButton1.id == 1)
            {
                this.mc.SetScreen(new SelectWorldScreen(this));
            }

            if (guiButton1.id == 2)
            {
                this.mc.SetScreen(new JoinMultiplayerScreen(this));
            }

            if (guiButton1.id == 3)
            {
                this.mc.SetScreen(new TexturePackSelectScreen(this));
            }

            if (guiButton1.id == 4)
            {
                this.mc.Shutdown();
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            Tessellator tessellator4 = Tessellator.Instance;
            short s5 = 274;
            int i6 = this.width / 2 - s5 / 2;
            byte b7 = 30;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/title/mclogo.png"));
            GL11.glColor4f(1F, 1F, 1F, 1F);
            this.DrawTexturedModalRect(i6 + 0, b7 + 0, 0, 0, 155, 44);
            this.DrawTexturedModalRect(i6 + 155, b7 + 0, 0, 45, 155, 44);
            tessellator4.Color(0xFFFFFF);
            GL11.glPushMatrix();
            GL11.glTranslatef(this.width / 2 + 90, 70F, 0F);
            GL11.glRotatef(-20F, 0F, 0F, 1F);
            float f8 = 1.8F - Mth.Abs(Mth.Sin(TimeUtil.MilliTime % 1000 / 1000F * Mth.PI * 2F) * 0.1F);
            f8 = f8 * 100F / (this.font.GetStringWidth(this.splashText) + 32);
            GL11.glScalef(f8, f8, f8);
            this.DrawCenteredString(this.font, this.splashText, 0, -8, 16776960);
            GL11.glPopMatrix();
            this.DrawString(this.font, SharedConstants.VERSION_STRING, 2, 2, 5263440);
            this.DrawString(this.font, Client.VERSION_COMMIT[..(Client.VERSION_COMMIT.Length - 10)], 2, height - 10, 5263440);
            string string9 = "Copyright Mojang AB. Do not distrubute.";
            this.DrawString(this.font, string9, this.width - this.font.GetStringWidth(string9) - 2, this.height - 10, 0xFFFFFF);
            
            base.DrawScreen(i1, i2, f3);
        }
    }
}