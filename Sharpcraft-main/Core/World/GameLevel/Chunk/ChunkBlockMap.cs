using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class ChunkBlockMap
    {
        private static byte[] blockMap = new byte[256];
        public static void Load(byte[] b0)
        {
            for (int i1 = 0; i1 < b0.Length; ++i1)
            {
                b0[i1] = blockMap[b0[i1] & 255];
            }
        }

        static ChunkBlockMap()
        {
            try
            {
                for (int i0 = 0; i0 < 256; ++i0)
                {
                    byte b1 = (byte)i0;
                    if (b1 != 0 && Tile.tiles[b1 & 255] == null)
                    {
                        b1 = 0;
                    }

                    blockMap[i0] = b1;
                }
            }
            catch (Exception exception2)
            {
                exception2.PrintStackTrace();
            }
        }
    }
}