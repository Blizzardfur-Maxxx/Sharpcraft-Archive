using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace SharpCraft.Core.World.GameLevel.LevelGen
{
    public class FlatLevelSource : IChunkSource
    {
        private bool noGen = false; 
        private Level level;

        public FlatLevelSource(Level level, bool noGen) 
        {
            this.level = level;
            this.noGen = noGen;
        }

        public FlatLevelSource(Level level) 
        {
            this.level = level;
        }

        public LevelChunk Create(int i1, int i2)
        {
            return GetChunk(i1, i2);
        }

        public string GatherStats()
        {
            return "FlatLevelSource";
        }

        public LevelChunk GetChunk(int cx, int cz)
        {
            byte[] tiles = new byte[16 * 128 * 16];
            if (!(noGen && (cx > 1 || cz > 1 || cz < -2 || cx < -2))) 
            {
                for (int y = 0; y < 128; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            int idx = x << 11 | z << 7 | y;
                            int id;

                            if (y == 0)
                                id = 7;
                            else if (y <= 59)
                                id = 1;
                            else if (y <= 62)
                                id = 3;
                            else if (y <= 63)
                                id = 2;
                            else
                                id = 0;

                            //spawn point bs
                            if (cx == 0 && cz == 0 && x == 0 && y == 63 && z == 0)
                            {
                                id = Tile.sand.id;
                            }

                            tiles[idx] = (byte)id;
                        }
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

        private bool placed = false;

        public void PostProcess(IChunkSource iChunkProvider1, int cx, int cz)
        {
            if (Enhancements.FLAT_LEVEL_ITEM_CHESTS) 
            {
                if (!placed && cx == 0 && cz == 0)
                {
                    placed = true;
                    int x = cx + 1;
                    int z = cz + 1;
                    int y = level.GetHeightValue(x, z);
                    level.SetTile(x, y, z, Tile.chest.id);
                    TileEntityChest choost = (TileEntityChest)level.GetTileEntity(x, y, z);
                    int chestSlot = 0;

                    int i;
                    for (i = 0; i < 256; ++i)
                    {
                        if (Tile.tiles[i] != null)
                        {
                            if (choost != null)
                            {
                                choost.SetItem(chestSlot, new ItemInstance(Tile.tiles[i], 64));
                            }

                            ++chestSlot;
                            if (chestSlot == 27)
                            {
                                chestSlot = 0;
                                x += 2;
                                y = level.GetHeightValue(x, z);
                                level.SetTile(x, y, z, Tile.chest.id);
                                choost = (TileEntityChest)level.GetTileEntity(x, y, z);
                            }
                        }
                    }

                    for (i = 0; i < Item.items.Length; ++i)
                    {
                        if (Item.items[i] != null)
                        {
                            if (choost != null)
                            {
                                choost.SetItem(chestSlot, new ItemInstance(Item.items[i], 64));
                            }

                            ++chestSlot;
                            if (chestSlot == 27)
                            {
                                chestSlot = 0;
                                x += 2;
                                y = level.GetHeightValue(x, z);
                                level.SetTile(x, y, z, Tile.chest.id);
                                choost = (TileEntityChest)level.GetTileEntity(x, y, z);
                            }
                        }
                    }
                }
            }
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
