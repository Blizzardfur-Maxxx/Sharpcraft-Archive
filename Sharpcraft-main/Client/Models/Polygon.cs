using SharpCraft.Client.Renderer;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class Polygon
    {
        public Vertex[] vertexPositions;
        public int nVertices;
        private bool invertNormal;
        public Polygon(Vertex[] positionTextureVertex1)
        {
            nVertices = 0;
            invertNormal = false;
            vertexPositions = positionTextureVertex1;
            nVertices = positionTextureVertex1.Length;
        }

        public Polygon(Vertex[] positionTextureVertex1, int i2, int i3, int i4, int i5) : this(positionTextureVertex1)
        {
            float f6 = 0.0015625F;
            float f7 = 0.003125F;
            positionTextureVertex1[0] = positionTextureVertex1[0].SetTexturePosition(i4 / 64F - f6, i3 / 32F + f7);
            positionTextureVertex1[1] = positionTextureVertex1[1].SetTexturePosition(i2 / 64F + f6, i3 / 32F + f7);
            positionTextureVertex1[2] = positionTextureVertex1[2].SetTexturePosition(i2 / 64F + f6, i5 / 32F - f7);
            positionTextureVertex1[3] = positionTextureVertex1[3].SetTexturePosition(i4 / 64F - f6, i5 / 32F - f7);
        }

        public virtual void FlipFace()
        {
            Vertex[] positionTextureVertex1 = new Vertex[vertexPositions.Length];
            for (int i2 = 0; i2 < vertexPositions.Length; ++i2)
            {
                positionTextureVertex1[i2] = vertexPositions[vertexPositions.Length - i2 - 1];
            }
            vertexPositions = positionTextureVertex1;
        }

        public virtual void Render(Tessellator tessellator1, float f2)
        {
            Vec3 vec3D3 = vertexPositions[1].vector3D.Subtract(vertexPositions[0].vector3D);
            Vec3 vec3D4 = vertexPositions[1].vector3D.Subtract(vertexPositions[2].vector3D);
            Vec3 vec3D5 = vec3D4.CrossProduct(vec3D3).Normalize();
            tessellator1.Begin();
            if (invertNormal)
            {
                tessellator1.Normal(-((float)vec3D5.y), -((float)vec3D5.y), -((float)vec3D5.z));
            }
            else
            {
                tessellator1.Normal((float)vec3D5.x, (float)vec3D5.y, (float)vec3D5.z);
            }

            for (int i6 = 0; i6 < 4; ++i6)
            {
                Vertex positionTextureVertex7 = vertexPositions[i6];
                tessellator1.VertexUV((float)positionTextureVertex7.vector3D.x * f2, (float)positionTextureVertex7.vector3D.y * f2, (float)positionTextureVertex7.vector3D.z * f2, positionTextureVertex7.texturePositionX, positionTextureVertex7.texturePositionY);
            }

            tessellator1.End();
        }
    }
}
