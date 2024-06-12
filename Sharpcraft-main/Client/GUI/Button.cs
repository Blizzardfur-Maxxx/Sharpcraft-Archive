using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI
{
    public class Button : GuiComponent
    {
        protected int width;
        protected int height;
        public int xPosition;
        public int yPosition;
        public string displayString;
        public int id;
        public bool enabled;
        public bool enabled2;

        public Button(int i1, int i2, int i3, string string4)
            : this(i1, i2, i3, 200, 20, string4)
        {
        }

        public Button(int i1, int i2, int i3, int i4, int i5, string string6)
        {
            this.width = 200;
            this.height = 20;
            this.enabled = true;
            this.enabled2 = true;
            this.id = i1;
            this.xPosition = i2;
            this.yPosition = i3;
            this.width = i4;
            this.height = i5;
            this.displayString = string6;
        }

        protected virtual int GetHoverState(bool z1)
        {
            byte b2 = 1;
            if (!this.enabled)
            {
                b2 = 0;
            }
            else if (z1)
            {
                b2 = 2;
            }

            return b2;
        }

        public virtual void DrawButton(Client instance, int mx, int my)
        {
            if (this.enabled2)
            {
                Font fontRenderer4 = instance.font;
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, instance.textures.LoadTexture("/gui/gui.png"));
                GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                bool hovered = mx >= this.xPosition && my >= this.yPosition && mx < this.xPosition + this.width && my < this.yPosition + this.height;
                int i6 = this.GetHoverState(hovered);
                this.DrawTexturedModalRect(this.xPosition, this.yPosition, 0, 46 + i6 * 20, this.width / 2, this.height);
                this.DrawTexturedModalRect(this.xPosition + this.width / 2, this.yPosition, 200 - this.width / 2, 46 + i6 * 20, this.width / 2, this.height);
                this.MouseDragged(instance, mx, my);
                if (!this.enabled)
                {
                    this.DrawCenteredString(fontRenderer4, this.displayString, this.xPosition + this.width / 2, this.yPosition + (this.height - 8) / 2, -6250336);
                }
                else if (hovered)
                {
                    this.DrawCenteredString(fontRenderer4, this.displayString, this.xPosition + this.width / 2, this.yPosition + (this.height - 8) / 2, 16777120);
                }
                else
                {
                    this.DrawCenteredString(fontRenderer4, this.displayString, this.xPosition + this.width / 2, this.yPosition + (this.height - 8) / 2, 14737632);
                }

            }
        }

        protected virtual void MouseDragged(Client instance, int i2, int i3)
        {
        }

        public virtual void MouseReleased(int i1, int i2)
        {
        }

        public virtual bool MousePressed(Client instance, int i2, int i3)
        {
            return this.enabled && i2 >= this.xPosition && i3 >= this.yPosition && i2 < this.xPosition + this.width && i3 < this.yPosition + this.height;
        }
    }
}
