using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class LevelChunk
    {
        public static bool isLit;
        public byte[] blocks;
        public bool isChunkLoaded;
        public Level worldObj;
        public DataLayer data;
        public DataLayer skylightMap;
        public DataLayer blocklightMap;
        public byte[] heightMap;
        public int lowestBlockHeight;
        public readonly int xPosition;
        public readonly int zPosition;
        public NullDictionary<TilePos, TileEntity> chunkTileEntityMap;
        public List<Entity>[] entities;
        public bool isTerrainPopulated;
        public bool isModified;
        public bool neverSave;
        public bool hasEntities;
        public long lastSaveTime;
        public LevelChunk(Level world1, int i2, int i3)
        {
            this.chunkTileEntityMap = new NullDictionary<TilePos, TileEntity>();
            this.entities = new List<Entity>[8];
            this.isTerrainPopulated = false;
            this.isModified = false;
            this.hasEntities = false;
            this.lastSaveTime = 0;
            this.worldObj = world1;
            this.xPosition = i2;
            this.zPosition = i3;
            this.heightMap = new byte[256];
            for (int i4 = 0; i4 < this.entities.Length; ++i4)
            {
                this.entities[i4] = new List<Entity>();
            }
        }

        public LevelChunk(Level world1, byte[] b2, int i3, int i4) : this(world1, i3, i4)
        {
            this.blocks = b2;
            this.data = new DataLayer(b2.Length);
            this.skylightMap = new DataLayer(b2.Length);
            this.blocklightMap = new DataLayer(b2.Length);
        }

        public virtual bool IsAtLocation(int i1, int i2)
        {
            return i1 == this.xPosition && i2 == this.zPosition;
        }

        public virtual int GetHeightValue(int i1, int i2)
        {
            return this.heightMap[i2 << 4 | i1] & 255;
        }

        public virtual void Func_a()
        {
        }

        public virtual void GenerateHeightMap()
        {
            int i1 = 127;
            for (int i2 = 0; i2 < 16; ++i2)
            {
                for (int i3 = 0; i3 < 16; ++i3)
                {
                    int i4 = 127;
                    for (int i5 = i2 << 11 | i3 << 7; i4 > 0 && Tile.lightBlock[this.blocks[i5 + i4 - 1] & 255] == 0; --i4)
                    {
                    }

                    this.heightMap[i3 << 4 | i2] = (byte)i4;
                    if (i4 < i1)
                    {
                        i1 = i4;
                    }
                }
            }

            this.lowestBlockHeight = i1;
            this.isModified = true;
        }

        public virtual void CalculateLight()
        {
            int i1 = 127;
            int i2;
            int i3;
            for (i2 = 0; i2 < 16; ++i2)
            {
                for (i3 = 0; i3 < 16; ++i3)
                {
                    int i4 = 127;
                    int i5;
                    for (i5 = i2 << 11 | i3 << 7; i4 > 0 && Tile.lightBlock[this.blocks[i5 + i4 - 1] & 255] == 0; --i4)
                    {
                    }

                    this.heightMap[i3 << 4 | i2] = (byte)i4;
                    if (i4 < i1)
                    {
                        i1 = i4;
                    }

                    if (!this.worldObj.dimension.hasNoSky)
                    {
                        int i6 = 15;
                        int i7 = 127;
                        do
                        {
                            i6 -= Tile.lightBlock[this.blocks[i5 + i7] & 255];
                            if (i6 > 0)
                            {
                                this.skylightMap.Set(i2, i7, i3, i6);
                            }

                            --i7;
                        }
                        while (i7 > 0 && i6 > 0);
                    }
                }
            }

            this.lowestBlockHeight = i1;
            for (i2 = 0; i2 < 16; ++i2)
            {
                for (i3 = 0; i3 < 16; ++i3)
                {
                    this.Func_996(i2, i3);
                }
            }

            this.isModified = true;
        }

        public virtual void Func_4143()
        {
        }

        private void Func_996(int i1, int i2)
        {
            int i3 = this.GetHeightValue(i1, i2);
            int i4 = this.xPosition * 16 + i1;
            int i5 = this.zPosition * 16 + i2;
            this.Func_1020(i4 - 1, i5, i3);
            this.Func_1020(i4 + 1, i5, i3);
            this.Func_1020(i4, i5 - 1, i3);
            this.Func_1020(i4, i5 + 1, i3);
        }

        private void Func_1020(int i1, int i2, int i3)
        {
            int i4 = this.worldObj.GetHeightValue(i1, i2);
            if (i4 > i3)
            {
                this.worldObj.UpdateLight(LightLayer.Sky, i1, i3, i2, i1, i4, i2);
                this.isModified = true;
            }
            else if (i4 < i3)
            {
                this.worldObj.UpdateLight(LightLayer.Sky, i1, i4, i2, i1, i3, i2);
                this.isModified = true;
            }
        }

        private void Func_1003(int i1, int i2, int i3)
        {
            int i4 = this.heightMap[i3 << 4 | i1] & 255;
            int i5 = i4;
            if (i2 > i4)
            {
                i5 = i2;
            }

            for (int i6 = i1 << 11 | i3 << 7; i5 > 0 && Tile.lightBlock[this.blocks[i6 + i5 - 1] & 255] == 0; --i5)
            {
            }

            if (i5 != i4)
            {
                this.worldObj.LightColumnChanged(i1, i3, i5, i4);
                this.heightMap[i3 << 4 | i1] = (byte)i5;
                int i7;
                int i8;
                int i9;
                if (i5 < this.lowestBlockHeight)
                {
                    this.lowestBlockHeight = i5;
                }
                else
                {
                    i7 = 127;
                    for (i8 = 0; i8 < 16; ++i8)
                    {
                        for (i9 = 0; i9 < 16; ++i9)
                        {
                            if ((this.heightMap[i9 << 4 | i8] & 255) < i7)
                            {
                                i7 = this.heightMap[i9 << 4 | i8] & 255;
                            }
                        }
                    }

                    this.lowestBlockHeight = i7;
                }

                i7 = this.xPosition * 16 + i1;
                i8 = this.zPosition * 16 + i3;
                if (i5 < i4)
                {
                    for (i9 = i5; i9 < i4; ++i9)
                    {
                        this.skylightMap.Set(i1, i9, i3, 15);
                    }
                }
                else
                {
                    this.worldObj.UpdateLight(LightLayer.Sky, i7, i4, i8, i7, i5, i8);
                    for (i9 = i4; i9 < i5; ++i9)
                    {
                        this.skylightMap.Set(i1, i9, i3, 0);
                    }
                }

                i9 = 15;
                int i10;
                for (i10 = i5; i5 > 0 && i9 > 0; this.skylightMap.Set(i1, i5, i3, i9))
                {
                    --i5;
                    int i11 = Tile.lightBlock[this.GetBlockID(i1, i5, i3)];
                    if (i11 == 0)
                    {
                        i11 = 1;
                    }

                    i9 -= i11;
                    if (i9 < 0)
                    {
                        i9 = 0;
                    }
                }

                while (i5 > 0 && Tile.lightBlock[this.GetBlockID(i1, i5 - 1, i3)] == 0)
                {
                    --i5;
                }

                if (i5 != i10)
                {
                    this.worldObj.UpdateLight(LightLayer.Sky, i7 - 1, i5, i8 - 1, i7 + 1, i10, i8 + 1);
                }

                this.isModified = true;
            }
        }

        public virtual int GetBlockID(int i1, int i2, int i3)
        {
            return this.blocks[i1 << 11 | i3 << 7 | i2] & 255;
        }

        public virtual bool SetBlockIDWithMetadata(int i1, int i2, int i3, int i4, int i5)
        {
            byte b6 = (byte)i4;
            int i7 = this.heightMap[i3 << 4 | i1] & 255;
            int i8 = this.blocks[i1 << 11 | i3 << 7 | i2] & 255;
            if (i8 == i4 && this.data.Get(i1, i2, i3) == i5)
            {
                return false;
            }
            else
            {
                int i9 = this.xPosition * 16 + i1;
                int i10 = this.zPosition * 16 + i3;
                this.blocks[i1 << 11 | i3 << 7 | i2] = (byte)(b6 & 255);
                if (i8 != 0 && !this.worldObj.isRemote)
                {
                    Tile.tiles[i8].OnBlockRemoval(this.worldObj, i9, i2, i10);
                }

                this.data.Set(i1, i2, i3, i5);
                if (!this.worldObj.dimension.hasNoSky)
                {
                    if (Tile.lightBlock[b6 & 255] != 0)
                    {
                        if (i2 >= i7)
                        {
                            this.Func_1003(i1, i2 + 1, i3);
                        }
                    }
                    else if (i2 == i7 - 1)
                    {
                        this.Func_1003(i1, i2, i3);
                    }

                    this.worldObj.UpdateLight(LightLayer.Sky, i9, i2, i10, i9, i2, i10);
                }

                this.worldObj.UpdateLight(LightLayer.Block, i9, i2, i10, i9, i2, i10);
                this.Func_996(i1, i3);
                this.data.Set(i1, i2, i3, i5);
                if (i4 != 0)
                {
                    Tile.tiles[i4].OnPlace(this.worldObj, i9, i2, i10);
                }

                this.isModified = true;
                return true;
            }
        }

        public virtual bool SetBlockID(int i1, int i2, int i3, int i4)
        {
            byte b5 = (byte)i4;
            int i6 = this.heightMap[i3 << 4 | i1] & 255;
            int i7 = this.blocks[i1 << 11 | i3 << 7 | i2] & 255;
            if (i7 == i4)
            {
                return false;
            }
            else
            {
                int i8 = this.xPosition * 16 + i1;
                int i9 = this.zPosition * 16 + i3;
                this.blocks[i1 << 11 | i3 << 7 | i2] = (byte)(b5 & 255);
                if (i7 != 0)
                {
                    Tile.tiles[i7].OnBlockRemoval(this.worldObj, i8, i2, i9);
                }

                this.data.Set(i1, i2, i3, 0);
                if (Tile.lightBlock[b5 & 255] != 0)
                {
                    if (i2 >= i6)
                    {
                        this.Func_1003(i1, i2 + 1, i3);
                    }
                }
                else if (i2 == i6 - 1)
                {
                    this.Func_1003(i1, i2, i3);
                }

                this.worldObj.UpdateLight(LightLayer.Sky, i8, i2, i9, i8, i2, i9);
                this.worldObj.UpdateLight(LightLayer.Block, i8, i2, i9, i8, i2, i9);
                this.Func_996(i1, i3);
                if (i4 != 0 && !this.worldObj.isRemote)
                {
                    Tile.tiles[i4].OnPlace(this.worldObj, i8, i2, i9);
                }

                this.isModified = true;
                return true;
            }
        }

        public virtual int GetBlockMetadata(int i1, int i2, int i3)
        {
            return this.data.Get(i1, i2, i3);
        }

        public virtual void SetBlockMetadata(int i1, int i2, int i3, int i4)
        {
            this.isModified = true;
            this.data.Set(i1, i2, i3, i4);
        }

        public virtual int GetSavedLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4)
        {
            return enumSkyBlock1 == LightLayer.Sky ? this.skylightMap.Get(i2, i3, i4) : (enumSkyBlock1 == LightLayer.Block ? this.blocklightMap.Get(i2, i3, i4) : 0);
        }

        public virtual void SetLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5)
        {
            this.isModified = true;
            if (enumSkyBlock1 == LightLayer.Sky)
            {
                this.skylightMap.Set(i2, i3, i4, i5);
            }
            else
            {
                if (enumSkyBlock1 != LightLayer.Block)
                {
                    return;
                }

                this.blocklightMap.Set(i2, i3, i4, i5);
            }
        }

        public virtual int IsSkyLit(int i1, int i2, int i3, int i4)
        {
            int i5 = this.skylightMap.Get(i1, i2, i3);
            if (i5 > 0)
            {
                isLit = true;
            }

            i5 -= i4;
            int i6 = this.blocklightMap.Get(i1, i2, i3);
            if (i6 > i5)
            {
                i5 = i6;
            }

            return i5;
        }

        public virtual void AddEntity(Entity entity1)
        {
            this.hasEntities = true;
            int i2 = Mth.Floor(entity1.x / 16);
            int i3 = Mth.Floor(entity1.z / 16);
            if (i2 != this.xPosition || i3 != this.zPosition)
            {
                Console.WriteLine("Wrong location! " + entity1);
                new Exception().PrintStackTrace();
            }

            int i4 = Mth.Floor(entity1.y / 16);
            if (i4 < 0)
            {
                i4 = 0;
            }

            if (i4 >= this.entities.Length)
            {
                i4 = this.entities.Length - 1;
            }

            entity1.addedToChunk = true;
            entity1.chunkCoordX = this.xPosition;
            entity1.chunkCoordY = i4;
            entity1.chunkCoordZ = this.zPosition;
            this.entities[i4].Add(entity1);
        }

        public virtual void RemoveEntity(Entity entity1)
        {
            this.RemoveEntityAtIndex(entity1, entity1.chunkCoordY);
        }

        public virtual void RemoveEntityAtIndex(Entity entity1, int i2)
        {
            if (i2 < 0)
            {
                i2 = 0;
            }

            if (i2 >= this.entities.Length)
            {
                i2 = this.entities.Length - 1;
            }

            this.entities[i2].Remove(entity1);
        }

        public virtual bool CanBlockSeeTheSky(int i1, int i2, int i3)
        {
            return i2 >= (this.heightMap[i3 << 4 | i1] & 255);
        }

        public virtual TileEntity GetChunkBlockTileEntity(int i1, int i2, int i3)
        {
            TilePos chunkPosition4 = new TilePos(i1, i2, i3);
            TileEntity tileEntity5 = this.chunkTileEntityMap[chunkPosition4];
            if (tileEntity5 == null)
            {
                int i6 = this.GetBlockID(i1, i2, i3);
                if (!Tile.isEntityTile[i6])
                {
                    return null;
                }

                EntityTile blockContainer7 = (EntityTile)Tile.tiles[i6];
                blockContainer7.OnPlace(this.worldObj, this.xPosition * 16 + i1, i2, this.zPosition * 16 + i3);
                tileEntity5 = this.chunkTileEntityMap[chunkPosition4];
            }

            if (tileEntity5 != null && tileEntity5.IsInvalid())
            {
                this.chunkTileEntityMap.Remove(chunkPosition4);
                return null;
            }
            else
            {
                return tileEntity5;
            }
        }

        public virtual void AddTileEntity(TileEntity tileEntity1)
        {
            int i2 = tileEntity1.xCoord - this.xPosition * 16;
            int i3 = tileEntity1.yCoord;
            int i4 = tileEntity1.zCoord - this.zPosition * 16;
            this.SetChunkBlockTileEntity(i2, i3, i4, tileEntity1);
            if (this.isChunkLoaded)
            {
                this.worldObj.loadedTileEntityList.Add(tileEntity1);
            }
        }

        public virtual void SetChunkBlockTileEntity(int i1, int i2, int i3, TileEntity tileEntity4)
        {
            TilePos chunkPosition5 = new TilePos(i1, i2, i3);
            tileEntity4.worldObj = this.worldObj;
            tileEntity4.xCoord = this.xPosition * 16 + i1;
            tileEntity4.yCoord = i2;
            tileEntity4.zCoord = this.zPosition * 16 + i3;
            if (this.GetBlockID(i1, i2, i3) != 0 && Tile.tiles[this.GetBlockID(i1, i2, i3)] is EntityTile)
            {
                tileEntity4.Validate();
                this.chunkTileEntityMap[chunkPosition5] = tileEntity4;
            }
            else
            {
                Console.WriteLine("Attempted to place a tile entity where there was no entity tile!");
            }
        }

        public virtual void RemoveChunkBlockTileEntity(int i1, int i2, int i3)
        {
            TilePos chunkPosition4 = new TilePos(i1, i2, i3);
            if (this.isChunkLoaded)
            {

                TileEntity tileEntity5 = this.chunkTileEntityMap[chunkPosition4];
                this.chunkTileEntityMap.Remove(chunkPosition4);
                if (tileEntity5 != null)
                {
                    tileEntity5.Invalidate();
                }
            }
        }

        public virtual void OnChunkLoad()
        {
            this.isChunkLoaded = true;
            this.worldObj.AddLoadedTileEntities(this.chunkTileEntityMap.Values);
            for (int i1 = 0; i1 < this.entities.Length; ++i1)
            {
                this.worldObj.AddEntities(this.entities[i1]);
            }
        }

        public virtual void OnChunkUnload()
        {
            this.isChunkLoaded = false;
            foreach (TileEntity te in this.chunkTileEntityMap.Values)
            {
                if (Enhancements.FIX_CHUNK_CACHE_MEM_LEAK)
                {
                    this.worldObj.RemoveTileEntity(te);
                }
                else
                {
                    te.Invalidate();
                }
            }

            for (int i3 = 0; i3 < this.entities.Length; ++i3)
            {
                this.worldObj.RemoveEntities(this.entities[i3]);
            }
        }

        public virtual void SetChunkModified()
        {
            this.isModified = true;
        }

        public virtual void GetEntitiesWithinAABBForEntity(Entity entity1, AABB axisAlignedBB2, IList<Entity> list3)
        {
            int i4 = Mth.Floor((axisAlignedBB2.y0 - 2) / 16);
            int i5 = Mth.Floor((axisAlignedBB2.y1 + 2) / 16);
            if (i4 < 0)
            {
                i4 = 0;
            }

            if (i5 >= this.entities.Length)
            {
                i5 = this.entities.Length - 1;
            }

            for (int i6 = i4; i6 <= i5; ++i6)
            {
                IList<Entity> list7 = this.entities[i6];
                for (int i8 = 0; i8 < list7.Count; ++i8)
                {
                    Entity entity9 = list7[i8];
                    if (entity9 != entity1 && entity9.boundingBox.IntersectsWith(axisAlignedBB2))
                    {
                        list3.Add(entity9);
                    }
                }
            }
        }

        public virtual void GetEntitiesOfTypeWithinAAAB<E>(Type type, AABB aabb, List<E> entities) where E : Entity
        {
            int min = Mth.Floor((aabb.y0 - 2) / 16);
            int max = Mth.Floor((aabb.y1 + 2) / 16);
            if (min < 0)
            {
                min = 0;
            }

            if (max >= this.entities.Length)
            {
                max = this.entities.Length - 1;
            }

            for (int j = min; j <= max; ++j)
            {
                IList<Entity> entitylist = this.entities[j];
                for (int i = 0; i < entitylist.Count; ++i)
                {
                    Entity entity = entitylist[i];
                    if (type.IsAssignableFrom(entity.GetType()) && entity.boundingBox.IntersectsWith(aabb))
                    {
                        E casted = (E)entity;
                        entities.Add(casted);
                    }
                }
            }
        }

        public virtual bool NeedsSaving(bool z1)
        {
            if (this.neverSave)
            {
                return false;
            }
            else
            {
                if (z1)
                {
                    if (this.hasEntities && this.worldObj.GetTime() != this.lastSaveTime)
                    {
                        return true;
                    }
                }
                else if (this.hasEntities && this.worldObj.GetTime() >= this.lastSaveTime + 600)
                {
                    return true;
                }

                return this.isModified;
            }
        }

        public virtual int GetChunkData(byte[] b1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)
        {
            int i9 = i5 - i2;
            int i10 = i6 - i3;
            int i11 = i7 - i4;
            if (i9 * i10 * i11 == this.blocks.Length)
            {
                Array.Copy(this.blocks, 0, b1, i8, this.blocks.Length);
                i8 += this.blocks.Length;
                Array.Copy(this.data.data, 0, b1, i8, this.data.data.Length);
                i8 += this.data.data.Length;
                Array.Copy(this.blocklightMap.data, 0, b1, i8, this.blocklightMap.data.Length);
                i8 += this.blocklightMap.data.Length;
                Array.Copy(this.skylightMap.data, 0, b1, i8, this.skylightMap.data.Length);
                i8 += this.skylightMap.data.Length;
                return i8;
            }
            else
            {
                int i12;
                int i13;
                int i14;
                int i15;
                for (i12 = i2; i12 < i5; ++i12)
                {
                    for (i13 = i4; i13 < i7; ++i13)
                    {
                        i14 = i12 << 11 | i13 << 7 | i3;
                        i15 = i6 - i3;
                        Array.Copy(this.blocks, i14, b1, i8, i15);
                        i8 += i15;
                    }
                }

                for (i12 = i2; i12 < i5; ++i12)
                {
                    for (i13 = i4; i13 < i7; ++i13)
                    {
                        i14 = (i12 << 11 | i13 << 7 | i3) >> 1;
                        i15 = (i6 - i3) / 2;
                        Array.Copy(this.data.data, i14, b1, i8, i15);
                        i8 += i15;
                    }
                }

                for (i12 = i2; i12 < i5; ++i12)
                {
                    for (i13 = i4; i13 < i7; ++i13)
                    {
                        i14 = (i12 << 11 | i13 << 7 | i3) >> 1;
                        i15 = (i6 - i3) / 2;
                        Array.Copy(this.blocklightMap.data, i14, b1, i8, i15);
                        i8 += i15;
                    }
                }

                for (i12 = i2; i12 < i5; ++i12)
                {
                    for (i13 = i4; i13 < i7; ++i13)
                    {
                        i14 = (i12 << 11 | i13 << 7 | i3) >> 1;
                        i15 = (i6 - i3) / 2;
                        Array.Copy(this.skylightMap.data, i14, b1, i8, i15);
                        i8 += i15;
                    }
                }

                return i8;
            }
        }

        public virtual int SetChunkData(byte[] b1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)
        {
            int i9;
            int i10;
            int i11;
            int i12;
            for (i9 = i2; i9 < i5; ++i9)
            {
                for (i10 = i4; i10 < i7; ++i10)
                {
                    i11 = i9 << 11 | i10 << 7 | i3;
                    i12 = i6 - i3;
                    Array.Copy(b1, i8, this.blocks, i11, i12);
                    i8 += i12;
                }
            }

            this.GenerateHeightMap();
            for (i9 = i2; i9 < i5; ++i9)
            {
                for (i10 = i4; i10 < i7; ++i10)
                {
                    i11 = (i9 << 11 | i10 << 7 | i3) >> 1;
                    i12 = (i6 - i3) / 2;
                    Array.Copy(b1, i8, this.data.data, i11, i12);
                    i8 += i12;
                }
            }

            for (i9 = i2; i9 < i5; ++i9)
            {
                for (i10 = i4; i10 < i7; ++i10)
                {
                    i11 = (i9 << 11 | i10 << 7 | i3) >> 1;
                    i12 = (i6 - i3) / 2;
                    Array.Copy(b1, i8, this.blocklightMap.data, i11, i12);
                    i8 += i12;
                }
            }

            for (i9 = i2; i9 < i5; ++i9)
            {
                for (i10 = i4; i10 < i7; ++i10)
                {
                    i11 = (i9 << 11 | i10 << 7 | i3) >> 1;
                    i12 = (i6 - i3) / 2;
                    Array.Copy(b1, i8, this.skylightMap.data, i11, i12);
                    i8 += i12;
                }
            }

            return i8;
        }

        public virtual JRandom GetRandom(long seed)
        {
            return new JRandom(this.worldObj.GetRandomSeed() + this.xPosition * this.xPosition * 4987142 + this.xPosition * 5947611 + this.zPosition * this.zPosition * 4392871 + this.zPosition * 389711 ^ seed);
        }

        public virtual bool IsEmpty()
        {
            return false;
        }

        public virtual void LoadChunkBlockMap()
        {
            ChunkBlockMap.Load(this.blocks);
        }

        public virtual ChunkPos GetPos()
        {
            return new ChunkPos(this.xPosition, this.zPosition);
        }
    }
}