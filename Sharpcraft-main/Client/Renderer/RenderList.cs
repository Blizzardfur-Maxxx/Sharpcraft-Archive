using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharpCraft.Client.Renderer
{
    public class RenderList
    {
        private int x;
        private int y;
        private int z;
        private double chunkX;
        private double chunkY;
        private double chunkZ;
        private readonly uint[] buff = new uint[65536];
        private readonly int cap;
        private int pos, lim;
        private bool render = false;
        private bool cached = false;

        public RenderList() 
        {
            cap = buff.Length;
            pos = buff.Length;
            lim = buff.Length;
        }

        public void Init(int x, int y, int z, double chunkX, double chunkY, double chunkZ)
        {
            render = true;
            pos = 0;
            lim = cap;
            this.x = x;
            this.y = y;
            this.z = z;
            this.chunkX = chunkX;
            this.chunkY = chunkY;
            this.chunkZ = chunkZ;
        }

        public bool IsAt(int x, int y, int z)
        {
            return !this.render ? false : x == this.x && y == this.y && z == this.z;
        }

        public void Add(uint i1)
        {
            this.buff[pos++] = i1;
            if ((lim - pos) == 0)
            {
                this.Render();
            }

        }

        public unsafe void Render()
        {
            if (this.render)
            {
                if (!this.cached)
                {
                    lim = pos;
                    pos = 0;
                    this.cached = true;
                }

                int rem = lim - pos;
                if (rem > 0)
                {
                    GL11.glPushMatrix();
                    GL11.glTranslated(this.x - this.chunkX, this.y - this.chunkY, this.z - this.chunkZ);
                    fixed (uint* p = &buff[0]) 
                    {
                        GL11_PTR.glCallLists(rem, GL11C.GL_UNSIGNED_INT, p);
                    }
                    GL11.glPopMatrix();
                }

            }
        }

        public void Func_859_b()
        {
            this.render = false;
            this.cached = false;
        }
    }

}
