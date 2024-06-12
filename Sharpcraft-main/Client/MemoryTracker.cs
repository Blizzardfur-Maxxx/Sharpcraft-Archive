using LWCSGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    public class MemoryTracker
    {
        private static List<uint> lists = new List<uint>();
        private static List<uint> textures = new List<uint>();

        public static uint GenLists(int count)
        {
            uint id = GL11.glGenLists(count);
            lists.Add(id);
            lists.Add((uint)count);
            return id;
        }

        public static void GenTextures(uint[] ib)
        {
            GL11.glGenTextures(ib.Length, ib);
            for (int i1 = 0; i1 < ib.Length; ++i1)
            {
                textures.Add(ib[i1]);
            }
        }

        public static void Release(uint pos)
        {
            int i1 = lists.IndexOf(pos);
            GL11.glDeleteLists(lists[i1], (int)lists[i1 + 1]);
            lists.Remove((uint)i1);
            lists.Remove((uint)i1);
        }

        public static void Release() 
        {
            for (int i0 = 0; i0 < lists.Count; i0 += 2)
            {
                GL11.glDeleteLists(lists[i0], (int)lists[i0 + 1]);
            }

            uint[] intBuffer2 = new uint[textures.Count];
            GL11.glDeleteTextures(intBuffer2.Length, intBuffer2);

            for (int i1 = 0; i1 < textures.Count; ++i1)
            {
                intBuffer2[i1] = textures[i1];
            }

            GL11.glDeleteTextures(intBuffer2.Length, intBuffer2);
            lists.Clear();
            textures.Clear();
        }
    }
}
