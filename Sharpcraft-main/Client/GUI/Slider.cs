using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Client.Options;

namespace SharpCraft.Client.GUI
{
    public class Slider : Button
    {
        public float sliderValue = 1F;
        public bool dragging = false;
        private Option idFloat = null;
        public Slider(int i1, int i2, int i3, Option enumOptions4, string string5, float f6) : base(i1, i2, i3, 150, 20, string5)
        {
            this.idFloat = enumOptions4;
            this.sliderValue = f6;
        }

        protected override int GetHoverState(bool z1)
        {
            return 0;
        }

        protected override void MouseDragged(Client instance, int i2, int i3)
        {
            if (this.enabled2)
            {
                if (this.dragging)
                {
                    this.sliderValue = (float)(i2 - (this.xPosition + 4)) / (float)(this.width - 8);
                    if (this.sliderValue < 0F)
                    {
                        this.sliderValue = 0F;
                    }

                    if (this.sliderValue > 1F)
                    {
                        this.sliderValue = 1F;
                    }

                    instance.options.SetOptionFloatValue(this.idFloat, this.sliderValue);
                    this.displayString = instance.options.GetKeyBinding(this.idFloat);
                }

                GL11.glColor4f(1F, 1F, 1F, 1F);
                this.DrawTexturedModalRect(this.xPosition + (int)(this.sliderValue * (this.width - 8)), this.yPosition, 0, 66, 4, 20);
                this.DrawTexturedModalRect(this.xPosition + (int)(this.sliderValue * (this.width - 8)) + 4, this.yPosition, 196, 66, 4, 20);
            }
        }

        public override bool MousePressed(Client instance, int i2, int i3)
        {
            if (base.MousePressed(instance, i2, i3))
            {
                this.sliderValue = (float)(i2 - (this.xPosition + 4)) / (float)(this.width - 8);
                if (this.sliderValue < 0F)
                {
                    this.sliderValue = 0F;
                }

                if (this.sliderValue > 1F)
                {
                    this.sliderValue = 1F;
                }

                instance.options.SetOptionFloatValue(this.idFloat, this.sliderValue);
                this.displayString = instance.options.GetKeyBinding(this.idFloat);
                this.dragging = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void MouseReleased(int i1, int i2)
        {
            this.dragging = false;
        }
    }
}
