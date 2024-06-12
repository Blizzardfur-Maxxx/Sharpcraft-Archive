using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class GuiMessage
    {
        public string message;
        public int updateCounter;

        public GuiMessage(string string1)
        {
            this.message = string1;
            this.updateCounter = 0;
        }
    }
}
