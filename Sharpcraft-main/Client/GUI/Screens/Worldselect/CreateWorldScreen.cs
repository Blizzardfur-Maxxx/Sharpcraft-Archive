using SharpCraft.Client.Gamemode;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core;
using System;
using LWCSGL.Input;

namespace SharpCraft.Client.GUI.Screens.Worldselect
{
    public class CreateWorldScreen : Screen
    {
        private Screen field_22131_a;
        private TextBox textboxWorldName;
        private TextBox textboxSeed;
        private string folderName;
        private bool createClicked;
        public CreateWorldScreen(Screen guiScreen1)
        {
            field_22131_a = guiScreen1;
        }

        public override void UpdateScreen()
        {
            textboxWorldName.UpdateCursorCounter();
            textboxSeed.UpdateCursorCounter();
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            Keyboard.EnableRepeatEvents(true);
            buttons.Clear();
            buttons.Add(new Button(0, width / 2 - 100, height / 4 + 96 + 12, stringTranslate1.TranslateKey("selectWorld.create")));
            buttons.Add(new Button(1, width / 2 - 100, height / 4 + 120 + 12, stringTranslate1.TranslateKey("gui.cancel")));
            textboxWorldName = new TextBox(this, font, width / 2 - 100, 60, 200, 20, stringTranslate1.TranslateKey("selectWorld.newWorld"));
            textboxWorldName.isFocused = true;
            textboxWorldName.SetMaxStringLength(32);
            textboxSeed = new TextBox(this, font, width / 2 - 100, 116, 200, 20, "");
            Func_22129_j();
        }

        private void Func_22129_j()
        {
            folderName = textboxWorldName.GetText().Trim();
            char[] c1 = SharedConstants.ILLEGAL_FILENAME_CHARACTERS;
            int i2 = c1.Length;
            for (int i3 = 0; i3 < i2; ++i3)
            {
                char c4 = c1[i3];
                folderName = folderName.Replace(c4, '_');
            }

            if (string.IsNullOrEmpty(folderName))
            {
                folderName = "World";
            }

            folderName = GenerateUnusedFolderName(mc.GetSaveLoader(), folderName);
        }

        public static string GenerateUnusedFolderName(ILevelStorageSource iSaveFormat0, string string1)
        {
            while (iSaveFormat0.GetTagDataFor(string1) != null)
            {
                string1 = string1 + "-";
            }

            return string1;
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
                    mc.SetScreen(field_22131_a);
                }
                else if (guiButton1.id == 0)
                {
                    mc.SetScreen(null);
                    if (createClicked)
                    {
                        return;
                    }

                    createClicked = true;
                    long j2 = new JRandom().NextLong();
                    string string4 = textboxSeed.GetText();
                    if (!string.IsNullOrEmpty(string4))
                    {
                        try
                        {
                            long j5 = long.Parse(string4);
                            if (j5 != 0)
                            {
                                j2 = j5;
                            }
                        }
                        catch
                        {
                            j2 = Mth.GetJHashCode(string4);
                        }
                    }

                    mc.gameMode = new SurvivalMode(mc);
                    mc.LoadLevel(folderName, textboxWorldName.GetText(), j2);
                    mc.SetScreen(null);
                }
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            if (textboxWorldName.isFocused)
            {
                textboxWorldName.TextboxKeyTyped(c1, i2);
            }
            else
            {
                textboxSeed.TextboxKeyTyped(c1, i2);
            }

            if (c1 == 13)
            {
                ActionPerformed(buttons[0]);
            }

            buttons[0].enabled = textboxWorldName.GetText().Length > 0;
            Func_22129_j();
        }

        protected override void MouseClicked(int i1, int i2, int i3)
        {
            base.MouseClicked(i1, i2, i3);
            textboxWorldName.MouseClicked(i1, i2, i3);
            textboxSeed.MouseClicked(i1, i2, i3);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            I18N stringTranslate4 = I18N.Instance;
            DrawDefaultBackground();
            DrawCenteredString(font, stringTranslate4.TranslateKey("selectWorld.create"), width / 2, height / 4 - 60 + 20, 0xFFFFFF);
            DrawString(font, stringTranslate4.TranslateKey("selectWorld.enterName"), width / 2 - 100, 47, 10526880);
            DrawString(font, stringTranslate4.TranslateKey("selectWorld.resultFolder") + " " + folderName, width / 2 - 100, 85, 10526880);
            DrawString(font, stringTranslate4.TranslateKey("selectWorld.enterSeed"), width / 2 - 100, 104, 10526880);
            DrawString(font, stringTranslate4.TranslateKey("selectWorld.seedInfo"), width / 2 - 100, 140, 10526880);
            textboxWorldName.DrawTextBox();
            textboxSeed.DrawTextBox();
            base.DrawScreen(i1, i2, f3);
        }

        public override void SelectNextField()
        {
            if (textboxWorldName.isFocused)
            {
                textboxWorldName.SetFocused(false);
                textboxSeed.SetFocused(true);
            }
            else
            {
                textboxWorldName.SetFocused(true);
                textboxSeed.SetFocused(false);
            }
        }
    }
}