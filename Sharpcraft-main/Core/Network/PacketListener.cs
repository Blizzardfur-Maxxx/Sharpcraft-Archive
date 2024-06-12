using SharpCraft.Core.Network.Packets;
using System;

namespace SharpCraft.Core.Network
{
    public abstract class PacketListener
    {
        public abstract bool IsServerPacketListener();

        protected virtual void OnUnhandledPacket(Packet packet)
        {
        }

        public virtual void OnDisconnect(string reason, params object[] args)
        {
        }

        public virtual void HandleMapChunk(Packet51MapChunk packet51MapChunk1)
        {
            OnUnhandledPacket(packet51MapChunk1);
        }

        public virtual void HandleKickDisconnect(Packet255KickDisconnect packet255KickDisconnect1)
        {
            OnUnhandledPacket(packet255KickDisconnect1);
        }

        public virtual void HandleLogin(Packet1Login packet1Login1)
        {
            OnUnhandledPacket(packet1Login1);
        }

        public virtual void HandleFlying(Packet10Flying packet10Flying1)
        {
            OnUnhandledPacket(packet10Flying1);
        }

        public virtual void HandleMultiBlockChange(Packet52MultiBlockChange packet52MultiBlockChange1)
        {
            OnUnhandledPacket(packet52MultiBlockChange1);
        }

        public virtual void HandleBlockDig(Packet14BlockDig packet14BlockDig1)
        {
            OnUnhandledPacket(packet14BlockDig1);
        }

        public virtual void HandleBlockChange(Packet53BlockChange packet53BlockChange1)
        {
            OnUnhandledPacket(packet53BlockChange1);
        }

        public virtual void HandlePreChunk(Packet50PreChunk packet50PreChunk1)
        {
            OnUnhandledPacket(packet50PreChunk1);
        }

        public virtual void HandleNamedEntitySpawn(Packet20NamedEntitySpawn packet20NamedEntitySpawn1)
        {
            OnUnhandledPacket(packet20NamedEntitySpawn1);
        }

        public virtual void HandleEntity(Packet30Entity packet30Entity1)
        {
            OnUnhandledPacket(packet30Entity1);
        }

        public virtual void HandleEntityTeleport(Packet34EntityTeleport packet34EntityTeleport1)
        {
            OnUnhandledPacket(packet34EntityTeleport1);
        }

        public virtual void HandlePlace(Packet15Place packet15Place1)
        {
            OnUnhandledPacket(packet15Place1);
        }

        public virtual void HandleBlockItemSwitch(Packet16BlockItemSwitch packet16BlockItemSwitch1)
        {
            OnUnhandledPacket(packet16BlockItemSwitch1);
        }

        public virtual void HandleDestroyEntity(Packet29DestroyEntity packet29DestroyEntity1)
        {
            OnUnhandledPacket(packet29DestroyEntity1);
        }

        public virtual void HandlePickupSpawn(Packet21PickupSpawn packet21PickupSpawn1)
        {
            OnUnhandledPacket(packet21PickupSpawn1);
        }

        public virtual void HandleCollect(Packet22Collect packet22Collect1)
        {
            OnUnhandledPacket(packet22Collect1);
        }

        public virtual void HandleChat(Packet3Chat packet3Chat1)
        {
            OnUnhandledPacket(packet3Chat1);
        }

        public virtual void HandleVehicleSpawn(Packet23VehicleSpawn packet23VehicleSpawn1)
        {
            OnUnhandledPacket(packet23VehicleSpawn1);
        }

        public virtual void HandleArmAnimation(Packet18Animation packet18Animation1)
        {
            OnUnhandledPacket(packet18Animation1);
        }

        public virtual void HandleEntityAction(Packet19EntityAction packet19EntityAction1)
        {
            OnUnhandledPacket(packet19EntityAction1);
        }

        public virtual void HandleHandshake(Packet2Handshake packet2Handshake1)
        {
            OnUnhandledPacket(packet2Handshake1);
        }

        public virtual void HandleMobSpawn(Packet24MobSpawn packet24MobSpawn1)
        {
            OnUnhandledPacket(packet24MobSpawn1);
        }

        public virtual void HandleUpdateTime(Packet4UpdateTime packet4UpdateTime1)
        {
            OnUnhandledPacket(packet4UpdateTime1);
        }

        public virtual void HandleSpawnPosition(Packet6SpawnPosition packet6SpawnPosition1)
        {
            OnUnhandledPacket(packet6SpawnPosition1);
        }

        public virtual void HandleEntityVelocity(Packet28EntityVelocity packet28EntityVelocity1)
        {
            OnUnhandledPacket(packet28EntityVelocity1);
        }

        public virtual void HandleEntityMetadata(Packet40EntityMetadata packet40EntityMetadata1)
        {
            OnUnhandledPacket(packet40EntityMetadata1);
        }

        public virtual void HandleAttachEntity(Packet39AttachEntity packet39AttachEntity1)
        {
            OnUnhandledPacket(packet39AttachEntity1);
        }

        public virtual void HandleUseEntity(Packet7UseEntity packet7UseEntity1)
        {
            OnUnhandledPacket(packet7UseEntity1);
        }

        public virtual void HandleEntityStatus(Packet38EntityHealth packet38EntityStatus1)
        {
            OnUnhandledPacket(packet38EntityStatus1);
        }

        public virtual void HandleHealth(Packet8UpdateHealth packet8UpdateHealth1)
        {
            OnUnhandledPacket(packet8UpdateHealth1);
        }

        public virtual void HandleRespawn(Packet9Respawn packet9Respawn1)
        {
            OnUnhandledPacket(packet9Respawn1);
        }

        public virtual void HandleExplosion(Packet60Explosion packet60Explosion1)
        {
            OnUnhandledPacket(packet60Explosion1);
        }

        public virtual void HandleOpenWindow(Packet100OpenWindow packet100OpenWindow1)
        {
            OnUnhandledPacket(packet100OpenWindow1);
        }

        public virtual void HandleCloseWindow(Packet101CloseWindow packet101CloseWindow1)
        {
            OnUnhandledPacket(packet101CloseWindow1);
        }

        public virtual void HandleWindowClick(Packet102WindowClick packet102WindowClick1)
        {
            OnUnhandledPacket(packet102WindowClick1);
        }

        public virtual void HandleSetSlot(Packet103SetSlot packet103SetSlot1)
        {
            OnUnhandledPacket(packet103SetSlot1);
        }

        public virtual void HandleWindowItems(Packet104WindowItems packet104WindowItems1)
        {
            OnUnhandledPacket(packet104WindowItems1);
        }

        public virtual void HandleSignUpdate(Packet130UpdateSign packet130UpdateSign1)
        {
            OnUnhandledPacket(packet130UpdateSign1);
        }

        public virtual void HandleUpdateProgress(Packet105UpdateProgressbar packet105UpdateProgressbar1)
        {
            OnUnhandledPacket(packet105UpdateProgressbar1);
        }

        public virtual void HandlePlayerInventory(Packet5PlayerInventory packet5PlayerInventory1)
        {
            OnUnhandledPacket(packet5PlayerInventory1);
        }

        public virtual void HandleTransaction(Packet106Transaction packet106Transaction1)
        {
            OnUnhandledPacket(packet106Transaction1);
        }

        public virtual void HandlePainting(Packet25EntityPainting packet25EntityPainting1)
        {
            OnUnhandledPacket(packet25EntityPainting1);
        }

        public virtual void HandleNotePlay(Packet54PlayNoteBlock packet54PlayNoteBlock1)
        {
            OnUnhandledPacket(packet54PlayNoteBlock1);
        }

        public virtual void HandleStatistic(Packet200Statistic packet200Statistic1)
        {
            OnUnhandledPacket(packet200Statistic1);
        }

        public virtual void HandleSleep(Packet17Sleep packet17Sleep1)
        {
            OnUnhandledPacket(packet17Sleep1);
        }

        public virtual void HandleMovementType(Packet27Position packet27Position1)
        {
            OnUnhandledPacket(packet27Position1);
        }

        public virtual void HandleBed(Packet70Bed packet70Bed1)
        {
            OnUnhandledPacket(packet70Bed1);
        }

        public virtual void HandleWeather(Packet71Weather packet71Weather1)
        {
            OnUnhandledPacket(packet71Weather1);
        }

        public virtual void HandleMapData(Packet131MapData packet131MapData1)
        {
            OnUnhandledPacket(packet131MapData1);
        }

        public virtual void HandleWorldEvent(Packet61WorldEvent packet61DoorChange1)
        {
            OnUnhandledPacket(packet61DoorChange1);
        }
    }
}