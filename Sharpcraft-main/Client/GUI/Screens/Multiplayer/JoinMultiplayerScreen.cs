using LWCSGL.Input;
using SharpCraft.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Multiplayer
{
    public class JoinMultiplayerScreen : Screen
    {
        private Screen parentScreen;
        private TextBox field_22111_h;
        public JoinMultiplayerScreen(Screen guiScreen1)
        {
            parentScreen = guiScreen1;
        }

        public override void UpdateScreen()
        {
            field_22111_h.UpdateCursorCounter();
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            Keyboard.EnableRepeatEvents(true);
            buttons.Clear();
            buttons.Add(new Button(0, width / 2 - 100, height / 4 + 96 + 12, stringTranslate1.TranslateKey("multiplayer.connect")));
            buttons.Add(new Button(1, width / 2 - 100, height / 4 + 120 + 12, stringTranslate1.TranslateKey("gui.cancel")));
            string string2 = mc.options.lastServer.Replace("_", ":");
            buttons[0].enabled = string2.Length > 0;
            field_22111_h = new TextBox(this, font, width / 2 - 100, height / 4 - 10 + 50 + 18, 200, 20, string2);
            field_22111_h.isFocused = true;
            field_22111_h.SetMaxStringLength(128);
        }

        public override void OnGuiClosed()
        {
            Keyboard.EnableRepeatEvents(false);
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 1)
                {
                    mc.SetScreen(parentScreen);
                }
                else if (guiButton1.id == 0)
                {
                    string string2 = field_22111_h.GetText().Trim();
                    mc.options.lastServer = string2.Replace(":", "_");
                    mc.options.SaveOptions();
                    string[] string3 = string2.Split(":");
                    if (string2.StartsWith("["))
                    {
                        int i4 = string2.IndexOf("]");
                        if (i4 > 0)
                        {
                            string string5 = string2.Substring(1, i4);
                            string string6 = string2.Substring(i4 + 1).Trim();
                            if (string6.StartsWith(":") && string6.Length > 0)
                            {
                                string6 = string6.Substring(1);
                                string3 = new string[]
                                {
                                    string5,
                                    string6
                                };
                            }
                            else
                            {
                                string3 = new string[]
                                {
                                    string5
                                };
                            }
                        }
                    }

                    if (string3.Length > 2)
                    {
                        string3 = new string[]
                        {
                            string2
                        };
                    }

                    mc.SetScreen(new ConnectScreen(mc, string3[0], string3.Length > 1 ? ParseIntWithDefault(string3[1], 25565) : 25565));
                }
            }
        }

        private int ParseIntWithDefault(string string1, int i2)
        {
            try
            {
                return int.Parse(string1.Trim());
            }
            catch
            {
                return i2;
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            field_22111_h.TextboxKeyTyped(c1, i2);
            if (c1 == 13)
            {
                ActionPerformed(buttons[0]);
            }

            buttons[0].enabled = field_22111_h.GetText().Length > 0;
        }

        protected override void MouseClicked(int i1, int i2, int i3)
        {
            base.MouseClicked(i1, i2, i3);
            field_22111_h.MouseClicked(i1, i2, i3);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            I18N stringTranslate4 = I18N.Instance;
            DrawDefaultBackground();
            DrawCenteredString(font, stringTranslate4.TranslateKey("multiplayer.title"), width / 2, height / 4 - 60 + 20, 0xFFFFFF);
            DrawString(font, stringTranslate4.TranslateKey("multiplayer.info1"), width / 2 - 140, height / 4 - 60 + 60 + 0, 10526880);
            DrawString(font, stringTranslate4.TranslateKey("multiplayer.info2"), width / 2 - 140, height / 4 - 60 + 60 + 9, 10526880);
            DrawString(font, stringTranslate4.TranslateKey("multiplayer.ipinfo"), width / 2 - 140, height / 4 - 60 + 60 + 36, 10526880);
            field_22111_h.DrawTextBox();
            base.DrawScreen(i1, i2, f3);
        }
    }
}
