using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelSheep2 : ModelQuadruped
    {
        public ModelSheep2() : base(12, 0.0f)
        {
            this.head = new ModelRenderer(0, 0);
            this.head.AddBox(-3.0F, -4.0F, -6.0F, 6, 6, 8, 0.0F);
            this.head.SetRotationPoint(0.0F, 6.0F, -8.0F);
            this.body = new ModelRenderer(28, 8);
            this.body.AddBox(-4.0F, -10.0F, -7.0F, 8, 16, 6, 0.0F);
            this.body.SetRotationPoint(0.0F, 5.0F, 2.0F);
        }
    }
}
