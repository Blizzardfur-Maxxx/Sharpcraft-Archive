using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class SignModel
    {
        public ModelRenderer signBoard = new ModelRenderer(0, 0);
        public ModelRenderer signStick;

        public SignModel()
        {
            signBoard.AddBox(-12.0F, -14.0F, -1.0F, 24, 12, 2, 0.0F);
            signStick = new ModelRenderer(0, 14);
            signStick.AddBox(-1.0F, -2.0F, -1.0F, 2, 14, 2, 0.0F);
        }

        public void func_887_a()
        {
            signBoard.Render(0.0625F);
            signStick.Render(0.0625F);
        }
    }
}
