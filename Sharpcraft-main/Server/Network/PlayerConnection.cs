using SharpCraft.Core;
using SharpCraft.Core.Network;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Inventory;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using SharpCraft.Server.Commands;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Levell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.Util.Facing;
using SharpCraft.Core.i18n;

namespace SharpCraft.Server.Network
{
    public class PlayerConnection : PacketListener, ICommandSource
    {
        public static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        public Connection netManager;
        public bool connectionClosed = false;
        private Server mcServer;
        private ServerPlayer playerEntity;
        private int lastSendTime;
        private int curSendTime;
        private int playerInAirTime;
        //private bool field_22003_h;
        private double lastPosX;
        private double lastPosY;
        private double lastPosZ;
        private bool hasMoved = true;
        private Dictionary<int, short> windowTransactionMap = new Dictionary<int, short>();

        public PlayerConnection(Server srv, Connection networkManager2, ServerPlayer entityPlayerMP3)
        {
            this.mcServer = srv;
            this.netManager = networkManager2;
            networkManager2.SetPacketListener(this);
            this.playerEntity = entityPlayerMP3;
            entityPlayerMP3.playerNetServerHandler = this;
        }

        public void Tick()
        {
            //this.field_22003_h = false;
            this.netManager.Tick();
            if (this.lastSendTime - this.curSendTime > 20)
            {
                this.SendPacket(new Packet0KeepAlive());
            }
        }

        public virtual void KickPlayer(string string1)
        {
            this.playerEntity.Func_30002_A();
            this.SendPacket(new Packet255KickDisconnect(string1));
            this.netManager.Shutdown();
            this.mcServer.configManager.SendPacketToAllPlayers(new Packet3Chat("§e" + this.playerEntity.username + " left the game."));
            this.mcServer.configManager.PlayerLoggedOut(this.playerEntity);
            this.connectionClosed = true;
        }

        public override void HandleMovementType(Packet27Position packet27Position1)
        {
            this.playerEntity.SetMovementType(packet27Position1.GetStrafe(), packet27Position1.GetForward(), packet27Position1.Func_g(), packet27Position1.GetJump(), packet27Position1.GetPitch(), packet27Position1.GetYaw());
        }

        public override void HandleFlying(Packet10Flying packet10Flying1)
        {
            ServerLevel worldServer2 = this.mcServer.GetLevel(this.playerEntity.dimension);
            //this.field_22003_h = true;
            double d3;
            if (!this.hasMoved)
            {
                d3 = packet10Flying1.yPosition - this.lastPosY;
                if (packet10Flying1.xPosition == this.lastPosX && d3 * d3 < 0.01d && packet10Flying1.zPosition == this.lastPosZ)
                {
                    this.hasMoved = true;
                }
            }

            if (this.hasMoved)
            {
                double d5;
                double d7;
                double d9;
                double d13;
                if (this.playerEntity.ridingEntity != null)
                {
                    float f26 = this.playerEntity.yaw;
                    float f4 = this.playerEntity.pitch;
                    this.playerEntity.ridingEntity.UpdateRiderPosition();
                    d5 = this.playerEntity.x;
                    d7 = this.playerEntity.y;
                    d9 = this.playerEntity.z;
                    double d27 = 0;
                    d13 = 0;
                    if (packet10Flying1.rotating)
                    {
                        f26 = packet10Flying1.yaw;
                        f4 = packet10Flying1.pitch;
                    }

                    if (packet10Flying1.moving && packet10Flying1.yPosition == -999d && packet10Flying1.stance == -999d)
                    {
                        d27 = packet10Flying1.xPosition;
                        d13 = packet10Flying1.zPosition;
                    }

                    this.playerEntity.onGround = packet10Flying1.onGround;
                    this.playerEntity.OnUpdateEntity(true);
                    this.playerEntity.MoveEntity(d27, 0d, d13);
                    this.playerEntity.SetPositionAndRotation(d5, d7, d9, f26, f4);
                    this.playerEntity.motionX = d27;
                    this.playerEntity.motionZ = d13;
                    if (this.playerEntity.ridingEntity != null)
                    {
                        worldServer2.Func_12017_b(this.playerEntity.ridingEntity, true);
                    }

                    if (this.playerEntity.ridingEntity != null)
                    {
                        this.playerEntity.ridingEntity.UpdateRiderPosition();
                    }

                    this.mcServer.configManager.Func_613_b(this.playerEntity);
                    this.lastPosX = this.playerEntity.x;
                    this.lastPosY = this.playerEntity.y;
                    this.lastPosZ = this.playerEntity.z;
                    worldServer2.UpdateEntity(this.playerEntity);
                    return;
                }

                if (this.playerEntity.IsSleeping())
                {
                    this.playerEntity.OnUpdateEntity(true);
                    this.playerEntity.SetPositionAndRotation(this.lastPosX, this.lastPosY, this.lastPosZ, this.playerEntity.yaw, this.playerEntity.pitch);
                    worldServer2.UpdateEntity(this.playerEntity);
                    return;
                }

                d3 = this.playerEntity.y;
                this.lastPosX = this.playerEntity.x;
                this.lastPosY = this.playerEntity.y;
                this.lastPosZ = this.playerEntity.z;
                d5 = this.playerEntity.x;
                d7 = this.playerEntity.y;
                d9 = this.playerEntity.z;
                float f11 = this.playerEntity.yaw;
                float f12 = this.playerEntity.pitch;
                if (packet10Flying1.moving && packet10Flying1.yPosition == -999d && packet10Flying1.stance == -999d)
                {
                    packet10Flying1.moving = false;
                }

                if (packet10Flying1.moving)
                {
                    d5 = packet10Flying1.xPosition;
                    d7 = packet10Flying1.yPosition;
                    d9 = packet10Flying1.zPosition;
                    d13 = packet10Flying1.stance - packet10Flying1.yPosition;
                    if (!this.playerEntity.IsSleeping() && (d13 > 1.65d || d13 < 0.1d))
                    {
                        this.KickPlayer("Illegal stance");
                        logger.Warning(this.playerEntity.username + " had an illegal stance: " + d13);
                        return;
                    }

                    if (Math.Abs(packet10Flying1.xPosition) > 32000000d || Math.Abs(packet10Flying1.zPosition) > 32000000d)
                    {
                        this.KickPlayer("Illegal position");
                        return;
                    }
                }

                if (packet10Flying1.rotating)
                {
                    f11 = packet10Flying1.yaw;
                    f12 = packet10Flying1.pitch;
                }

                this.playerEntity.OnUpdateEntity(true);
                this.playerEntity.ySize = 0F;
                this.playerEntity.SetPositionAndRotation(this.lastPosX, this.lastPosY, this.lastPosZ, f11, f12);
                if (!this.hasMoved)
                {
                    return;
                }

                d13 = d5 - this.playerEntity.x;
                double d15 = d7 - this.playerEntity.y;
                double d17 = d9 - this.playerEntity.z;
                double d19 = d13 * d13 + d15 * d15 + d17 * d17;
                if (d19 > 100d)
                {
                    logger.Warning(this.playerEntity.username + " moved too quickly!");
                    this.KickPlayer("You moved too quickly :( (Hacking?)");
                    return;
                }

                float f21 = 0.0625F;
                bool z22 = worldServer2.GetCubes(this.playerEntity, this.playerEntity.boundingBox.Copy().GetInsetBoundingBox(f21, f21, f21)).Count == 0;
                this.playerEntity.MoveEntity(d13, d15, d17);
                d13 = d5 - this.playerEntity.x;
                d15 = d7 - this.playerEntity.y;
                if (d15 > -0.5d || d15 < 0.5d)
                {
                    d15 = 0;
                }

                d17 = d9 - this.playerEntity.z;
                d19 = d13 * d13 + d15 * d15 + d17 * d17;
                bool z23 = false;
                if (d19 > 0.0625d && !this.playerEntity.IsSleeping())
                {
                    z23 = true;
                    logger.Warning(this.playerEntity.username + " moved wrongly!");
                    Console.WriteLine("Got position " + d5 + ", " + d7 + ", " + d9);
                    Console.WriteLine("Expected " + this.playerEntity.x + ", " + this.playerEntity.y + ", " + this.playerEntity.z);
                }

                this.playerEntity.SetPositionAndRotation(d5, d7, d9, f11, f12);
                bool z24 = worldServer2.GetCubes(this.playerEntity, this.playerEntity.boundingBox.Copy().GetInsetBoundingBox(f21, f21, f21)).Count == 0;
                if (z22 && (z23 || !z24) && !this.playerEntity.IsSleeping())
                {
                    this.TeleportTo(this.lastPosX, this.lastPosY, this.lastPosZ, f11, f12);
                    return;
                }

                AABB axisAlignedBB25 = this.playerEntity.boundingBox.Copy().Expand(f21, f21, f21).AddCoord(0, -0.55d, 0);
                if (!this.mcServer.allowFlight && !worldServer2.Func_27069(axisAlignedBB25))
                {
                    if (d15 >= -0.03125d)
                    {
                        ++this.playerInAirTime;
                        if (this.playerInAirTime > 80)
                        {
                            logger.Warning(this.playerEntity.username + " was kicked for floating too long!");
                            this.KickPlayer("Flying is not enabled on this server");
                            return;
                        }
                    }
                }
                else
                {
                    this.playerInAirTime = 0;
                }

                this.playerEntity.onGround = packet10Flying1.onGround;
                this.mcServer.configManager.Func_613_b(this.playerEntity);
                this.playerEntity.HandleFalling(this.playerEntity.y - d3, packet10Flying1.onGround);
            }
        }

        public virtual void TeleportTo(double x, double y, double z, float xr, float yr)
        {
            this.hasMoved = false;
            this.lastPosX = x;
            this.lastPosY = y;
            this.lastPosZ = z;
            this.playerEntity.SetPositionAndRotation(x, y, z, xr, yr);
            this.playerEntity.playerNetServerHandler.SendPacket(new Packet13PlayerLookMove(x, y + 1.62F, y, z, xr, yr, false));
        }

        public override void HandleBlockDig(Packet14BlockDig packet14BlockDig1)
        {
            ServerLevel worldServer2 = this.mcServer.GetLevel(this.playerEntity.dimension);
            if (packet14BlockDig1.status == PlayerDigActionType.DROP_ITEM)
            {
                this.playerEntity.DropCurrentItem();
            }
            else
            {
                bool z3 = worldServer2.spawnProtected = worldServer2.dimension.dimension != 0 || this.mcServer.configManager.IsOp(this.playerEntity.username);
                bool z4 = false;
                if (packet14BlockDig1.status == PlayerDigActionType.BEGIN_DIG)
                {
                    z4 = true;
                }

                if (packet14BlockDig1.status == PlayerDigActionType.END_DIG)
                {
                    z4 = true;
                }

                int i5 = packet14BlockDig1.xPosition;
                int i6 = packet14BlockDig1.yPosition;
                int i7 = packet14BlockDig1.zPosition;
                if (z4)
                {
                    double d8 = this.playerEntity.x - (i5 + 0.5d);
                    double d10 = this.playerEntity.y - (i6 + 0.5d);
                    double d12 = this.playerEntity.z - (i7 + 0.5d);
                    double d14 = d8 * d8 + d10 * d10 + d12 * d12;
                    if (d14 > 36)
                    {
                        return;
                    }
                }

                Pos chunkCoordinates19 = worldServer2.GetSpawnPos();
                int i9 = (int)Mth.Abs(i5 - chunkCoordinates19.x);
                int i20 = (int)Mth.Abs(i7 - chunkCoordinates19.z);
                if (i9 > i20)
                {
                    i20 = i9;
                }

                if (packet14BlockDig1.status == PlayerDigActionType.BEGIN_DIG)
                {
                    if (i20 <= this.mcServer.spawnProtectionRadius && !z3)
                    {
                        this.playerEntity.playerNetServerHandler.SendPacket(new Packet53BlockChange(i5, i6, i7, worldServer2));
                    }
                    else
                    {
                        this.playerEntity.gameMode.Func_324_a(i5, i6, i7, packet14BlockDig1.face);
                    }
                }
                else if (packet14BlockDig1.status == PlayerDigActionType.END_DIG)
                {
                    this.playerEntity.gameMode.Func_22045_b(i5, i6, i7);
                    if (worldServer2.GetTile(i5, i6, i7) != 0)
                    {
                        this.playerEntity.playerNetServerHandler.SendPacket(new Packet53BlockChange(i5, i6, i7, worldServer2));
                    }
                }
                else if (packet14BlockDig1.status == PlayerDigActionType.UNK)
                {
                    double d11 = this.playerEntity.x - (i5 + 0.5);
                    double d13 = this.playerEntity.y - (i6 + 0.5);
                    double d15 = this.playerEntity.z - (i7 + 0.5);
                    double d17 = d11 * d11 + d13 * d13 + d15 * d15;
                    if (d17 < 256)
                    {
                        this.playerEntity.playerNetServerHandler.SendPacket(new Packet53BlockChange(i5, i6, i7, worldServer2));
                    }
                }

                worldServer2.spawnProtected = false;
            }
        }

        public override void HandlePlace(Packet15Place packet)
        {
            ServerLevel level = this.mcServer.GetLevel(this.playerEntity.dimension);
            ItemInstance item = this.playerEntity.inventory.GetCurrentItem();
            bool spawnProtected = level.spawnProtected = level.dimension.dimension != 0 || this.mcServer.configManager.IsOp(this.playerEntity.username);
            if (packet.direction == Facing.TileFace.UNDEFINED)
            {
                if (item == null)
                {
                    return;
                }

                this.playerEntity.gameMode.Func_6154_a(this.playerEntity, level, item);
            }
            else
            {
                int x = packet.xPosition;
                int y = packet.yPosition;
                int z = packet.zPosition;
                TileFace i8 = packet.direction;
                Pos spawnPos = level.GetSpawnPos();
                int xSpawn = (int)Mth.Abs(x - spawnPos.x);
                int zSpawn = (int)Mth.Abs(z - spawnPos.z);
                if (xSpawn > zSpawn)
                {
                    zSpawn = xSpawn;
                }

                if (this.hasMoved && this.playerEntity.GetDistanceSq(x + 0.5d, y + 0.5d, z + 0.5d) < 64d && (zSpawn > this.mcServer.spawnProtectionRadius || spawnProtected))
                {
                    this.playerEntity.gameMode.ActiveBlockOrUseItem(this.playerEntity, level, item, x, y, z, i8);
                }

                this.playerEntity.playerNetServerHandler.SendPacket(new Packet53BlockChange(x, y, z, level));
                if (i8 == TileFace.DOWN)
                {
                    --y;
                }

                if (i8 == TileFace.UP)
                {
                    ++y;
                }

                if (i8 == TileFace.NORTH)
                {
                    --z;
                }

                if (i8 == TileFace.SOUTH)
                {
                    ++z;
                }

                if (i8 == TileFace.WEST)
                {
                    --x;
                }

                if (i8 == TileFace.EAST)
                {
                    ++x;
                }

                this.playerEntity.playerNetServerHandler.SendPacket(new Packet53BlockChange(x, y, z, level));
            }

            item = this.playerEntity.inventory.GetCurrentItem();
            if (item != null && item.stackSize == 0)
            {
                this.playerEntity.inventory.mainInventory[this.playerEntity.inventory.currentItem] = null;
            }

            this.playerEntity.isChangingQuantityOnly = true;
            this.playerEntity.inventory.mainInventory[this.playerEntity.inventory.currentItem] = ItemInstance.CopyItemStack(this.playerEntity.inventory.mainInventory[this.playerEntity.inventory.currentItem]);
            Slot slot = this.playerEntity.curCraftingInventory.GetSlotFor(this.playerEntity.inventory, this.playerEntity.inventory.currentItem);
            this.playerEntity.curCraftingInventory.UpdateCraftingResults();
            this.playerEntity.isChangingQuantityOnly = false;
            if (!ItemInstance.AreItemStacksEqual(this.playerEntity.inventory.GetCurrentItem(), packet.itemStack))
            {
                this.SendPacket(new Packet103SetSlot(this.playerEntity.curCraftingInventory.windowId, slot.id, this.playerEntity.inventory.GetCurrentItem()));
            }

            level.spawnProtected = false;
        }

        public override void OnDisconnect(string string1, params object[] object2)
        {
            //original code does not use translate key format as is supposed to so you get the raw translate key string
            logger.Info(this.playerEntity.username + " lost connection: " + I18N.Instance.TranslateKeyFormat(string1, object2));
            this.mcServer.configManager.SendPacketToAllPlayers(new Packet3Chat("§e" + this.playerEntity.username + " left the game."));
            this.mcServer.configManager.PlayerLoggedOut(this.playerEntity);
            this.connectionClosed = true;
        }

        protected override void OnUnhandledPacket(Packet packet1)
        {
            logger.Warning(this.GetType() + " wasn't prepared to deal with a " + packet1.GetType());
            this.KickPlayer("Protocol error, unexpected packet");
        }

        public virtual void SendPacket(Packet packet1)
        {
            this.netManager.AddToSendQueue(packet1);
            this.curSendTime = this.lastSendTime;
        }

        public override void HandleBlockItemSwitch(Packet16BlockItemSwitch packet16BlockItemSwitch1)
        {
            if (packet16BlockItemSwitch1.id >= 0 && packet16BlockItemSwitch1.id <= Inventory.Func_25054_e())
            {
                this.playerEntity.inventory.currentItem = packet16BlockItemSwitch1.id;
            }
            else
            {
                logger.Warning(this.playerEntity.username + " tried to set an invalid carried item");
            }
        }

        public override void HandleChat(Packet3Chat packet3Chat1)
        {
            string string2 = packet3Chat1.message;
            if (string2.Length > 100)
            {
                this.KickPlayer("Chat message too long");
            }
            else
            {
                string2 = string2.Trim();
                for (int i3 = 0; i3 < string2.Length; ++i3)
                {
                    if (SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(string2[i3]) < 0)
                    {
                        this.KickPlayer("Illegal characters in chat");
                        return;
                    }
                }

                if (string2.StartsWith("/"))
                {
                    this.HandleSlashCommand(string2);
                }
                else
                {
                    string2 = "<" + this.playerEntity.username + "> " + string2;
                    logger.Info(string2);
                    this.mcServer.configManager.SendPacketToAllPlayers(new Packet3Chat(string2));
                }
            }
        }

        private void HandleSlashCommand(string cmd)
        {
            if (cmd.ToLower().StartsWith("/tpx"))
            {
                try 
                {
                    string[] str = cmd.Split(' ');

                    int x = int.Parse(str[1]);
                    int y = int.Parse(str[2]);
                    int z = int.Parse(str[3]);
                    this.TeleportTo(x, y + 1.62d, z, 0, 0);
                    this.SendPacket(new Packet3Chat($"Teleported to {x}, {y}, {z}"));
                } 
                catch (Exception e) 
                { this.SendPacket(new Packet3Chat("shit: " + e)); }
                

                return;
            }
            if (cmd.ToLower().StartsWith("/me "))
            {
                cmd = "* " + this.playerEntity.username + " " + cmd.Substring(cmd.IndexOf(" ")).Trim();
                logger.Info(cmd);
                this.mcServer.configManager.SendPacketToAllPlayers(new Packet3Chat(cmd));
            }
            else if (cmd.ToLower().StartsWith("/kill"))
            {
                this.playerEntity.AttackEntityFrom((Entity)null, 1000);
            }
            else if (cmd.ToLower().StartsWith("/tell "))
            {
                String[] string2 = cmd.Split(" ");
                if (string2.Length >= 3)
                {
                    cmd = cmd.Substring(cmd.IndexOf(" ")).Trim();
                    cmd = cmd.Substring(cmd.IndexOf(" ")).Trim();
                    cmd = "§7" + this.playerEntity.username + " whispers " + cmd;
                    logger.Info(cmd + " to " + string2[1]);
                    if (!this.mcServer.configManager.SendPacketToPlayer(string2[1], new Packet3Chat(cmd)))
                    {
                        this.SendPacket(new Packet3Chat("§cThere's no player by that name online."));
                    }
                }
            }
            else
            {
                string string3;
                if (this.mcServer.configManager.IsOp(this.playerEntity.username))
                {
                    string3 = cmd.Substring(1);
                    logger.Info(this.playerEntity.username + " issued server command: " + string3);
                    this.mcServer.AddCommand(string3, this);
                }
                else
                {
                    string3 = cmd.Substring(1);
                    logger.Info(this.playerEntity.username + " tried command: " + string3);
                }
            }
        }

        public override void HandleArmAnimation(Packet18Animation packet18Animation1)
        {
            if (packet18Animation1.animate == EntityAnimationType.SWING)
            {
                this.playerEntity.SwingItem();
            }
        }

        public override void HandleEntityAction(Packet19EntityAction packet19EntityAction1)
        {
            if (packet19EntityAction1.state == EntityActionType.SNEAK)
            {
                this.playerEntity.SetSneaking(true);
            }
            else if (packet19EntityAction1.state == EntityActionType.UNSNEAK)
            {
                this.playerEntity.SetSneaking(false);
            }
            else if (packet19EntityAction1.state == EntityActionType.LEAVE_BED)
            {
                this.playerEntity.WakeUpPlayer(false, true, true);
                this.hasMoved = false;
            }
        }

        public override void HandleKickDisconnect(Packet255KickDisconnect packet255KickDisconnect1)
        {
            this.netManager.CloseConnection("disconnect.quitting");
        }

        public virtual int GetNumChunkDataPackets()
        {
            return this.netManager.GetNumChunkDataPackets();
        }

        public virtual void Log(string string1)
        {
            this.SendPacket(new Packet3Chat("§7" + string1));
        }

        public virtual string GetUsername()
        {
            return this.playerEntity.username;
        }

        public override void HandleUseEntity(Packet7UseEntity packet7UseEntity1)
        {
            ServerLevel worldServer2 = this.mcServer.GetLevel(this.playerEntity.dimension);
            Entity entity3 = worldServer2.Func_6158_a(packet7UseEntity1.targetEntity);
            if (entity3 != null && this.playerEntity.CanEntityBeSeen(entity3) && this.playerEntity.GetDistanceSqToEntity(entity3) < 36)
            {
                if (packet7UseEntity1.isLeftClick == 0)
                {
                    this.playerEntity.UseCurrentItemOnEntity(entity3);
                }
                else if (packet7UseEntity1.isLeftClick == 1)
                {
                    this.playerEntity.AttackTargetEntityWithCurrentItem(entity3);
                }
            }
        }

        public override void HandleRespawn(Packet9Respawn packet9Respawn1)
        {
            if (this.playerEntity.health <= 0)
            {
                this.playerEntity = this.mcServer.configManager.RecreatePlayerEntity(this.playerEntity, 0);
            }
        }

        public override void HandleCloseWindow(Packet101CloseWindow packet101CloseWindow1)
        {
            this.playerEntity.CloseCraftingGui();
        }

        public override void HandleWindowClick(Packet102WindowClick packet102WindowClick1)
        {
            if (this.playerEntity.curCraftingInventory.windowId == packet102WindowClick1.window_Id && this.playerEntity.curCraftingInventory.GetCanCraft(this.playerEntity))
            {
                ItemInstance itemStack2 = this.playerEntity.curCraftingInventory.Func_27085(packet102WindowClick1.inventorySlot, packet102WindowClick1.mouseClick, packet102WindowClick1.field, this.playerEntity);
                if (ItemInstance.AreItemStacksEqual(packet102WindowClick1.itemStack, itemStack2))
                {
                    this.playerEntity.playerNetServerHandler.SendPacket(new Packet106Transaction(packet102WindowClick1.window_Id, packet102WindowClick1.action, true));
                    this.playerEntity.isChangingQuantityOnly = true;
                    this.playerEntity.curCraftingInventory.UpdateCraftingResults();
                    this.playerEntity.UpdateHeldItem();
                    this.playerEntity.isChangingQuantityOnly = false;
                }
                else
                {
                    this.windowTransactionMap[this.playerEntity.curCraftingInventory.windowId] = packet102WindowClick1.action;
                    this.playerEntity.playerNetServerHandler.SendPacket(new Packet106Transaction(packet102WindowClick1.window_Id, packet102WindowClick1.action, false));
                    this.playerEntity.curCraftingInventory.SetCanCraft(this.playerEntity, false);
                    List<ItemInstance> arrayList3 = new ();
                    for (int i4 = 0; i4 < this.playerEntity.curCraftingInventory.slots.Count; ++i4)
                    {
                        arrayList3.Add(this.playerEntity.curCraftingInventory.slots[i4].GetItem());
                    }

                    this.playerEntity.UpdateCraftingInventory(this.playerEntity.curCraftingInventory, arrayList3);
                }
            }
        }

        public override void HandleTransaction(Packet106Transaction packet106Transaction1)
        {
            bool result = this.windowTransactionMap.TryGetValue(this.playerEntity.curCraftingInventory.windowId, out short short2);
            if (result && packet106Transaction1.shortWindowId == short2 && this.playerEntity.curCraftingInventory.windowId == packet106Transaction1.windowId && !this.playerEntity.curCraftingInventory.GetCanCraft(this.playerEntity))
            {
                this.playerEntity.curCraftingInventory.SetCanCraft(this.playerEntity, true);
            }
        }

        public override void HandleSignUpdate(Packet130UpdateSign packet130UpdateSign1)
        {
            ServerLevel worldServer2 = this.mcServer.GetLevel(this.playerEntity.dimension);
            if (worldServer2.HasChunkAt(packet130UpdateSign1.xPosition, packet130UpdateSign1.yPosition, packet130UpdateSign1.zPosition))
            {
                TileEntity tileEntity3 = worldServer2.GetTileEntity(packet130UpdateSign1.xPosition, packet130UpdateSign1.yPosition, packet130UpdateSign1.zPosition);
                if (tileEntity3 is TileEntitySign)
                {
                    TileEntitySign tileEntitySign4 = (TileEntitySign)tileEntity3;
                    if (!tileEntitySign4.IsEditable())
                    {
                        this.mcServer.LogWarning("Player " + this.playerEntity.username + " just tried to change non-editable sign");
                        return;
                    }
                }

                int i6;
                int i9;
                for (i9 = 0; i9 < 4; ++i9)
                {
                    bool z5 = true;
                    if (packet130UpdateSign1.signLines[i9].Length > 15)
                    {
                        z5 = false;
                    }
                    else
                    {
                        for (i6 = 0; i6 < packet130UpdateSign1.signLines[i9].Length; ++i6)
                        {
                            if (SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(packet130UpdateSign1.signLines[i9][i6]) < 0)
                            {
                                z5 = false;
                            }
                        }
                    }

                    if (!z5)
                    {
                        packet130UpdateSign1.signLines[i9] = "!?";
                    }
                }

                if (tileEntity3 is TileEntitySign)
                {
                    i9 = packet130UpdateSign1.xPosition;
                    int i10 = packet130UpdateSign1.yPosition;
                    i6 = packet130UpdateSign1.zPosition;
                    TileEntitySign tileEntitySign7 = (TileEntitySign)tileEntity3;
                    for (int i8 = 0; i8 < 4; ++i8)
                    {
                        tileEntitySign7.signText[i8] = packet130UpdateSign1.signLines[i8];
                    }

                    tileEntitySign7.SetEditable(false);
                    tileEntitySign7.SetChanged();
                    worldServer2.SendTileUpdated(i9, i10, i6);
                }
            }
        }

        public override bool IsServerPacketListener()
        {
            return true;
        }
    }
}
