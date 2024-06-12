using LWCSGL.Input;
using SharpCraft.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Multiplayer
{
    public class DisconnectionScreen : Screen
    {
        private string errorMessage;
        private string errorDetail;
        public DisconnectionScreen(string string1, string string2, params object[] object3)
        {
            I18N stringTranslate4 = I18N.Instance;
            this.errorMessage = stringTranslate4.TranslateKey(string1);
            if (object3 != null)
            {
                this.errorDetail = stringTranslate4.TranslateKeyFormat(string2, object3);
            }
            else
            {
                this.errorDetail = stringTranslate4.TranslateKey(string2);
            }
        }

        public override void UpdateScreen()
        {
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            this.buttons.Clear();
            this.buttons.Add(new Button(0, this.width / 2 - 100, this.height / 4 + 120 + 12, stringTranslate1.TranslateKey("gui.toMenu")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 0)
            {
                this.mc.SetScreen(new StartMenuScreen());
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, this.errorMessage, this.width / 2, this.height / 2 - 50, 0xFFFFFF);
            this.DrawCenteredString(this.font, this.errorDetail, this.width / 2, this.height / 2 - 10, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
