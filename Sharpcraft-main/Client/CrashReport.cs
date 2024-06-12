using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class CrashReport
    {
        public readonly string Description;
        public readonly Exception Ex;

        public CrashReport(string desc, Exception ex)
        {
            Description = desc;
            Ex = ex;
        }
    }
}
