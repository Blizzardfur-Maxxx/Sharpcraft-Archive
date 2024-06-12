using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Server.Levell.Chonk;

namespace SharpCraft.Server.Levell
{
    public class ServerLevel : Level
    {
        public IChunkCacheAdapter serverChunkCache;
        public bool saveDisabled;
        public bool spawnProtected = false;
        private Server mcServer;
        private IntHashMap<Entity> entityMap = new ();
        public ServerLevel(Server srv, ILevelStorage storage, string levelname, int dim, long seed) : base(storage, levelname, seed, Dimension.GetNew(dim))
        {
            this.mcServer = srv;
        }

        public override void UpdateEntityWithOptionalForce(Entity entity1, bool z2)
        {
            if (!this.mcServer.spawnPeacefulMobs && (entity1 is Animal || entity1 is WaterCreature))
            {
                entity1.SetEntityDead();
            }

            if (entity1.riddenByEntity == null || !(entity1.riddenByEntity is Player))
            {
                base.UpdateEntityWithOptionalForce(entity1, z2);
            }
        }

        public virtual void Func_12017_b(Entity entity1, bool z2)
        {
            base.UpdateEntityWithOptionalForce(entity1, z2);
        }

        protected override IChunkSource CreateChunkSource()
        {
            IChunkStorage storag = this.levelStorage.CreateChunkStorage(this.dimension);
            IChunkCacheAdapter cca = null;
            if (Enhancements.FIX_CHUNK_CACHE_MEM_LEAK)
            {
                cca = new ImprovedDedicatedServerChunkCache(this, storag, this.dimension.CreateRandomLevelSource());
            }
            else
            {
                cca = new DedicatedServerChunkCache(this, storag, this.dimension.CreateRandomLevelSource());
            }

            this.serverChunkCache = cca;
            return this.serverChunkCache;
        }

        public virtual IList<TileEntity> GetTileEntityList(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            List<TileEntity> arrayList7 = new ();
            for (int i8 = 0; i8 < this.loadedTileEntityList.Count; ++i8)
            {
                TileEntity tileEntity9 = this.loadedTileEntityList[i8];
                if (tileEntity9.xCoord >= i1 && tileEntity9.yCoord >= i2 && tileEntity9.zCoord >= i3 && tileEntity9.xCoord < i4 && tileEntity9.yCoord < i5 && tileEntity9.zCoord < i6)
                {
                    arrayList7.Add(tileEntity9);
                }
            }

            return arrayList7;
        }

        public override bool CanMineBlock(Player entityPlayer1, int i2, int i3, int i4)
        {
            int i5 = (int)Mth.Abs(i2 - this.levelData.GetSpawnX());
            int i6 = (int)Mth.Abs(i4 - this.levelData.GetSpawnZ());
            if (i5 > i6)
            {
                i6 = i5;
            }

            return i6 > mcServer.spawnProtectionRadius || this.mcServer.configManager.IsOp(entityPlayer1.username);
        }

        protected override void EntityAdded(Entity entity1)
        {
            base.EntityAdded(entity1);
            this.entityMap.Put(entity1.entityID, entity1);
        }

        protected override void EntityRemoved(Entity entity1)
        {
            base.EntityRemoved(entity1);
            this.entityMap.Remove(entity1.entityID);
        }

        public virtual Entity Func_6158_a(int i1)
        {
            return this.entityMap.Get(i1);
        }

        public override bool AddWeatherEffect(Entity entity1)
        {
            if (base.AddWeatherEffect(entity1))
            {
                this.mcServer.configManager.SendPacketToPlayersAroundPoint(entity1.x, entity1.y, entity1.z, 512, this.dimension.dimension, new Packet71Weather(entity1));
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void SendTrackedEntityStatusUpdatePacket(Entity entity1, byte b2)
        {
            Packet38EntityHealth packet38EntityStatus3 = new Packet38EntityHealth(entity1.entityID, b2);
            this.mcServer.GetEntityTracker(this.dimension.dimension).SendPacketToTrackedPlayersAndTrackedEntity(entity1, packet38EntityStatus3);
        }

        public override Explosion Explode(Entity entity1, double d2, double d4, double d6, float f8, bool z9)
        {
            Explosion explosion10 = new Explosion(this, entity1, d2, d4, d6, f8);
            explosion10.isFlaming = z9;
            explosion10.Explode();
            explosion10.FinalizeExplosion(false);
            this.mcServer.configManager.SendPacketToPlayersAroundPoint(d2, d4, d6, 64, this.dimension.dimension, new Packet60Explosion(d2, d4, d6, f8, explosion10.destroyedBlockPositions));
            return explosion10;
        }

        public override void TileEvent(int i1, int i2, int i3, int i4, int i5)
        {
            base.TileEvent(i1, i2, i3, i4, i5);
            this.mcServer.configManager.SendPacketToPlayersAroundPoint(i1, i2, i3, 64, this.dimension.dimension, new Packet54PlayNoteBlock(i1, i2, i3, i4, i5));
        }

        public virtual void CloseAll()
        {
            this.levelStorage.CloseAll();
        }

        protected override void TickWeather()
        {
            bool z1 = this.Func_C();
            base.TickWeather();
            if (z1 != this.Func_C())
            {
                if (z1)
                {
                    this.mcServer.configManager.SendPacketToAllPlayers(new Packet70Bed(2));
                }
                else
                {
                    this.mcServer.configManager.SendPacketToAllPlayers(new Packet70Bed(1));
                }
            }
        }
    }
}
