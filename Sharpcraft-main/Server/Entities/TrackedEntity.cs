using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.Network;

namespace SharpCraft.Server.Entities
{
    public class TrackedEntity
    {
        public Entity trackedEntity;
        public int trackingDistanceThreshold;
        public int field_9234_e;
        public int encodedPosX;
        public int encodedPosY;
        public int encodedPosZ;
        public int encodedRotationYaw;
        public int encodedRotationPitch;
        public double lastTrackedEntityMotionX;
        public double lastTrackedEntityMotionY;
        public double lastTrackedEntityMotionZ;
        public int updateCounter = 0;
        private double lastTrackedEntityPosX;
        private double lastTrackedEntityPosY;
        private double lastTrackedEntityPosZ;
        private bool firstUpdateDone = false;
        private bool shouldSendMotionUpdates;
        private int field_28165_t = 0;
        public bool playerEntitiesUpdated = false;
        public HashSet<ServerPlayer> trackedPlayers = new HashSet<ServerPlayer>();
        public TrackedEntity(Entity entity1, int i2, int i3, bool z4)
        {
            this.trackedEntity = entity1;
            this.trackingDistanceThreshold = i2;
            this.field_9234_e = i3;
            this.shouldSendMotionUpdates = z4;
            this.encodedPosX = Mth.Floor(entity1.x * 32);
            this.encodedPosY = Mth.Floor(entity1.y * 32);
            this.encodedPosZ = Mth.Floor(entity1.z * 32);
            this.encodedRotationYaw = Mth.Floor(entity1.yaw * 256F / 360F);
            this.encodedRotationPitch = Mth.Floor(entity1.pitch * 256F / 360F);
        }

        public override bool Equals(object object1)
        {
            return object1 is TrackedEntity ? ((TrackedEntity)object1).trackedEntity.entityID == this.trackedEntity.entityID : false;
        }

        public override int GetHashCode()
        {
            return this.trackedEntity.entityID;
        }

        public virtual void UpdatePlayerList(IList<Player> list1)
        {
            this.playerEntitiesUpdated = false;
            if (!this.firstUpdateDone || this.trackedEntity.GetDistanceSq(this.lastTrackedEntityPosX, this.lastTrackedEntityPosY, this.lastTrackedEntityPosZ) > 16)
            {
                this.lastTrackedEntityPosX = this.trackedEntity.x;
                this.lastTrackedEntityPosY = this.trackedEntity.y;
                this.lastTrackedEntityPosZ = this.trackedEntity.z;
                this.firstUpdateDone = true;
                this.playerEntitiesUpdated = true;
                this.UpdatePlayerEntities(list1);
            }

            ++this.field_28165_t;
            if (++this.updateCounter % this.field_9234_e == 0)
            {
                int i2 = Mth.Floor(this.trackedEntity.x * 32f);
                int i3 = Mth.Floor(this.trackedEntity.y * 32f);
                int i4 = Mth.Floor(this.trackedEntity.z * 32f);
                int i5 = Mth.Floor(this.trackedEntity.yaw * 256F / 360F);
                int i6 = Mth.Floor(this.trackedEntity.pitch * 256F / 360F);
                int i7 = i2 - this.encodedPosX;
                int i8 = i3 - this.encodedPosY;
                int i9 = i4 - this.encodedPosZ;
                Packet object10 = null;
                bool z11 = Math.Abs(i2) >= 8 || Math.Abs(i3) >= 8 || Math.Abs(i4) >= 8;
                bool z12 = Math.Abs(i5 - this.encodedRotationYaw) >= 8 || Math.Abs(i6 - this.encodedRotationPitch) >= 8;
                if (i7 >= -128 && i7 < 128 && i8 >= -128 && i8 < 128 && i9 >= -128 && i9 < 128 && this.field_28165_t <= 400)
                {
                    if (z11 && z12)
                    {
                        object10 = new Packet33RelEntityMoveLook(this.trackedEntity.entityID, (sbyte)i7, (sbyte)i8, (sbyte)i9, (sbyte)i5, (sbyte)i6);
                    }
                    else if (z11)
                    {
                        object10 = new Packet31RelEntityMove(this.trackedEntity.entityID, (sbyte)i7, (sbyte)i8, (sbyte)i9);
                    }
                    else if (z12)
                    {
                        object10 = new Packet32EntityLook(this.trackedEntity.entityID, (sbyte)i5, (sbyte)i6);
                    }
                }
                else
                {
                    this.field_28165_t = 0;
                    this.trackedEntity.x = i2 / 32d;
                    this.trackedEntity.y = i3 / 32d;
                    this.trackedEntity.z = i4 / 32d;
                    object10 = new Packet34EntityTeleport(this.trackedEntity.entityID, i2, i3, i4, (sbyte)i5, (sbyte)i6);
                }

                if (this.shouldSendMotionUpdates)
                {
                    double d13 = this.trackedEntity.motionX - this.lastTrackedEntityMotionX;
                    double d15 = this.trackedEntity.motionY - this.lastTrackedEntityMotionY;
                    double d17 = this.trackedEntity.motionZ - this.lastTrackedEntityMotionZ;
                    double d19 = 0.02;
                    double d21 = d13 * d13 + d15 * d15 + d17 * d17;
                    if (d21 > d19 * d19 || d21 > 0 && this.trackedEntity.motionX == 0 && this.trackedEntity.motionY == 0 && this.trackedEntity.motionZ == 0)
                    {
                        this.lastTrackedEntityMotionX = this.trackedEntity.motionX;
                        this.lastTrackedEntityMotionY = this.trackedEntity.motionY;
                        this.lastTrackedEntityMotionZ = this.trackedEntity.motionZ;
                        this.SendPacketToTrackedPlayers(new Packet28EntityVelocity(this.trackedEntity.entityID, this.lastTrackedEntityMotionX, this.lastTrackedEntityMotionY, this.lastTrackedEntityMotionZ));
                    }
                }

                if (object10 != null)
                {
                    this.SendPacketToTrackedPlayers(object10);
                }

                SynchedEntityData dataWatcher23 = this.trackedEntity.GetDataWatcher();
                if (dataWatcher23.HasObjectChanged())
                {
                    this.SendPacketToTrackedPlayersAndTrackedEntity(new Packet40EntityMetadata(this.trackedEntity.entityID, dataWatcher23));
                }

                if (z11)
                {
                    this.encodedPosX = i2;
                    this.encodedPosY = i3;
                    this.encodedPosZ = i4;
                }

                if (z12)
                {
                    this.encodedRotationYaw = i5;
                    this.encodedRotationPitch = i6;
                }
            }

            if (this.trackedEntity.beenAttacked)
            {
                this.SendPacketToTrackedPlayersAndTrackedEntity(new Packet28EntityVelocity(this.trackedEntity));
                this.trackedEntity.beenAttacked = false;
            }
        }

        public virtual void SendPacketToTrackedPlayers(Packet packet1)
        {
            IEnumerator<ServerPlayer> iterator2 = this.trackedPlayers.GetEnumerator();
            while (iterator2.MoveNext())
            {
                ServerPlayer entityPlayerMP3 = iterator2.Current;
                entityPlayerMP3.playerNetServerHandler.SendPacket(packet1);
            }
        }

        public virtual void SendPacketToTrackedPlayersAndTrackedEntity(Packet packet1)
        {
            this.SendPacketToTrackedPlayers(packet1);
            if (this.trackedEntity is ServerPlayer)
            {
                ((ServerPlayer)this.trackedEntity).playerNetServerHandler.SendPacket(packet1);
            }
        }

        public virtual void SendDestroyEntityPacketToTrackedPlayers()
        {
            this.SendPacketToTrackedPlayers(new Packet29DestroyEntity(this.trackedEntity.entityID));
        }

        public virtual void RemoveFromTrackedPlayers(ServerPlayer entityPlayerMP1)
        {
            if (this.trackedPlayers.Contains(entityPlayerMP1))
            {
                this.trackedPlayers.Remove(entityPlayerMP1);
            }
        }

        public virtual void UpdatePlayerEntity(ServerPlayer entityPlayerMP1)
        {
            if (entityPlayerMP1 != this.trackedEntity)
            {
                double d2 = entityPlayerMP1.x - this.encodedPosX / 32d;
                double d4 = entityPlayerMP1.z - this.encodedPosZ / 32d;
                if (d2 >= (-this.trackingDistanceThreshold) && d2 <= this.trackingDistanceThreshold && d4 >= (-this.trackingDistanceThreshold) && d4 <= this.trackingDistanceThreshold)
                {
                    if (!this.trackedPlayers.Contains(entityPlayerMP1))
                    {
                        this.trackedPlayers.Add(entityPlayerMP1);
                        entityPlayerMP1.playerNetServerHandler.SendPacket(this.GetAddEntityPacket());
                        if (this.shouldSendMotionUpdates)
                        {
                            entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet28EntityVelocity(this.trackedEntity.entityID, this.trackedEntity.motionX, this.trackedEntity.motionY, this.trackedEntity.motionZ));
                        }

                        ItemInstance[] itemStack6 = this.trackedEntity.GetInventory();
                        if (itemStack6 != null)
                        {
                            for (int i7 = 0; i7 < itemStack6.Length; ++i7)
                            {
                                entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet5PlayerInventory(this.trackedEntity.entityID, i7, itemStack6[i7]));
                            }
                        }

                        if (this.trackedEntity is Player)
                        {
                            Player entityPlayer8 = (Player)this.trackedEntity;
                            if (entityPlayer8.IsSleeping())
                            {
                                entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet17Sleep(this.trackedEntity, 0, Mth.Floor(this.trackedEntity.x), Mth.Floor(this.trackedEntity.y), Mth.Floor(this.trackedEntity.z)));
                            }
                        }
                    }
                }
                else if (this.trackedPlayers.Contains(entityPlayerMP1))
                {
                    this.trackedPlayers.Remove(entityPlayerMP1);
                    entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet29DestroyEntity(this.trackedEntity.entityID));
                }
            }
        }

        public virtual void UpdatePlayerEntities(IList<Player> list1)
        {
            for (int i2 = 0; i2 < list1.Count; ++i2)
            {
                this.UpdatePlayerEntity((ServerPlayer)list1[i2]);
            }
        }

        private Packet GetAddEntityPacket()
        {
            if (this.trackedEntity is ItemEntity)
            {
                ItemEntity entityItem6 = (ItemEntity)this.trackedEntity;
                Packet21PickupSpawn packet21PickupSpawn7 = new Packet21PickupSpawn(entityItem6);
                entityItem6.x = packet21PickupSpawn7.xPosition / 32d;
                entityItem6.y = packet21PickupSpawn7.yPosition / 32d;
                entityItem6.z = packet21PickupSpawn7.zPosition / 32d;
                return packet21PickupSpawn7;
            }
            else if (this.trackedEntity is ServerPlayer)
            {
                return new Packet20NamedEntitySpawn((Player)this.trackedEntity);
            }
            else
            {
                if (this.trackedEntity is Minecart)
                {
                    Minecart entityMinecart1 = (Minecart)this.trackedEntity;
                    if (entityMinecart1.minecartType == 0)
                    {
                        return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.MINECART_0);
                    }

                    if (entityMinecart1.minecartType == 1)
                    {
                        return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.MINECART_1);
                    }

                    if (entityMinecart1.minecartType == 2)
                    {
                        return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.MINECART_2);
                    }
                }

                if (this.trackedEntity is Boat)
                {
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.BOAT);
                }
                else if (this.trackedEntity is IAnimals)
                {
                    return new Packet24MobSpawn((Mob)this.trackedEntity);
                }
                else if (this.trackedEntity is FishingHook)
                {
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.FISHING_HOOK);
                }
                else if (this.trackedEntity is Arrow)
                {
                    Mob entityLiving5 = ((Arrow)this.trackedEntity).owner;
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.ARROW, entityLiving5 != null ? entityLiving5.entityID : this.trackedEntity.entityID);
                }
                else if (this.trackedEntity is Snowball)
                {
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.SNOWBALL);
                }
                else if (this.trackedEntity is Fireball)
                {
                    Fireball entityFireball4 = (Fireball)this.trackedEntity;
                    Packet23VehicleSpawn packet23VehicleSpawn2 = new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.FIREBALL, ((Fireball)this.trackedEntity).owner.entityID);
                    packet23VehicleSpawn2.field_e = (int)(entityFireball4.xOff * 8000d);
                    packet23VehicleSpawn2.field_f = (int)(entityFireball4.yOff * 8000d);
                    packet23VehicleSpawn2.field_g = (int)(entityFireball4.zOff * 8000d);
                    return packet23VehicleSpawn2;
                }
                else if (this.trackedEntity is ThrownEgg)
                {
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.THROWN_EGG);
                }
                else if (this.trackedEntity is PrimedTnt)
                {
                    return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.PRIMED_TNT);
                }
                else
                {
                    if (this.trackedEntity is FallingTile)
                    {
                        FallingTile entityFallingSand3 = (FallingTile)this.trackedEntity;
                        if (entityFallingSand3.blockID == Tile.sand.id)
                        {
                            return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.SAND);
                        }

                        if (entityFallingSand3.blockID == Tile.gravel.id)
                        {
                            return new Packet23VehicleSpawn(this.trackedEntity, SpawnObjectType.GRAVEL);
                        }
                    }

                    if (this.trackedEntity is Painting)
                    {
                        return new Packet25EntityPainting((Painting)this.trackedEntity);
                    }
                    else
                    {
                        throw new ArgumentException("Don't know how to add " + this.trackedEntity.GetType() + "!");
                    }
                }
            }
        }

        public virtual void RemoveTrackedPlayerSymmetric(ServerPlayer entityPlayerMP1)
        {
            if (this.trackedPlayers.Contains(entityPlayerMP1))
            {
                this.trackedPlayers.Remove(entityPlayerMP1);
                entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet29DestroyEntity(this.trackedEntity.entityID));
            }
        }
    }
}
