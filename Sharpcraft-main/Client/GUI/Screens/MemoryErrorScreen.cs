using LWCSGL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens
{
    public class MemoryErrorScreen : Screen
    {
        private int field_28098_a = 0;
        public override void UpdateScreen()
        {
            ++this.field_28098_a;
        }

        public override void InitGui()
        {
        }

        protected override void ActionPerformed(Button guiButton1)
        {
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, "Out of memory!", this.width / 2, this.height / 4 - 60 + 20, 0xFFFFFF);
            this.DrawString(this.font, "Shartcraft has run out of memory.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 0, 10526880);
            this.DrawString(this.font, "This could be caused by a bug in the game or by the", this.width / 2 - 140, this.height / 4 - 60 + 60 + 18, 10526880);
            this.DrawString(this.font, "Virtual Machine not being allocated enough", this.width / 2 - 140, this.height / 4 - 60 + 60 + 27, 10526880);
            this.DrawString(this.font, "memory. If you are playing in a web browser, try", this.width / 2 - 140, this.height / 4 - 60 + 60 + 36, 10526880);
            this.DrawString(this.font, "downloading the game and playing it offline.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 45, 10526880);
            this.DrawString(this.font, "To prevent level corruption, the current game has quit.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 63, 10526880);
            this.DrawString(this.font, "Please restart the game.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 81, 10526880);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
