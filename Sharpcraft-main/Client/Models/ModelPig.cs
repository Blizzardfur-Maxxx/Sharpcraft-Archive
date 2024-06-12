using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelPig : ModelQuadruped
    {
        public ModelPig() : base(6, 0.0F)
        { }

        public ModelPig(float f1) : base(6, f1)
        { }
    }
}
