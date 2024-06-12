using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Sound
{
    public class SoundData
    {
        public string name;
        public string filePath;

        public SoundData(string name, string filePath)
        {
            this.name = name;
            this.filePath = filePath;
        }
    }
}
