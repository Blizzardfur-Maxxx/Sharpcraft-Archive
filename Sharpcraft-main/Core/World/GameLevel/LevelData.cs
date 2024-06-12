using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel
{
    public class LevelData
    {
        private long randomSeed;
        private int spawnX;
        private int spawnY;
        private int spawnZ;
        private long worldTime;
        private long lastTimePlayed;
        private long sizeOnDisk;
        private CompoundTag playerTag;
        private int dimension;
        private string levelName;
        private int saveVersion;
        private bool raining;
        private int rainTime;
        private bool thundering;
        private int thunderTime;
        public LevelData(CompoundTag nBTTagCompound1)
        {
            this.randomSeed = nBTTagCompound1.GetLong("RandomSeed");
            this.spawnX = nBTTagCompound1.GetInteger("SpawnX");
            this.spawnY = nBTTagCompound1.GetInteger("SpawnY");
            this.spawnZ = nBTTagCompound1.GetInteger("SpawnZ");
            this.worldTime = nBTTagCompound1.GetLong("Time");
            this.lastTimePlayed = nBTTagCompound1.GetLong("LastPlayed");
            this.sizeOnDisk = nBTTagCompound1.GetLong("SizeOnDisk");
            this.levelName = nBTTagCompound1.GetString("LevelName");
            this.saveVersion = nBTTagCompound1.GetInteger("version");
            this.rainTime = nBTTagCompound1.GetInteger("rainTime");
            this.raining = nBTTagCompound1.GetBoolean("raining");
            this.thunderTime = nBTTagCompound1.GetInteger("thunderTime");
            this.thundering = nBTTagCompound1.GetBoolean("thundering");
            if (nBTTagCompound1.HasKey("Player"))
            {
                this.playerTag = nBTTagCompound1.GetCompoundTag("Player");
                this.dimension = this.playerTag.GetInteger("Dimension");
            }
        }

        public LevelData(long j1, string string3)
        {
            this.randomSeed = j1;
            this.levelName = string3;
        }

        public LevelData(LevelData worldInfo1)
        {
            this.randomSeed = worldInfo1.randomSeed;
            this.spawnX = worldInfo1.spawnX;
            this.spawnY = worldInfo1.spawnY;
            this.spawnZ = worldInfo1.spawnZ;
            this.worldTime = worldInfo1.worldTime;
            this.lastTimePlayed = worldInfo1.lastTimePlayed;
            this.sizeOnDisk = worldInfo1.sizeOnDisk;
            this.playerTag = worldInfo1.playerTag;
            this.dimension = worldInfo1.dimension;
            this.levelName = worldInfo1.levelName;
            this.saveVersion = worldInfo1.saveVersion;
            this.rainTime = worldInfo1.rainTime;
            this.raining = worldInfo1.raining;
            this.thunderTime = worldInfo1.thunderTime;
            this.thundering = worldInfo1.thundering;
        }

        public virtual CompoundTag GetCompoundTag()
        {
            CompoundTag nBTTagCompound1 = new CompoundTag();
            this.UpdateTagCompound(nBTTagCompound1, this.playerTag);
            return nBTTagCompound1;
        }

        public virtual CompoundTag GetPlayerTag(IList<Player> list1)
        {
            CompoundTag nBTTagCompound2 = new CompoundTag();
            Player entityPlayer3 = null;
            CompoundTag nBTTagCompound4 = null;
            if (list1.Count > 0)
            {
                entityPlayer3 = list1[0];
            }

            if (entityPlayer3 != null)
            {
                nBTTagCompound4 = new CompoundTag();
                entityPlayer3.WriteToNBT(nBTTagCompound4);
            }

            this.UpdateTagCompound(nBTTagCompound2, nBTTagCompound4);
            return nBTTagCompound2;
        }

        private void UpdateTagCompound(CompoundTag nBTTagCompound1, CompoundTag nBTTagCompound2)
        {
            nBTTagCompound1.SetLong("RandomSeed", this.randomSeed);
            nBTTagCompound1.SetInteger("SpawnX", this.spawnX);
            nBTTagCompound1.SetInteger("SpawnY", this.spawnY);
            nBTTagCompound1.SetInteger("SpawnZ", this.spawnZ);
            nBTTagCompound1.SetLong("Time", this.worldTime);
            nBTTagCompound1.SetLong("SizeOnDisk", this.sizeOnDisk);
            nBTTagCompound1.SetLong("LastPlayed", TimeUtil.MilliTime);
            nBTTagCompound1.SetString("LevelName", this.levelName);
            nBTTagCompound1.SetInteger("version", this.saveVersion);
            nBTTagCompound1.SetInteger("rainTime", this.rainTime);
            nBTTagCompound1.SetBoolean("raining", this.raining);
            nBTTagCompound1.SetInteger("thunderTime", this.thunderTime);
            nBTTagCompound1.SetBoolean("thundering", this.thundering);
            if (nBTTagCompound2 != null)
            {
                nBTTagCompound1.SetCompoundTag("Player", nBTTagCompound2);
            }
        }

        public virtual long GetRandomSeed()
        {
            return this.randomSeed;
        }

        public virtual int GetSpawnX()
        {
            return this.spawnX;
        }

        public virtual int GetSpawnY()
        {
            return this.spawnY;
        }

        public virtual int GetSpawnZ()
        {
            return this.spawnZ;
        }

        public virtual long GetTime()
        {
            return this.worldTime;
        }

        public virtual long GetSizeOnDisk()
        {
            return this.sizeOnDisk;
        }

        public virtual CompoundTag GetPlayerNBTTagCompound()
        {
            return this.playerTag;
        }

        public virtual int GetDimension()
        {
            return this.dimension;
        }

        public virtual void SetSpawnX(int i1)
        {
            this.spawnX = i1;
        }

        public virtual void SetSpawnY(int i1)
        {
            this.spawnY = i1;
        }

        public virtual void SetSpawnZ(int i1)
        {
            this.spawnZ = i1;
        }

        public virtual void SetTime(long j1)
        {
            this.worldTime = j1;
        }

        public virtual void SetSizeOnDisk(long j1)
        {
            this.sizeOnDisk = j1;
        }

        public virtual void SetPlayerNBTTagCompound(CompoundTag nBTTagCompound1)
        {
            this.playerTag = nBTTagCompound1;
        }

        public virtual void SetSpawn(int i1, int i2, int i3)
        {
            this.spawnX = i1;
            this.spawnY = i2;
            this.spawnZ = i3;
        }

        public virtual string GetLevelName()
        {
            return this.levelName;
        }

        public virtual void SetLevelName(string string1)
        {
            this.levelName = string1;
        }

        public virtual int GetSaveVersion()
        {
            return this.saveVersion;
        }

        public virtual void SetSaveVersion(int i1)
        {
            this.saveVersion = i1;
        }

        public virtual long GetLastTimePlayed()
        {
            return this.lastTimePlayed;
        }

        public virtual bool GetThundering()
        {
            return this.thundering;
        }

        public virtual void SetThundering(bool z1)
        {
            this.thundering = z1;
        }

        public virtual int GetThunderTime()
        {
            return this.thunderTime;
        }

        public virtual void SetThunderTime(int i1)
        {
            this.thunderTime = i1;
        }

        public virtual bool GetRaining()
        {
            return this.raining;
        }

        public virtual void SetRaining(bool z1)
        {
            this.raining = z1;
        }

        public virtual int GetRainTime()
        {
            return this.rainTime;
        }

        public virtual void SetRainTime(int i1)
        {
            this.rainTime = i1;
        }
    }
}