using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelSkeleton : ModelZombie
    {
        public ModelSkeleton() 
        {
            float f1 = 0.0F;
            this.bipedRightArm = new ModelRenderer(40, 16);
            this.bipedRightArm.AddBox(-1.0F, -2.0F, -1.0F, 2, 12, 2, f1);
            this.bipedRightArm.SetRotationPoint(-5.0F, 2.0F, 0.0F);
            this.bipedLeftArm = new ModelRenderer(40, 16);
            this.bipedLeftArm.mirror = true;
            this.bipedLeftArm.AddBox(-1.0F, -2.0F, -1.0F, 2, 12, 2, f1);
            this.bipedLeftArm.SetRotationPoint(5.0F, 2.0F, 0.0F);
            this.bipedRightLeg = new ModelRenderer(0, 16);
            this.bipedRightLeg.AddBox(-1.0F, 0.0F, -1.0F, 2, 12, 2, f1);
            this.bipedRightLeg.SetRotationPoint(-2.0F, 12.0F, 0.0F);
            this.bipedLeftLeg = new ModelRenderer(0, 16);
            this.bipedLeftLeg.mirror = true;
            this.bipedLeftLeg.AddBox(-1.0F, 0.0F, -1.0F, 2, 12, 2, f1);
            this.bipedLeftLeg.SetRotationPoint(2.0F, 12.0F, 0.0F);
        }
    }
}
