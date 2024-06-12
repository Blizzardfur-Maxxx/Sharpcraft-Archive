using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Culling
{
    public class FrustumData
    {
        public float[,] frustrum = new float[16, 16];
        public float[] proj = new float[16];
        public float[] modl = new float[16];
        public float[] clip = new float[16];

        public bool CubeInFrustum(double d1, double d3, double d5, double d7, double d9, double d11)
        {
            for (int i13 = 0; i13 < 6; ++i13)
            {
                if (this.frustrum[i13,0] * d1 + this.frustrum[i13,1] * d3 + this.frustrum[i13,2] *
                        d5 + this.frustrum[i13,3] <= 0.0D && this.frustrum[i13,0] * d7 + this.frustrum[i13,1]
                            * d3 + this.frustrum[i13,2] * d5 + this.frustrum[i13,3] <= 0.0D && this.frustrum[i13,0]
                                * d1 + this.frustrum[i13,1] * d9 + this.frustrum[i13,2] * d5 + this.frustrum[i13,3] <= 0.0D
                                    && this.frustrum[i13,0] * d7 + this.frustrum[i13,1] * d9 + this.frustrum[i13,2] * d5 + this.frustrum[i13,3]
                                    <= 0.0D && this.frustrum[i13,0] * d1 + this.frustrum[i13,1] * d3 + this.frustrum[i13,2] * d11 + this.frustrum[i13,3]
                                        <= 0.0D && this.frustrum[i13,0] * d7 + this.frustrum[i13,1] * d3 + this.frustrum[i13,2] * d11 + this.frustrum[i13,3]
                                            <= 0.0D && this.frustrum[i13,0] * d1 + this.frustrum[i13,1] * d9 + this.frustrum[i13,2] * d11 + this.frustrum[i13,3]
                                                <= 0.0D && this.frustrum[i13,0] * d7 + this.frustrum[i13,1] * d9 + this.frustrum[i13,2] * d11 + this.frustrum[i13,3] <= 0.0D)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
