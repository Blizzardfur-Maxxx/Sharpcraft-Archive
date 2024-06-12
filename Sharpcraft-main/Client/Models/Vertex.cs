using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class Vertex
    {
        public Vec3 vector3D;
        public float texturePositionX;
        public float texturePositionY;

        public Vertex(float f1, float f2, float f3, float f4, float f5) : this(Vec3.CreateVec3(f1, f2, f3), f4, f5)
        {
        }

        public virtual Vertex SetTexturePosition(float f1, float f2)
        {
            return new Vertex(this, f1, f2);
        }

        public Vertex(Vertex positionTextureVertex1, float f2, float f3)
        {
            vector3D = positionTextureVertex1.vector3D;
            texturePositionX = f2;
            texturePositionY = f3;
        }

        public Vertex(Vec3 vec3D1, float f2, float f3)
        {
            vector3D = vec3D1;
            texturePositionX = f2;
            texturePositionY = f3;
        }
    }
}
