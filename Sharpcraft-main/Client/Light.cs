using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class Light
    {
        private static int pos = 0;
        private static readonly float[] lb = new float[16];

        public static void TurnOff()
        {
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_LIGHT0);
            GL11.glDisable(GL11C.GL_LIGHT1);
            GL11.glDisable(GL11C.GL_COLOR_MATERIAL);
        }

        public static void TurnOn()
        {
            GL11.glEnable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_LIGHT0);
            GL11.glEnable(GL11C.GL_LIGHT1);
            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            GL11.glColorMaterial(GL11C.GL_FRONT_AND_BACK, GL11C.GL_AMBIENT_AND_DIFFUSE);
            float f0 = 0.4F;
            float ambient = 0.6F;
            float specular = 0.0F;
            Vec3 vec3D3 = Vec3.Of(0.2F, 1.0D, -0.699999988079071D).Normalize();
            GL11.glLightfv(GL11C.GL_LIGHT0, GL11C.GL_POSITION, GetBuffer(vec3D3.x, vec3D3.y, vec3D3.z, 0.0D));
            GL11.glLightfv(GL11C.GL_LIGHT0, GL11C.GL_DIFFUSE, GetBuffer(ambient, ambient, ambient, 1.0F));
            GL11.glLightfv(GL11C.GL_LIGHT0, GL11C.GL_AMBIENT, GetBuffer(0.0F, 0.0F, 0.0F, 1.0F));
            GL11.glLightfv(GL11C.GL_LIGHT0, GL11C.GL_SPECULAR, GetBuffer(specular, specular, specular, 1.0F));
            vec3D3 = Vec3.Of(-0.20000000298023224D, 1.0D, 0.7F).Normalize();
            GL11.glLightfv(GL11C.GL_LIGHT1, GL11C.GL_POSITION, GetBuffer(vec3D3.x, vec3D3.y, vec3D3.z, 0.0D));
            GL11.glLightfv(GL11C.GL_LIGHT1, GL11C.GL_DIFFUSE, GetBuffer(ambient, ambient, ambient, 1.0F));
            GL11.glLightfv(GL11C.GL_LIGHT1, GL11C.GL_AMBIENT, GetBuffer(0.0F, 0.0F, 0.0F, 1.0F));
            GL11.glLightfv(GL11C.GL_LIGHT1, GL11C.GL_SPECULAR, GetBuffer(specular, specular, specular, 1.0F));
            GL11.glShadeModel(GL11C.GL_FLAT);
            GL11.glLightModelfv(GL11C.GL_LIGHT_MODEL_AMBIENT, GetBuffer(f0, f0, f0, 1.0F));
        }

        public static float[] GetBuffer(double a, double b, double c, double d) 
        {
            return GetBuffer((float)a, (float)b, (float)c, (float)d);
        }

        private static float[] GetBuffer(float r, float g, float b, float a)
        {
            pos = 0;
            lb[pos++] = r;
            lb[pos++] = g;
            lb[pos++] = b;
            lb[pos++] = a;
            pos = 0;
            return lb;
        }
    }
}
