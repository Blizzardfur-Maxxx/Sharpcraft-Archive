using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Client.Options;

namespace SharpCraft.Client.GUI
{
    public class SmallButton : Button
    {
        private readonly Option option;
        public SmallButton(int i1, int i2, int i3, string string4) : this(i1, i2, i3, null, string4)
        {
        }

        public SmallButton(int i1, int i2, int i3, int i4, int i5, string string6) : base(i1, i2, i3, i4, i5, string6)
        {
            this.option = null;
        }

        public SmallButton(int i1, int i2, int i3, Option enumOptions4, string string5) : base(i1, i2, i3, 150, 20, string5)
        {
            this.option = enumOptions4;
        }

        public virtual Option ReturnEnumOptions()
        {
            return this.option;
        }
    }
}
