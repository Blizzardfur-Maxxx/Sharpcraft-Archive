using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static LWCSGL.OpenGL.GL11;
using static LWCSGL.OpenGL.GL11C;

namespace SharpCraft.Client.Renderer
{
    public class Tessellator
    {
        private static readonly int MAX_VERTICES = 2097152;
        private float[] vertexBuffer;
        private float[] texCoordBuffer;
        private float[] colorBuffer;
        private byte[] normalBuffer;
        private int vertices = 0;
        private double u;
        private double v;
        private float r;
        private float g;
        private float b;
        private float a;
        private bool hasColor = false;
        private bool hasTexture = false;
        private bool hasNormal = false;
        private bool noColor = false;
        private uint mode;
        private double xo;
        private double yo;
        private double zo;
        private byte normalX;
        private byte normalY;
        private byte normalZ;
        public static readonly Tessellator Instance = new Tessellator(MAX_VERTICES);
        private bool tesselating = false;

        private Tessellator(int bufferSize)
        {
            vertexBuffer = new float[bufferSize];
            texCoordBuffer = new float[bufferSize];
            colorBuffer = new float[bufferSize];
            normalBuffer = new byte[bufferSize * 3];
        }

        public void End()
        {
            if (!tesselating)
            {
                throw new InvalidOperationException("Not tesselating!");
            }
            else
            {
                tesselating = false;

                if (vertices > 0)
                {
                    GCHandle vbHandle = GCHandle.Alloc(vertexBuffer, GCHandleType.Pinned);
                    GCHandle tcbHandle = GCHandle.Alloc(texCoordBuffer, GCHandleType.Pinned);
                    GCHandle cbHandle = GCHandle.Alloc(colorBuffer, GCHandleType.Pinned);
                    GCHandle nbHandle = GCHandle.Alloc(normalBuffer, GCHandleType.Pinned);

                    glVertexPointer(3, GL_FLOAT, 0, vbHandle.AddrOfPinnedObject());
                    if (hasTexture) glTexCoordPointer(2, GL_FLOAT, 0, tcbHandle.AddrOfPinnedObject());
                    if (hasColor) glColorPointer(4, GL_FLOAT, 0, cbHandle.AddrOfPinnedObject());
                    if (hasNormal) glNormalPointer(GL_BYTE, 0, nbHandle.AddrOfPinnedObject());

                    glEnableClientState(GL_VERTEX_ARRAY);
                    if (hasTexture) glEnableClientState(GL_TEXTURE_COORD_ARRAY);
                    if (hasColor) glEnableClientState(GL_COLOR_ARRAY);
                    if (hasNormal) glEnableClientState(GL_NORMAL_ARRAY);

                    glDrawArrays(mode, (int)GL_POINTS, vertices);

                    glDisableClientState(GL_VERTEX_ARRAY);
                    if (hasTexture) glDisableClientState(GL_TEXTURE_COORD_ARRAY);
                    if (hasColor) glDisableClientState(GL_COLOR_ARRAY);
                    if (hasNormal) glDisableClientState(GL_NORMAL_ARRAY);

                    vbHandle.Free();
                    tcbHandle.Free();
                    cbHandle.Free();
                    nbHandle.Free();
                }

                Clear();
            }
        }

        private void Clear()
        {
            vertices = 0;
        }

        public void Begin()
        {
            Begin(GL_QUADS);
        }

        public void Begin(uint mode)
        {
            if (tesselating)
            {
                throw new InvalidOperationException("Already tesselating!");
            }
            else
            {
                tesselating = true;
                Clear();
                this.mode = mode;
                hasNormal = false;
                hasColor = false;
                hasTexture = false;
                noColor = false;
            }
        }

        public void Tex(double u, double v)
        {
            hasTexture = true;
            this.u = u;
            this.v = v;
        }

        public void Color(float r, float g, float b, float a)
        {
            if (noColor) return;
            hasColor = true;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public void Color(float r, float g, float b)
        {
            Color(r, g, b, 1.0F);
        }

        public void Color(int r, int g, int b)
        {
            Color(r, g, b, 255);
        }

        public void Color(int r, int g, int b, int a)
        {
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            a = Math.Clamp(a, 0, 255);
            Color(r / 255.0F, g / 255.0F, b / 255.0F, a / 255.0F);
        }

        public void VertexUV(double x, double y, double z, double u, double v)
        {
            Tex(u, v);
            Vertex(x, y, z);
        }

        public void Vertex(double x, double y, double z)
        {
            vertexBuffer[vertices * 3 + 0] = (float)(x + xo);
            vertexBuffer[vertices * 3 + 1] = (float)(y + yo);
            vertexBuffer[vertices * 3 + 2] = (float)(z + zo);

            if (hasTexture)
            {
                texCoordBuffer[vertices * 2 + 0] = (float)u;
                texCoordBuffer[vertices * 2 + 1] = (float)v;
            }

            if (hasColor)
            {
                colorBuffer[vertices * 4 + 0] = r;
                colorBuffer[vertices * 4 + 1] = g;
                colorBuffer[vertices * 4 + 2] = b;
                colorBuffer[vertices * 4 + 3] = a;
            }

            if (hasNormal)
            {
                normalBuffer[vertices * 3 + 0] = normalX;
                normalBuffer[vertices * 3 + 1] = normalY;
                normalBuffer[vertices * 3 + 2] = normalZ;
            }

            vertices++;
            if (vertices == MAX_VERTICES)
                End();
        }

        public void Color(int color)
        {
            int r = color >> 16 & 255;
            int g = color >> 8 & 255;
            int b = color & 255;
            Color(r, g, b);
        }

        public void Color(int color, int alpha)
        {
            int r = color >> 16 & 255;
            int g = color >> 8 & 255;
            int b = color & 255;
            Color(r, g, b, alpha);
        }

        public void NoColor()
        {
            noColor = true;
        }

        public void Normal(float x, float y, float z)
        {
            if (!tesselating)
                Console.WriteLine("But..");

            hasNormal = true;
            byte xb = (byte)(int)(x * 128F);
            byte yb = (byte)(int)(y * 127F);
            byte zb = (byte)(int)(z * 127F);
            normalX = xb;
            normalY = yb;
            normalZ = zb;
        }

        public void Offset(double x, double y, double z)
        {
            xo = x;
            yo = y;
            zo = z;
        }

        public void AddOffset(float x, float y, float z)
        {
            xo += x;
            yo += y;
            zo += z;
        }
    }
}
