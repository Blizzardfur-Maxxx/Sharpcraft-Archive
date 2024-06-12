using System;
using System.Collections.Generic;
using System.IO;
using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Dimensions;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class DirectoryLevelStorage : IPlayerIO, ILevelStorage
    {
        private static readonly Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        private readonly JFile worldDir;
        private readonly JFile playersDir;
        private readonly JFile dataDir;
        private readonly long now = TimeUtil.MilliTime;

        public DirectoryLevelStorage(JFile file1, string string2, bool z3)
        {
            this.worldDir = new JFile(file1, string2);
            //this.worldDir.Mkdirs();
            this.worldDir.Mkdir();
            this.playersDir = new JFile(this.worldDir, "players");
            this.dataDir = new JFile(this.worldDir, "data");
            //this.dataDir.Mkdirs();
            this.dataDir.Mkdir();
            if (z3)
            {
                //this.playersDir.Mkdirs();
                this.playersDir.Mkdir();
            }

            this.Pchecksession();
        }

        private void Pchecksession()
        {
            try
            {
                JFile file1 = new JFile(this.worldDir, "session.lock");
                BinaryWriter dataOutputStream2 = new BinaryWriter(file1.GetWriteStream());
                try
                {
                    dataOutputStream2.WriteBELong(this.now);
                }
                finally
                {
                    dataOutputStream2.Dispose();
                }
            }
            catch (IOException iOException7)
            {
                iOException7.PrintStackTrace();
                throw new Exception("Failed to check session lock, aborting");
            }
        }

        protected virtual JFile GetDirectory()
        {
            return this.worldDir;
        }

        public virtual void CheckSession()
        {
            try
            {
                JFile file1 = new JFile(this.worldDir, "session.lock");
                BinaryReader dataInputStream2 = new BinaryReader(file1.GetReadStream());
                try
                {
                    if (dataInputStream2.ReadBELong() != this.now)
                    {
                        throw new LevelConflictException("The save is being accessed from another location, aborting");
                    }
                }
                finally
                {
                    dataInputStream2.Dispose();
                }
            }
            catch (IOException)
            {
                throw new LevelConflictException("Failed to check session lock, aborting");
            }
        }

        public virtual IChunkStorage CreateChunkStorage(Dimension worldProvider1)
        {
            if (worldProvider1 is HellDimension)
            {
                JFile file2 = new JFile(this.worldDir, "DIM-1");
                //file2.Mkdirs();
                file2.Mkdir();
                return new DirectoryChunkStorage(file2, true);
            }
            else
            {
                return new DirectoryChunkStorage(this.worldDir, true);
            }
        }

        public virtual LevelData PrepareLevel()
        {
            JFile file1 = new JFile(this.worldDir, "level.dat");
            CompoundTag nBTTagCompound2;
            CompoundTag nBTTagCompound3;
            if (file1.Exists())
            {
                try
                {
                    nBTTagCompound2 = NbtIO.ReadCompressed(file1.GetReadStream());
                    nBTTagCompound3 = nBTTagCompound2.GetCompoundTag("Data");
                    return new LevelData(nBTTagCompound3);
                }
                catch (Exception exception5)
                {
                    exception5.PrintStackTrace();
                }
            }

            file1 = new JFile(this.worldDir, "level.dat_old");
            if (file1.Exists())
            {
                try
                {
                    nBTTagCompound2 = NbtIO.ReadCompressed(file1.GetReadStream());
                    nBTTagCompound3 = nBTTagCompound2.GetCompoundTag("Data");
                    return new LevelData(nBTTagCompound3);
                }
                catch (Exception exception4)
                {
                    exception4.PrintStackTrace();
                }
            }

            return null;
        }

        public virtual void SaveLevelData(LevelData worldInfo1, IList<Player> list2)
        {
            CompoundTag nBTTagCompound3 = worldInfo1.GetPlayerTag(list2);
            CompoundTag nBTTagCompound4 = new CompoundTag();
            nBTTagCompound4.SetTag("Data", nBTTagCompound3);
            try
            {
                JFile file5 = new JFile(this.worldDir, "level.dat_new");
                JFile file6 = new JFile(this.worldDir, "level.dat_old");
                JFile file7 = new JFile(this.worldDir, "level.dat");
                NbtIO.WriteCompressed(nBTTagCompound4, file5.GetWriteStream());
                if (file6.Exists())
                {
                    file6.Delete();
                }

                file7.RenameTo(file6);
                if (file7.Exists())
                {
                    file7.Delete();
                }

                file5.RenameTo(file7);
                if (file5.Exists())
                {
                    file5.Delete();
                }
            }
            catch (Exception exception8)
            {
                exception8.PrintStackTrace();
            }
        }

        public virtual void SaveLevelData(LevelData worldInfo1)
        {
            CompoundTag nBTTagCompound2 = worldInfo1.GetCompoundTag();
            CompoundTag nBTTagCompound3 = new CompoundTag();
            nBTTagCompound3.SetTag("Data", nBTTagCompound2);
            try
            {
                JFile file4 = new JFile(this.worldDir, "level.dat_new");
                JFile file5 = new JFile(this.worldDir, "level.dat_old");
                JFile file6 = new JFile(this.worldDir, "level.dat");
                NbtIO.WriteCompressed(nBTTagCompound3, file4.GetWriteStream());
                if (file5.Exists())
                {
                    file5.Delete();
                }

                file6.RenameTo(file5);
                if (file6.Exists())
                {
                    file6.Delete();
                }

                file4.RenameTo(file6);
                if (file4.Exists())
                {
                    file4.Delete();
                }
            }
            catch (Exception exception7)
            {
                exception7.PrintStackTrace();
            }
        }

        public virtual void Write(Player entityPlayer1)
        {
            try
            {
                CompoundTag nBTTagCompound2 = new CompoundTag();
                entityPlayer1.WriteToNBT(nBTTagCompound2);
                JFile file3 = new JFile(this.playersDir, "_tmp_.dat");
                JFile file4 = new JFile(this.playersDir, entityPlayer1.username + ".dat");
                NbtIO.WriteCompressed(nBTTagCompound2, file3.GetWriteStream());
                if (file4.Exists())
                {
                    file4.Delete();
                }

                file3.RenameTo(file4);
            }
            catch
            {
                logger.Warning("Failed to save player data for " + entityPlayer1.username);
            }
        }

        public virtual void Read(Player entityPlayer1)
        {
            CompoundTag nBTTagCompound2 = this.GetPlayerData(entityPlayer1.username);
            if (nBTTagCompound2 != null)
            {
                entityPlayer1.ReadFromNBT(nBTTagCompound2);
            }
        }

        public virtual CompoundTag GetPlayerData(string string1)
        {
            try
            {
                JFile file2 = new JFile(this.playersDir, string1 + ".dat");
                if (file2.Exists())
                {
                    return NbtIO.ReadCompressed(file2.GetReadStream());
                }
            }
            catch
            {
                logger.Warning("Failed to load player data for " + string1);
            }

            return null;
        }

        public virtual IPlayerIO GetPlayerIO()
        {
            return this;
        }

        public virtual void CloseAll()
        {
        }

        public virtual JFile GetDataFile(string string1)
        {
            return new JFile(this.dataDir, string1 + ".dat");
        }
    }
}