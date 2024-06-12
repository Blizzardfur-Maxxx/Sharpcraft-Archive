using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core
{
    public struct ProfilerResult : IComparable<ProfilerResult>
    {
        public double sectionPercentage;
        public double globalPercentage;
        public string name;

        public ProfilerResult(string name, double sectPercent, double globalPercent)
        {
            this.name = name;
            this.sectionPercentage = sectPercent;
            this.globalPercentage = globalPercent;
        }

        public readonly int GetColor()
        {
            return (Mth.GetJHashCode(name) & 11184810) + 4473924;
        }

        public readonly int CompareTo(ProfilerResult object1)
        {
            return object1.sectionPercentage < this.sectionPercentage ? -1 : (object1.sectionPercentage > this.sectionPercentage ? 1 : object1.name.CompareTo(this.name));
        }
    }
}
