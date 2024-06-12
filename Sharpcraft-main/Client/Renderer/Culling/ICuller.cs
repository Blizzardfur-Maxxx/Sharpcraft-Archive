using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Culling
{
    public interface ICuller
    {
        bool IsVisible(AABB bb);

        void Prepare(double x, double y, double z);
    }
}
