using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.World.GameLevel.LevelGen
{
    public class SkyGridLevelSource : IChunkSource
    {
        private JRandom random;
        private Level level;

        public SkyGridLevelSource(Level level, long seed)
        {
            this.level = level;
            this.random = new JRandom(seed);
        }

        public LevelChunk Create(int i1, int i2)
        {
            return GetChunk(i1, i2);
        }

        public string GatherStats()
        {
            return "SkyGridLevelSource";
        }


        private static int PickRandomTile(JRandom rng, int x, int z, int y) 
        {
            int mod = 2; //use 4 if too crashy, i would use 3 but not aligned to grid
            if (x % mod == 0 && z % mod == 0 && y % mod == 0)
            {
                Tile t = null;
                while (t == null) 
                {
                    t = Tile.tiles[rng.NextInt(Tile.tiles.Length)];
                }
                return t.id;
            }

            return 0;
        }

        public LevelChunk GetChunk(int cx, int cz)
        {
            byte[] tiles = new byte[16 * 128 * 16];
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 0; y < 128; y++)
                    {
                        tiles[x << 11 | z << 7 | y] = (byte)PickRandomTile(this.random, x, z, y);
                    }
                }
            }
            
            LevelChunk chunk = new LevelChunk(level, tiles, cx, cz);
            chunk.CalculateLight();
            return chunk;
        }

        public bool HasChunk(int i1, int i2)
        {
            return true;
        }

        public void PostProcess(IChunkSource iChunkProvider1, int i2, int i3)
        {
        }

        public bool Save(bool z1, IProgressListener iProgressUpdate2)
        {
            return true;
        }

        public bool ShouldSave()
        {
            return true;
        }

        public bool Tick()
        {
            return false;
        }
    }
}
