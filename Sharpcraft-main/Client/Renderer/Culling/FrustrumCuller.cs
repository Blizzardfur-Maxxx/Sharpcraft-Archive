using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Culling
{
    public class FrustrumCuller : ICuller //frussy culler
    {
        private FrustumData frustum = Frustum.Get();
        private double x;
        private double y;
        private double z;

        public virtual void Prepare(double d1, double d3, double d5)
        {
            this.x = d1;
            this.y = d3;
            this.z = d5;
        }

        public virtual bool CubeInFrustum(double d1, double d3, double d5, double d7, double d9, double d11)
        {
            return this.frustum.CubeInFrustum(d1 - this.x, d3 - this.y, d5 - this.z, d7 - this.x, d9 - this.y, d11 - this.z);
        }

        public virtual bool IsVisible(AABB bb)
        {
            return this.CubeInFrustum(bb.x0, bb.y0, bb.z0, bb.x1, bb.y1, bb.z1);
        }
    }
}
