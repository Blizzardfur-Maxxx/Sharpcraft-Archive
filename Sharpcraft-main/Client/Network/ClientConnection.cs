using SharpCraft.Client.Gamemode;
using SharpCraft.Client.GUI.Screens.Multiplayer;
using SharpCraft.Client.Players;
using SharpCraft.Core;
using SharpCraft.Core.Network;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Entities.Weather;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.GameSavedData;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.Particles;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.Util;
using static SharpCraft.Core.World.Entities.SynchedEntityData;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Inventory;

namespace SharpCraft.Client.Network
{
    public class ClientConnection : PacketListener
    {
        private bool disconnected = false;
        private Connection connection;
        private Client mc;
        public string LoginStatus;
        private ClientLevel worldClient;
        public MapStorage MapStrg = new MapStorage(null);
        private bool hasMoved;
        private JRandom rand = new JRandom();
       

        public ClientConnection(Client mc, string ip, int port) 
        {
            this.mc = mc;
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            connection = new Connection(socket, "Client", this); 
        }

        public override bool IsServerPacketListener()
        {
            return false;
        }

        public virtual void AddToSendQueue(Packet packet)
        {
            if (disconnected) return;
            connection.AddToSendQueue(packet);
            connection.Interrupt();
        }

        public void SendQuitPacket(Packet packet)
        {
            if (disconnected) return;
            connection.AddToSendQueue(packet);
            connection.Shutdown();
        }

        public void Tick()
        {
            if (!disconnected) connection.Tick();
            connection.Interrupt();
        }

        public void Disconnect()
        {
            disconnected = true;
            connection.Interrupt();
            connection.CloseConnection("disconnect.closed");
        }

        public override void OnDisconnect(string reason, params object[] args)
        {
            if (disconnected) return;
            disconnected = true;
            mc.SetLevel(null);
            mc.SetScreen(new DisconnectionScreen("disconnect.lost", reason, args));
        }

        private Entity GetEntityByID(int id)
        {
            return id == this.mc.player.entityID ? this.mc.player : this.worldClient.GetEntityByID(id);
        }

        #region Packet handling
        public override void HandleLogin(Packet1Login packet1Login1)
        {
            mc.gameMode = new MultiPlayerGameMode(this.mc, this);
            mc.statFileWriter.ReadStat(StatList.joinMP, 1);
            worldClient = new ClientLevel(this, packet1Login1.mapSeed, packet1Login1.dimension);
            worldClient.isRemote = true;
            mc.SetLevel(this.worldClient);
            mc.player.dimension = packet1Login1.dimension;
            mc.SetScreen(new ReceivingLevelScreen(this));
            mc.player.entityID = packet1Login1.protocolVersion;
        }

        public override void HandleHandshake(Packet2Handshake packet)
        {
            if (packet.username.Equals("-"))
            {
                AddToSendQueue(new Packet1Login(mc.user.name, SharedConstants.PROTOCOL_VERSION));
            }
            else
            {
                try
                {
                    string url = string.Format(SharedConstants.JOINSERVER, mc.user.name, mc.user.sessionId, packet.username);
                    using HttpResponseMessage response = SharedConstants.HTTP_CLIENT.GetAsync(url).Result;
                    string str = response.Content.ReadAsStringAsync().Result.Trim();

                    if (str.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
                        AddToSendQueue(new Packet1Login(mc.user.name, SharedConstants.PROTOCOL_VERSION));
                    else
                        connection.CloseConnection("disconnect.loginFailedInfo", str);
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                    connection.CloseConnection("disconnect.genericReason", $"Internal client error: {ex}");
                }
            }
        }

        public override void HandlePickupSpawn(Packet21PickupSpawn packet21PickupSpawn1)
        {
            double d2 = packet21PickupSpawn1.xPosition / 32.0D;
            double d4 = packet21PickupSpawn1.yPosition / 32.0D;
            double d6 = packet21PickupSpawn1.zPosition / 32.0D;
            ItemEntity entityItem8 = new ItemEntity(this.worldClient, d2, d4, d6, new ItemInstance(packet21PickupSpawn1.itemID, packet21PickupSpawn1.count, packet21PickupSpawn1.itemDamage));
            entityItem8.motionX = packet21PickupSpawn1.rotation / 128.0D;
            entityItem8.motionY = packet21PickupSpawn1.pitch / 128.0D;
            entityItem8.motionZ = packet21PickupSpawn1.roll / 128.0D;
            entityItem8.serverPosX = packet21PickupSpawn1.xPosition;
            entityItem8.serverPosY = packet21PickupSpawn1.yPosition;
            entityItem8.serverPosZ = packet21PickupSpawn1.zPosition;
            this.worldClient.Func_712_a(packet21PickupSpawn1.entityId, entityItem8);
        }

        private static Entity GetEntity(Packet23VehicleSpawn pack, double x, double y, double z, ClientLevel worldClient) 
        {
            switch (pack.type) 
            {
                case SpawnObjectType.BOAT:
                    return new Boat(worldClient, x, y, z);
                case SpawnObjectType.MINECART_0:
                    return new Minecart(worldClient, x, y, z, 0);
                case SpawnObjectType.MINECART_1:
                    return new Minecart(worldClient, x, y, z, 1);
                case SpawnObjectType.MINECART_2:
                    return new Minecart(worldClient, x, y, z, 2);
                case SpawnObjectType.PRIMED_TNT:
                    return new PrimedTnt(worldClient, x, y, z);
                case SpawnObjectType.ARROW:
                    return new Arrow(worldClient, x, y, z);
                case SpawnObjectType.SNOWBALL:
                    return new Snowball(worldClient, x, y, z);
                case SpawnObjectType.THROWN_EGG:
                    return new ThrownEgg(worldClient, x, y, z);
                case SpawnObjectType.FIREBALL:
                    pack.field_i = 0;
                    return new Fireball(worldClient, x, y, z, pack.field_e / 8000.0D, pack.field_f / 8000.0D, pack.field_g / 8000.0D);
                case SpawnObjectType.SAND:
                    return new FallingTile(worldClient, x, y, z, Tile.sand.id);
                case SpawnObjectType.GRAVEL:
                    return new FallingTile(worldClient, x, y, z, Tile.gravel.id);
                case SpawnObjectType.FISHING_HOOK:
                    return new FishingHook(worldClient, x, y, z);
                default:
                    return null;
            }
        }

        public override void HandleVehicleSpawn(Packet23VehicleSpawn packet23VehicleSpawn1)
        {
            double x = packet23VehicleSpawn1.xPosition / 32.0D;
            double y = packet23VehicleSpawn1.yPosition / 32.0D;
            double z = packet23VehicleSpawn1.zPosition / 32.0D;
            Entity entity = GetEntity(packet23VehicleSpawn1, x, y, z, this.worldClient);

            if (entity != null)
            {
                entity.serverPosX = packet23VehicleSpawn1.xPosition;
                entity.serverPosY = packet23VehicleSpawn1.yPosition;
                entity.serverPosZ = packet23VehicleSpawn1.zPosition;
                entity.yaw = 0F;
                entity.pitch = 0F;
                entity.entityID = packet23VehicleSpawn1.entityId;
                this.worldClient.Func_712_a(packet23VehicleSpawn1.entityId, entity);
                if (packet23VehicleSpawn1.field_i > 0)
                {
                    if (packet23VehicleSpawn1.type == SpawnObjectType.ARROW)
                    {
                        Entity entity9 = this.GetEntityByID(packet23VehicleSpawn1.field_i);
                        if (entity9 is Mob)
                        {
                            ((Arrow)entity).owner = (Mob)entity9;
                        }
                    }

                    entity.SetVelocity(packet23VehicleSpawn1.field_e / 8000.0D, packet23VehicleSpawn1.field_f / 8000.0D, packet23VehicleSpawn1.field_g / 8000.0D);
                }
            }
        }

        public override void HandleWeather(Packet71Weather packet71Weather1)
        {
            double d2 = packet71Weather1.x / 32.0D;
            double d4 = packet71Weather1.y / 32.0D;
            double d6 = packet71Weather1.z / 32.0D;
            LightningBolt entityLightningBolt8 = null;
            if (packet71Weather1.type == 1)
            {
                entityLightningBolt8 = new LightningBolt(this.worldClient, d2, d4, d6);
            }

            if (entityLightningBolt8 != null)
            {
                entityLightningBolt8.serverPosX = packet71Weather1.x;
                entityLightningBolt8.serverPosY = packet71Weather1.y;
                entityLightningBolt8.serverPosZ = packet71Weather1.z;
                entityLightningBolt8.yaw = 0F;
                entityLightningBolt8.pitch = 0F;
                entityLightningBolt8.entityID = packet71Weather1.entityId;
                this.worldClient.AddWeatherEffect(entityLightningBolt8);
            }
        }

        public override void HandlePainting(Packet25EntityPainting packet25EntityPainting1)
        {
            Painting entityPainting2 = new Painting(this.worldClient, packet25EntityPainting1.xPosition, packet25EntityPainting1.yPosition, packet25EntityPainting1.zPosition, packet25EntityPainting1.direction, packet25EntityPainting1.title);
            this.worldClient.Func_712_a(packet25EntityPainting1.entityId, entityPainting2);
        }

        public override void HandleEntityVelocity(Packet28EntityVelocity packet28EntityVelocity1)
        {
            Entity entity2 = this.GetEntityByID(packet28EntityVelocity1.entityId);
            if (entity2 != null)
            {
                entity2.SetVelocity(packet28EntityVelocity1.motionX / 8000.0D, packet28EntityVelocity1.motionY / 8000.0D, packet28EntityVelocity1.motionZ / 8000.0D);
            }
        }

        public override void HandleEntityMetadata(Packet40EntityMetadata packet40EntityMetadata1)
        {
            Entity entity2 = this.GetEntityByID(packet40EntityMetadata1.entityId);
            if (entity2 != null && packet40EntityMetadata1.GetMetadata() != null)
            {
                entity2.GetDataWatcher().UpdateWatchedObjectsFromList(packet40EntityMetadata1.GetMetadata());
            }
        }

        public override void HandleNamedEntitySpawn(Packet20NamedEntitySpawn packet20NamedEntitySpawn1)
        {
            double d2 = packet20NamedEntitySpawn1.xPosition / 32.0D;
            double d4 = packet20NamedEntitySpawn1.yPosition / 32.0D;
            double d6 = packet20NamedEntitySpawn1.zPosition / 32.0D;
            float f8 = packet20NamedEntitySpawn1.rotation * 360 / 256F;
            float f9 = packet20NamedEntitySpawn1.pitch * 360 / 256F;
            RemotePlayer entityOtherPlayerMP10 = new RemotePlayer(this.mc.level, packet20NamedEntitySpawn1.name);
            entityOtherPlayerMP10.prevX = entityOtherPlayerMP10.lastTickPosX = entityOtherPlayerMP10.serverPosX = packet20NamedEntitySpawn1.xPosition;
            entityOtherPlayerMP10.prevY = entityOtherPlayerMP10.lastTickPosY = entityOtherPlayerMP10.serverPosY = packet20NamedEntitySpawn1.yPosition;
            entityOtherPlayerMP10.prevZ = entityOtherPlayerMP10.lastTickPosZ = entityOtherPlayerMP10.serverPosZ = packet20NamedEntitySpawn1.zPosition;
            int i11 = packet20NamedEntitySpawn1.currentItem;
            if (i11 == 0)
            {
                entityOtherPlayerMP10.inventory.mainInventory[entityOtherPlayerMP10.inventory.currentItem] = null;
            }
            else
            {
                entityOtherPlayerMP10.inventory.mainInventory[entityOtherPlayerMP10.inventory.currentItem] = new ItemInstance(i11, 1, 0);
            }

            entityOtherPlayerMP10.SetPositionAndRotation(d2, d4, d6, f8, f9);
            this.worldClient.Func_712_a(packet20NamedEntitySpawn1.entityId, entityOtherPlayerMP10);
        }

        public override void HandleEntityTeleport(Packet34EntityTeleport packet34EntityTeleport1)
        {
            Entity entity2 = this.GetEntityByID(packet34EntityTeleport1.entityId);
            if (entity2 != null)
            {
                entity2.serverPosX = packet34EntityTeleport1.xPosition;
                entity2.serverPosY = packet34EntityTeleport1.yPosition;
                entity2.serverPosZ = packet34EntityTeleport1.zPosition;
                double d3 = entity2.serverPosX / 32.0D;
                double d5 = entity2.serverPosY / 32.0D + 0.015625;
                double d7 = entity2.serverPosZ / 32.0D;
                float f9 = packet34EntityTeleport1.yaw * 360 / 256F;
                float f10 = packet34EntityTeleport1.pitch * 360 / 256F;
                entity2.SetPositionAndRotation2(d3, d5, d7, f9, f10, 3);
            }
        }

        public override void HandleEntity(Packet30Entity packet30Entity1)
        {
            Entity entity2 = this.GetEntityByID(packet30Entity1.entityId);
            if (entity2 != null)
            {
                entity2.serverPosX += packet30Entity1.xPosition;
                entity2.serverPosY += packet30Entity1.yPosition;
                entity2.serverPosZ += packet30Entity1.zPosition;
                double d3 = entity2.serverPosX / 32.0D;
                double d5 = entity2.serverPosY / 32.0D;
                double d7 = entity2.serverPosZ / 32.0D;
                float f9 = packet30Entity1.rotating ? packet30Entity1.yaw * 360 / 256F : entity2.yaw;
                float f10 = packet30Entity1.rotating ? packet30Entity1.pitch * 360 / 256F : entity2.pitch;
                entity2.SetPositionAndRotation2(d3, d5, d7, f9, f10, 3);
            }
        }

        public override void HandleDestroyEntity(Packet29DestroyEntity packet29DestroyEntity1)
        {
            this.worldClient.RemoveEntityFromWorld(packet29DestroyEntity1.entityId);
        }

        public override void HandleFlying(Packet10Flying packet10Flying1)
        {
            LocalPlayer entityPlayerSP2 = this.mc.player;
            double d3 = entityPlayerSP2.x;
            double d5 = entityPlayerSP2.y;
            double d7 = entityPlayerSP2.z;
            float f9 = entityPlayerSP2.yaw;
            float f10 = entityPlayerSP2.pitch;
            if (packet10Flying1.moving)
            {
                d3 = packet10Flying1.xPosition;
                d5 = packet10Flying1.yPosition;
                d7 = packet10Flying1.zPosition;
            }

            if (packet10Flying1.rotating)
            {
                f9 = packet10Flying1.yaw;
                f10 = packet10Flying1.pitch;
            }

            entityPlayerSP2.ySize = 0F;
            entityPlayerSP2.motionX = entityPlayerSP2.motionY = entityPlayerSP2.motionZ = 0;
            entityPlayerSP2.SetPositionAndRotation(d3, d5, d7, f9, f10);
            packet10Flying1.xPosition = entityPlayerSP2.x;
            packet10Flying1.yPosition = entityPlayerSP2.boundingBox.y0;
            packet10Flying1.zPosition = entityPlayerSP2.z;
            packet10Flying1.stance = entityPlayerSP2.y;
            connection.AddToSendQueue(packet10Flying1);

            if (!this.hasMoved)
            {
                this.mc.player.prevX = this.mc.player.x;
                this.mc.player.prevY = this.mc.player.y;
                this.mc.player.prevZ = this.mc.player.z;
                this.hasMoved = true;
                this.mc.SetScreen(null);
            }
        }

        public override void HandlePreChunk(Packet50PreChunk packet50PreChunk1)
        {
            this.worldClient.DoPreChunk(packet50PreChunk1.xPosition, packet50PreChunk1.yPosition, packet50PreChunk1.mode);
        }

        public override void HandleMultiBlockChange(Packet52MultiBlockChange packet52MultiBlockChange1)
        {
            LevelChunk chunk2 = this.worldClient.GetChunk(packet52MultiBlockChange1.xPosition, packet52MultiBlockChange1.zPosition);
            int i3 = packet52MultiBlockChange1.xPosition * 16;
            int i4 = packet52MultiBlockChange1.zPosition * 16;
            for (int i5 = 0; i5 < packet52MultiBlockChange1.size; ++i5)
            {
                short s6 = packet52MultiBlockChange1.coordinateArray[i5];
                int i7 = packet52MultiBlockChange1.typeArray[i5] & 255;
                byte b8 = packet52MultiBlockChange1.metadataArray[i5];
                int i9 = s6 >> 12 & 15;
                int i10 = s6 >> 8 & 15;
                int i11 = s6 & 255;
                chunk2.SetBlockIDWithMetadata(i9, i11, i10, i7, b8);
                this.worldClient.Func_711_c(i9 + i3, i11, i10 + i4, i9 + i3, i11, i10 + i4);
                this.worldClient.SetTilesDirty(i9 + i3, i11, i10 + i4, i9 + i3, i11, i10 + i4);
            }
        }

        public override void HandleMapChunk(Packet51MapChunk p)
        {
            this.worldClient.Func_711_c(p.xp, p.yp, p.zp, p.xp + p.xSize - 1, p.yp + p.ySize - 1, p.zp + p.zSize - 1);
            this.worldClient.SetChunkData(p.xp, p.yp, p.zp, p.xSize, p.ySize, p.zSize, p.chunk);
        }

        public override void HandleBlockChange(Packet53BlockChange packet53BlockChange1)
        {
            this.worldClient.Func_714_c(packet53BlockChange1.xPosition, packet53BlockChange1.yPosition, packet53BlockChange1.zPosition, packet53BlockChange1.type, packet53BlockChange1.metadata);
        }

        public override void HandleCollect(Packet22Collect packet22Collect1)
        {
            Entity entity2 = this.GetEntityByID(packet22Collect1.collectedEntityId);
            Entity object3 = this.GetEntityByID(packet22Collect1.collectorEntityId);
            if (object3 == null)
            {
                object3 = this.mc.player;
            }

            if (entity2 != null)
            {
                this.worldClient.PlaySound(entity2, "random.pop", 0.2F, ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.7F + 1F) * 2F);
                this.mc.effectRenderer.AddEffect(new EntityPickupFX(this.mc.level, entity2, object3, -0.5F));
                this.worldClient.RemoveEntityFromWorld(packet22Collect1.collectedEntityId);
            }
        }

        public override void HandleChat(Packet3Chat packet3Chat1)
        {
            this.mc.ingameGUI.AddChatMessage(packet3Chat1.message);
        }

        public override void HandleArmAnimation(Packet18Animation packet18Animation1)
        {
            Entity entity2 = this.GetEntityByID(packet18Animation1.entityId);
            if (entity2 != null)
            {
                Player entityPlayer3;
                if (packet18Animation1.animate == EntityAnimationType.SWING)
                {
                    entityPlayer3 = (Player)entity2;
                    entityPlayer3.SwingItem();
                }
                else if (packet18Animation1.animate == EntityAnimationType.HURT)
                {
                    entity2.PerformHurtAnimation();
                }
                else if (packet18Animation1.animate == EntityAnimationType.WAKE_UP)
                {
                    entityPlayer3 = (Player)entity2;
                    entityPlayer3.WakeUpPlayer(false, false, false);
                }
                else if (packet18Animation1.animate == EntityAnimationType.UNKNOWN)
                {
                    entityPlayer3 = (Player)entity2;
                    entityPlayer3.Fun_o();
                }
            }
        }

        public override void HandleSleep(Packet17Sleep packet17Sleep1)
        {
            Entity entity2 = this.GetEntityByID(packet17Sleep1.field_a);
            if (entity2 != null)
            {
                if (packet17Sleep1.field_e == 0)
                {
                    Player entityPlayer3 = (Player)entity2;
                    entityPlayer3.SleepInBedAt(packet17Sleep1.field_b, packet17Sleep1.field_c, packet17Sleep1.field_d);
                }
            }
        }

        public override void HandleMobSpawn(Packet24MobSpawn packet24MobSpawn1)
        {
            double d2 = packet24MobSpawn1.xPosition / 32.0D;
            double d4 = packet24MobSpawn1.yPosition / 32.0D;
            double d6 = packet24MobSpawn1.zPosition / 32.0D;
            float f8 = packet24MobSpawn1.yaw * 360 / 256F;
            float f9 = packet24MobSpawn1.pitch * 360 / 256F;
            Mob entityLiving10 = (Mob)EntityFactory.CreateEntity(packet24MobSpawn1.type, this.mc.level);
            entityLiving10.serverPosX = packet24MobSpawn1.xPosition;
            entityLiving10.serverPosY = packet24MobSpawn1.yPosition;
            entityLiving10.serverPosZ = packet24MobSpawn1.zPosition;
            entityLiving10.entityID = packet24MobSpawn1.entityId;
            entityLiving10.SetPositionAndRotation(d2, d4, d6, f8, f9);
            entityLiving10.isMultiplayerEntity = true;
            this.worldClient.Func_712_a(packet24MobSpawn1.entityId, entityLiving10);
            IList<DataItem> list11 = packet24MobSpawn1.GetMetadata();
            if (list11 != null)
            {
                entityLiving10.GetDataWatcher().UpdateWatchedObjectsFromList(list11);
            }
        }

        public override void HandleUpdateTime(Packet4UpdateTime packet4UpdateTime1)
        {
            this.mc.level.SetTime(packet4UpdateTime1.time);
        }

        public override void HandleSpawnPosition(Packet6SpawnPosition packet6SpawnPosition1)
        {
            this.mc.player.SetSpawn(new Pos(packet6SpawnPosition1.xPosition, packet6SpawnPosition1.yPosition, packet6SpawnPosition1.zPosition));
            this.mc.level.GetLevelData().SetSpawn(packet6SpawnPosition1.xPosition, packet6SpawnPosition1.yPosition, packet6SpawnPosition1.zPosition);
        }

        public override void HandleAttachEntity(Packet39AttachEntity packet39AttachEntity1)
        {
            Entity object2 = this.GetEntityByID(packet39AttachEntity1.entityId);
            Entity entity3 = this.GetEntityByID(packet39AttachEntity1.vehicleEntityId);
            if (packet39AttachEntity1.entityId == this.mc.player.entityID)
            {
                object2 = this.mc.player;
            }

            if (object2 != null)
            {
                object2.MountEntity(entity3);
            }
        }

        public override void HandleEntityStatus(Packet38EntityHealth packet38EntityStatus1)
        {
            Entity entity2 = this.GetEntityByID(packet38EntityStatus1.entityId);
            if (entity2 != null)
            {
                entity2.HandleHealthUpdate(packet38EntityStatus1.entityStatus);
            }
        }

        public override void HandleHealth(Packet8UpdateHealth packet8UpdateHealth1)
        {
            this.mc.player.SetHealth(packet8UpdateHealth1.healthMP);
        }

        public override void HandleRespawn(Packet9Respawn packet9Respawn1)
        {
            if (packet9Respawn1.dimension != this.mc.player.dimension)
            {
                this.hasMoved = false;
                this.worldClient = new ClientLevel(this, this.worldClient.GetLevelData().GetRandomSeed(), packet9Respawn1.dimension);
                this.worldClient.isRemote = true;
                this.mc.SetLevel(this.worldClient);
                this.mc.player.dimension = packet9Respawn1.dimension;
                this.mc.SetScreen(new ReceivingLevelScreen(this));
            }

            this.mc.Respawn(true, packet9Respawn1.dimension);
        }

        public override void HandleExplosion(Packet60Explosion packet60Explosion1)
        {
            Explosion explosion2 = new Explosion(this.mc.level, (Entity)null, packet60Explosion1.explosionX, packet60Explosion1.explosionY, packet60Explosion1.explosionZ, packet60Explosion1.explosionSize);
            explosion2.destroyedBlockPositions = packet60Explosion1.destroyedBlockPositions;
            explosion2.FinalizeExplosion(true);
        }

        public override void HandleOpenWindow(Packet100OpenWindow packet100OpenWindow1)
        {
            if (packet100OpenWindow1.inventoryType == InventoryType.GENERIC_INVENTORY)
            {
                SimpleContainer inventoryBasic2 = new SimpleContainer(packet100OpenWindow1.windowTitle, packet100OpenWindow1.slotsCount);
                this.mc.player.DisplayGUIChest(inventoryBasic2);
                this.mc.player.curCraftingInventory.windowId = packet100OpenWindow1.windowId;
            }
            else if (packet100OpenWindow1.inventoryType == InventoryType.FURNACE)
            {
                TileEntityFurnace tileEntityFurnace3 = new TileEntityFurnace();
                this.mc.player.DisplayGUIFurnace(tileEntityFurnace3);
                this.mc.player.curCraftingInventory.windowId = packet100OpenWindow1.windowId;
            }
            else if (packet100OpenWindow1.inventoryType == InventoryType.DISPENSER)
            {
                TileEntityDispenser tileEntityDispenser4 = new TileEntityDispenser();
                this.mc.player.DisplayGUIDispenser(tileEntityDispenser4);
                this.mc.player.curCraftingInventory.windowId = packet100OpenWindow1.windowId;
            }
            else if (packet100OpenWindow1.inventoryType == InventoryType.WORKBENCH)
            {
                LocalPlayer entityPlayerSP5 = this.mc.player;
                this.mc.player.DisplayWorkbenchGUI(Mth.Floor(entityPlayerSP5.x), Mth.Floor(entityPlayerSP5.y), Mth.Floor(entityPlayerSP5.z));
                this.mc.player.curCraftingInventory.windowId = packet100OpenWindow1.windowId;
            }
        }

        public override void HandleSetSlot(Packet103SetSlot packet103SetSlot1)
        {
            if (packet103SetSlot1.windowId == -1)
            {
                this.mc.player.inventory.SetItem(packet103SetSlot1.myItemStack);
            }
            else if (packet103SetSlot1.windowId == 0 && packet103SetSlot1.itemSlot >= 36 && packet103SetSlot1.itemSlot < 45)
            {
                ItemInstance itemStack2 = this.mc.player.inventorySlots.GetSlot(packet103SetSlot1.itemSlot).GetItem();
                if (packet103SetSlot1.myItemStack != null && (itemStack2 == null || itemStack2.stackSize < packet103SetSlot1.myItemStack.stackSize))
                {
                    packet103SetSlot1.myItemStack.animationsToGo = 5;
                }

                this.mc.player.inventorySlots.PutStackInSlot(packet103SetSlot1.itemSlot, packet103SetSlot1.myItemStack);
            }
            else if (packet103SetSlot1.windowId == this.mc.player.curCraftingInventory.windowId)
            {
                this.mc.player.curCraftingInventory.PutStackInSlot(packet103SetSlot1.itemSlot, packet103SetSlot1.myItemStack);
            }
        }

        public override void HandleTransaction(Packet106Transaction packet106Transaction1)
        {
            AbstractContainerMenu container2 = null;
            if (packet106Transaction1.windowId == 0)
            {
                container2 = this.mc.player.inventorySlots;
            }
            else if (packet106Transaction1.windowId == this.mc.player.curCraftingInventory.windowId)
            {
                container2 = this.mc.player.curCraftingInventory;
            }

            if (container2 != null)
            {
                if (packet106Transaction1.field)
                {
                    container2.Func_20113(packet106Transaction1.shortWindowId);
                }
                else
                {
                    container2.Func_20110(packet106Transaction1.shortWindowId);
                    this.AddToSendQueue(new Packet106Transaction(packet106Transaction1.windowId, packet106Transaction1.shortWindowId, true));
                }
            }
        }

        public override void HandleWindowItems(Packet104WindowItems packet104WindowItems1)
        {
            if (packet104WindowItems1.windowId == 0)
            {
                this.mc.player.inventorySlots.PutStacksInSlots(packet104WindowItems1.items);
            }
            else if (packet104WindowItems1.windowId == this.mc.player.curCraftingInventory.windowId)
            {
                this.mc.player.curCraftingInventory.PutStacksInSlots(packet104WindowItems1.items);
            }
        }

        public override void HandleSignUpdate(Packet130UpdateSign packet130UpdateSign1)
        {
            if (this.mc.level.HasChunkAt(packet130UpdateSign1.xPosition, packet130UpdateSign1.yPosition, packet130UpdateSign1.zPosition))
            {
                TileEntity tileEntity2 = this.mc.level.GetTileEntity(packet130UpdateSign1.xPosition, packet130UpdateSign1.yPosition, packet130UpdateSign1.zPosition);
                if (tileEntity2 is TileEntitySign)
                {
                    TileEntitySign tileEntitySign3 = (TileEntitySign)tileEntity2;
                    for (int i4 = 0; i4 < 4; ++i4)
                    {
                        tileEntitySign3.signText[i4] = packet130UpdateSign1.signLines[i4];
                    }

                    tileEntitySign3.SetChanged();
                }
            }
        }

        public override void HandleUpdateProgress(Packet105UpdateProgressbar packet105UpdateProgressbar1)
        {
            this.OnUnhandledPacket(packet105UpdateProgressbar1);
            if (this.mc.player.curCraftingInventory != null && this.mc.player.curCraftingInventory.windowId == packet105UpdateProgressbar1.windowId)
            {
                this.mc.player.curCraftingInventory.Func_20112(packet105UpdateProgressbar1.progressBar, packet105UpdateProgressbar1.progressBarValue);
            }
        }

        public override void HandlePlayerInventory(Packet5PlayerInventory packet5PlayerInventory1)
        {
            Entity entity2 = this.GetEntityByID(packet5PlayerInventory1.entityID);
            if (entity2 != null)
            {
                entity2.OutfitWithItem(packet5PlayerInventory1.slot, packet5PlayerInventory1.itemID, packet5PlayerInventory1.itemDamage);
            }
        }

        public override void HandleCloseWindow(Packet101CloseWindow packet101CloseWindow1)
        {
            this.mc.player.AccessorCloseScreen();
        }

        public override void HandleNotePlay(Packet54PlayNoteBlock packet54PlayNoteBlock1)
        {
            this.mc.level.TileEvent(packet54PlayNoteBlock1.xLocation, packet54PlayNoteBlock1.yLocation, packet54PlayNoteBlock1.zLocation, packet54PlayNoteBlock1.instrumentType, packet54PlayNoteBlock1.pitch);
        }

        public override void HandleBed(Packet70Bed packet70Bed1)
        {
            int i2 = packet70Bed1.b_ReadByte;
            if (i2 >= 0 && i2 < Packet70Bed.tile_bed_notValid.Length && Packet70Bed.tile_bed_notValid[i2] != null)
            {
                this.mc.player.AddChatMessage(Packet70Bed.tile_bed_notValid[i2]);
            }

            if (i2 == 1)
            {
                this.worldClient.GetLevelData().SetRaining(true);
                this.worldClient.SetRainStrength(1F);
            }
            else if (i2 == 2)
            {
                this.worldClient.GetLevelData().SetRaining(false);
                this.worldClient.SetRainStrength(0F);
            }
        }

        public override void HandleMapData(Packet131MapData packet131MapData1)
        {
            if (packet131MapData1.xpos == Item.mapItem.id)
            {
                ItemMap.GetShortMapData(packet131MapData1.ypos, this.mc.level).SetData(packet131MapData1.data);
            }
            else
            {
                Console.WriteLine("Unknown itemid: " + packet131MapData1.ypos);
            }
        }

        public override void HandleWorldEvent(Packet61WorldEvent packet61DoorChange1)
        {
            this.mc.level.LevelEvent(packet61DoorChange1.type, packet61DoorChange1.x, packet61DoorChange1.y, packet61DoorChange1.z, packet61DoorChange1.data);
        }

        public override void HandleStatistic(Packet200Statistic packet200Statistic1)
        {
            ((MultiplayerLocalPlayer)this.mc.player).Func_27027_b(StatList.GetStat(packet200Statistic1.statId), packet200Statistic1.statCount);
        }

        public override void HandleKickDisconnect(Packet255KickDisconnect packet)
        {
            connection.CloseConnection("disconnect.kicked");
            disconnected = true;
            mc.SetLevel(null);
            mc.SetScreen(new DisconnectionScreen("disconnect.disconnected", "disconnect.genericReason", packet.reason));
        }
        #endregion
    }
}
