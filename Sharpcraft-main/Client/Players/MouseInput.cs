using LWCSGL.Input;
using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Players
{
    public class MouseInput
    {
        public int deltaX;
        public int deltaY;

        public MouseInput()
        {
        }

        public void GrabMouse()
        {
            Mouse.SetGrabbed(true);
            this.deltaX = 0;
            this.deltaY = 0;
        }

        public void UngrabMouse()
        {
            Mouse.SetCursorPosition(Display.GetWidth() / 2, Display.GetHeight() / 2);
            Mouse.SetGrabbed(false);
        }

        public void Tick()
        {
            this.deltaX = (int) Mouse.GetDX();
            this.deltaY = (int) Mouse.GetDY();
        }
    }
}
