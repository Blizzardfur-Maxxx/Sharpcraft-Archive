using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Entities
{
    public class EntityTracker
    {
        private HashSet<TrackedEntity> trackedEntitySet = new HashSet<TrackedEntity>();
        private IntHashMap<TrackedEntity> trackedEntityHashTable = new ();
        private Server mcServer;
        private int maxTrackingDistanceThreshold;
        private int dimension;
        public EntityTracker(Server server, int dimension)
        {
            this.mcServer = server;
            this.dimension = dimension;
            this.maxTrackingDistanceThreshold = server.configManager.GetMaxTrackingDistance();
        }

        public virtual void TrackEntity(Entity entity1)
        {
            if (entity1 is ServerPlayer)
            {
                this.TrackEntity(entity1, 512, 2);
                ServerPlayer entityPlayerMP2 = (ServerPlayer)entity1;
                IEnumerator<TrackedEntity> iterator3 = this.trackedEntitySet.GetEnumerator();
                while (iterator3.MoveNext())
                {
                    TrackedEntity entityTrackerEntry4 = iterator3.Current;
                    if (entityTrackerEntry4.trackedEntity != entityPlayerMP2)
                    {
                        entityTrackerEntry4.UpdatePlayerEntity(entityPlayerMP2);
                    }
                }
            }
            else if (entity1 is FishingHook)
            {
                this.TrackEntity(entity1, 64, 5, true);
            }
            else if (entity1 is Arrow)
            {
                this.TrackEntity(entity1, 64, 20, false);
            }
            else if (entity1 is Fireball)
            {
                this.TrackEntity(entity1, 64, 10, false);
            }
            else if (entity1 is Snowball)
            {
                this.TrackEntity(entity1, 64, 10, true);
            }
            else if (entity1 is ThrownEgg)
            {
                this.TrackEntity(entity1, 64, 10, true);
            }
            else if (entity1 is ItemEntity)
            {
                this.TrackEntity(entity1, 64, 20, true);
            }
            else if (entity1 is Minecart)
            {
                this.TrackEntity(entity1, 160, 5, true);
            }
            else if (entity1 is Boat)
            {
                this.TrackEntity(entity1, 160, 5, true);
            }
            else if (entity1 is Squid)
            {
                this.TrackEntity(entity1, 160, 3, true);
            }
            else if (entity1 is IAnimals)
            {
                this.TrackEntity(entity1, 160, 3);
            }
            else if (entity1 is PrimedTnt)
            {
                this.TrackEntity(entity1, 160, 10, true);
            }
            else if (entity1 is FallingTile)
            {
                this.TrackEntity(entity1, 160, 20, true);
            }
            else if (entity1 is Painting)
            {
                this.TrackEntity(entity1, 160, int.MaxValue, false);
            }
        }

        public virtual void TrackEntity(Entity entity1, int i2, int i3)
        {
            this.TrackEntity(entity1, i2, i3, false);
        }

        public virtual void TrackEntity(Entity entity1, int i2, int i3, bool z4)
        {
            if (i2 > this.maxTrackingDistanceThreshold)
            {
                i2 = this.maxTrackingDistanceThreshold;
            }

            if (this.trackedEntityHashTable.ContainsKey(entity1.entityID))
            {
                throw new InvalidOperationException("Entity is already tracked!");
            }
            else
            {
                TrackedEntity entityTrackerEntry5 = new TrackedEntity(entity1, i2, i3, z4);
                this.trackedEntitySet.Add(entityTrackerEntry5);
                this.trackedEntityHashTable.Put(entity1.entityID, entityTrackerEntry5);
                entityTrackerEntry5.UpdatePlayerEntities(this.mcServer.GetLevel(this.dimension).playerEntities);
            }
        }

        public virtual void UntrackEntity(Entity entity1)
        {
            if (entity1 is ServerPlayer)
            {
                ServerPlayer entityPlayerMP2 = (ServerPlayer)entity1;
                IEnumerator<TrackedEntity> iterator3 = this.trackedEntitySet.GetEnumerator();
                while (iterator3.MoveNext())
                {
                    TrackedEntity entityTrackerEntry4 = iterator3.Current;
                    entityTrackerEntry4.RemoveFromTrackedPlayers(entityPlayerMP2);
                }
            }

            TrackedEntity entityTrackerEntry5 = this.trackedEntityHashTable.Remove(entity1.entityID);
            if (entityTrackerEntry5 != null)
            {
                this.trackedEntitySet.Remove(entityTrackerEntry5);
                entityTrackerEntry5.SendDestroyEntityPacketToTrackedPlayers();
            }
        }

        public virtual void Tick()
        {
            List<ServerPlayer> arrayList1 = new ();
            IEnumerator<TrackedEntity> iterator2 = this.trackedEntitySet.GetEnumerator();
            while (iterator2.MoveNext())
            {
                TrackedEntity entityTrackerEntry3 = iterator2.Current;
                entityTrackerEntry3.UpdatePlayerList(this.mcServer.GetLevel(this.dimension).playerEntities);
                if (entityTrackerEntry3.playerEntitiesUpdated && entityTrackerEntry3.trackedEntity is ServerPlayer)
                {
                    arrayList1.Add((ServerPlayer)entityTrackerEntry3.trackedEntity);
                }
            }

            for (int i6 = 0; i6 < arrayList1.Count; ++i6)
            {
                ServerPlayer entityPlayerMP7 = arrayList1[i6];
                IEnumerator<TrackedEntity> iterator4 = this.trackedEntitySet.GetEnumerator();
                while (iterator4.MoveNext())
                {
                    TrackedEntity entityTrackerEntry5 = iterator4.Current;
                    if (entityTrackerEntry5.trackedEntity != entityPlayerMP7)
                    {
                        entityTrackerEntry5.UpdatePlayerEntity(entityPlayerMP7);
                    }
                }
            }
        }

        public virtual void SendPacketToTrackedPlayers(Entity entity1, Packet packet2)
        {
            TrackedEntity entityTrackerEntry3 = this.trackedEntityHashTable.Get(entity1.entityID);
            if (entityTrackerEntry3 != null)
            {
                entityTrackerEntry3.SendPacketToTrackedPlayers(packet2);
            }
        }

        public virtual void SendPacketToTrackedPlayersAndTrackedEntity(Entity entity1, Packet packet2)
        {
            TrackedEntity entityTrackerEntry3 = this.trackedEntityHashTable.Get(entity1.entityID);
            if (entityTrackerEntry3 != null)
            {
                entityTrackerEntry3.SendPacketToTrackedPlayersAndTrackedEntity(packet2);
            }
        }

        public virtual void RemoveTrackedPlayerSymmetric(ServerPlayer entityPlayerMP1)
        {
            IEnumerator<TrackedEntity> iterator2 = this.trackedEntitySet.GetEnumerator();
            while (iterator2.MoveNext())
            {
                TrackedEntity entityTrackerEntry3 = iterator2.Current;
                entityTrackerEntry3.RemoveTrackedPlayerSymmetric(entityPlayerMP1);
            }
        }
    }
}
