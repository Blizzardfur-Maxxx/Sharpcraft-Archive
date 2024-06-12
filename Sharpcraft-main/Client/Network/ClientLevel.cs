using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.Util;
using SharpCraft.Client.Network;
using System.Collections.Generic;
using SharpCraft.Core;
using SharpCraft.Core.Network.Packets;

namespace SharpCraft.Client.Network
{
    public class ClientLevel : Level
    {
        private List<PosUpdate> positionChanges = new List<PosUpdate>();
        private ClientConnection sendQueue;
        private MultiplayerChunkCache chunkCache;
        private IntHashMap<Entity> field_1055_D = new IntHashMap<Entity>();
        private HashSet<Entity> field_20914_E = new HashSet<Entity>();
        private HashSet<Entity> field_1053_F = new HashSet<Entity>();

        public ClientLevel(ClientConnection netClientHandler1, long j2, int i4) : base(new MultiplayerLevelStorage(), "MpServer", Dimension.GetNew(i4), j2)
        {
            this.sendQueue = netClientHandler1;
            this.SetSpawnPos(new Pos(8, 64, 8));
            this.mapStorage = netClientHandler1.MapStrg;
        }

        public override void Tick()
        {
            this.SetTime(this.GetTime() + 1);
            int i1 = this.GetSkyDarken(1F);
            int i2;
            if (i1 != this.skyDarken)
            {
                this.skyDarken = i1;
                for (i2 = 0; i2 < this.levelListeners.Count; ++i2)
                {
                    this.levelListeners[i2].AllChanged();
                }
            }

            IEnumerator<Entity> enumerator = this.field_1053_F.GetEnumerator();
            for (i2 = 0; i2 < 10 && this.field_1053_F.Count > 0; ++i2)
            {
                if (!enumerator.MoveNext()) break;
                Entity entity3 = enumerator.Current;
                if (!this.loadedEntityList.Contains(entity3))
                {
                    this.AddEntity(entity3);
                }
            }

            this.sendQueue.Tick();
            for (i2 = 0; i2 < this.positionChanges.Count; ++i2)
            {
                PosUpdate pos = this.positionChanges[i2];
                if (--pos.counter == 0)
                {
                    base.SetTileAndDataNoUpdate(pos.x, pos.y, pos.z, pos.id, pos.data);
                    base.SendTileUpdated(pos.x, pos.y, pos.z);
                    this.positionChanges.RemoveAt(i2--);
                   
                }
            }
        }

        public virtual void Func_711_c(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            for (int i7 = 0; i7 < this.positionChanges.Count; ++i7)
            {
                PosUpdate worldBlockPositionType8 = positionChanges[i7];
                if (worldBlockPositionType8.x >= i1 && worldBlockPositionType8.y >= i2 && worldBlockPositionType8.z >= i3 && worldBlockPositionType8.x <= i4 && worldBlockPositionType8.y <= i5 && worldBlockPositionType8.z <= i6)
                {
                    this.positionChanges.RemoveAt(i7--);
                }
            }
        }

        protected override IChunkSource CreateChunkSource()
        {
            this.chunkCache = new MultiplayerChunkCache(this);
            return this.chunkCache;
        }

        public override void SetSpawnLocation()
        {
            this.SetSpawnPos(new Pos(8, 64, 8));
        }

        protected override void DoRandomUpdateTicks()
        {
        }

        public override void ScheduleBlockUpdate(int i1, int i2, int i3, int i4, int i5)
        {
        }

        public override bool TickUpdates(bool z1)
        {
            return false;
        }

        public virtual void DoPreChunk(int i1, int i2, bool z3)
        {
            if (z3)
            {
                this.chunkCache.Create(i1, i2);
            }
            else
            {
                this.chunkCache.Remove(i1, i2);
            }

            if (!z3)
            {
                this.SetTilesDirty(i1 * 16, 0, i2 * 16, i1 * 16 + 15, 128, i2 * 16 + 15);
            }
        }

        public override bool AddEntity(Entity entity1)
        {
            bool z2 = base.AddEntity(entity1);
            this.field_20914_E.Add(entity1);
            if (!z2)
            {
                this.field_1053_F.Add(entity1);
            }

            return z2;
        }

        public override void SetEntityDead(Entity entity1)
        {
            base.SetEntityDead(entity1);
            this.field_20914_E.Remove(entity1);
        }

        protected override void EntityAdded(Entity entity1)
        {
            base.EntityAdded(entity1);
            if (this.field_1053_F.Contains(entity1))
            {
                this.field_1053_F.Remove(entity1);
            }
        }

        protected override void EntityRemoved(Entity entity1)
        {
            base.EntityRemoved(entity1);
            if (this.field_20914_E.Contains(entity1))
            {
                this.field_1053_F.Add(entity1);
            }
        }

        public virtual void Func_712_a(int i1, Entity entity2)
        {
            Entity entity3 = this.GetEntityByID(i1);
            if (entity3 != null)
            {
                this.SetEntityDead(entity3);
            }

            this.field_20914_E.Add(entity2);
            entity2.entityID = i1;
            if (!this.AddEntity(entity2))
            {
                this.field_1053_F.Add(entity2);
            }

            this.field_1055_D.Put(i1, entity2);
        }

        public virtual Entity GetEntityByID(int i1)
        {
            return this.field_1055_D.Get(i1);
        }

        public virtual Entity RemoveEntityFromWorld(int i1)
        {
            Entity entity2 = this.field_1055_D.Remove(i1);
            if (entity2 != null)
            {
                this.field_20914_E.Remove(entity2);
                this.SetEntityDead(entity2);
            }

            return entity2;
        }

        public override bool SetDataNoUpdate(int i1, int i2, int i3, int i4)
        {
            int i5 = this.GetTile(i1, i2, i3);
            int i6 = this.GetData(i1, i2, i3);
            if (base.SetDataNoUpdate(i1, i2, i3, i4))
            {
                this.positionChanges.Add(new PosUpdate(i1, i2, i3, i5, i6));
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool SetTileAndDataNoUpdate(int i1, int i2, int i3, int i4, int i5)
        {
            int i6 = this.GetTile(i1, i2, i3);
            int i7 = this.GetData(i1, i2, i3);
            if (base.SetTileAndDataNoUpdate(i1, i2, i3, i4, i5))
            {
                this.positionChanges.Add(new PosUpdate(i1, i2, i3, i6, i7));
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool SetTileNoUpdate(int x, int y, int z, int i4)
        {
            int id = this.GetTile(x, y, z);
            int data = this.GetData(x, y, z);
            if (base.SetTileNoUpdate(x, y, z, i4))
            {
                this.positionChanges.Add(new PosUpdate(x, y, z, id, data));
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool Func_714_c(int i1, int i2, int i3, int i4, int i5)
        {
            this.Func_711_c(i1, i2, i3, i1, i2, i3);
            if (base.SetTileAndDataNoUpdate(i1, i2, i3, i4, i5))
            {
                this.NotifyBlockChange(i1, i2, i3, i4);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void SendQuittingDisconnectingPacket()
        {
            sendQueue.SendQuitPacket(new Packet255KickDisconnect("Quitting"));
        }

        protected override void TickWeather()
        {
            if (!this.dimension.hasNoSky)
            {
                if (this.field_F > 0)
                {
                    --this.field_F;
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

        private class PosUpdate
        {
            public int x;
            public int y;
            public int z;
            public int counter;
            public int id;
            public int data;

            public PosUpdate(int x, int y, int z, int id, int data)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.counter = 80;
                this.id = id;
                this.data = data;
            }
        }
    }
}