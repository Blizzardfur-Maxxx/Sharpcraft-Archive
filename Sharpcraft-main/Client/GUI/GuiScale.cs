using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI
{
    public class GuiScale
    {
        private int width;
        private int height;
        public double WidthScale;
        public double HeightScale;
        public int ScaleFac;

        public GuiScale(Options options, int width, int height)
        {
            this.width = width;
            this.height = height;
            ScaleFac = 1;

            int scale = options.guiScale;
            if (scale == 0)
            {
                scale = 1000;
            }

            while (ScaleFac < scale && 
                this.width / (ScaleFac + 1) >= 320 && 
                this.height / (ScaleFac + 1) >= 240)
                ScaleFac++;

            WidthScale = this.width / (double)ScaleFac;
            HeightScale = this.height / (double)ScaleFac;
            this.width = (int)Math.Ceiling(WidthScale);
            this.height = (int)Math.Ceiling(HeightScale);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
    }
}
