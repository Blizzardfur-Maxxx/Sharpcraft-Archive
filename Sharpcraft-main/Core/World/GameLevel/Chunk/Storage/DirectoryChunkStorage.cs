using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using System;
using System.IO;

namespace SharpCraft.Core.World.GameLevel.Chunk.Storage
{
    public class DirectoryChunkStorage : IChunkStorage
    {
        private JFile saveDir;
        private bool createIfNecessary;

        public DirectoryChunkStorage(JFile dir, bool create)
        {
            this.saveDir = dir;
            this.createIfNecessary = create;
        }

        private JFile ChunkFileForXZ(int x, int z)
        {
            string root = "c." + Base36.ToString(x, 36) + "." + Base36.ToString(z, 36) + ".dat";
            string xstring = Base36.ToString(x & 63, 36);
            string zstring = Base36.ToString(z & 63, 36);
            JFile chunkfile = new JFile(this.saveDir, xstring);
            if (!chunkfile.Exists())
            {
                if (!this.createIfNecessary)
                {
                    return null;
                }

                chunkfile.Mkdir();
            }

            chunkfile = new JFile(chunkfile, zstring);
            if (!chunkfile.Exists())
            {
                if (!this.createIfNecessary)
                {
                    return null;
                }

                chunkfile.Mkdir();
            }

            chunkfile = new JFile(chunkfile, root);
            return !chunkfile.Exists() && !this.createIfNecessary ? null : chunkfile;
        }

        public virtual LevelChunk Load(Level level, int x, int z)
        {
            JFile file = this.ChunkFileForXZ(x, z);
            if (file != null && file.Exists())
            {
                try
                {
                    FileStream filestream = file.GetReadStream();
                    CompoundTag root = NbtIO.ReadCompressed(filestream);
                    if (!root.HasKey("Level"))
                    {
                        Console.WriteLine("Chunk file at " + x + "," + z + " is missing level data, skipping");
                        return null;
                    }

                    if (!root.GetCompoundTag("Level").HasKey("Blocks"))
                    {
                        Console.WriteLine("Chunk file at " + x + "," + z + " is missing block data, skipping");
                        return null;
                    }

                    LevelChunk chonk = Load(level, root.GetCompoundTag("Level"));
                    if (!chonk.IsAtLocation(x, z))
                    {
                        Console.WriteLine("Chunk file at " + x + "," + z + " is in the wrong location; relocating. (Expected " + x + ", " + z + ", got " + chonk.xPosition + ", " + chonk.zPosition + ")");
                        root.SetInteger("xPos", x);
                        root.SetInteger("zPos", z);
                        chonk = Load(level, root.GetCompoundTag("Level"));
                    }

                    chonk.LoadChunkBlockMap();
                    return chonk;
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                }
            }

            return null;
        }

        public virtual void Save(Level level, LevelChunk chonk)
        {
            level.CheckSession();
            JFile file = this.ChunkFileForXZ(chonk.xPosition, chonk.zPosition);
            if (file.Exists())
            {
                LevelData leveldata = level.GetLevelData();
                leveldata.SetSizeOnDisk(leveldata.GetSizeOnDisk() - file.Length());
            }

            try
            {
                JFile tmpfile = new JFile(this.saveDir, "tmp_chunk.dat");
                FileStream fileoutstream = tmpfile.GetWriteStream();
                CompoundTag root = new CompoundTag();
                CompoundTag levelTag = new CompoundTag();
                root.SetTag("Level", levelTag);
                Save(chonk, level, levelTag);
                NbtIO.WriteCompressed(root, fileoutstream);
                fileoutstream.Dispose();
                if (file.Exists())
                {
                    file.Delete();
                }

                tmpfile.RenameTo(file);
                LevelData ldata = level.GetLevelData();
                ldata.SetSizeOnDisk(ldata.GetSizeOnDisk() + file.Length());
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
        }

        public static void Save(LevelChunk chunk, Level level, CompoundTag chunkTag)
        {
            level.CheckSession();
            chunkTag.SetInteger("xPos", chunk.xPosition);
            chunkTag.SetInteger("zPos", chunk.zPosition);
            chunkTag.SetLong("LastUpdate", level.GetTime());
            chunkTag.SetByteArray("Blocks", chunk.blocks);
            chunkTag.SetByteArray("Data", chunk.data.data);
            chunkTag.SetByteArray("SkyLight", chunk.skylightMap.data);
            chunkTag.SetByteArray("BlockLight", chunk.blocklightMap.data);
            chunkTag.SetByteArray("HeightMap", chunk.heightMap);
            chunkTag.SetBoolean("TerrainPopulated", chunk.isTerrainPopulated);
            chunk.hasEntities = false;
            ListTag<CompoundTag> entities = new();
            CompoundTag compound;
            for (int i4 = 0; i4 < chunk.entities.Length; ++i4)
            {
                foreach (Entity e in chunk.entities[i4])
                {
                    chunk.hasEntities = true;
                    compound = new CompoundTag();
                    if (e.AddEntityID(compound))
                    {
                        entities.Add(compound);
                    }
                }
            }

            chunkTag.SetTag("Entities", entities);
            ListTag<CompoundTag> tileentities = new();
            foreach (TileEntity te in chunk.chunkTileEntityMap.Values)
            {
                compound = new CompoundTag();
                te.Save(compound);
                tileentities.Add(compound);
            }

            chunkTag.SetTag("TileEntities", tileentities);
        }

        public static LevelChunk Load(Level level, CompoundTag levelTag)
        {
            int xpos = levelTag.GetInteger("xPos");
            int zpos = levelTag.GetInteger("zPos");
            LevelChunk chonk = new LevelChunk(level, xpos, zpos);
            chonk.blocks = levelTag.GetByteArray("Blocks");
            chonk.data = new DataLayer(levelTag.GetByteArray("Data"));
            chonk.skylightMap = new DataLayer(levelTag.GetByteArray("SkyLight"));
            chonk.blocklightMap = new DataLayer(levelTag.GetByteArray("BlockLight"));
            chonk.heightMap = levelTag.GetByteArray("HeightMap");
            chonk.isTerrainPopulated = levelTag.GetBoolean("TerrainPopulated");
            if (!chonk.data.IsValid())
            {
                chonk.data = new DataLayer(chonk.blocks.Length);
            }

            if (chonk.heightMap == null || !chonk.skylightMap.IsValid())
            {
                chonk.heightMap = new byte[256];
                chonk.skylightMap = new DataLayer(chonk.blocks.Length);
                chonk.CalculateLight();
            }

            if (!chonk.blocklightMap.IsValid())
            {
                chonk.blocklightMap = new DataLayer(chonk.blocks.Length);
                chonk.Func_a();
            }

            ListTag<CompoundTag> entities = levelTag.GetTagList<CompoundTag>("Entities");
            if (entities != null)
            {
                for (int i = 0; i < entities.Count; ++i)
                {
                    CompoundTag entityTag = entities[i];
                    Entity entity = EntityFactory.LoadEntity(entityTag, level);
                    chonk.hasEntities = true;
                    if (entity != null)
                    {
                        chonk.AddEntity(entity);
                    }
                }
            }

            ListTag<CompoundTag> tileentities = levelTag.GetTagList<CompoundTag>("TileEntities");
            if (tileentities != null)
            {
                for (int i = 0; i < tileentities.Count; ++i)
                {
                    CompoundTag teTag = tileentities[i];
                    TileEntity te = TileEntity.CreateAndLoadEntity(teTag);
                    if (te != null)
                    {
                        chonk.AddTileEntity(te);
                    }
                }
            }

            return chonk;
        }

        public virtual void Tick()
        {
        }

        public virtual void Flush()
        {
        }

        public virtual void SaveEntities(Level world1, LevelChunk chunk2)
        {
        }
    }
}