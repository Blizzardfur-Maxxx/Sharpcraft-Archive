using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Players
{
    //this is actually SmoothFloat and the method is GetNewDeltaValue
    public class MouseFilter
    {
        private float field_a;
        private float field_b;
        private float field_c;

        public float Apply(float axisMul, float alpha)
        {
            this.field_a += axisMul;
            axisMul = (this.field_a - this.field_b) * alpha;
            this.field_c += (axisMul - this.field_c) * 0.5F;
            if (axisMul > 0.0F && axisMul > this.field_c || axisMul < 0.0F && axisMul < this.field_c)
            {
                axisMul = this.field_c;
            }

            this.field_b += axisMul;
            return axisMul;
        }
    }
}
