using LWCSGL.Input;
using SharpCraft.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Optionss
{
    public class ControlsScreen : Screen
    {
        private Screen parentScreen;
        protected string screenTitle = "Controls";
        private Options options;
        private int buttonId = -1;
        public ControlsScreen(Screen guiScreen1, Options gameSettings2)
        {
            this.parentScreen = guiScreen1;
            this.options = gameSettings2;
        }

        private int Func_20080_j()
        {
            return this.width / 2 - 155;
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            int i2 = this.Func_20080_j();
            for (int i3 = 0; i3 < this.options.keyBindings.Length; ++i3)
            {
                this.buttons.Add(new SmallButton(i3, i2 + i3 % 2 * 160, this.height / 6 + 24 * (i3 >> 1), 70, 20, this.options.GetOptionDisplayString(i3)));
            }

            this.buttons.Add(new Button(200, this.width / 2 - 100, this.height / 6 + 168, stringTranslate1.TranslateKey("gui.done")));
            this.screenTitle = stringTranslate1.TranslateKey("controls.title");
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            for (int i2 = 0; i2 < this.options.keyBindings.Length; ++i2)
            {
                this.buttons[i2].displayString = this.options.GetOptionDisplayString(i2);
            }

            if (guiButton1.id == 200)
            {
                this.mc.SetScreen(this.parentScreen);
            }
            else
            {
                this.buttonId = guiButton1.id;
                guiButton1.displayString = "> " + this.options.GetOptionDisplayString(guiButton1.id) + " <";
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            if (this.buttonId >= 0)
            {
                this.options.SetKeyBinding(this.buttonId, (int)i2);
                this.buttons[this.buttonId].displayString = this.options.GetOptionDisplayString(this.buttonId);
                this.buttonId = -1;
            }
            else
            {
                base.KeyTyped(c1, i2);
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, this.screenTitle, this.width / 2, 20, 0xFFFFFF);
            int i4 = this.Func_20080_j();
            for (int i5 = 0; i5 < this.options.keyBindings.Length; ++i5)
            {
                this.DrawString(this.font, this.options.GetKeyBindingDescription(i5), i4 + i5 % 2 * 160 + 70 + 6, this.height / 6 + 24 * (i5 >> 1) + 7, -1);
            }

            base.DrawScreen(i1, i2, f3);
        }
    }
}
