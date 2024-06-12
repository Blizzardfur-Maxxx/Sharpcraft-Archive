using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    [StructLayout(LayoutKind.Explicit)]
    public struct FloatToIntConverter //to be used if BitConverter.SingleToInt32Bits is not available
    {
        [FieldOffset(0)]
        public int intValue;
        [FieldOffset(0)]
        public float floatValue;

        public FloatToIntConverter(float f)
        {
            intValue = 0;
            floatValue = f;
        }

        public FloatToIntConverter(int i)
        {
            floatValue = 0;
            intValue = i;
        }
    }
}
