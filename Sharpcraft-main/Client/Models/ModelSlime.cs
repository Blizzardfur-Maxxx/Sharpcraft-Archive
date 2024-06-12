using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelSlime : Model
    {
        ModelRenderer slimeBodies;
        ModelRenderer slimeRightEye;
        ModelRenderer slimeLeftEye;
        ModelRenderer slimeMouth;

        public ModelSlime(int i1)
        {
            this.slimeBodies = new ModelRenderer(0, i1);
            this.slimeBodies.AddBox(-4.0F, 16.0F, -4.0F, 8, 8, 8);
            if (i1 > 0)
            {
                this.slimeBodies = new ModelRenderer(0, i1);
                this.slimeBodies.AddBox(-3.0F, 17.0F, -3.0F, 6, 6, 6);
                this.slimeRightEye = new ModelRenderer(32, 0);
                this.slimeRightEye.AddBox(-3.25F, 18.0F, -3.5F, 2, 2, 2);
                this.slimeLeftEye = new ModelRenderer(32, 4);
                this.slimeLeftEye.AddBox(1.25F, 18.0F, -3.5F, 2, 2, 2);
                this.slimeMouth = new ModelRenderer(32, 8);
                this.slimeMouth.AddBox(0.0F, 21.0F, -3.5F, 1, 1, 1);
            }

        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.slimeBodies.Render(f6);
            if (this.slimeRightEye != null)
            {
                this.slimeRightEye.Render(f6);
                this.slimeLeftEye.Render(f6);
                this.slimeMouth.Render(f6);
            }

        }
    }
}
