using LWCSGL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens
{
    public class ErrorScreen : Screen
    {
        private string title;
        private string body;

        public ErrorScreen(string title, string body) 
        {
            this.title = title;
            this.body = body;
        }

        public override void InitGui()
        {
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            DrawGradientRect(0, 0, width, height, -12574688, -11530224);
            DrawCenteredString(font, title, width / 2, 90, 0xFFFFFF);
            DrawCenteredString(font, body, width / 2, 110, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }
    }
}
