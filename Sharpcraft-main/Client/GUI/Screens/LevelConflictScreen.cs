using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens
{
    public class LevelConflictScreen : Screen
    {
        private int updateCounter = 0;
        public override void UpdateScreen()
        {
            ++this.updateCounter;
        }

        public override void InitGui()
        {
            this.buttons.Clear();
            this.buttons.Add(new Button(0, this.width / 2 - 100, this.height / 4 + 120 + 12, "Back to title screen"));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 0)
                {
                    this.mc.SetScreen(new StartMenuScreen());
                }
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, "Level save conflict", this.width / 2, this.height / 4 - 60 + 20, 0xFFFFFF);
            this.DrawString(this.font, "Shartcraft detected a conflict in the level save data.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 0, 10526880);
            this.DrawString(this.font, "This could be caused by two copies of the game", this.width / 2 - 140, this.height / 4 - 60 + 60 + 18, 10526880);
            this.DrawString(this.font, "accessing the same level.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 27, 10526880);
            this.DrawString(this.font, "To prevent level corruption, the current game has quit.", this.width / 2 - 140, this.height / 4 - 60 + 60 + 45, 10526880);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
