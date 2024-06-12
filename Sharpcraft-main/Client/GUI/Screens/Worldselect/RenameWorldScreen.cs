using SharpCraft.Core.i18n;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core.World.GameLevel;
using LWCSGL.Input;

namespace SharpCraft.Client.GUI.Screens.Worldselect
{
    public class RenameWorldScreen : Screen
    {
        private Screen field_22112_a;
        private TextBox field_22114_h;
        private readonly string field_22113_i;
        public RenameWorldScreen(Screen guiScreen1, string string2)
        {
            field_22112_a = guiScreen1;
            field_22113_i = string2;
        }

        public override void UpdateScreen()
        {
            field_22114_h.UpdateCursorCounter();
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            Keyboard.EnableRepeatEvents(true);
            buttons.Clear();
            buttons.Add(new Button(0, width / 2 - 100, height / 4 + 96 + 12, stringTranslate1.TranslateKey("selectWorld.renameButton")));
            buttons.Add(new Button(1, width / 2 - 100, height / 4 + 120 + 12, stringTranslate1.TranslateKey("gui.cancel")));
            ILevelStorageSource iSaveFormat2 = mc.GetSaveLoader();
            LevelData worldInfo3 = iSaveFormat2.GetTagDataFor(field_22113_i);
            string string4 = worldInfo3.GetLevelName();
            field_22114_h = new TextBox(this, font, width / 2 - 100, 60, 200, 20, string4);
            field_22114_h.isFocused = true;
            field_22114_h.SetMaxStringLength(32);
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
                    mc.SetScreen(field_22112_a);
                }
                else if (guiButton1.id == 0)
                {
                    ILevelStorageSource iSaveFormat2 = mc.GetSaveLoader();
                    iSaveFormat2.RenameLevel(field_22113_i, field_22114_h.GetText().Trim());
                    mc.SetScreen(field_22112_a);
                }
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            field_22114_h.TextboxKeyTyped(c1, i2);
            buttons[0].enabled = field_22114_h.GetText().Trim().Length > 0;
            if (c1 == 13)
            {
                ActionPerformed(buttons[0]);
            }
        }

        protected override void MouseClicked(int i1, int i2, int i3)
        {
            base.MouseClicked(i1, i2, i3);
            field_22114_h.MouseClicked(i1, i2, i3);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            I18N stringTranslate4 = I18N.Instance;
            DrawDefaultBackground();
            DrawCenteredString(font, stringTranslate4.TranslateKey("selectWorld.renameTitle"), width / 2, height / 4 - 60 + 20, 0xFFFFFF);
            DrawString(font, stringTranslate4.TranslateKey("selectWorld.enterName"), width / 2 - 100, 47, 10526880);
            field_22114_h.DrawTextBox();
            base.DrawScreen(i1, i2, f3);
        }
    }
}