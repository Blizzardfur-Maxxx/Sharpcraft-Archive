using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.GameLevel
{
    public class Region : ILevelSource
    {
        private int chunkX;
        private int chunkZ;
        private LevelChunk[][] chunkArray;
        private Level worldObj;

        public Region(Level world1, int x0, int y0, int z0, int x1, int y1, int z1)
        {
            this.worldObj = world1;
            this.chunkX = x0 >> 4;
            this.chunkZ = z0 >> 4;
            int i8 = x1 >> 4;
            int i9 = z1 >> 4;
            this.chunkArray = new LevelChunk[i8 - this.chunkX + 1][];
            ArrayUtil.Init2DArray(chunkArray, i9 - this.chunkZ + 1);

            for (int i10 = this.chunkX; i10 <= i8; ++i10)
            {
                for (int i11 = this.chunkZ; i11 <= i9; ++i11)
                {
                    this.chunkArray[i10 - this.chunkX][i11 - this.chunkZ] = world1.GetChunk(i10, i11);
                }
            }
        }

        public virtual int GetTile(int i1, int i2, int i3)
        {
            if (i2 < 0)
            {
                return 0;
            }
            else if (i2 >= 128)
            {
                return 0;
            }
            else
            {
                int i4 = (i1 >> 4) - this.chunkX;
                int i5 = (i3 >> 4) - this.chunkZ;
                if (i4 >= 0 && i4 < this.chunkArray.Length && i5 >= 0 && i5 < this.chunkArray[i4].Length)
                {
                    LevelChunk chunk6 = this.chunkArray[i4][i5];
                    return chunk6 == null ? 0 : chunk6.GetBlockID(i1 & 15, i2, i3 & 15);
                }
                else
                {
                    return 0;
                }
            }
        }

        public virtual TileEntity GetTileEntity(int i1, int i2, int i3)
        {
            int i4 = (i1 >> 4) - this.chunkX;
            int i5 = (i3 >> 4) - this.chunkZ;
            return this.chunkArray[i4][i5].GetChunkBlockTileEntity(i1 & 15, i2, i3 & 15);
        }

        public virtual float GetBrightness(int i1, int i2, int i3, int i4)
        {
            int i5 = this.GetRawBrightness(i1, i2, i3);
            if (i5 < i4)
            {
                i5 = i4;
            }

            return this.worldObj.dimension.lightBrightnessTable[i5];
        }

        public virtual float GetBrightness(int i1, int i2, int i3)
        {
            return this.worldObj.dimension.lightBrightnessTable[this.GetRawBrightness(i1, i2, i3)];
        }

        public virtual int GetRawBrightness(int i1, int i2, int i3)
        {
            return this.GetRawBrightness(i1, i2, i3, true);
        }

        public virtual int GetRawBrightness(int i1, int i2, int i3, bool z4)
        {
            if (i1 >= -32000000 && i3 >= -32000000 && i1 < 32000000 && i3 <= 32000000)
            {
                int i5;
                int i6;
                if (z4)
                {
                    i5 = this.GetTile(i1, i2, i3);
                    if (i5 == Tile.stoneSlabHalf.id || i5 == Tile.farmland.id || i5 == Tile.stairs_wood.id || i5 == Tile.stairs_stone.id)
                    {
                        i6 = this.GetRawBrightness(i1, i2 + 1, i3, false);
                        int i7 = this.GetRawBrightness(i1 + 1, i2, i3, false);
                        int i8 = this.GetRawBrightness(i1 - 1, i2, i3, false);
                        int i9 = this.GetRawBrightness(i1, i2, i3 + 1, false);
                        int i10 = this.GetRawBrightness(i1, i2, i3 - 1, false);
                        if (i7 > i6)
                        {
                            i6 = i7;
                        }

                        if (i8 > i6)
                        {
                            i6 = i8;
                        }

                        if (i9 > i6)
                        {
                            i6 = i9;
                        }

                        if (i10 > i6)
                        {
                            i6 = i10;
                        }

                        return i6;
                    }
                }

                if (i2 < 0)
                {
                    return 0;
                }
                else if (i2 >= 128)
                {
                    i5 = 15 - this.worldObj.skyDarken;
                    if (i5 < 0)
                    {
                        i5 = 0;
                    }

                    return i5;
                }
                else
                {
                    i5 = (i1 >> 4) - this.chunkX;
                    i6 = (i3 >> 4) - this.chunkZ;
                    return this.chunkArray[i5][i6].IsSkyLit(i1 & 15, i2, i3 & 15, this.worldObj.skyDarken);
                }
            }
            else
            {
                return 15;
            }
        }

        public virtual int GetData(int i1, int i2, int i3)
        {
            if (i2 < 0)
            {
                return 0;
            }
            else if (i2 >= 128)
            {
                return 0;
            }
            else
            {
                int i4 = (i1 >> 4) - this.chunkX;
                int i5 = (i3 >> 4) - this.chunkZ;
                return this.chunkArray[i4][i5].GetBlockMetadata(i1 & 15, i2, i3 & 15);
            }
        }

        public virtual Material GetMaterial(int i1, int i2, int i3)
        {
            int i4 = this.GetTile(i1, i2, i3);
            return i4 == 0 ? Material.air : Tile.tiles[i4].material;
        }

        public virtual BiomeSource GetBiomeSource()
        {
            return this.worldObj.GetBiomeSource();
        }

        public virtual bool IsSolidRenderTile(int i1, int i2, int i3)
        {
            Tile block4 = Tile.tiles[this.GetTile(i1, i2, i3)];
            return block4 == null ? false : block4.IsSolidRender();
        }

        public virtual bool IsSolidBlockingTile(int i1, int i2, int i3)
        {
            Tile block4 = Tile.tiles[this.GetTile(i1, i2, i3)];
            return block4 == null ? false : block4.material.BlocksMotion() && block4.IsCubeShaped();
        }
    }
}