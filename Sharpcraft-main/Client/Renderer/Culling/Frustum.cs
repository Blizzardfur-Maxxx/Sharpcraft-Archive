using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Culling
{
    public class Frustum : FrustumData
    {
        private static Frustum instance = new Frustum();

        public static FrustumData Get()
        {
            instance.Init();
            return instance;
        }

        private void NormalizePlane(float[,] matrix, int idx)
        {
            float f3 = Mth.Sqrt(matrix[idx,0] * matrix[idx,0] + matrix[idx, 1] * matrix[idx, 1] + matrix[idx, 2] * matrix[idx, 2]);
            matrix[idx, 0] /= f3;
            matrix[idx, 1] /= f3;
            matrix[idx, 2] /= f3;
            matrix[idx, 3] /= f3;
        }

        private void Init() 
        {
            GL11.glGetFloatv(GL11C.GL_PROJECTION_MATRIX, proj);
            GL11.glGetFloatv(GL11C.GL_MODELVIEW_MATRIX, modl);

            this.clip[0] = this.modl[0] * this.proj[0] + this.modl[1] * this.proj[4] + this.modl[2] * this.proj[8] + this.modl[3] * this.proj[12];
            this.clip[1] = this.modl[0] * this.proj[1] + this.modl[1] * this.proj[5] + this.modl[2] * this.proj[9] + this.modl[3] * this.proj[13];
            this.clip[2] = this.modl[0] * this.proj[2] + this.modl[1] * this.proj[6] + this.modl[2] * this.proj[10] + this.modl[3] * this.proj[14];
            this.clip[3] = this.modl[0] * this.proj[3] + this.modl[1] * this.proj[7] + this.modl[2] * this.proj[11] + this.modl[3] * this.proj[15];
            this.clip[4] = this.modl[4] * this.proj[0] + this.modl[5] * this.proj[4] + this.modl[6] * this.proj[8] + this.modl[7] * this.proj[12];
            this.clip[5] = this.modl[4] * this.proj[1] + this.modl[5] * this.proj[5] + this.modl[6] * this.proj[9] + this.modl[7] * this.proj[13];
            this.clip[6] = this.modl[4] * this.proj[2] + this.modl[5] * this.proj[6] + this.modl[6] * this.proj[10] + this.modl[7] * this.proj[14];
            this.clip[7] = this.modl[4] * this.proj[3] + this.modl[5] * this.proj[7] + this.modl[6] * this.proj[11] + this.modl[7] * this.proj[15];
            this.clip[8] = this.modl[8] * this.proj[0] + this.modl[9] * this.proj[4] + this.modl[10] * this.proj[8] + this.modl[11] * this.proj[12];
            this.clip[9] = this.modl[8] * this.proj[1] + this.modl[9] * this.proj[5] + this.modl[10] * this.proj[9] + this.modl[11] * this.proj[13];
            this.clip[10] = this.modl[8] * this.proj[2] + this.modl[9] * this.proj[6] + this.modl[10] * this.proj[10] + this.modl[11] * this.proj[14];
            this.clip[11] = this.modl[8] * this.proj[3] + this.modl[9] * this.proj[7] + this.modl[10] * this.proj[11] + this.modl[11] * this.proj[15];
            this.clip[12] = this.modl[12] * this.proj[0] + this.modl[13] * this.proj[4] + this.modl[14] * this.proj[8] + this.modl[15] * this.proj[12];
            this.clip[13] = this.modl[12] * this.proj[1] + this.modl[13] * this.proj[5] + this.modl[14] * this.proj[9] + this.modl[15] * this.proj[13];
            this.clip[14] = this.modl[12] * this.proj[2] + this.modl[13] * this.proj[6] + this.modl[14] * this.proj[10] + this.modl[15] * this.proj[14];
            this.clip[15] = this.modl[12] * this.proj[3] + this.modl[13] * this.proj[7] + this.modl[14] * this.proj[11] + this.modl[15] * this.proj[15];
            this.frustrum[0,0] = this.clip[3] - this.clip[0];
            this.frustrum[0,1] = this.clip[7] - this.clip[4];
            this.frustrum[0,2] = this.clip[11] - this.clip[8];
            this.frustrum[0,3] = this.clip[15] - this.clip[12];
            this.NormalizePlane(this.frustrum, 0);
            this.frustrum[1,0] = this.clip[3] + this.clip[0];
            this.frustrum[1,1] = this.clip[7] + this.clip[4];
            this.frustrum[1,2] = this.clip[11] + this.clip[8];
            this.frustrum[1,3] = this.clip[15] + this.clip[12];
            this.NormalizePlane(this.frustrum, 1);
            this.frustrum[2,0] = this.clip[3] + this.clip[1];
            this.frustrum[2,1] = this.clip[7] + this.clip[5];
            this.frustrum[2,2] = this.clip[11] + this.clip[9];
            this.frustrum[2,3] = this.clip[15] + this.clip[13];
            this.NormalizePlane(this.frustrum, 2);
            this.frustrum[3,0] = this.clip[3] - this.clip[1];
            this.frustrum[3,1] = this.clip[7] - this.clip[5];
            this.frustrum[3,2] = this.clip[11] - this.clip[9];
            this.frustrum[3,3] = this.clip[15] - this.clip[13];
            this.NormalizePlane(this.frustrum, 3);
            this.frustrum[4,0] = this.clip[3] - this.clip[2];
            this.frustrum[4,1] = this.clip[7] - this.clip[6];
            this.frustrum[4,2] = this.clip[11] - this.clip[10];
            this.frustrum[4,3] = this.clip[15] - this.clip[14];
            this.NormalizePlane(this.frustrum, 4);
            this.frustrum[5,0] = this.clip[3] + this.clip[2];
            this.frustrum[5,1] = this.clip[7] + this.clip[6];
            this.frustrum[5,2] = this.clip[11] + this.clip[10];
            this.frustrum[5,3] = this.clip[15] + this.clip[14];
            this.NormalizePlane(this.frustrum, 5);
        }
    }
}
