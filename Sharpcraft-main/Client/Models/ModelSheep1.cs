using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelSheep1 : ModelQuadruped
    {
        public ModelSheep1() : base(12, 0.0F)
        {

            this.head = new ModelRenderer(0, 0);
            this.head.AddBox(-3.0F, -4.0F, -4.0F, 6, 6, 6, 0.6F);
            this.head.SetRotationPoint(0.0F, 6.0F, -8.0F);
            this.body = new ModelRenderer(28, 8);
            this.body.AddBox(-4.0F, -10.0F, -7.0F, 8, 16, 6, 1.75F);
            this.body.SetRotationPoint(0.0F, 5.0F, 2.0F);
            float f1 = 0.5F;
            this.leg1 = new ModelRenderer(0, 16);
            this.leg1.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg1.SetRotationPoint(-3.0F, 12.0F, 7.0F);
            this.leg2 = new ModelRenderer(0, 16);
            this.leg2.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg2.SetRotationPoint(3.0F, 12.0F, 7.0F);
            this.leg3 = new ModelRenderer(0, 16);
            this.leg3.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg3.SetRotationPoint(-3.0F, 12.0F, -5.0F);
            this.leg4 = new ModelRenderer(0, 16);
            this.leg4.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg4.SetRotationPoint(3.0F, 12.0F, -5.0F);
        }
    }
}
