using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Weather;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.World.GameLevel.GameSavedData;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.PathFinding;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel
{
    public class Level : ILevelSource
    {
        public const int MAXDIST = 32000000;
        public bool scheduledUpdatesAreImmediate;
        private List<LightUpdate> lightingToUpdate;
        public List<Entity> loadedEntityList;
        private List<Entity> unloadedEntityList;
        private SortedSet<TickNextTickData> scheduledTickTreeSet;
        private HashSet<TickNextTickData> scheduledTickSet;
        public List<TileEntity> loadedTileEntityList;
        private List<TileEntity> addedTEs;
        private List<TileEntity> tileentityRemoval;
        public List<Player> playerEntities;
        public List<Entity> weatherEffects;
        private long cloudColor;
        public int skyDarken;
        protected int distHashCounter;
        protected readonly int distHashMagic;
        protected float prevRainingStrength;
        protected float rainingStrength;
        protected float prevThunderingStrength;
        protected float thunderingStrength;
        protected int field_F;
        public int field_i;
        public bool editingBlocks;
        private long lockTimestamp;
        protected int autosavePeriod;
        public int difficultySetting;
        public JRandom rand;
        public bool isNewWorld;
        public readonly Dimension dimension;
        protected List<ILevelListener> levelListeners;
        protected IChunkSource chunkSource;
        protected readonly ILevelStorage levelStorage;
        protected LevelData levelData;
        public bool findingSpawnPoint;
        private bool allPlayersSleeping;
        public MapStorage mapStorage;
        private List<AABB> cubes;
        private bool scanningTEs;
        private int lightingUpdatesCounter;
        private bool spawnHostileMobs;
        private bool spawnPeacefulMobs;
        static int lightingUpdatesScheduled = 0;
        private HashSet<ChunkPos> activeChunkSet;
        private int soundCounter;
        private List<Entity> field_M;
        public bool isRemote;
        public virtual BiomeSource GetBiomeSource()
        {
            return this.dimension.biomeSource;
        }

        //TODO: clean up the constructors so it's less messy, i,e the lists are initialized in the fields
        public Level(ILevelStorage iSaveHandler1, string string2, Dimension worldProvider3, long j4)
        {
            this.scheduledUpdatesAreImmediate = false;
            this.lightingToUpdate = new List<LightUpdate>();
            this.loadedEntityList = new List<Entity>();
            this.unloadedEntityList = new List<Entity>();
            this.scheduledTickTreeSet = new SortedSet<TickNextTickData>();
            this.scheduledTickSet = new HashSet<TickNextTickData>();
            this.loadedTileEntityList = new List<TileEntity>();
            this.addedTEs = new List<TileEntity>();
            this.tileentityRemoval = new List<TileEntity>();
            this.playerEntities = new List<Player>();
            this.weatherEffects = new List<Entity>();
            this.cloudColor = 16777215;
            this.skyDarken = 0;
            this.distHashCounter = (new JRandom()).NextInt();
            this.distHashMagic = 1013904223;
            this.field_F = 0;
            this.field_i = 0;
            this.editingBlocks = false;
            this.lockTimestamp = TimeUtil.MilliTime;//System.CurrentTimeMillis(); //not sure if correct fix later
            this.autosavePeriod = Enhancements.SAVE_INTERVAL_TICKS;
            this.rand = new JRandom();
            this.isNewWorld = false;
            this.levelListeners = new List<ILevelListener>();
            this.cubes = new List<AABB>();
            this.lightingUpdatesCounter = 0;
            this.spawnHostileMobs = true;
            this.spawnPeacefulMobs = true;
            this.activeChunkSet = new HashSet<ChunkPos>();
            this.soundCounter = this.rand.NextInt(12000);
            this.field_M = new List<Entity>();
            this.isRemote = false;
            this.levelStorage = iSaveHandler1;
            this.levelData = new LevelData(j4, string2);
            this.dimension = worldProvider3;
            this.mapStorage = new MapStorage(iSaveHandler1);
            worldProvider3.Init(this);
            this.chunkSource = this.CreateChunkSource();
            this.CalculateInitialSkylight();
            this.Func_27163();
        }

        public Level(Level world1, Dimension worldProvider2)
        {
            this.scheduledUpdatesAreImmediate = false;
            this.lightingToUpdate = new List<LightUpdate>();
            this.loadedEntityList = new List<Entity>();
            this.unloadedEntityList = new List<Entity>();
            this.scheduledTickTreeSet = new SortedSet<TickNextTickData>();
            this.scheduledTickSet = new HashSet<TickNextTickData>();
            this.loadedTileEntityList = new List<TileEntity>();
            this.addedTEs = new List<TileEntity>();
            this.tileentityRemoval = new List<TileEntity>();
            this.playerEntities = new List<Player>();
            this.weatherEffects = new List<Entity>();
            this.cloudColor = 16777215;
            this.skyDarken = 0;
            this.distHashCounter = (new JRandom()).NextInt();
            this.distHashMagic = 1013904223;
            this.field_F = 0;
            this.field_i = 0;
            this.editingBlocks = false;
            this.lockTimestamp = TimeUtil.MilliTime;//System.CurrentTimeMillis();
            this.autosavePeriod = Enhancements.SAVE_INTERVAL_TICKS;
            this.rand = new JRandom();
            this.isNewWorld = false;
            this.levelListeners = new List<ILevelListener>();
            this.cubes = new List<AABB>();
            this.lightingUpdatesCounter = 0;
            this.spawnHostileMobs = true;
            this.spawnPeacefulMobs = true;
            this.activeChunkSet = new HashSet<ChunkPos>();
            this.soundCounter = this.rand.NextInt(12000);
            this.field_M = new List<Entity>();
            this.isRemote = false;
            this.lockTimestamp = world1.lockTimestamp;
            this.levelStorage = world1.levelStorage;
            this.levelData = new LevelData(world1.levelData);
            this.mapStorage = new MapStorage(this.levelStorage);
            this.dimension = worldProvider2;
            worldProvider2.Init(this);
            this.chunkSource = this.CreateChunkSource();
            this.CalculateInitialSkylight();
            this.Func_27163();
        }

        public Level(ILevelStorage iSaveHandler1, string string2, long j3) : this(iSaveHandler1, string2, j3, null)
        {
        }

        public Level(ILevelStorage iSaveHandler1, string string2, long j3, Dimension dim)
        {
            this.scheduledUpdatesAreImmediate = false;
            this.lightingToUpdate = new List<LightUpdate>();
            this.loadedEntityList = new List<Entity>();
            this.unloadedEntityList = new List<Entity>();
            this.scheduledTickTreeSet = new SortedSet<TickNextTickData>();
            this.scheduledTickSet = new HashSet<TickNextTickData>();
            this.loadedTileEntityList = new List<TileEntity>();
            this.addedTEs = new List<TileEntity>();
            this.tileentityRemoval = new List<TileEntity>();
            this.playerEntities = new List<Player>();
            this.weatherEffects = new List<Entity>();
            this.cloudColor = 16777215;
            this.skyDarken = 0;
            this.distHashCounter = (new JRandom()).NextInt();
            this.distHashMagic = 1013904223;
            this.field_F = 0;
            this.field_i = 0;
            this.editingBlocks = false;
            this.lockTimestamp = TimeUtil.MilliTime;
            this.autosavePeriod = Enhancements.SAVE_INTERVAL_TICKS;
            this.rand = new JRandom();
            this.isNewWorld = false;
            this.levelListeners = new List<ILevelListener>();
            this.cubes = new List<AABB>();
            this.lightingUpdatesCounter = 0;
            this.spawnHostileMobs = true;
            this.spawnPeacefulMobs = true;
            this.activeChunkSet = new HashSet<ChunkPos>();
            this.soundCounter = this.rand.NextInt(12000);
            this.field_M = new List<Entity>();
            this.isRemote = false;
            this.levelStorage = iSaveHandler1;
            this.mapStorage = new MapStorage(iSaveHandler1);
            this.levelData = iSaveHandler1.PrepareLevel();
            this.isNewWorld = this.levelData == null;
            if (dim != null)
            {
                this.dimension = dim;
            }
            else if (this.levelData != null && this.levelData.GetDimension() == -1)
            {
                this.dimension = Dimension.GetNew(-1);
            }
            else
            {
                this.dimension = Dimension.GetNew(0);
            }

            bool z6 = false;
            if (this.levelData == null)
            {
                this.levelData = new LevelData(j3, string2);
                z6 = true;
            }
            else
            {
                this.levelData.SetLevelName(string2);
            }

            this.dimension.Init(this);
            this.chunkSource = this.CreateChunkSource();
            if (z6)
            {
                this.GetInitialSpawnLocation();
            }

            this.CalculateInitialSkylight();
            this.Func_27163();
        }


        protected virtual IChunkSource CreateChunkSource()
        {
            IChunkStorage storage = this.levelStorage.CreateChunkStorage(this.dimension);
            IChunkSource cache = null;
            if (Enhancements.FIX_CHUNK_CACHE_MEM_LEAK)
            {
                cache = new ImprovedServerChunkCache(this, storage, this.dimension.CreateRandomLevelSource());
            }
            else
            {
                cache = new ServerChunkCache(this, storage, this.dimension.CreateRandomLevelSource());
            }

            return cache;
        }


        protected virtual void GetInitialSpawnLocation()
        {
            this.findingSpawnPoint = true;
            if (Enhancements.FORCE_SPAWN_POINT) 
            {
                levelData.SetSpawn(0, 128, 0);
                findingSpawnPoint = false;
                return;
            }
            int i1 = 0;
            byte b2 = 64;
            int i3;
            for (i3 = 0; !this.dimension.CanCoordinateBeSpawn(i1, i3); i3 += this.rand.NextInt(64) - this.rand.NextInt(64))
            {
                i1 += this.rand.NextInt(64) - this.rand.NextInt(64);
            }

            this.levelData.SetSpawn(i1, b2, i3);
            this.findingSpawnPoint = false;
        }


        public virtual void SetSpawnLocation()
        {
            if (Enhancements.FORCE_SPAWN_POINT)
            {
                levelData.SetSpawnX(0);
                levelData.SetSpawnY(128);
                levelData.SetSpawnZ(0);
                return;
            }
            if (this.levelData.GetSpawnY() <= 0)
            {
                this.levelData.SetSpawnY(64);
            }

            int i1 = this.levelData.GetSpawnX();
            int i2;
            for (i2 = this.levelData.GetSpawnZ(); this.GetFirstUncoveredBlock(i1, i2) == 0; i2 += this.rand.NextInt(8) - this.rand.NextInt(8))
            {
                i1 += this.rand.NextInt(8) - this.rand.NextInt(8);
            }

            this.levelData.SetSpawnX(i1);
            this.levelData.SetSpawnZ(i2);
        }


        public virtual int GetFirstUncoveredBlock(int i1, int i2)
        {
            int i3;
            for (i3 = 63; !this.IsAirBlock(i1, i3 + 1, i2); ++i3)
            {
            }

            return this.GetTile(i1, i3, i2);
        }


        public virtual void EmptyMethod1()
        {
        }


        public virtual Player Func_48456_a(double d1, double d3, double d5)
        {
            double d7 = -1;
            Player entityPlayer9 = null;
            for (int i10 = 0; i10 < this.playerEntities.Count; ++i10)
            {
                Player entityPlayer11 = (Player)this.playerEntities[i10];
                double d12 = entityPlayer11.GetDistanceSq(d1, entityPlayer11.y, d3);
                if ((d5 < 0 || d12 < d5 * d5) && (d7 == -1 || d12 < d7))
                {
                    d7 = d12;
                    entityPlayer9 = entityPlayer11;
                }
            }

            return entityPlayer9;
        }


        public virtual void SpawnPlayerWithLoadedChunks(Player entityPlayer1)
        {
            try
            {
                CompoundTag nBTTagCompound2 = this.levelData.GetPlayerNBTTagCompound();
                if (nBTTagCompound2 != null)
                {
                    entityPlayer1.ReadFromNBT(nBTTagCompound2);
                    this.levelData.SetPlayerNBTTagCompound((CompoundTag)null);
                }

                if (this.chunkSource is ChunkCache)
                {
                    ChunkCache chunkProviderLoadOrGenerate3 = (ChunkCache)this.chunkSource;
                    int i4 = Mth.Floor(((int)entityPlayer1.x)) >> 4;
                    int i5 = Mth.Floor(((int)entityPlayer1.z)) >> 4;
                    chunkProviderLoadOrGenerate3.SetPos(i4, i5);
                }

                this.AddEntity(entityPlayer1);
            }
            catch (Exception exception6)
            {
                exception6.PrintStackTrace();
            }
        }


        public virtual void Save(bool z1, IProgressListener iProgressUpdate2)
        {
            if (this.chunkSource.ShouldSave())
            {
                if (iProgressUpdate2 != null)
                {
                    iProgressUpdate2.StartLoading("Saving level");
                }

                this.SaveLevel();
                if (iProgressUpdate2 != null)
                {
                    iProgressUpdate2.DisplayLoadingString("Saving chunks");
                }

                this.chunkSource.Save(z1, iProgressUpdate2);
            }
        }


        private void SaveLevel()
        {
            this.CheckSession();
            this.levelStorage.SaveLevelData(this.levelData, this.playerEntities);
            this.mapStorage.SaveAllData();
        }


        public virtual bool Func_650_a(int i1)
        {
            if (!this.chunkSource.ShouldSave())
            {
                return true;
            }
            else
            {
                if (i1 == 0)
                {
                    this.SaveLevel();
                }

                return this.chunkSource.Save(false, (IProgressListener)null);
            }
        }


        public virtual int GetTile(int i1, int i2, int i3)
        {
            return i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST ? (i2 < 0 ? 0 : (i2 >= 128 ? 0 : this.GetChunk(i1 >> 4, i3 >> 4).GetBlockID(i1 & 15, i2, i3 & 15))) : 0;
        }


        public virtual bool IsAirBlock(int i1, int i2, int i3)
        {
            return this.GetTile(i1, i2, i3) == 0;
        }


        public virtual bool HasChunkAt(int i1, int i2, int i3)
        {
            return i2 >= 0 && i2 < 128 ? this.HasChunk(i1 >> 4, i3 >> 4) : false;
        }


        public virtual bool DoChunksNearChunkExist(int i1, int i2, int i3, int i4)
        {
            return this.CheckChunksExist(i1 - i4, i2 - i4, i3 - i4, i1 + i4, i2 + i4, i3 + i4);
        }


        public virtual bool CheckChunksExist(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            if (i5 >= 0 && i2 < 128)
            {
                i1 >>= 4;
                i2 >>= 4;
                i3 >>= 4;
                i4 >>= 4;
                i5 >>= 4;
                i6 >>= 4;
                for (int i7 = i1; i7 <= i4; ++i7)
                {
                    for (int i8 = i3; i8 <= i6; ++i8)
                    {
                        if (!this.HasChunk(i7, i8))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        private bool HasChunk(int x, int z)
        {
            return this.chunkSource.HasChunk(x, z);
        }


        public virtual LevelChunk GetChunkAt(int tilex, int tilez)
        {
            return this.GetChunk(tilex >> 4, tilez >> 4);
        }


        public virtual LevelChunk GetChunk(int x, int z)
        {
            return this.chunkSource.GetChunk(x, z);
        }


        public virtual bool SetTileAndDataNoUpdate(int i1, int i2, int i3, int i4, int i5)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
            {
                if (i2 < 0)
                {
                    return false;
                }
                else if (i2 >= 128)
                {
                    return false;
                }
                else
                {
                    LevelChunk chunk6 = this.GetChunk(i1 >> 4, i3 >> 4);
                    return chunk6.SetBlockIDWithMetadata(i1 & 15, i2, i3 & 15, i4, i5);
                }
            }
            else
            {
                return false;
            }
        }


        public virtual bool SetTileNoUpdate(int i1, int i2, int i3, int i4)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
            {
                if (i2 < 0)
                {
                    return false;
                }
                else if (i2 >= 128)
                {
                    return false;
                }
                else
                {
                    LevelChunk chunk5 = this.GetChunk(i1 >> 4, i3 >> 4);
                    return chunk5.SetBlockID(i1 & 15, i2, i3 & 15, i4);
                }
            }
            else
            {
                return false;
            }
        }


        public virtual Material GetMaterial(int i1, int i2, int i3)
        {
            int i4 = this.GetTile(i1, i2, i3);
            return i4 == 0 ? Material.air : Tile.tiles[i4].material;
        }


        public virtual int GetData(int i1, int i2, int i3)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
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
                    LevelChunk chunk4 = this.GetChunk(i1 >> 4, i3 >> 4);
                    i1 &= 15;
                    i3 &= 15;
                    return chunk4.GetBlockMetadata(i1, i2, i3);
                }
            }
            else
            {
                return 0;
            }
        }


        public virtual void SetData(int i1, int i2, int i3, int i4)
        {
            if (this.SetDataNoUpdate(i1, i2, i3, i4))
            {
                int i5 = this.GetTile(i1, i2, i3);
                if (Tile.requiresSelfNotify[i5 & 255])
                {
                    this.NotifyBlockChange(i1, i2, i3, i5);
                }
                else
                {
                    this.UpdateNeighborsAt(i1, i2, i3, i5);
                }
            }
        }


        public virtual bool SetDataNoUpdate(int i1, int i2, int i3, int i4)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
            {
                if (i2 < 0)
                {
                    return false;
                }
                else if (i2 >= 128)
                {
                    return false;
                }
                else
                {
                    LevelChunk chunk5 = this.GetChunk(i1 >> 4, i3 >> 4);
                    i1 &= 15;
                    i3 &= 15;
                    chunk5.SetBlockMetadata(i1, i2, i3, i4);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        public virtual bool SetTile(int i1, int i2, int i3, int i4)
        {
            if (this.SetTileNoUpdate(i1, i2, i3, i4))
            {
                this.NotifyBlockChange(i1, i2, i3, i4);
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual bool SetTileAndData(int i1, int i2, int i3, int i4, int i5)
        {
            if (this.SetTileAndDataNoUpdate(i1, i2, i3, i4, i5))
            {
                this.NotifyBlockChange(i1, i2, i3, i4);
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void SendTileUpdated(int i1, int i2, int i3)
        {
            for (int i4 = 0; i4 < this.levelListeners.Count; ++i4)
            {
                this.levelListeners[i4].TileChanged(i1, i2, i3);
            }
        }


        protected virtual void NotifyBlockChange(int i1, int i2, int i3, int i4)
        {
            this.SendTileUpdated(i1, i2, i3);
            this.UpdateNeighborsAt(i1, i2, i3, i4);
        }


        public virtual void LightColumnChanged(int x, int z, int y1, int y2)
        {
            if (y1 > y2)
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            this.SetTilesDirty(x, y1, z, x, y2, z);
        }


        public virtual void SetTileDirty(int x, int y, int z)
        {
            for (int i4 = 0; i4 < this.levelListeners.Count; ++i4)
            {
                this.levelListeners[i4].SetTilesDirty(x, y, z, x, y, z);
            }
        }


        public virtual void SetTilesDirty(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            for (int i7 = 0; i7 < this.levelListeners.Count; ++i7)
            {
                this.levelListeners[i7].SetTilesDirty(i1, i2, i3, i4, i5, i6);
            }
        }


        public virtual void UpdateNeighborsAt(int x, int y, int z, int tile)
        {
            this.NeighborChanged(x - 1, y, z, tile);
            this.NeighborChanged(x + 1, y, z, tile);
            this.NeighborChanged(x, y - 1, z, tile);
            this.NeighborChanged(x, y + 1, z, tile);
            this.NeighborChanged(x, y, z - 1, tile);
            this.NeighborChanged(x, y, z + 1, tile);
        }


        private void NeighborChanged(int x, int y, int z, int tile)
        {
            if (!this.editingBlocks && !this.isRemote)
            {
                Tile block5 = Tile.tiles[this.GetTile(x, y, z)];
                if (block5 != null)
                {
                    block5.NeighborChanged(this, x, y, z, tile);
                }
            }
        }


        public virtual bool CanCockSeeTheSky(int i1, int i2, int i3)
        {
            return this.GetChunk(i1 >> 4, i3 >> 4).CanBlockSeeTheSky(i1 & 15, i2, i3 & 15);
        }


        public virtual int IsSkyLit(int i1, int i2, int i3)
        {
            if (i2 < 0)
            {
                return 0;
            }
            else
            {
                if (i2 >= 128)
                {
                    i2 = 127;
                }

                return this.GetChunk(i1 >> 4, i3 >> 4).IsSkyLit(i1 & 15, i2, i3 & 15, 0);
            }
        }


        public virtual int GetRawBrightness(int i1, int i2, int i3)
        {
            return this.GetRawBrightness(i1, i2, i3, true);
        }


        public virtual int GetRawBrightness(int i1, int i2, int i3, bool z4)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
            {
                if (z4)
                {
                    int i5 = this.GetTile(i1, i2, i3);
                    if (i5 == Tile.stoneSlabHalf.id || i5 == Tile.farmland.id || i5 == Tile.stairs_stone.id || i5 == Tile.stairs_wood.id)
                    {
                        int i6 = this.GetRawBrightness(i1, i2 + 1, i3, false);
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
                else
                {
                    if (i2 >= 128)
                    {
                        i2 = 127;
                    }

                    LevelChunk chunk11 = this.GetChunk(i1 >> 4, i3 >> 4);
                    i1 &= 15;
                    i3 &= 15;
                    return chunk11.IsSkyLit(i1, i2, i3, this.skyDarken);
                }
            }
            else
            {
                return 15;
            }
        }


        public virtual bool CanExistingBlockSeeTheSky(int i1, int i2, int i3)
        {
            if (i1 >= -MAXDIST && i3 >= -MAXDIST && i1 < MAXDIST && i3 <= MAXDIST)
            {
                if (i2 < 0)
                {
                    return false;
                }
                else if (i2 >= 128)
                {
                    return true;
                }
                else if (!this.HasChunk(i1 >> 4, i3 >> 4))
                {
                    return false;
                }
                else
                {
                    LevelChunk chunk4 = this.GetChunk(i1 >> 4, i3 >> 4);
                    i1 &= 15;
                    i3 &= 15;
                    return chunk4.CanBlockSeeTheSky(i1, i2, i3);
                }
            }
            else
            {
                return false;
            }
        }


        public virtual int GetHeightValue(int i1, int i2)
        {
            if (i1 >= -MAXDIST && i2 >= -MAXDIST && i1 < MAXDIST && i2 <= MAXDIST)
            {
                if (!this.HasChunk(i1 >> 4, i2 >> 4))
                {
                    return 0;
                }
                else
                {
                    LevelChunk chunk3 = this.GetChunk(i1 >> 4, i2 >> 4);
                    return chunk3.GetHeightValue(i1 & 15, i2 & 15);
                }
            }
            else
            {
                return 0;
            }
        }


        public virtual void NeighborLightPropagationChanged(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5)
        {
            if (!this.dimension.hasNoSky || enumSkyBlock1 != LightLayer.Sky)
            {
                if (this.HasChunkAt(i2, i3, i4))
                {
                    if (enumSkyBlock1 == LightLayer.Sky)
                    {
                        if (this.CanExistingBlockSeeTheSky(i2, i3, i4))
                        {
                            i5 = 15;
                        }
                    }
                    else if (enumSkyBlock1 == LightLayer.Block)
                    {
                        int i6 = this.GetTile(i2, i3, i4);
                        if (Tile.lightEmission[i6] > i5)
                        {
                            i5 = Tile.lightEmission[i6];
                        }
                    }

                    if (this.GetSavedLightValue(enumSkyBlock1, i2, i3, i4) != i5)
                    {
                        this.UpdateLight(enumSkyBlock1, i2, i3, i4, i2, i3, i4);
                    }
                }
            }
        }


        public virtual int GetSavedLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4)
        {
            if (i3 < 0)
            {
                i3 = 0;
            }

            if (i3 >= 128)
            {
                i3 = 127;
            }

            if (i3 >= 0 && i3 < 128 && i2 >= -MAXDIST && i4 >= -MAXDIST && i2 < MAXDIST && i4 <= MAXDIST)
            {
                int i5 = i2 >> 4;
                int i6 = i4 >> 4;
                if (!this.HasChunk(i5, i6))
                {
                    return 0;
                }
                else
                {
                    LevelChunk chunk7 = this.GetChunk(i5, i6);
                    return chunk7.GetSavedLightValue(enumSkyBlock1, i2 & 15, i3, i4 & 15);
                }
            }
            else
            {
                return (int)enumSkyBlock1;
            }
        }


        public virtual void SetLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5)
        {
            if (i2 >= -MAXDIST && i4 >= -MAXDIST && i2 < MAXDIST && i4 <= MAXDIST)
            {
                if (i3 >= 0)
                {
                    if (i3 < 128)
                    {
                        if (this.HasChunk(i2 >> 4, i4 >> 4))
                        {
                            LevelChunk chunk6 = this.GetChunk(i2 >> 4, i4 >> 4);
                            chunk6.SetLightValue(enumSkyBlock1, i2 & 15, i3, i4 & 15, i5);
                            for (int i7 = 0; i7 < this.levelListeners.Count; ++i7)
                            {
                                this.levelListeners[i7].TileChanged(i2, i3, i4);
                            }
                        }
                    }
                }
            }
        }


        public virtual float GetBrightness(int i1, int i2, int i3, int i4)
        {
            int i5 = this.GetRawBrightness(i1, i2, i3);
            if (i5 < i4)
            {
                i5 = i4;
            }

            return this.dimension.lightBrightnessTable[i5];
        }


        public virtual float GetBrightness(int i1, int i2, int i3)
        {
            return this.dimension.lightBrightnessTable[this.GetRawBrightness(i1, i2, i3)];
        }


        public virtual bool IsDaytime()
        {
            return this.skyDarken < 4;
        }


        public virtual HitResult Clip(Vec3 vec3D1, Vec3 vec3D2)
        {
            return this.Clip(vec3D1, vec3D2, false, false);
        }


        public virtual HitResult Clip(Vec3 vec3D1, Vec3 vec3D2, bool z3)
        {
            return this.Clip(vec3D1, vec3D2, z3, false);
        }


        public virtual HitResult Clip(Vec3 vec3D1, Vec3 vec3D2, bool z3, bool z4)
        {
            if (!Double.IsNaN(vec3D1.x) && !Double.IsNaN(vec3D1.y) && !Double.IsNaN(vec3D1.z))
            {
                if (!Double.IsNaN(vec3D2.x) && !Double.IsNaN(vec3D2.y) && !Double.IsNaN(vec3D2.z))
                {
                    int i5 = Mth.Floor(vec3D2.x);
                    int i6 = Mth.Floor(vec3D2.y);
                    int i7 = Mth.Floor(vec3D2.z);
                    int i8 = Mth.Floor(vec3D1.x);
                    int i9 = Mth.Floor(vec3D1.y);
                    int i10 = Mth.Floor(vec3D1.z);
                    int i11 = this.GetTile(i8, i9, i10);
                    int i12 = this.GetData(i8, i9, i10);
                    Tile block13 = Tile.tiles[i11];
                    if ((!z4 || block13 == null || block13.GetAABB(this, i8, i9, i10) != null) && i11 > 0 && block13.MayPick(i12, z3))
                    {
                        HitResult movingObjectPosition14 = block13.Clip(this, i8, i9, i10, vec3D1, vec3D2);
                        if (movingObjectPosition14 != null)
                        {
                            return movingObjectPosition14;
                        }
                    }

                    i11 = 200;
                    while (i11-- >= 0)
                    {
                        if (Double.IsNaN(vec3D1.x) || Double.IsNaN(vec3D1.y) || Double.IsNaN(vec3D1.z))
                        {
                            return null;
                        }

                        if (i8 == i5 && i9 == i6 && i10 == i7)
                        {
                            return null;
                        }

                        bool z39 = true;
                        bool z40 = true;
                        bool z41 = true;
                        double d15 = 999;
                        double d17 = 999;
                        double d19 = 999;
                        if (i5 > i8)
                        {
                            d15 = i8 + 1;
                        }
                        else if (i5 < i8)
                        {
                            d15 = i8 + 0;
                        }
                        else
                        {
                            z39 = false;
                        }

                        if (i6 > i9)
                        {
                            d17 = i9 + 1;
                        }
                        else if (i6 < i9)
                        {
                            d17 = i9 + 0;
                        }
                        else
                        {
                            z40 = false;
                        }

                        if (i7 > i10)
                        {
                            d19 = i10 + 1;
                        }
                        else if (i7 < i10)
                        {
                            d19 = i10 + 0;
                        }
                        else
                        {
                            z41 = false;
                        }

                        double d21 = 999;
                        double d23 = 999;
                        double d25 = 999;
                        double d27 = vec3D2.x - vec3D1.x;
                        double d29 = vec3D2.y - vec3D1.y;
                        double d31 = vec3D2.z - vec3D1.z;
                        if (z39)
                        {
                            d21 = (d15 - vec3D1.x) / d27;
                        }

                        if (z40)
                        {
                            d23 = (d17 - vec3D1.y) / d29;
                        }

                        if (z41)
                        {
                            d25 = (d19 - vec3D1.z) / d31;
                        }

                        byte b42;
                        if (d21 < d23 && d21 < d25)
                        {
                            if (i5 > i8)
                            {
                                b42 = 4;
                            }
                            else
                            {
                                b42 = 5;
                            }

                            vec3D1.x = d15;
                            vec3D1.y += d29 * d21;
                            vec3D1.z += d31 * d21;
                        }
                        else if (d23 < d25)
                        {
                            if (i6 > i9)
                            {
                                b42 = 0;
                            }
                            else
                            {
                                b42 = 1;
                            }

                            vec3D1.x += d27 * d23;
                            vec3D1.y = d17;
                            vec3D1.z += d31 * d23;
                        }
                        else
                        {
                            if (i7 > i10)
                            {
                                b42 = 2;
                            }
                            else
                            {
                                b42 = 3;
                            }

                            vec3D1.x += d27 * d25;
                            vec3D1.y += d29 * d25;
                            vec3D1.z = d19;
                        }

                        Vec3 vec3D34 = Vec3.Of(vec3D1.x, vec3D1.y, vec3D1.z);
                        i8 = (int)(vec3D34.x = Mth.Floor(vec3D1.x));
                        if (b42 == 5)
                        {
                            --i8;
                            ++vec3D34.x;
                        }

                        i9 = (int)(vec3D34.y = Mth.Floor(vec3D1.y));
                        if (b42 == 1)
                        {
                            --i9;
                            ++vec3D34.y;
                        }

                        i10 = (int)(vec3D34.z = Mth.Floor(vec3D1.z));
                        if (b42 == 3)
                        {
                            --i10;
                            ++vec3D34.z;
                        }

                        int i35 = this.GetTile(i8, i9, i10);
                        int i36 = this.GetData(i8, i9, i10);
                        Tile block37 = Tile.tiles[i35];
                        if ((!z4 || block37 == null || block37.GetAABB(this, i8, i9, i10) != null) && i35 > 0 && block37.MayPick(i36, z3))
                        {
                            HitResult movingObjectPosition38 = block37.Clip(this, i8, i9, i10, vec3D1, vec3D2);
                            if (movingObjectPosition38 != null)
                            {
                                return movingObjectPosition38;
                            }
                        }
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public virtual void PlaySound(Entity entity1, string string2, float f3, float f4)
        {
            for (int i5 = 0; i5 < this.levelListeners.Count; ++i5)
            {
                this.levelListeners[i5].PlaySound(string2, entity1.x, entity1.y - entity1.yOffset, entity1.z, f3, f4);
            }
        }


        public virtual void PlaySound(double d1, double d3, double d5, string string7, float f8, float f9)
        {
            for (int i10 = 0; i10 < this.levelListeners.Count; ++i10)
            {
                this.levelListeners[i10].PlaySound(string7, d1, d3, d5, f8, f9);
            }
        }


        public virtual void PlayStreamingMusic(string string1, int i2, int i3, int i4)
        {
            for (int i5 = 0; i5 < this.levelListeners.Count; ++i5)
            {
                this.levelListeners[i5].PlayStreamingMusic(string1, i2, i3, i4);
            }
        }


        public virtual void AddParticle(string string1, double d2, double d4, double d6, double d8, double d10, double d12)
        {
            for (int i14 = 0; i14 < this.levelListeners.Count; ++i14)
            {
                this.levelListeners[i14].AddParticle(string1, d2, d4, d6, d8, d10, d12);
            }
        }


        public virtual bool AddWeatherEffect(Entity entity1)
        {
            this.weatherEffects.Add(entity1);
            return true;
        }


        public virtual bool AddEntity(Entity entity1)
        {
            int i2 = Mth.Floor(entity1.x / 16);
            int i3 = Mth.Floor(entity1.z / 16);
            bool z4 = false;
            if (entity1 is Player)
            {
                z4 = true;
            }

            if (!z4 && !this.HasChunk(i2, i3))
            {
                return false;
            }
            else
            {
                if (entity1 is Player)
                {
                    Player entityPlayer5 = (Player)entity1;
                    this.playerEntities.Add(entityPlayer5);
                    this.UpdateAllPlayersSleepingFlag();
                }

                this.GetChunk(i2, i3).AddEntity(entity1);
                this.loadedEntityList.Add(entity1);
                this.EntityAdded(entity1);
                return true;
            }
        }


        protected virtual void EntityAdded(Entity entity1)
        {
            for (int i2 = 0; i2 < this.levelListeners.Count; ++i2)
            {
                this.levelListeners[i2].EntityAdded(entity1);
            }
        }


        protected virtual void EntityRemoved(Entity entity1)
        {
            for (int i2 = 0; i2 < this.levelListeners.Count; ++i2)
            {
                this.levelListeners[i2].EntityRemoved(entity1);
            }
        }


        public virtual void SetEntityDead(Entity entity1)
        {
            if (entity1.riddenByEntity != null)
            {
                entity1.riddenByEntity.MountEntity((Entity)null);
            }

            if (entity1.ridingEntity != null)
            {
                entity1.MountEntity((Entity)null);
            }

            entity1.SetEntityDead();
            if (entity1 is Player)
            {
                this.playerEntities.Remove((Player)entity1);
                this.UpdateAllPlayersSleepingFlag();
            }
        }


        public virtual void RemovePlayer(Entity entity1)
        {
            entity1.SetEntityDead();
            if (entity1 is Player)
            {
                this.playerEntities.Remove((Player)entity1);
                this.UpdateAllPlayersSleepingFlag();
            }

            int i2 = entity1.chunkCoordX;
            int i3 = entity1.chunkCoordZ;
            if (entity1.addedToChunk && this.HasChunk(i2, i3))
            {
                this.GetChunk(i2, i3).RemoveEntity(entity1);
            }

            this.loadedEntityList.Remove(entity1);
            this.EntityRemoved(entity1);
        }


        public virtual void AddListener(ILevelListener iWorldAccess1)
        {
            this.levelListeners.Add(iWorldAccess1);
        }


        public virtual void RemoveListener(ILevelListener iWorldAccess1)
        {
            this.levelListeners.Remove(iWorldAccess1);
        }


        public virtual IList<AABB> GetCubes(Entity entity1, AABB axisAlignedBB2)
        {
            this.cubes.Clear();
            int i3 = Mth.Floor(axisAlignedBB2.x0);
            int i4 = Mth.Floor(axisAlignedBB2.x1 + 1);
            int i5 = Mth.Floor(axisAlignedBB2.y0);
            int i6 = Mth.Floor(axisAlignedBB2.y1 + 1);
            int i7 = Mth.Floor(axisAlignedBB2.z0);
            int i8 = Mth.Floor(axisAlignedBB2.z1 + 1);
            for (int i9 = i3; i9 < i4; ++i9)
            {
                for (int i10 = i7; i10 < i8; ++i10)
                {
                    if (this.HasChunkAt(i9, 64, i10))
                    {
                        for (int i11 = i5 - 1; i11 < i6; ++i11)
                        {
                            Tile block12 = Tile.tiles[this.GetTile(i9, i11, i10)];
                            if (block12 != null)
                            {
                                block12.AddAABBs(this, i9, i11, i10, axisAlignedBB2, this.cubes);
                            }
                        }
                    }
                }
            }

            double d14 = 0.25;
            IList<Entity> list15 = this.GetEntities(entity1, axisAlignedBB2.Expand(d14, d14, d14));
            for (int i16 = 0; i16 < list15.Count; ++i16)
            {
                AABB axisAlignedBB13 = list15[i16].GetBoundingBox();
                if (axisAlignedBB13 != null && axisAlignedBB13.IntersectsWith(axisAlignedBB2))
                {
                    this.cubes.Add(axisAlignedBB13);
                }

                axisAlignedBB13 = entity1.GetCollisionBox(list15[i16]);
                if (axisAlignedBB13 != null && axisAlignedBB13.IntersectsWith(axisAlignedBB2))
                {
                    this.cubes.Add(axisAlignedBB13);
                }
            }

            return this.cubes;
        }


        public virtual int GetSkyDarken(float f1)
        {
            float f2 = this.GetSunAngle(f1);
            float f3 = 1F - (Mth.Cos(f2 * Mth.PI * 2F) * 2F + 0.5F);
            if (f3 < 0F)
            {
                f3 = 0F;
            }

            if (f3 > 1F)
            {
                f3 = 1F;
            }

            f3 = 1F - f3;
            f3 = (float)(f3 * (1 - this.Func_g(f1) * 5F / 16));
            f3 = (float)(f3 * (1 - this.Func_f(f1) * 5F / 16));
            f3 = 1F - f3;
            return (int)(f3 * 11F);
        }


        public virtual Vec3 GetSkyColor(Entity entity1, float f2)
        {
            float f3 = this.GetSunAngle(f2);
            float f4 = Mth.Cos(f3 * Mth.PI * 2F) * 2F + 0.5F;
            if (f4 < 0F)
            {
                f4 = 0F;
            }

            if (f4 > 1F)
            {
                f4 = 1F;
            }

            int i5 = Mth.Floor(entity1.x);
            int i6 = Mth.Floor(entity1.z);
            float f7 = (float)this.GetBiomeSource().GetTemperature(i5, i6);
            int i8 = this.GetBiomeSource().GetBiomeGenAt(i5, i6).GetSkyColor(f7);
            float f9 = (i8 >> 16 & 255) / 255F;
            float f10 = (i8 >> 8 & 255) / 255F;
            float f11 = (i8 & 255) / 255F;
            f9 *= f4;
            f10 *= f4;
            f11 *= f4;
            float f12 = this.Func_g(f2);
            float f13;
            float f14;
            if (f12 > 0F)
            {
                f13 = (f9 * 0.3F + f10 * 0.59F + f11 * 0.11F) * 0.6F;
                f14 = 1F - f12 * 0.75F;
                f9 = f9 * f14 + f13 * (1F - f14);
                f10 = f10 * f14 + f13 * (1F - f14);
                f11 = f11 * f14 + f13 * (1F - f14);
            }

            f13 = this.Func_f(f2);
            if (f13 > 0F)
            {
                f14 = (f9 * 0.3F + f10 * 0.59F + f11 * 0.11F) * 0.2F;
                float f15 = 1F - f13 * 0.75F;
                f9 = f9 * f15 + f14 * (1F - f15);
                f10 = f10 * f15 + f14 * (1F - f15);
                f11 = f11 * f15 + f14 * (1F - f15);
            }

            if (this.field_i > 0)
            {
                f14 = this.field_i - f2;
                if (f14 > 1F)
                {
                    f14 = 1F;
                }

                f14 *= 0.45F;
                f9 = f9 * (1F - f14) + 0.8F * f14;
                f10 = f10 * (1F - f14) + 0.8F * f14;
                f11 = f11 * (1F - f14) + 1F * f14;
            }

            return Vec3.Of(f9, f10, f11);
        }


        public virtual float GetSunAngle(float f1)
        {
            return this.dimension.GetSunAngle(this.levelData.GetTime(), f1);
        }


        public virtual Vec3 GetCloudColor(float f1)
        {
            float f2 = this.GetSunAngle(f1);
            float f3 = Mth.Cos(f2 * Mth.PI * 2F) * 2F + 0.5F;
            if (f3 < 0F)
            {
                f3 = 0F;
            }

            if (f3 > 1F)
            {
                f3 = 1F;
            }

            float f4 = (this.cloudColor >> 16 & 255) / 255F;
            float f5 = (this.cloudColor >> 8 & 255) / 255F;
            float f6 = (this.cloudColor & 255) / 255F;
            float f7 = this.Func_g(f1);
            float f8;
            float f9;
            if (f7 > 0F)
            {
                f8 = (f4 * 0.3F + f5 * 0.59F + f6 * 0.11F) * 0.6F;
                f9 = 1F - f7 * 0.95F;
                f4 = f4 * f9 + f8 * (1F - f9);
                f5 = f5 * f9 + f8 * (1F - f9);
                f6 = f6 * f9 + f8 * (1F - f9);
            }

            f4 *= f3 * 0.9F + 0.1F;
            f5 *= f3 * 0.9F + 0.1F;
            f6 *= f3 * 0.85F + 0.15F;
            f8 = this.Func_f(f1);
            if (f8 > 0F)
            {
                f9 = (f4 * 0.3F + f5 * 0.59F + f6 * 0.11F) * 0.2F;
                float f10 = 1F - f8 * 0.95F;
                f4 = f4 * f10 + f9 * (1F - f10);
                f5 = f5 * f10 + f9 * (1F - f10);
                f6 = f6 * f10 + f9 * (1F - f10);
            }

            return Vec3.Of(f4, f5, f6);
        }


        public virtual Vec3 GetFogColor(float f1)
        {
            float f2 = this.GetSunAngle(f1);
            return this.dimension.GetFogColor(f2, f1);
        }

        public virtual int GetTopSolidBlock(int i1, int i2)
        {
            LevelChunk chunk3 = this.GetChunkAt(i1, i2);
            int i4 = 127;
            i1 &= 15;
            for (i2 &= 15; i4 > 0; --i4)
            {
                int i5 = chunk3.GetBlockID(i1, i4, i2);
                Material material6 = i5 == 0 ? Material.air : Tile.tiles[i5].material;
                if (material6.BlocksMotion() || material6.IsLiquid())
                {
                    return i4 + 1;
                }
            }

            return -1;
        }


        public virtual int GetTop(int i1, int i2)
        {
            LevelChunk chunk3 = this.GetChunkAt(i1, i2);
            int i4 = 127;
            i1 &= 15;
            for (i2 &= 15; i4 > 0; --i4)
            {
                int i5 = chunk3.GetBlockID(i1, i4, i2);
                if (i5 != 0 && Tile.tiles[i5].material.BlocksMotion())
                {
                    return i4 + 1;
                }
            }

            return -1;
        }


        public virtual float GetStarBrightness(float f1)
        {
            float f2 = this.GetSunAngle(f1);
            float f3 = 1F - (Mth.Cos(f2 * Mth.PI * 2F) * 2F + 0.75F);
            if (f3 < 0F)
            {
                f3 = 0F;
            }

            if (f3 > 1F)
            {
                f3 = 1F;
            }

            return f3 * f3 * 0.5F;
        }


        public virtual void ScheduleBlockUpdate(int i1, int i2, int i3, int i4, int i5)
        {
            TickNextTickData nextTickListEntry6 = new TickNextTickData(i1, i2, i3, i4);
            byte b7 = 8;
            if (this.scheduledUpdatesAreImmediate)
            {
                if (this.CheckChunksExist(nextTickListEntry6.xCoord - b7, nextTickListEntry6.yCoord - b7, nextTickListEntry6.zCoord - b7, nextTickListEntry6.xCoord + b7, nextTickListEntry6.yCoord + b7, nextTickListEntry6.zCoord + b7))
                {
                    int i8 = this.GetTile(nextTickListEntry6.xCoord, nextTickListEntry6.yCoord, nextTickListEntry6.zCoord);
                    if (i8 == nextTickListEntry6.blockID && i8 > 0)
                    {
                        Tile.tiles[i8].Tick(this, nextTickListEntry6.xCoord, nextTickListEntry6.yCoord, nextTickListEntry6.zCoord, this.rand);
                    }
                }
            }
            else
            {
                if (this.CheckChunksExist(i1 - b7, i2 - b7, i3 - b7, i1 + b7, i2 + b7, i3 + b7))
                {
                    if (i4 > 0)
                    {
                        nextTickListEntry6.SetScheduledTime(i5 + this.levelData.GetTime());
                    }

                    if (!this.scheduledTickSet.Contains(nextTickListEntry6))
                    {
                        this.scheduledTickSet.Add(nextTickListEntry6);
                        this.scheduledTickTreeSet.Add(nextTickListEntry6);
                    }
                }
            }
        }

        public virtual void TickEntities()
        {
            Profiler.StartSection("entities");
            Profiler.StartSection("global");
            int i1;
            Entity entity2;
            for (i1 = 0; i1 < this.weatherEffects.Count; ++i1)
            {
                entity2 = this.weatherEffects[i1];
                entity2.OnUpdate();
                if (entity2.isDead)
                {
                    this.weatherEffects.RemoveAt(i1--);
                }
            }

            Profiler.EndStartSection("remove");
            foreach (Entity entity in unloadedEntityList) loadedEntityList.Remove(entity);
            int i3;
            int i4;
            for (i1 = 0; i1 < this.unloadedEntityList.Count; ++i1)
            {
                entity2 = this.unloadedEntityList[i1];
                i3 = entity2.chunkCoordX;
                i4 = entity2.chunkCoordZ;
                if (entity2.addedToChunk && this.HasChunk(i3, i4))
                {
                    this.GetChunk(i3, i4).RemoveEntity(entity2);
                }
            }

            for (i1 = 0; i1 < this.unloadedEntityList.Count; ++i1)
            {
                this.EntityRemoved(this.unloadedEntityList[i1]);
            }
            Profiler.EndStartSection("regular");
            this.unloadedEntityList.Clear();
            for (i1 = 0; i1 < this.loadedEntityList.Count; ++i1)
            {
                entity2 = this.loadedEntityList[i1];
                if (entity2.ridingEntity != null)
                {
                    if (!entity2.ridingEntity.isDead && entity2.ridingEntity.riddenByEntity == entity2)
                    {
                        continue;
                    }

                    entity2.ridingEntity.riddenByEntity = null;
                    entity2.ridingEntity = null;
                }

                if (!entity2.isDead)
                {
                    this.UpdateEntity(entity2);
                }

                Profiler.StartSection("remove");
                if (entity2.isDead)
                {
                    i3 = entity2.chunkCoordX;
                    i4 = entity2.chunkCoordZ;
                    if (entity2.addedToChunk && this.HasChunk(i3, i4))
                    {
                        this.GetChunk(i3, i4).RemoveEntity(entity2);
                    }

                    this.loadedEntityList.RemoveAt(i1--);
                    this.EntityRemoved(entity2);
                }
                Profiler.EndSection();
            }

            Profiler.EndStartSection("tileEntities");
            this.scanningTEs = true;

            for (int i = loadedTileEntityList.Count - 1; i >= 0; i--)
            {
                TileEntity tileEntity5 = loadedTileEntityList[i];
                if (!tileEntity5.IsInvalid())
                    tileEntity5.UpdateEntity();

                if (tileEntity5.IsInvalid())
                {
                    loadedTileEntityList.RemoveAt(i);
                    i--;
                    LevelChunk chunk7 = this.GetChunk(tileEntity5.xCoord >> 4, tileEntity5.zCoord >> 4);
                    if (chunk7 != null)
                        chunk7.RemoveChunkBlockTileEntity(tileEntity5.xCoord & 15, tileEntity5.yCoord, tileEntity5.zCoord & 15);
                }
            }

            this.scanningTEs = false;
            if (Enhancements.FIX_CHUNK_CACHE_MEM_LEAK)
            {
                if (this.tileentityRemoval.Count != 0)
                {
                    foreach (TileEntity te in tileentityRemoval) loadedTileEntityList.Remove(te);
                    this.tileentityRemoval.Clear();
                }
            }

            Profiler.EndStartSection("pendingTileEntities");
            if (this.addedTEs.Count != 0)
            {
                IEnumerator<TileEntity> iterator6 = this.addedTEs.GetEnumerator();
                while (iterator6.MoveNext())
                {
                    TileEntity tileEntity8 = iterator6.Current;
                    if (!tileEntity8.IsInvalid())
                    {
                        if (!this.loadedTileEntityList.Contains(tileEntity8))
                        {
                            this.loadedTileEntityList.Add(tileEntity8);
                        }

                        LevelChunk chunk9 = this.GetChunk(tileEntity8.xCoord >> 4, tileEntity8.zCoord >> 4);
                        if (chunk9 != null)
                        {
                            chunk9.SetChunkBlockTileEntity(tileEntity8.xCoord & 15, tileEntity8.yCoord, tileEntity8.zCoord & 15, tileEntity8);
                        }

                        this.SendTileUpdated(tileEntity8.xCoord, tileEntity8.yCoord, tileEntity8.zCoord);
                    }
                }

                this.addedTEs.Clear();
            }

            Profiler.EndSection();
            Profiler.EndSection();
        }

        public virtual void AddLoadedTileEntities(IEnumerable<TileEntity> collection1)
        {
            if (this.scanningTEs)
            {
                this.addedTEs.AddRange(collection1);
            }
            else
            {
                this.loadedTileEntityList.AddRange(collection1);
            }
        }


        public virtual void RemoveTileEntity(TileEntity c)
        {
            if (Enhancements.FIX_CHUNK_CACHE_MEM_LEAK)
            {
                this.tileentityRemoval.Add(c);
            }
        }


        public virtual void UpdateEntity(Entity entity1)
        {
            this.UpdateEntityWithOptionalForce(entity1, true);
        }


        public virtual void UpdateEntityWithOptionalForce(Entity entity1, bool z2)
        {
            int i3 = Mth.Floor(entity1.x);
            int i4 = Mth.Floor(entity1.z);
            byte b5 = 32;
            if (!z2 || this.CheckChunksExist(i3 - b5, 0, i4 - b5, i3 + b5, 128, i4 + b5))
            {
                entity1.lastTickPosX = entity1.x;
                entity1.lastTickPosY = entity1.y;
                entity1.lastTickPosZ = entity1.z;
                entity1.prevYaw = entity1.yaw;
                entity1.prevPitch = entity1.pitch;
                if (z2 && entity1.addedToChunk)
                {
                    if (entity1.ridingEntity != null)
                    {
                        entity1.UpdateRidden();
                    }
                    else
                    {
                        entity1.OnUpdate();
                    }
                }

                if (Double.IsNaN(entity1.x) || Double.IsInfinity(entity1.x))
                {
                    entity1.x = entity1.lastTickPosX;
                }

                if (Double.IsNaN(entity1.y) || Double.IsInfinity(entity1.y))
                {
                    entity1.y = entity1.lastTickPosY;
                }

                if (Double.IsNaN(entity1.z) || Double.IsInfinity(entity1.z))
                {
                    entity1.z = entity1.lastTickPosZ;
                }

                if (Double.IsNaN(entity1.pitch) || Double.IsInfinity(entity1.pitch))
                {
                    entity1.pitch = entity1.prevPitch;
                }

                if (Double.IsNaN(entity1.yaw) || Double.IsInfinity(entity1.yaw))
                {
                    entity1.yaw = entity1.prevYaw;
                }

                int i6 = Mth.Floor(entity1.x / 16);
                int i7 = Mth.Floor(entity1.y / 16);
                int i8 = Mth.Floor(entity1.z / 16);
                if (!entity1.addedToChunk || entity1.chunkCoordX != i6 || entity1.chunkCoordY != i7 || entity1.chunkCoordZ != i8)
                {
                    if (entity1.addedToChunk && this.HasChunk(entity1.chunkCoordX, entity1.chunkCoordZ))
                    {
                        this.GetChunk(entity1.chunkCoordX, entity1.chunkCoordZ).RemoveEntityAtIndex(entity1, entity1.chunkCoordY);
                    }

                    if (this.HasChunk(i6, i8))
                    {
                        entity1.addedToChunk = true;
                        this.GetChunk(i6, i8).AddEntity(entity1);
                    }
                    else
                    {
                        entity1.addedToChunk = false;
                    }
                }

                if (z2 && entity1.addedToChunk && entity1.riddenByEntity != null)
                {
                    if (!entity1.riddenByEntity.isDead && entity1.riddenByEntity.ridingEntity == entity1)
                    {
                        this.UpdateEntity(entity1.riddenByEntity);
                    }
                    else
                    {
                        entity1.riddenByEntity.ridingEntity = null;
                        entity1.riddenByEntity = null;
                    }
                }
            }
        }


        public virtual bool CheckIfAABBIsClear(AABB axisAlignedBB1)
        {
            IList<Entity> list2 = this.GetEntities((Entity)null, axisAlignedBB1);
            for (int i3 = 0; i3 < list2.Count; ++i3)
            {
                Entity entity4 = list2[i3];
                if (!entity4.isDead && entity4.preventEntitySpawning)
                {
                    return false;
                }
            }

            return true;
        }


        public virtual bool Func_27069(AABB axisAlignedBB1)
        {
            int i2 = Mth.Floor(axisAlignedBB1.x0);
            int i3 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i4 = Mth.Floor(axisAlignedBB1.y0);
            int i5 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i6 = Mth.Floor(axisAlignedBB1.z0);
            int i7 = Mth.Floor(axisAlignedBB1.z1 + 1);
            if (axisAlignedBB1.x0 < 0)
            {
                --i2;
            }

            if (axisAlignedBB1.y0 < 0)
            {
                --i4;
            }

            if (axisAlignedBB1.z0 < 0)
            {
                --i6;
            }

            for (int i8 = i2; i8 < i3; ++i8)
            {
                for (int i9 = i4; i9 < i5; ++i9)
                {
                    for (int i10 = i6; i10 < i7; ++i10)
                    {
                        Tile block11 = Tile.tiles[this.GetTile(i8, i9, i10)];
                        if (block11 != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public virtual bool ContainsAnyLiquid(AABB axisAlignedBB1)
        {
            int i2 = Mth.Floor(axisAlignedBB1.x0);
            int i3 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i4 = Mth.Floor(axisAlignedBB1.y0);
            int i5 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i6 = Mth.Floor(axisAlignedBB1.z0);
            int i7 = Mth.Floor(axisAlignedBB1.z1 + 1);
            if (axisAlignedBB1.x0 < 0)
            {
                --i2;
            }

            if (axisAlignedBB1.y0 < 0)
            {
                --i4;
            }

            if (axisAlignedBB1.z0 < 0)
            {
                --i6;
            }

            for (int i8 = i2; i8 < i3; ++i8)
            {
                for (int i9 = i4; i9 < i5; ++i9)
                {
                    for (int i10 = i6; i10 < i7; ++i10)
                    {
                        Tile block11 = Tile.tiles[this.GetTile(i8, i9, i10)];
                        if (block11 != null && block11.material.IsLiquid())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public virtual bool ContainsFireTile(AABB axisAlignedBB1)
        {
            int i2 = Mth.Floor(axisAlignedBB1.x0);
            int i3 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i4 = Mth.Floor(axisAlignedBB1.y0);
            int i5 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i6 = Mth.Floor(axisAlignedBB1.z0);
            int i7 = Mth.Floor(axisAlignedBB1.z1 + 1);
            if (this.CheckChunksExist(i2, i4, i6, i3, i5, i7))
            {
                for (int i8 = i2; i8 < i3; ++i8)
                {
                    for (int i9 = i4; i9 < i5; ++i9)
                    {
                        for (int i10 = i6; i10 < i7; ++i10)
                        {
                            int i11 = this.GetTile(i8, i9, i10);
                            if (i11 == Tile.fire.id || i11 == Tile.lava.id || i11 == Tile.calmLava.id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }


        public virtual bool CheckAndHandleWater(AABB axisAlignedBB1, Material material2, Entity entity3)
        {
            int i4 = Mth.Floor(axisAlignedBB1.x0);
            int i5 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i6 = Mth.Floor(axisAlignedBB1.y0);
            int i7 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i8 = Mth.Floor(axisAlignedBB1.z0);
            int i9 = Mth.Floor(axisAlignedBB1.z1 + 1);
            if (!this.CheckChunksExist(i4, i6, i8, i5, i7, i9))
            {
                return false;
            }
            else
            {
                bool z10 = false;
                Vec3 vec3D11 = Vec3.Of(0, 0, 0);
                for (int i12 = i4; i12 < i5; ++i12)
                {
                    for (int i13 = i6; i13 < i7; ++i13)
                    {
                        for (int i14 = i8; i14 < i9; ++i14)
                        {
                            Tile block15 = Tile.tiles[this.GetTile(i12, i13, i14)];
                            if (block15 != null && block15.material == material2)
                            {
                                double d16 = i13 + 1 - LiquidTile.GetHeight(this.GetData(i12, i13, i14));
                                if (i7 >= d16)
                                {
                                    z10 = true;
                                    block15.HandleEntityInside(this, i12, i13, i14, entity3, vec3D11);
                                }
                            }
                        }
                    }
                }

                if (vec3D11.LengthVector() > 0)
                {
                    vec3D11 = vec3D11.Normalize();
                    double d18 = 0.014;
                    entity3.motionX += vec3D11.x * d18;
                    entity3.motionY += vec3D11.y * d18;
                    entity3.motionZ += vec3D11.z * d18;
                }

                return z10;
            }
        }


        public virtual bool ContainsMaterial(AABB axisAlignedBB1, Material material2)
        {
            int i3 = Mth.Floor(axisAlignedBB1.x0);
            int i4 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i5 = Mth.Floor(axisAlignedBB1.y0);
            int i6 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i7 = Mth.Floor(axisAlignedBB1.z0);
            int i8 = Mth.Floor(axisAlignedBB1.z1 + 1);
            for (int i9 = i3; i9 < i4; ++i9)
            {
                for (int i10 = i5; i10 < i6; ++i10)
                {
                    for (int i11 = i7; i11 < i8; ++i11)
                    {
                        Tile block12 = Tile.tiles[this.GetTile(i9, i10, i11)];
                        if (block12 != null && block12.material == material2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public virtual bool ContainsLiquid(AABB axisAlignedBB1, Material material2)
        {
            int i3 = Mth.Floor(axisAlignedBB1.x0);
            int i4 = Mth.Floor(axisAlignedBB1.x1 + 1);
            int i5 = Mth.Floor(axisAlignedBB1.y0);
            int i6 = Mth.Floor(axisAlignedBB1.y1 + 1);
            int i7 = Mth.Floor(axisAlignedBB1.z0);
            int i8 = Mth.Floor(axisAlignedBB1.z1 + 1);
            for (int i9 = i3; i9 < i4; ++i9)
            {
                for (int i10 = i5; i10 < i6; ++i10)
                {
                    for (int i11 = i7; i11 < i8; ++i11)
                    {
                        Tile block12 = Tile.tiles[this.GetTile(i9, i10, i11)];
                        if (block12 != null && block12.material == material2)
                        {
                            int i13 = this.GetData(i9, i10, i11);
                            double d14 = i10 + 1;
                            if (i13 < 8)
                            {
                                d14 = i10 + 1 - i13 / 8;
                            }

                            if (d14 >= axisAlignedBB1.y0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }


        public virtual Explosion Explode(Entity entity1, double d2, double d4, double d6, float f8)
        {
            return this.Explode(entity1, d2, d4, d6, f8, false);
        }


        public virtual Explosion Explode(Entity entity1, double d2, double d4, double d6, float f8, bool z9)
        {
            Explosion explosion10 = new Explosion(this, entity1, d2, d4, d6, f8);
            explosion10.isFlaming = z9;
            explosion10.Explode();
            explosion10.FinalizeExplosion(true);
            return explosion10;
        }


        public virtual float GetSeenPercent(Vec3 vec3D1, AABB axisAlignedBB2)
        {
            double d3 = 1 / ((axisAlignedBB2.x1 - axisAlignedBB2.x0) * 2 + 1);
            double d5 = 1 / ((axisAlignedBB2.y1 - axisAlignedBB2.y0) * 2 + 1);
            double d7 = 1 / ((axisAlignedBB2.z1 - axisAlignedBB2.z0) * 2 + 1);
            int i9 = 0;
            int i10 = 0;
            for (float f11 = 0.0F; f11 <= 1F; f11 = (float)(f11 + d3))
            {
                for (float f12 = 0.0F; f12 <= 1F; f12 = (float)(f12 + d5))
                {
                    for (float f13 = 0.0F; f13 <= 1F; f13 = (float)(f13 + d7))
                    {
                        double d14 = axisAlignedBB2.x0 + (axisAlignedBB2.x1 - axisAlignedBB2.x0) * f11;
                        double d16 = axisAlignedBB2.y0 + (axisAlignedBB2.y1 - axisAlignedBB2.y0) * f12;
                        double d18 = axisAlignedBB2.z0 + (axisAlignedBB2.z1 - axisAlignedBB2.z0) * f13;
                        if (this.Clip(Vec3.Of(d14, d16, d18), vec3D1) == null)
                        {
                            ++i9;
                        }

                        ++i10;
                    }
                }
            }

            return (float)i9 / (float)i10;
        }


        public virtual void ExtinguishFire(Player entityPlayer1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.DOWN)
            {
                --i3;
            }

            if (i5 == TileFace.UP)
            {
                ++i3;
            }

            if (i5 == TileFace.NORTH)
            {
                --i4;
            }

            if (i5 == TileFace.SOUTH)
            {
                ++i4;
            }

            if (i5 == TileFace.WEST)
            {
                --i2;
            }

            if (i5 == TileFace.EAST)
            {
                ++i2;
            }

            if (this.GetTile(i2, i3, i4) == Tile.fire.id)
            {
                this.LevelEvent(entityPlayer1, LevelEventType.FIZZ, i2, i3, i4, 0);
                this.SetTile(i2, i3, i4, 0);
            }
        }


        public virtual object FindInstanceOf<E>(Type class1)
        {
            return null;
        }


        public virtual string GetLoadedEntityStats()
        {
            return "All: " + this.loadedEntityList.Count;
        }


        public virtual string GetChunkSourceStats()
        {
            return this.chunkSource.GatherStats();
        }


        public virtual TileEntity GetTileEntity(int i1, int i2, int i3)
        {
            LevelChunk chunk4 = this.GetChunk(i1 >> 4, i3 >> 4);
            return chunk4 != null ? chunk4.GetChunkBlockTileEntity(i1 & 15, i2, i3 & 15) : null;
        }


        public virtual void SetTileEntity(int i1, int i2, int i3, TileEntity tileEntity4)
        {
            if (!tileEntity4.IsInvalid())
            {
                if (this.scanningTEs)
                {
                    tileEntity4.xCoord = i1;
                    tileEntity4.yCoord = i2;
                    tileEntity4.zCoord = i3;
                    this.addedTEs.Add(tileEntity4);
                }
                else
                {
                    this.loadedTileEntityList.Add(tileEntity4);
                    LevelChunk chunk5 = this.GetChunk(i1 >> 4, i3 >> 4);
                    if (chunk5 != null)
                    {
                        chunk5.SetChunkBlockTileEntity(i1 & 15, i2, i3 & 15, tileEntity4);
                    }
                }
            }
        }


        public virtual void RemoveTileEntity(int i1, int i2, int i3)
        {
            TileEntity tileEntity4 = this.GetTileEntity(i1, i2, i3);
            if (tileEntity4 != null && this.scanningTEs)
            {
                tileEntity4.Invalidate();
            }
            else
            {
                if (tileEntity4 != null)
                {
                    this.loadedTileEntityList.Remove(tileEntity4);
                }

                LevelChunk chunk5 = this.GetChunk(i1 >> 4, i3 >> 4);
                if (chunk5 != null)
                {
                    chunk5.RemoveChunkBlockTileEntity(i1 & 15, i2, i3 & 15);
                }
            }
        }


        public virtual bool IsSolidRenderTile(int i1, int i2, int i3)
        {
            Tile block4 = Tile.tiles[this.GetTile(i1, i2, i3)];
            return block4 == null ? false : block4.IsSolidRender();
        }


        public virtual bool IsSolidBlockingTile(int i1, int i2, int i3)
        {
            Tile block4 = Tile.tiles[this.GetTile(i1, i2, i3)];
            return block4 == null ? false : block4.material.GetIsTranslucent() && block4.IsCubeShaped();
        }


        public virtual void Save(IProgressListener iProgressUpdate1)
        {
            this.Save(true, iProgressUpdate1);
            if (Enhancements.THREADED_LEVEL_IO)
            {
                try
                {
                    ThreadedIO.Instance.WaitForFinish();
                }
                catch (Exception)
                {
                }
            }
        }


        public virtual bool UpdateLights()
        {
            if (this.lightingUpdatesCounter >= 50)
            {
                return false;
            }
            else
            {
                ++this.lightingUpdatesCounter;
                try
                {
                    int i1 = 500;
                    bool z2;
                    while (this.lightingToUpdate.Count > 0)
                    {
                        --i1;
                        if (i1 <= 0)
                        {
                            z2 = true;
                            return z2;
                        }

                        LightUpdate obj = lightingToUpdate[lightingToUpdate.Count - 1];
                        obj.Update(this);
                        lightingToUpdate.RemoveAt(lightingToUpdate.Count - 1);
                    }

                    z2 = false;
                    return z2;
                }
                finally
                {
                    --this.lightingUpdatesCounter;
                }
            }
        }


        public virtual void UpdateLight(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            this.UpdateLight(enumSkyBlock1, i2, i3, i4, i5, i6, i7, true);
        }


        public virtual void UpdateLight(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5, int i6, int i7, bool z8)
        {
            if (!this.dimension.hasNoSky || enumSkyBlock1 != LightLayer.Sky)
            {
                ++lightingUpdatesScheduled;
                try
                {
                    if (lightingUpdatesScheduled == 50)
                    {
                        return;
                    }

                    int i9 = (i5 + i2) / 2;
                    int i10 = (i7 + i4) / 2;
                    if (this.HasChunkAt(i9, 64, i10))
                    {
                        if (this.GetChunkAt(i9, i10).IsEmpty())
                        {
                            return;
                        }

                        int i11 = this.lightingToUpdate.Count;
                        int i12;
                        if (z8)
                        {
                            i12 = 5;
                            if (i12 > i11)
                            {
                                i12 = i11;
                            }

                            for (int i13 = 0; i13 < i12; ++i13)
                            {
                                LightUpdate metadataChunkBlock14 = this.lightingToUpdate[this.lightingToUpdate.Count - i13 - 1];
                                if (metadataChunkBlock14.layer == enumSkyBlock1 && metadataChunkBlock14.Func866(i2, i3, i4, i5, i6, i7))
                                {
                                    return;
                                }
                            }
                        }

                        this.lightingToUpdate.Add(new LightUpdate(enumSkyBlock1, i2, i3, i4, i5, i6, i7));
                        i12 = 1000000;
                        if (this.lightingToUpdate.Count > 1000000)
                        {
                            Console.WriteLine("More than " + i12 + " updates, aborting lighting updates");
                            this.lightingToUpdate.Clear();
                        }

                        return;
                    }
                }
                finally
                {
                    --lightingUpdatesScheduled;
                }
            }
        }


        public virtual void CalculateInitialSkylight()
        {
            int i1 = this.GetSkyDarken(1F);
            if (i1 != this.skyDarken)
            {
                this.skyDarken = i1;
            }
        }


        public virtual void SetSpawnSettings(bool z1, bool z2)
        {
            this.spawnHostileMobs = z1;
            this.spawnPeacefulMobs = z2;
        }


        public virtual void Tick()
        {
            this.TickWeather();
            long time;
            if (this.IsAllPlayersFullyAsleep())
            {
                bool sleepSpawn = false;
                if (this.spawnHostileMobs && this.difficultySetting >= 1)
                {
                    sleepSpawn = MobSpawner.PerformSleepSpawning(this, this.playerEntities);
                }

                if (!sleepSpawn)
                {
                    time = this.levelData.GetTime() + 24000;
                    this.levelData.SetTime(time - time % 24000);
                    this.WakeUpAllPlayers();
                }
            }
            Profiler.StartSection("mobSpawner");
            MobSpawner.Tick(this, this.spawnHostileMobs, this.spawnPeacefulMobs);
            Profiler.EndStartSection("chunkSource");
            this.chunkSource.Tick();
            int i4 = this.GetSkyDarken(1F);
            if (i4 != this.skyDarken)
            {
                this.skyDarken = i4;
                for (int i5 = 0; i5 < this.levelListeners.Count; ++i5)
                {
                    this.levelListeners[i5].AllChanged();
                }
            }

            time = this.levelData.GetTime() + 1;
            if (time % this.autosavePeriod == 0)
            {
                Profiler.EndStartSection("save");
                this.Save(false, (IProgressListener)null);
            }

            this.levelData.SetTime(time);
            Profiler.EndStartSection("tickPending");
            this.TickUpdates(false);
            Profiler.EndStartSection("tickTiles");
            this.DoRandomUpdateTicks();
            Profiler.EndSection();
        }

        private void Func_27163()
        {
            if (this.levelData.GetRaining())
            {
                this.rainingStrength = 1F;
                if (this.levelData.GetThundering())
                {
                    this.thunderingStrength = 1F;
                }
            }
        }


        protected virtual void TickWeather()
        {
            if (!this.dimension.hasNoSky)
            {
                if (this.field_F > 0)
                {
                    --this.field_F;
                }

                int i1 = this.levelData.GetThunderTime();
                if (i1 <= 0)
                {
                    if (this.levelData.GetThundering())
                    {
                        this.levelData.SetThunderTime(this.rand.NextInt(12000) + 3600);
                    }
                    else
                    {
                        this.levelData.SetThunderTime(this.rand.NextInt(168000) + 12000);
                    }
                }
                else
                {
                    --i1;
                    this.levelData.SetThunderTime(i1);
                    if (i1 <= 0)
                    {
                        this.levelData.SetThundering(!this.levelData.GetThundering());
                    }
                }

                int i2 = this.levelData.GetRainTime();
                if (i2 <= 0)
                {
                    if (this.levelData.GetRaining())
                    {
                        this.levelData.SetRainTime(this.rand.NextInt(12000) + 12000);
                    }
                    else
                    {
                        this.levelData.SetRainTime(this.rand.NextInt(168000) + 12000);
                    }
                }
                else
                {
                    --i2;
                    this.levelData.SetRainTime(i2);
                    if (i2 <= 0)
                    {
                        this.levelData.SetRaining(!this.levelData.GetRaining());
                    }
                }

                this.prevRainingStrength = this.rainingStrength;
                if (this.levelData.GetRaining())
                {
                    this.rainingStrength = (float)(this.rainingStrength + 0.01);
                }
                else
                {
                    this.rainingStrength = (float)(this.rainingStrength - 0.01);
                }

                if (this.rainingStrength < 0F)
                {
                    this.rainingStrength = 0F;
                }

                if (this.rainingStrength > 1F)
                {
                    this.rainingStrength = 1F;
                }

                this.prevThunderingStrength = this.thunderingStrength;
                if (this.levelData.GetThundering())
                {
                    this.thunderingStrength = (float)(this.thunderingStrength + 0.01);
                }
                else
                {
                    this.thunderingStrength = (float)(this.thunderingStrength - 0.01);
                }

                if (this.thunderingStrength < 0F)
                {
                    this.thunderingStrength = 0F;
                }

                if (this.thunderingStrength > 1F)
                {
                    this.thunderingStrength = 1F;
                }
            }
        }


        private void ClearWeather()
        {
            this.levelData.SetRainTime(0);
            this.levelData.SetRaining(false);
            this.levelData.SetThunderTime(0);
            this.levelData.SetThundering(false);
        }


        protected virtual void DoRandomUpdateTicks()
        {
            this.activeChunkSet.Clear();
            Profiler.StartSection("buildList");
            int i3;
            int i4;
            int i6;
            int i7;
            for (int i1 = 0; i1 < this.playerEntities.Count; ++i1)
            {
                Player entityPlayer2 = this.playerEntities[i1];
                i3 = Mth.Floor(entityPlayer2.x / 16);
                i4 = Mth.Floor(entityPlayer2.z / 16);
                byte b5 = 9;
                for (i6 = -b5; i6 <= b5; ++i6)
                {
                    for (i7 = -b5; i7 <= b5; ++i7)
                    {
                        this.activeChunkSet.Add(new ChunkPos(i6 + i3, i7 + i4));
                    }
                }
            }
            Profiler.EndSection();
            if (this.soundCounter > 0)
            {
                --this.soundCounter;
            }

            IEnumerator<ChunkPos> iterator12 = this.activeChunkSet.GetEnumerator();
            while (iterator12.MoveNext())
            {
                ChunkPos chunkCoordIntPair13 = iterator12.Current;
                i3 = chunkCoordIntPair13.x * 16;
                i4 = chunkCoordIntPair13.z * 16;
                Profiler.StartSection("getChunk");
                LevelChunk chunk14 = this.GetChunk(chunkCoordIntPair13.x, chunkCoordIntPair13.z);
                int i8;
                int i9;
                int i10;
                Profiler.EndStartSection("moodSound");
                if (this.soundCounter == 0)
                {
                    this.distHashCounter = this.distHashCounter * 3 + 1013904223;
                    i6 = this.distHashCounter >> 2;
                    i7 = i6 & 15;
                    i8 = i6 >> 8 & 15;
                    i9 = i6 >> 16 & 127;
                    i10 = chunk14.GetBlockID(i7, i9, i8);
                    i7 += i3;
                    i8 += i4;
                    if (i10 == 0 && this.IsSkyLit(i7, i9, i8) <= this.rand.NextInt(8) && this.GetSavedLightValue(LightLayer.Sky, i7, i9, i8) <= 0)
                    {
                        Player entityPlayer11 = this.GetClosestPlayer(i7 + 0.5, i9 + 0.5, i8 + 0.5, 8);
                        if (entityPlayer11 != null && entityPlayer11.GetDistanceSq(i7 + 0.5, i9 + 0.5, i8 + 0.5) > 4)
                        {
                            this.PlaySound(i7 + 0.5, i9 + 0.5, i8 + 0.5, "ambient.cave.cave", 0.7F, 0.8F + this.rand.NextFloat() * 0.2F);
                            this.soundCounter = this.rand.NextInt(12000) + 6000;
                        }
                    }
                }

                Profiler.EndStartSection("thunder");
                if (this.rand.NextInt(100000) == 0 && this.Func_C() && this.Func_B())
                {
                    this.distHashCounter = this.distHashCounter * 3 + 1013904223;
                    i6 = this.distHashCounter >> 2;
                    i7 = i3 + (i6 & 15);
                    i8 = i4 + (i6 >> 8 & 15);
                    i9 = this.GetTopSolidBlock(i7, i8);
                    if (this.CanLightningStrikeAt(i7, i9, i8))
                    {
                        this.AddWeatherEffect(new LightningBolt(this, i7, i9, i8));
                        this.field_F = 2;
                    }
                }

                Profiler.EndStartSection("iceandsnow");
                int i15;
                if (this.rand.NextInt(16) == 0)
                {
                    this.distHashCounter = this.distHashCounter * 3 + 1013904223;
                    i6 = this.distHashCounter >> 2;
                    i7 = i6 & 15;
                    i8 = i6 >> 8 & 15;
                    i9 = this.GetTopSolidBlock(i7 + i3, i8 + i4);
                    if (this.GetBiomeSource().GetBiomeGenAt(i7 + i3, i8 + i4).IsSnowCovered() && i9 >= 0 && i9 < 128 && chunk14.GetSavedLightValue(LightLayer.Block, i7, i9, i8) < 10)
                    {
                        i10 = chunk14.GetBlockID(i7, i9 - 1, i8);
                        i15 = chunk14.GetBlockID(i7, i9, i8);
                        if (this.Func_C() && i15 == 0 && Tile.topSnow.CanPlaceBlockAt(this, i7 + i3, i9, i8 + i4) && i10 != 0 && i10 != Tile.ice.id && Tile.tiles[i10].material.BlocksMotion())
                        {
                            this.SetTile(i7 + i3, i9, i8 + i4, Tile.topSnow.id);
                        }

                        if (i10 == Tile.calmWater.id && chunk14.GetBlockMetadata(i7, i9 - 1, i8) == 0)
                        {
                            this.SetTile(i7 + i3, i9 - 1, i8 + i4, Tile.ice.id);
                        }
                    }
                }

                Profiler.EndStartSection("tickTiles");
                for (i6 = 0; i6 < 80; ++i6)
                {
                    this.distHashCounter = this.distHashCounter * 3 + 1013904223;
                    i7 = this.distHashCounter >> 2;
                    i8 = i7 & 15;
                    i9 = i7 >> 8 & 15;
                    i10 = i7 >> 16 & 127;
                    i15 = chunk14.blocks[i8 << 11 | i9 << 7 | i10] & 255;
                    if (Tile.shouldTick[i15])
                    {
                        Tile.tiles[i15].Tick(this, i8 + i3, i10, i9 + i4, this.rand);
                    }
                }
                Profiler.EndSection();
            }
        }


        public virtual bool TickUpdates(bool z1)
        {
            int i2 = this.scheduledTickTreeSet.Count;
            if (i2 != this.scheduledTickSet.Count)
            {
                throw new InvalidOperationException("TickNextTick list out of synch");
            }
            else
            {
                if (i2 > 1000)
                {
                    i2 = 1000;
                }

                for (int i3 = 0; i3 < i2; ++i3)
                {
                    TickNextTickData nextTickListEntry4 = this.scheduledTickTreeSet.First();
                    if (!z1 && nextTickListEntry4.scheduledTime > this.levelData.GetTime())
                    {
                        break;
                    }

                    this.scheduledTickTreeSet.Remove(nextTickListEntry4);
                    this.scheduledTickSet.Remove(nextTickListEntry4);
                    byte b5 = 8;
                    if (this.CheckChunksExist(nextTickListEntry4.xCoord - b5, nextTickListEntry4.yCoord - b5, nextTickListEntry4.zCoord - b5, nextTickListEntry4.xCoord + b5, nextTickListEntry4.yCoord + b5, nextTickListEntry4.zCoord + b5))
                    {
                        int i6 = this.GetTile(nextTickListEntry4.xCoord, nextTickListEntry4.yCoord, nextTickListEntry4.zCoord);
                        if (i6 == nextTickListEntry4.blockID && i6 > 0)
                        {
                            Tile.tiles[i6].Tick(this, nextTickListEntry4.xCoord, nextTickListEntry4.yCoord, nextTickListEntry4.zCoord, this.rand);
                        }
                    }
                }

                return this.scheduledTickTreeSet.Count != 0;
            }
        }


        public virtual void RandomDisplayUpdates(int i1, int i2, int i3)
        {
            byte b4 = 16;
            JRandom random5 = new JRandom();
            for (int i6 = 0; i6 < 1000; ++i6)
            {
                int i7 = i1 + this.rand.NextInt(b4) - this.rand.NextInt(b4);
                int i8 = i2 + this.rand.NextInt(b4) - this.rand.NextInt(b4);
                int i9 = i3 + this.rand.NextInt(b4) - this.rand.NextInt(b4);
                int i10 = this.GetTile(i7, i8, i9);
                if (i10 > 0)
                {
                    Tile.tiles[i10].AnimateTick(this, i7, i8, i9, random5);
                }
            }
        }


        public virtual IList<Entity> GetEntities(Entity entity1, AABB axisAlignedBB2)
        {
            this.field_M.Clear();
            int i3 = Mth.Floor((axisAlignedBB2.x0 - 2) / 16);
            int i4 = Mth.Floor((axisAlignedBB2.x1 + 2) / 16);
            int i5 = Mth.Floor((axisAlignedBB2.z0 - 2) / 16);
            int i6 = Mth.Floor((axisAlignedBB2.z1 + 2) / 16);
            for (int i7 = i3; i7 <= i4; ++i7)
            {
                for (int i8 = i5; i8 <= i6; ++i8)
                {
                    if (this.HasChunk(i7, i8))
                    {
                        this.GetChunk(i7, i8).GetEntitiesWithinAABBForEntity(entity1, axisAlignedBB2, this.field_M);
                    }
                }
            }

            return this.field_M;
        }


        public virtual List<E> GetEntitiesOfClass<E>(Type class1, AABB axisAlignedBB2) where E : Entity
        {
            int i3 = Mth.Floor((axisAlignedBB2.x0 - 2) / 16);
            int i4 = Mth.Floor((axisAlignedBB2.x1 + 2) / 16);
            int i5 = Mth.Floor((axisAlignedBB2.z0 - 2) / 16);
            int i6 = Mth.Floor((axisAlignedBB2.z1 + 2) / 16);
            List<E> arrayList7 = new List<E>();
            for (int i8 = i3; i8 <= i4; ++i8)
            {
                for (int i9 = i5; i9 <= i6; ++i9)
                {
                    if (this.HasChunk(i8, i9))
                    {
                        this.GetChunk(i8, i9).GetEntitiesOfTypeWithinAAAB(class1, axisAlignedBB2, arrayList7);
                    }
                }
            }

            return arrayList7;
        }


        public virtual IList<Entity> GetLoadedEntityList()
        {
            return this.loadedEntityList;
        }


        public virtual void TileEntityChanged(int i1, int i2, int i3, TileEntity tileEntity4)
        {
            if (this.HasChunkAt(i1, i2, i3))
            {
                this.GetChunkAt(i1, i3).SetChunkModified();
            }

            for (int i5 = 0; i5 < this.levelListeners.Count; ++i5)
            {
                this.levelListeners[i5].TileEntityChanged(i1, i2, i3, tileEntity4);
            }
        }


        public virtual int CountInstanceOf(Type c)
        {
            int i2 = 0;
            for (int i3 = 0; i3 < this.loadedEntityList.Count; ++i3)
            {
                Entity entity4 = this.loadedEntityList[i3];
                if (c.IsAssignableFrom(entity4.GetType()))
                {
                    ++i2;
                }
            }

            return i2;
        }


        public virtual void AddEntities(IList<Entity> list1)
        {
            foreach (Entity e in list1) loadedEntityList.Add(e);

            for (int i2 = 0; i2 < list1.Count; ++i2)
            {
                this.EntityAdded(list1[i2]);
            }
        }


        public virtual void RemoveEntities(IList<Entity> list1)
        {
            foreach (Entity e in list1) unloadedEntityList.Add(e);
        }


        public virtual void Prepare()
        {
            while (this.chunkSource.Tick())
            {
            }
        }


        public virtual bool CanBlockBePlacedAt(int i1, int i2, int i3, int i4, bool z5, TileFace i6)
        {
            int i7 = this.GetTile(i2, i3, i4);
            Tile block8 = Tile.tiles[i7];
            Tile block9 = Tile.tiles[i1];
            AABB axisAlignedBB10 = block9.GetAABB(this, i2, i3, i4);
            if (z5)
            {
                axisAlignedBB10 = null;
            }

            if (axisAlignedBB10 != null && !this.CheckIfAABBIsClear(axisAlignedBB10))
            {
                return false;
            }
            else
            {
                if (block8 == Tile.water || block8 == Tile.calmWater || block8 == Tile.lava || block8 == Tile.calmLava || block8 == Tile.fire || block8 == Tile.topSnow)
                {
                    block8 = null;
                }

                return i1 > 0 && block8 == null && block9.CanPlaceBlockOnSide(this, i2, i3, i4, i6);
            }
        }


        public virtual PathFinding.Path GetPathToEntity(Entity entity1, Entity entity2, float f3)
        {
            Profiler.StartSection("pathfind");
            int i4 = Mth.Floor(entity1.x);
            int i5 = Mth.Floor(entity1.y);
            int i6 = Mth.Floor(entity1.z);
            int i7 = (int)(f3 + 16F);
            int i8 = i4 - i7;
            int i9 = i5 - i7;
            int i10 = i6 - i7;
            int i11 = i4 + i7;
            int i12 = i5 + i7;
            int i13 = i6 + i7;
            Region chunkCache14 = new Region(this, i8, i9, i10, i11, i12, i13);
            PathFinding.Path p = (new PathFinder(chunkCache14)).CreateEntityPathTo(entity1, entity2, f3);
            Profiler.EndSection();
            return p;
        }


        public virtual PathFinding.Path GetEntityPathToXYZ(Entity entity1, int i2, int i3, int i4, float f5)
        {
            Profiler.StartSection("pathfind");
            int i6 = Mth.Floor(entity1.x);
            int i7 = Mth.Floor(entity1.y);
            int i8 = Mth.Floor(entity1.z);
            int i9 = (int)(f5 + 8F);
            int i10 = i6 - i9;
            int i11 = i7 - i9;
            int i12 = i8 - i9;
            int i13 = i6 + i9;
            int i14 = i7 + i9;
            int i15 = i8 + i9;
            Region chunkCache16 = new Region(this, i10, i11, i12, i13, i14, i15);
            PathFinding.Path p = (new PathFinder(chunkCache16)).CreateEntityPathTo(entity1, i2, i3, i4, f5);
            Profiler.EndSection();
            return p;
        }


        public virtual bool IsBlockProvidingPowerTo(int i1, int i2, int i3, int i4)
        {
            int i5 = this.GetTile(i1, i2, i3);
            return i5 == 0 ? false : Tile.tiles[i5].GetSignal(this, i1, i2, i3, i4);
        }


        public virtual bool IsBlockGettingPowered(int i1, int i2, int i3)
        {
            return this.IsBlockProvidingPowerTo(i1, i2 - 1, i3, 0) ? true : (this.IsBlockProvidingPowerTo(i1, i2 + 1, i3, 1) ? true : (this.IsBlockProvidingPowerTo(i1, i2, i3 - 1, 2) ? true : (this.IsBlockProvidingPowerTo(i1, i2, i3 + 1, 3) ? true : (this.IsBlockProvidingPowerTo(i1 - 1, i2, i3, 4) ? true : this.IsBlockProvidingPowerTo(i1 + 1, i2, i3, 5)))));
        }


        public virtual bool IsBlockIndirectlyProvidingPowerTo(int i1, int i2, int i3, int i4)
        {
            if (this.IsSolidBlockingTile(i1, i2, i3))
            {
                return this.IsBlockGettingPowered(i1, i2, i3);
            }
            else
            {
                int i5 = this.GetTile(i1, i2, i3);
                return i5 == 0 ? false : Tile.tiles[i5].GetDirectSignal(this, i1, i2, i3, i4);
            }
        }


        public virtual bool IsBlockIndirectlyGettingPowered(int i1, int i2, int i3)
        {
            return this.IsBlockIndirectlyProvidingPowerTo(i1, i2 - 1, i3, 0) ? true : (this.IsBlockIndirectlyProvidingPowerTo(i1, i2 + 1, i3, 1) ? true : (this.IsBlockIndirectlyProvidingPowerTo(i1, i2, i3 - 1, 2) ? true : (this.IsBlockIndirectlyProvidingPowerTo(i1, i2, i3 + 1, 3) ? true : (this.IsBlockIndirectlyProvidingPowerTo(i1 - 1, i2, i3, 4) ? true : this.IsBlockIndirectlyProvidingPowerTo(i1 + 1, i2, i3, 5)))));
        }


        public virtual Player GetClosestPlayerToEntity(Entity entity1, double d2)
        {
            return this.GetClosestPlayer(entity1.x, entity1.y, entity1.z, d2);
        }


        public virtual Player GetClosestPlayer(double d1, double d3, double d5, double d7)
        {
            double d9 = -1;
            Player entityPlayer11 = null;
            for (int i12 = 0; i12 < this.playerEntities.Count; ++i12)
            {
                Player entityPlayer13 = this.playerEntities[i12];
                double d14 = entityPlayer13.GetDistanceSq(d1, d3, d5);
                if ((d7 < 0 || d14 < d7 * d7) && (d9 == -1 || d14 < d9))
                {
                    d9 = d14;
                    entityPlayer11 = entityPlayer13;
                }
            }

            return entityPlayer11;
        }


        public virtual Player GetPlayerEntityByName(string string1)
        {
            for (int i2 = 0; i2 < this.playerEntities.Count; ++i2)
            {
                if (string1.Equals(this.playerEntities[i2].username))
                {
                    return this.playerEntities[i2];
                }
            }

            return null;
        }


        public virtual void SetChunkData(int i1, int i2, int i3, int i4, int i5, int i6, byte[] b7)
        {
            int i8 = i1 >> 4;
            int i9 = i3 >> 4;
            int i10 = i1 + i4 - 1 >> 4;
            int i11 = i3 + i6 - 1 >> 4;
            int i12 = 0;
            int i13 = i2;
            int i14 = i2 + i5;
            if (i2 < 0)
            {
                i13 = 0;
            }

            if (i14 > 128)
            {
                i14 = 128;
            }

            for (int i15 = i8; i15 <= i10; ++i15)
            {
                int i16 = i1 - i15 * 16;
                int i17 = i1 + i4 - i15 * 16;
                if (i16 < 0)
                {
                    i16 = 0;
                }

                if (i17 > 16)
                {
                    i17 = 16;
                }

                for (int i18 = i9; i18 <= i11; ++i18)
                {
                    int i19 = i3 - i18 * 16;
                    int i20 = i3 + i6 - i18 * 16;
                    if (i19 < 0)
                    {
                        i19 = 0;
                    }

                    if (i20 > 16)
                    {
                        i20 = 16;
                    }

                    i12 = this.GetChunk(i15, i18).SetChunkData(b7, i16, i13, i19, i17, i14, i20, i12);
                    this.SetTilesDirty(i15 * 16 + i16, i13, i18 * 16 + i19, i15 * 16 + i17, i14, i18 * 16 + i20);
                }
            }
        }


        public virtual void SendQuittingDisconnectingPacket()
        {
        }


        public virtual byte[] GetChunkData(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            byte[] b7 = new byte[i4 * i5 * i6 * 5 / 2];
            int i8 = i1 >> 4;
            int i9 = i3 >> 4;
            int i10 = i1 + i4 - 1 >> 4;
            int i11 = i3 + i6 - 1 >> 4;
            int i12 = 0;
            int i13 = i2;
            int i14 = i2 + i5;
            if (i2 < 0)
            {
                i13 = 0;
            }

            if (i14 > 128)
            {
                i14 = 128;
            }

            for (int i15 = i8; i15 <= i10; ++i15)
            {
                int i16 = i1 - i15 * 16;
                int i17 = i1 + i4 - i15 * 16;
                if (i16 < 0)
                {
                    i16 = 0;
                }

                if (i17 > 16)
                {
                    i17 = 16;
                }

                for (int i18 = i9; i18 <= i11; ++i18)
                {
                    int i19 = i3 - i18 * 16;
                    int i20 = i3 + i6 - i18 * 16;
                    if (i19 < 0)
                    {
                        i19 = 0;
                    }

                    if (i20 > 16)
                    {
                        i20 = 16;
                    }

                    i12 = this.GetChunk(i15, i18).GetChunkData(b7, i16, i13, i19, i17, i14, i20, i12);
                }
            }

            return b7;
        }


        public virtual void CheckSession()
        {
            this.levelStorage.CheckSession();
        }


        public virtual void SetTime(long j1)
        {
            this.levelData.SetTime(j1);
        }


        public virtual void Func_32005(long j1)
        {
            long j3 = j1 - this.levelData.GetTime();
            TickNextTickData nextTickListEntry6;
            for (IEnumerator<TickNextTickData> iterator5 = this.scheduledTickSet.GetEnumerator(); iterator5.MoveNext(); nextTickListEntry6.scheduledTime += j3)
            {
                nextTickListEntry6 = iterator5.Current;
            }

            this.SetTime(j1);
        }


        public virtual long GetRandomSeed()
        {
            return this.levelData.GetRandomSeed();
        }


        public virtual long GetTime()
        {
            return this.levelData.GetTime();
        }


        public virtual Pos GetSpawnPos()
        {
            return new Pos(this.levelData.GetSpawnX(), this.levelData.GetSpawnY(), this.levelData.GetSpawnZ());
        }


        public virtual void SetSpawnPos(Pos pos)
        {
            this.levelData.SetSpawn(pos.x, pos.y, pos.z);
        }


        public virtual void JoinEntityInSurroundings(Entity entity1)
        {
            int i2 = Mth.Floor(entity1.x / 16);
            int i3 = Mth.Floor(entity1.z / 16);
            byte b4 = 2;
            for (int i5 = i2 - b4; i5 <= i2 + b4; ++i5)
            {
                for (int i6 = i3 - b4; i6 <= i3 + b4; ++i6)
                {
                    this.GetChunk(i5, i6);
                }
            }

            if (!this.loadedEntityList.Contains(entity1))
            {
                this.loadedEntityList.Add(entity1);
            }
        }


        public virtual bool CanMineBlock(Player entityPlayer1, int i2, int i3, int i4)
        {
            return true;
        }


        public virtual void SendTrackedEntityStatusUpdatePacket(Entity entity1, byte b2)
        {
        }


        public virtual void UpdateEntityList()
        {
            foreach (Entity e in unloadedEntityList) loadedEntityList.Remove(e);
            int i1;
            Entity entity2;
            int i3;
            int i4;
            for (i1 = 0; i1 < this.unloadedEntityList.Count; ++i1)
            {
                entity2 = this.unloadedEntityList[i1];
                i3 = entity2.chunkCoordX;
                i4 = entity2.chunkCoordZ;
                if (entity2.addedToChunk && this.HasChunk(i3, i4))
                {
                    this.GetChunk(i3, i4).RemoveEntity(entity2);
                }
            }

            for (i1 = 0; i1 < this.unloadedEntityList.Count; ++i1)
            {
                this.EntityRemoved(this.unloadedEntityList[i1]);
            }

            this.unloadedEntityList.Clear();
            for (i1 = 0; i1 < this.loadedEntityList.Count; ++i1)
            {
                entity2 = this.loadedEntityList[i1];
                if (entity2.ridingEntity != null)
                {
                    if (!entity2.ridingEntity.isDead && entity2.ridingEntity.riddenByEntity == entity2)
                    {
                        continue;
                    }

                    entity2.ridingEntity.riddenByEntity = null;
                    entity2.ridingEntity = null;
                }

                if (entity2.isDead)
                {
                    i3 = entity2.chunkCoordX;
                    i4 = entity2.chunkCoordZ;
                    if (entity2.addedToChunk && this.HasChunk(i3, i4))
                    {
                        this.GetChunk(i3, i4).RemoveEntity(entity2);
                    }

                    this.loadedEntityList.RemoveAt(i1--);
                    this.EntityRemoved(entity2);
                }
            }
        }


        public virtual IChunkSource GetChunkSource()
        {
            return this.chunkSource;
        }


        public virtual void TileEvent(int i1, int i2, int i3, int i4, int i5)
        {
            int i6 = this.GetTile(i1, i2, i3);
            if (i6 > 0)
            {
                Tile.tiles[i6].TileEvent(this, i1, i2, i3, i4, i5);
            }
        }


        public virtual ILevelStorage GetLevelStorage()
        {
            return this.levelStorage;
        }


        public virtual LevelData GetLevelData()
        {
            return this.levelData;
        }


        public virtual void UpdateAllPlayersSleepingFlag()
        {
            this.allPlayersSleeping = this.playerEntities.Count > 0;
            IEnumerator<Player> iterator1 = this.playerEntities.GetEnumerator();
            while (iterator1.MoveNext())
            {
                Player entityPlayer2 = iterator1.Current;
                if (!entityPlayer2.IsSleeping())
                {
                    this.allPlayersSleeping = false;
                    break;
                }
            }
        }


        protected virtual void WakeUpAllPlayers()
        {
            this.allPlayersSleeping = false;
            IEnumerator<Player> iterator1 = this.playerEntities.GetEnumerator();
            while (iterator1.MoveNext())
            {
                Player entityPlayer2 = iterator1.Current;
                if (entityPlayer2.IsSleeping())
                {
                    entityPlayer2.WakeUpPlayer(false, false, true);
                }
            }

            this.ClearWeather();
        }


        public virtual bool IsAllPlayersFullyAsleep()
        {
            if (this.allPlayersSleeping && !this.isRemote)
            {
                IEnumerator<Player> iterator1 = this.playerEntities.GetEnumerator();
                Player entityPlayer2;
                do
                {
                    if (!iterator1.MoveNext())
                    {
                        return true;
                    }

                    entityPlayer2 = iterator1.Current;
                }
                while (entityPlayer2.IsPlayerFullyAsleep());
                return false;
            }
            else
            {
                return false;
            }
        }


        public virtual float Func_f(float f1)
        {
            return (this.prevThunderingStrength + (this.thunderingStrength - this.prevThunderingStrength) * f1) * this.Func_g(f1);
        }


        public virtual float Func_g(float f1)
        {
            return this.prevRainingStrength + (this.rainingStrength - this.prevRainingStrength) * f1;
        }


        public virtual void SetRainStrength(float f1)
        {
            this.prevRainingStrength = f1;
            this.rainingStrength = f1;
        }


        public virtual bool Func_B()
        {
            return this.Func_f(1F) > 0.9;
        }


        public virtual bool Func_C()
        {
            return this.Func_g(1F) > 0.2;
        }


        public virtual bool CanLightningStrikeAt(int i1, int i2, int i3)
        {
            if (!this.Func_C())
            {
                return false;
            }
            else if (!this.CanCockSeeTheSky(i1, i2, i3))
            {
                return false;
            }
            else if (this.GetTopSolidBlock(i1, i3) > i2)
            {
                return false;
            }
            else
            {
                Biome biomeGenBase4 = this.GetBiomeSource().GetBiomeGenAt(i1, i3);
                return biomeGenBase4.IsSnowCovered() ? false : biomeGenBase4.CanThunder();
            }
        }


        public virtual void SetItemData(string string1, SavedData mapDataBase2)
        {
            this.mapStorage.SetData(string1, mapDataBase2);
        }


        public virtual SavedData LoadItemData(Type class1, string string2)
        {
            return this.mapStorage.LoadData(class1, string2);
        }


        public virtual int GetUniqueDataId(string string1)
        {
            return this.mapStorage.GetUniqueDataId(string1);
        }


        public virtual void LevelEvent(LevelEventType type, int x, int y, int z, int data)
        {
            this.LevelEvent(null, type, x, y, z, data);
        }


        public virtual void LevelEvent(Player player, LevelEventType type, int x, int y, int z, int data)
        {
            for (int i7 = 0; i7 < this.levelListeners.Count; ++i7)
            {
                this.levelListeners[i7].LevelEvent(player, type, x, y, z, data);
            }
        }
    }
}