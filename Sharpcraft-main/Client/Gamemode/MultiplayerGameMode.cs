using SharpCraft.Client.Players;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.Network;
using SharpCraft.Core.Network.Packets;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Client.Gamemode
{
    public class MultiPlayerGameMode : GameMode
    {
        private int currentBlockX = -1;
        private int currentBlockY = -1;
        private int currentblockZ = -1;
        private float curBlockDamageMP = 0F;
        private float prevBlockDamageMP = 0F;
        private float field_9441_h = 0F;
        private int blockHitDelay = 0;
        private bool isHittingBlock = false;
        private ClientConnection netClientHandler;
        private int currentPlayerItem = 0;

        public MultiPlayerGameMode(Client instance, ClientConnection netClientHandler2) : base(instance)
        {
            this.netClientHandler = netClientHandler2;
        }

        public override void FlipPlayer(Player entityPlayer1)
        {
            entityPlayer1.yaw = -180F;
        }

        public override bool SendBlockRemoved(int i1, int i2, int i3, TileFace i4)
        {
            int i5 = this.mc.level.GetTile(i1, i2, i3);
            bool z6 = base.SendBlockRemoved(i1, i2, i3, i4);
            ItemInstance itemStack7 = this.mc.player.GetCurrentEquippedItem();
            if (itemStack7 != null)
            {
                itemStack7.OnDestroyBlock(i5, i1, i2, i3, this.mc.player);
                if (itemStack7.stackSize == 0)
                {
                    itemStack7.Func_1097(this.mc.player);
                    this.mc.player.DestroyCurrentEquippedItem();
                }
            }

            return z6;
        }

        public override void ClickBlock(int i1, int i2, int i3, TileFace i4)
        {
            if (!this.isHittingBlock || i1 != this.currentBlockX || i2 != this.currentBlockY || i3 != this.currentblockZ)
            {
                this.netClientHandler.AddToSendQueue(new Packet14BlockDig(0, i1, i2, i3, i4));
                int i5 = this.mc.level.GetTile(i1, i2, i3);
                if (i5 > 0 && this.curBlockDamageMP == 0F)
                {
                    Tile.tiles[i5].OnBlockClicked(this.mc.level, i1, i2, i3, this.mc.player);
                }

                if (i5 > 0 && Tile.tiles[i5].BlockStrength(this.mc.player) >= 1F)
                {
                    this.SendBlockRemoved(i1, i2, i3, i4);
                }
                else
                {
                    this.isHittingBlock = true;
                    this.currentBlockX = i1;
                    this.currentBlockY = i2;
                    this.currentblockZ = i3;
                    this.curBlockDamageMP = 0F;
                    this.prevBlockDamageMP = 0F;
                    this.field_9441_h = 0F;
                }
            }
        }

        public override void ResetBlockRemoving()
        {
            this.curBlockDamageMP = 0F;
            this.isHittingBlock = false;
        }

        public override void SendBlockRemoving(int i1, int i2, int i3, TileFace i4)
        {
            if (this.isHittingBlock)
            {
                this.SyncCurrentPlayItem();
                if (this.blockHitDelay > 0)
                {
                    --this.blockHitDelay;
                }
                else
                {
                    if (i1 == this.currentBlockX && i2 == this.currentBlockY && i3 == this.currentblockZ)
                    {
                        int i5 = this.mc.level.GetTile(i1, i2, i3);
                        if (i5 == 0)
                        {
                            this.isHittingBlock = false;
                            return;
                        }

                        Tile block6 = Tile.tiles[i5];
                        this.curBlockDamageMP += block6.BlockStrength(this.mc.player);
                        if (this.field_9441_h % 4F == 0F && block6 != null)
                        {
                            this.mc.soundEngine.PlaySound(block6.soundType.GetStepSound(), i1 + 0.5F, i2 + 0.5F, i3 + 0.5F, (block6.soundType.GetVolume() + 1F) / 8F, block6.soundType.GetPitch() * 0.5F);
                        }

                        ++this.field_9441_h;
                        if (this.curBlockDamageMP >= 1F)
                        {
                            this.isHittingBlock = false;
                            this.netClientHandler.AddToSendQueue(new Packet14BlockDig(Core.Network.PlayerDigActionType.END_DIG, i1, i2, i3, i4));
                            this.SendBlockRemoved(i1, i2, i3, i4);
                            this.curBlockDamageMP = 0F;
                            this.prevBlockDamageMP = 0F;
                            this.field_9441_h = 0F;
                            this.blockHitDelay = 5;
                        }
                    }
                    else
                    {
                        this.ClickBlock(i1, i2, i3, i4);
                    }
                }
            }
        }

        public override void SetPartialTime(float f1)
        {
            if (this.curBlockDamageMP <= 0F)
            {
                this.mc.ingameGUI.damageGuiPartialTime = 0F;
                this.mc.renderGlobal.damagePartialTime = 0F;
            }
            else
            {
                float f2 = this.prevBlockDamageMP + (this.curBlockDamageMP - this.prevBlockDamageMP) * f1;
                this.mc.ingameGUI.damageGuiPartialTime = f2;
                this.mc.renderGlobal.damagePartialTime = f2;
            }
        }

        public override float GetBlockReachDistance()
        {
            return 4F;
        }

        public override void Func_717_a(Level world1)
        {
            base.Func_717_a(world1);
        }

        public override void UpdateController()
        {
            this.SyncCurrentPlayItem();
            this.prevBlockDamageMP = this.curBlockDamageMP;
            this.mc.soundEngine.PlayRandomMusicIfReady();
        }

        private void SyncCurrentPlayItem()
        {
            int i1 = this.mc.player.inventory.currentItem;
            if (i1 != this.currentPlayerItem)
            {
                this.currentPlayerItem = i1;
                this.netClientHandler.AddToSendQueue(new Packet16BlockItemSwitch(this.currentPlayerItem));
            }
        }

        public override bool SendPlaceBlock(Player entityPlayer1, Level world2, ItemInstance itemStack3, int i4, int i5, int i6, TileFace i7)
        {
            this.SyncCurrentPlayItem();
            this.netClientHandler.AddToSendQueue(new Packet15Place(i4, i5, i6, i7, entityPlayer1.inventory.GetCurrentItem()));
            bool z8 = base.SendPlaceBlock(entityPlayer1, world2, itemStack3, i4, i5, i6, i7);
            return z8;
        }

        public override bool SendUseItem(Player entityPlayer1, Level world2, ItemInstance itemStack3)
        {
            this.SyncCurrentPlayItem();
            this.netClientHandler.AddToSendQueue(new Packet15Place(-1, -1, -1, TileFace.UNDEFINED, entityPlayer1.inventory.GetCurrentItem()));
            bool z4 = base.SendUseItem(entityPlayer1, world2, itemStack3);
            return z4;
        }

        public override Player CreatePlayer(Level world1)
        {
            return new MultiplayerLocalPlayer(this.mc, world1, this.mc.user, this.netClientHandler);
        }

        public override void AttackEntity(Player entityPlayer1, Entity entity2)
        {
            this.SyncCurrentPlayItem();
            this.netClientHandler.AddToSendQueue(new Packet7UseEntity(entityPlayer1.entityID, entity2.entityID, 1));
            entityPlayer1.AttackTargetEntityWithCurrentItem(entity2);
        }

        public override void InteractWithEntity(Player entityPlayer1, Entity entity2)
        {
            this.SyncCurrentPlayItem();
            this.netClientHandler.AddToSendQueue(new Packet7UseEntity(entityPlayer1.entityID, entity2.entityID, 0));
            entityPlayer1.UseCurrentItemOnEntity(entity2);
        }

        public override ItemInstance Func_27174_a(int i1, int i2, int i3, bool z4, Player entityPlayer5)
        {
            short s6 = entityPlayer5.curCraftingInventory.Func_20111(entityPlayer5.inventory);
            ItemInstance itemStack7 = base.Func_27174_a(i1, i2, i3, z4, entityPlayer5);
            this.netClientHandler.AddToSendQueue(new Packet102WindowClick(i1, i2, i3, z4, itemStack7, s6));
            return itemStack7;
        }

        public override void Func_20086_a(int i1, Player entityPlayer2)
        {
            if (i1 != -9999)
            {
            }
        }
    }
}
