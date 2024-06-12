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
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Client.Gamemode
{
    public class GameMode
    {
        protected readonly Client mc;
        public bool field_1064_b = false;
        public GameMode(Client instance)
        {
            this.mc = instance;
        }

        public virtual void Func_717_a(Level world1)
        {
        }

        public virtual void ClickBlock(int i1, int i2, int i3, TileFace i4)
        {
            this.mc.level.ExtinguishFire(this.mc.player, i1, i2, i3, i4);
            this.SendBlockRemoved(i1, i2, i3, i4);
        }

        public virtual bool SendBlockRemoved(int i1, int i2, int i3, TileFace i4)
        {
            Level world5 = this.mc.level;
            Tile block6 = Tile.tiles[world5.GetTile(i1, i2, i3)];
            world5.LevelEvent(LevelEventType.BREAK_SOUND, i1, i2, i3, block6.id + world5.GetData(i1, i2, i3) * 256);
            int i7 = world5.GetData(i1, i2, i3);
            bool z8 = world5.SetTile(i1, i2, i3, 0);
            if (block6 != null && z8)
            {
                block6.OnBlockDestroyedByPlayer(world5, i1, i2, i3, i7);
            }

            return z8;
        }

        public virtual void SendBlockRemoving(int i1, int i2, int i3, TileFace i4)
        {
        }

        public virtual void ResetBlockRemoving()
        {
        }

        public virtual void SetPartialTime(float f1)
        {
        }

        public virtual float GetBlockReachDistance()
        {
            return 5F;
        }

        public virtual bool SendUseItem(Player entityPlayer1, Level world2, ItemInstance itemStack3)
        {
            int i4 = itemStack3.stackSize;
            ItemInstance itemStack5 = itemStack3.UseItemRightClick(world2, entityPlayer1);
            if (itemStack5 != itemStack3 || itemStack5 != null && itemStack5.stackSize != i4)
            {
                entityPlayer1.inventory.mainInventory[entityPlayer1.inventory.currentItem] = itemStack5;
                if (itemStack5.stackSize == 0)
                {
                    entityPlayer1.inventory.mainInventory[entityPlayer1.inventory.currentItem] = null;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void FlipPlayer(Player entityPlayer1)
        {
        }

        public virtual void UpdateController()
        {
        }

        public virtual bool ShouldDrawHUD()
        {
            return true;
        }

        public virtual void Func_6473_b(Player entityPlayer1)
        {
        }

        public virtual bool SendPlaceBlock(Player entityPlayer1, Level world2, ItemInstance itemStack3, int i4, int i5, int i6, TileFace i7)
        {
            int i8 = world2.GetTile(i4, i5, i6);
            return i8 > 0 && Tile.tiles[i8].BlockActivated(world2, i4, i5, i6, entityPlayer1) ? true : (itemStack3 == null ? false : itemStack3.UseItem(entityPlayer1, world2, i4, i5, i6, i7));
        }

        public virtual Player CreatePlayer(Level world1)
        {
            return new LocalPlayer(this.mc, world1, this.mc.user, world1.dimension.dimension);
        }

        public virtual void InteractWithEntity(Player entityPlayer1, Entity entity2)
        {
            entityPlayer1.UseCurrentItemOnEntity(entity2);
        }

        public virtual void AttackEntity(Player entityPlayer1, Entity entity2)
        {
            entityPlayer1.AttackTargetEntityWithCurrentItem(entity2);
        }

        public virtual ItemInstance Func_27174_a(int i1, int i2, int i3, bool z4, Player entityPlayer5)
        {
            return entityPlayer5.curCraftingInventory.Func_27085(i2, i3, z4, entityPlayer5);
        }

        public virtual void Func_20086_a(int i1, Player entityPlayer2)
        {
            entityPlayer2.curCraftingInventory.Removed(entityPlayer2);
            entityPlayer2.curCraftingInventory = entityPlayer2.inventorySlots;
        }
    }
}
