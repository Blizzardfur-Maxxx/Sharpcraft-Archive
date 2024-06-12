using LWCSGL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class KeyMapping
    {
        public string keyDescription;
        public VirtualKey keyCode;

        public KeyMapping(string keyDescription, VirtualKey keyCode) 
        {
            this.keyDescription = keyDescription;
            this.keyCode = keyCode;
        }

        public override string ToString()
        {
            return "key_" + keyDescription + ":" + (int)keyCode;
        }
    }
}
