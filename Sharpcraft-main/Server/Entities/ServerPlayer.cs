using SharpCraft.Core.i18n;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Inventory;
using SharpCraft.Core.World.Items;
using SharpCraft.Core;
using SharpCraft.Server.Gamemode;
using SharpCraft.Server.Levell;
using SharpCraft.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.World.Entities.Players.Player;
using SharpCraft.Core.World;
using SharpCraft.Core.Network;

namespace SharpCraft.Server.Entities
{
    public class ServerPlayer : Player, IContainerListener
    {
        public PlayerConnection playerNetServerHandler;
        public Server mcServer;
        public ServerPlayerGameMode gameMode;
        public double field_9155_d;
        public double field_9154_e;
        public List<ChunkPos> loadedChunks = new List<ChunkPos>();
        public HashSet<ChunkPos> chunkPositions = new HashSet<ChunkPos>();
        private int lastHealth = -99999999;
        private int ticksOfInvuln = 60;
        private ItemInstance[] playerInventory = new ItemInstance[]
        {
            null,
            null,
            null,
            null,
            null
        };
        private int currentWindowId = 0;
        public bool isChangingQuantityOnly;

        public ServerPlayer(Server srv, Level world2, string string3, ServerPlayerGameMode itemInWorldManager4) : base(world2)
        {
            itemInWorldManager4.thisPlayer = this;
            this.gameMode = itemInWorldManager4;
            Pos chunkCoordinates5 = world2.GetSpawnPos();
            int i6 = chunkCoordinates5.x;
            int i7 = chunkCoordinates5.z;
            int i8 = chunkCoordinates5.y;
            if (!world2.dimension.hasNoSky)
            {
                i6 += this.rand.NextInt(20) - 10;
                i8 = world2.GetTop(i6, i7);
                i7 += this.rand.NextInt(20) - 10;
            }

            this.SetLocationAndAngles(i6 + 0.5, i8, i7 + 0.5, 0F, 0F);
            this.mcServer = srv;
            this.stepHeight = 0F;
            this.username = string3;
            this.yOffset = 0F;
        }

        public override void SetWorld(Level world1)
        {
            base.SetWorld(world1);
            this.gameMode = new ServerPlayerGameMode((ServerLevel)world1);
            this.gameMode.thisPlayer = this;
        }

        public virtual void Func_20057_k()
        {
            this.curCraftingInventory.OnCraftGuiOpened(this);
        }

        public override ItemInstance[] GetInventory()
        {
            return this.playerInventory;
        }

        protected override void ResetHeight()
        {
            this.yOffset = 0F;
        }

        public override float GetEyeHeight()
        {
            return 1.62F;
        }

        public override void OnUpdate()
        {
            this.gameMode.Func_328_a();
            --this.ticksOfInvuln;
            this.curCraftingInventory.UpdateCraftingResults();
            for (int i1 = 0; i1 < 5; ++i1)
            {
                ItemInstance itemStack2 = this.GetEquipmentInSlot(i1);
                if (itemStack2 != this.playerInventory[i1])
                {
                    this.mcServer.GetEntityTracker(this.dimension).SendPacketToTrackedPlayers(this, new Packet5PlayerInventory(this.entityID, i1, itemStack2));
                    this.playerInventory[i1] = itemStack2;
                }
            }
        }

        public virtual ItemInstance GetEquipmentInSlot(int i1)
        {
            return i1 == 0 ? this.inventory.GetCurrentItem() : this.inventory.armorInventory[i1 - 1];
        }

        public override void OnDeath(Entity entity1)
        {
            this.inventory.DropAllItems();
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (this.ticksOfInvuln > 0)
            {
                return false;
            }
            else
            {
                if (!this.mcServer.pvpOn)
                {
                    if (entity1 is Player)
                    {
                        return false;
                    }

                    if (entity1 is Arrow)
                    {
                        Arrow entityArrow3 = (Arrow)entity1;
                        if (entityArrow3.owner is Player)
                        {
                            return false;
                        }
                    }
                }

                return base.AttackEntityFrom(entity1, i2);
            }
        }

        protected override bool IsPVPEnabled()
        {
            return this.mcServer.pvpOn;
        }

        public override void Heal(int i1)
        {
            base.Heal(i1);
        }

        public virtual void OnUpdateEntity(bool z1)
        {
            base.OnUpdate();
            for (int i2 = 0; i2 < this.inventory.GetContainerSize(); ++i2)
            {
                ItemInstance itemStack3 = this.inventory.GetItem(i2);
                if (itemStack3 != null && Item.items[itemStack3.itemID].MapItemFunc() && this.playerNetServerHandler.GetNumChunkDataPackets() <= 2)
                {
                    Packet packet4 = ((ItemMapBase)Item.items[itemStack3.itemID]).Func_28022_b(itemStack3, this.worldObj, this);
                    if (packet4 != null)
                    {
                        this.playerNetServerHandler.SendPacket(packet4);
                    }
                }
            }

            if (z1 && this.loadedChunks.Count != 0)
            {
                ChunkPos chunkCoordIntPair7 = this.loadedChunks[0];
                //BAD!!
                //if (chunkCoordIntPair7 != default)
                //{
                    bool z8 = false;
                    if (this.playerNetServerHandler.GetNumChunkDataPackets() < 4)
                    {
                        z8 = true;
                    }

                    if (z8)
                    {
                        ServerLevel worldServer9 = this.mcServer.GetLevel(this.dimension);
                        this.loadedChunks.Remove(chunkCoordIntPair7);
                        this.playerNetServerHandler.SendPacket(new Packet51MapChunk(chunkCoordIntPair7.x * 16, 0, chunkCoordIntPair7.z * 16, 16, 128, 16, worldServer9));
                        IList<TileEntity> list5 = worldServer9.GetTileEntityList(chunkCoordIntPair7.x * 16, 0, chunkCoordIntPair7.z * 16, chunkCoordIntPair7.x * 16 + 16, 128, chunkCoordIntPair7.z * 16 + 16);
                        for (int i6 = 0; i6 < list5.Count; ++i6)
                        {
                            this.GetTileEntityInfo(list5[i6]);
                        }
                    }
                //}
            }

            if (this.inPortal)
            {
                if (this.mcServer.properties.GetBooleanProperty("allow-nether", true))
                {
                    if (this.curCraftingInventory != this.inventorySlots)
                    {
                        this.CloseScreen();
                    }

                    if (this.ridingEntity != null)
                    {
                        this.MountEntity(this.ridingEntity);
                    }
                    else
                    {
                        this.timeInPortal += 0.0125F;
                        if (this.timeInPortal >= 1F)
                        {
                            this.timeInPortal = 1F;
                            this.timeUntilPortal = 10;
                            this.mcServer.configManager.SendPlayerToOtherDimension(this);
                        }
                    }

                    this.inPortal = false;
                }
            }
            else
            {
                if (this.timeInPortal > 0F)
                {
                    this.timeInPortal -= 0.05F;
                }

                if (this.timeInPortal < 0F)
                {
                    this.timeInPortal = 0F;
                }
            }

            if (this.timeUntilPortal > 0)
            {
                --this.timeUntilPortal;
            }

            if (this.health != this.lastHealth)
            {
                this.playerNetServerHandler.SendPacket(new Packet8UpdateHealth(this.health));
                this.lastHealth = this.health;
            }
        }

        private void GetTileEntityInfo(TileEntity tileEntity1)
        {
            if (tileEntity1 != null)
            {
                Packet packet2 = tileEntity1.GetDescriptionPacket();
                if (packet2 != null)
                {
                    this.playerNetServerHandler.SendPacket(packet2);
                }
            }
        }

        public override void OnLivingUpdate()
        {
            base.OnLivingUpdate();
        }

        public override void OnItemPickup(Entity entity1, int i2)
        {
            if (!entity1.isDead)
            {
                EntityTracker entityTracker3 = this.mcServer.GetEntityTracker(this.dimension);
                if (entity1 is ItemEntity)
                {
                    entityTracker3.SendPacketToTrackedPlayers(entity1, new Packet22Collect(entity1.entityID, this.entityID));
                }

                if (entity1 is Arrow)
                {
                    entityTracker3.SendPacketToTrackedPlayers(entity1, new Packet22Collect(entity1.entityID, this.entityID));
                }
            }

            base.OnItemPickup(entity1, i2);
            this.curCraftingInventory.UpdateCraftingResults();
        }

        public override void SwingItem()
        {
            if (!this.isSwinging)
            {
                this.swingProgressInt = -1;
                this.isSwinging = true;
                EntityTracker entityTracker1 = this.mcServer.GetEntityTracker(this.dimension);
                entityTracker1.SendPacketToTrackedPlayers(this, new Packet18Animation(this, Core.Network.EntityAnimationType.SWING));
            }
        }

        public virtual void Func_22068_s()
        {
        }

        public override BedSleepingProblem SleepInBedAt(int i1, int i2, int i3)
        {
            BedSleepingProblem enumStatus4 = base.SleepInBedAt(i1, i2, i3);
            if (enumStatus4 == BedSleepingProblem.OK)
            {
                EntityTracker entityTracker5 = this.mcServer.GetEntityTracker(this.dimension);
                Packet17Sleep packet17Sleep6 = new Packet17Sleep(this, 0, i1, i2, i3);
                entityTracker5.SendPacketToTrackedPlayers(this, packet17Sleep6);
                this.playerNetServerHandler.TeleportTo(this.x, this.y, this.z, this.yaw, this.pitch);
                this.playerNetServerHandler.SendPacket(packet17Sleep6);
            }

            return enumStatus4;
        }

        public override void WakeUpPlayer(bool z1, bool z2, bool z3)
        {
            if (this.IsSleeping())
            {
                EntityTracker entityTracker4 = this.mcServer.GetEntityTracker(this.dimension);
                entityTracker4.SendPacketToTrackedPlayersAndTrackedEntity(this, new Packet18Animation(this, Core.Network.EntityAnimationType.WAKE_UP));
            }

            base.WakeUpPlayer(z1, z2, z3);
            if (this.playerNetServerHandler != null)
            {
                this.playerNetServerHandler.TeleportTo(this.x, this.y, this.z, this.yaw, this.pitch);
            }
        }

        public override void MountEntity(Entity entity1)
        {
            base.MountEntity(entity1);
            this.playerNetServerHandler.SendPacket(new Packet39AttachEntity(this, this.ridingEntity));
            this.playerNetServerHandler.TeleportTo(this.x, this.y, this.z, this.yaw, this.pitch);
        }

        protected override void UpdateFallState(double d1, bool z3)
        {
        }

        public virtual void HandleFalling(double d1, bool z3)
        {
            base.UpdateFallState(d1, z3);
        }

        private void GetNextWidowId()
        {
            this.currentWindowId = this.currentWindowId % 100 + 1;
        }

        public override void DisplayWorkbenchGUI(int i1, int i2, int i3)
        {
            this.GetNextWidowId();
            this.playerNetServerHandler.SendPacket(new Packet100OpenWindow(this.currentWindowId, InventoryType.WORKBENCH, "Crafting", 9));
            this.curCraftingInventory = new CraftingMenu(this.inventory, this.worldObj, i1, i2, i3);
            this.curCraftingInventory.windowId = this.currentWindowId;
            this.curCraftingInventory.OnCraftGuiOpened(this);
        }

        public override void DisplayGUIChest(IContainer iInventory1)
        {
            this.GetNextWidowId();
            this.playerNetServerHandler.SendPacket(new Packet100OpenWindow(this.currentWindowId, InventoryType.GENERIC_INVENTORY, iInventory1.GetName(), iInventory1.GetContainerSize()));
            this.curCraftingInventory = new ContainerMenu(this.inventory, iInventory1);
            this.curCraftingInventory.windowId = this.currentWindowId;
            this.curCraftingInventory.OnCraftGuiOpened(this);
        }

        public override void DisplayGUIFurnace(TileEntityFurnace tileEntityFurnace1)
        {
            this.GetNextWidowId();
            this.playerNetServerHandler.SendPacket(new Packet100OpenWindow(this.currentWindowId, InventoryType.FURNACE, tileEntityFurnace1.GetName(), tileEntityFurnace1.GetContainerSize()));
            this.curCraftingInventory = new FurnaceMenu(this.inventory, tileEntityFurnace1);
            this.curCraftingInventory.windowId = this.currentWindowId;
            this.curCraftingInventory.OnCraftGuiOpened(this);
        }

        public override void DisplayGUIDispenser(TileEntityDispenser tileEntityDispenser1)
        {
            this.GetNextWidowId();
            this.playerNetServerHandler.SendPacket(new Packet100OpenWindow(this.currentWindowId, InventoryType.DISPENSER, tileEntityDispenser1.GetName(), tileEntityDispenser1.GetContainerSize()));
            this.curCraftingInventory = new TrapMenu(this.inventory, tileEntityDispenser1);
            this.curCraftingInventory.windowId = this.currentWindowId;
            this.curCraftingInventory.OnCraftGuiOpened(this);
        }

        public virtual void UpdateCraftingInventorySlot(AbstractContainerMenu container1, int i2, ItemInstance itemStack3)
        {
            if (!(container1.GetSlot(i2) is SlotCrafting))
            {
                if (!this.isChangingQuantityOnly)
                {
                    this.playerNetServerHandler.SendPacket(new Packet103SetSlot(container1.windowId, i2, itemStack3));
                }
            }
        }

        public virtual void Func_28017_a(AbstractContainerMenu container1)
        {
            this.UpdateCraftingInventory(container1, container1.GetItems());
        }

        public virtual void UpdateCraftingInventory(AbstractContainerMenu container1, IList<ItemInstance> list2)
        {
            this.playerNetServerHandler.SendPacket(new Packet104WindowItems(container1.windowId, list2));
            this.playerNetServerHandler.SendPacket(new Packet103SetSlot(-1, -1, this.inventory.GetItem()));
        }

        public virtual void UpdateCraftingInventoryInfo(AbstractContainerMenu container1, int i2, int i3)
        {
            this.playerNetServerHandler.SendPacket(new Packet105UpdateProgressbar(container1.windowId, i2, i3));
        }

        public override void OnItemStackChanged(ItemInstance itemStack1)
        {
        }

        protected override void CloseScreen()
        {
            this.playerNetServerHandler.SendPacket(new Packet101CloseWindow(this.curCraftingInventory.windowId));
            this.CloseCraftingGui();
        }

        public virtual void UpdateHeldItem()
        {
            if (!this.isChangingQuantityOnly)
            {
                this.playerNetServerHandler.SendPacket(new Packet103SetSlot(-1, -1, this.inventory.GetItem()));
            }
        }

        public virtual void CloseCraftingGui()
        {
            this.curCraftingInventory.Removed(this);
            this.curCraftingInventory = this.inventorySlots;
        }

        public virtual void SetMovementType(float f1, float f2, bool z3, bool z4, float f5, float f6)
        {
            this.moveStrafing = f1;
            this.moveForward = f2;
            this.isJumping = z3;
            this.SetSneaking(z4);
            this.pitch = f5;
            this.yaw = f6;
        }

        public override void AddStat(Stat statBase1, int i2)
        {
            if (statBase1 != null)
            {
                if (!statBase1.field_g)
                {
                    while (i2 > 100)
                    {
                        this.playerNetServerHandler.SendPacket(new Packet200Statistic(statBase1.statId, 100));
                        i2 -= 100;
                    }

                    this.playerNetServerHandler.SendPacket(new Packet200Statistic(statBase1.statId, i2));
                }
            }
        }

        public virtual void Func_30002_A()
        {
            if (this.ridingEntity != null)
            {
                this.MountEntity(this.ridingEntity);
            }

            if (this.riddenByEntity != null)
            {
                this.riddenByEntity.MountEntity(this);
            }

            if (this.sleeping)
            {
                this.WakeUpPlayer(true, false, false);
            }
        }

        public virtual void Func_30001_B()
        {
            this.lastHealth = -99999999;
        }

        public override void AddChatMessage(string str)
        {
            I18N intl = I18N.Instance;
            string tring = intl.TranslateKey(str);
            this.playerNetServerHandler.SendPacket(new Packet3Chat(tring));
        }

        public override void Fun_o()
        {
        }
    }
}
