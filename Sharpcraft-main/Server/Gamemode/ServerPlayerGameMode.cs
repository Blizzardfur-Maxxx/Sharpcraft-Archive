using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Levell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Server.Gamemode
{
    public class ServerPlayerGameMode
    {
        private ServerLevel thisWorld;
        public Player thisPlayer;
        //private float field_672_d = 0F;
        private int field_22055_d;
        private int field_22054_g;
        private int field_22053_h;
        private int field_22052_i;
        private int field_22051_j;
        private bool field_22050_k;
        private int field_22049_l;
        private int field_22048_m;
        private int field_22047_n;
        private int field_22046_o;
        public ServerPlayerGameMode(ServerLevel worldServer1)
        {
            this.thisWorld = worldServer1;
        }

        public virtual void Func_328_a()
        {
            ++this.field_22051_j;
            if (this.field_22050_k)
            {
                int i1 = this.field_22051_j - this.field_22046_o;
                int i2 = this.thisWorld.GetTile(this.field_22049_l, this.field_22048_m, this.field_22047_n);
                if (i2 != 0)
                {
                    Tile block3 = Tile.tiles[i2];
                    float f4 = block3.BlockStrength(this.thisPlayer) * (i1 + 1);
                    if (f4 >= 1F)
                    {
                        this.field_22050_k = false;
                        this.Func_325_c(this.field_22049_l, this.field_22048_m, this.field_22047_n);
                    }
                }
                else
                {
                    this.field_22050_k = false;
                }
            }
        }

        public virtual void Func_324_a(int i1, int i2, int i3, TileFace i4)
        {
            this.thisWorld.ExtinguishFire((Player)null, i1, i2, i3, i4);
            this.field_22055_d = this.field_22051_j;
            int i5 = this.thisWorld.GetTile(i1, i2, i3);
            if (i5 > 0)
            {
                Tile.tiles[i5].OnBlockClicked(this.thisWorld, i1, i2, i3, this.thisPlayer);
            }

            if (i5 > 0 && Tile.tiles[i5].BlockStrength(this.thisPlayer) >= 1F)
            {
                this.Func_325_c(i1, i2, i3);
            }
            else
            {
                this.field_22054_g = i1;
                this.field_22053_h = i2;
                this.field_22052_i = i3;
            }
        }

        public virtual void Func_22045_b(int i1, int i2, int i3)
        {
            if (i1 == this.field_22054_g && i2 == this.field_22053_h && i3 == this.field_22052_i)
            {
                int i4 = this.field_22051_j - this.field_22055_d;
                int i5 = this.thisWorld.GetTile(i1, i2, i3);
                if (i5 != 0)
                {
                    Tile block6 = Tile.tiles[i5];
                    float f7 = block6.BlockStrength(this.thisPlayer) * (i4 + 1);
                    if (f7 >= 0.7F)
                    {
                        this.Func_325_c(i1, i2, i3);
                    }
                    else if (!this.field_22050_k)
                    {
                        this.field_22050_k = true;
                        this.field_22049_l = i1;
                        this.field_22048_m = i2;
                        this.field_22047_n = i3;
                        this.field_22046_o = this.field_22055_d;
                    }
                }
            }

            //this.field_672_d = 0F;
        }

        public virtual bool RemoveBlock(int i1, int i2, int i3)
        {
            Tile block4 = Tile.tiles[this.thisWorld.GetTile(i1, i2, i3)];
            int i5 = this.thisWorld.GetData(i1, i2, i3);
            bool z6 = this.thisWorld.SetTile(i1, i2, i3, 0);
            if (block4 != null && z6)
            {
                block4.OnBlockDestroyedByPlayer(this.thisWorld, i1, i2, i3, i5);
            }

            return z6;
        }

        public virtual bool Func_325_c(int i1, int i2, int i3)
        {
            int i4 = this.thisWorld.GetTile(i1, i2, i3);
            int i5 = this.thisWorld.GetData(i1, i2, i3);
            this.thisWorld.LevelEvent(this.thisPlayer, LevelEventType.BREAK_SOUND, i1, i2, i3, i4 + this.thisWorld.GetData(i1, i2, i3) * 256);
            bool z6 = this.RemoveBlock(i1, i2, i3);
            ItemInstance itemStack7 = this.thisPlayer.GetCurrentEquippedItem();
            if (itemStack7 != null)
            {
                itemStack7.OnDestroyBlock(i4, i1, i2, i3, this.thisPlayer);
                if (itemStack7.stackSize == 0)
                {
                    itemStack7.Func_1097(this.thisPlayer);
                    this.thisPlayer.DestroyCurrentEquippedItem();
                }
            }

            if (z6 && this.thisPlayer.CanHarvestBlock(Tile.tiles[i4]))
            {
                Tile.tiles[i4].HarvestBlock(this.thisWorld, this.thisPlayer, i1, i2, i3, i5);
                ((ServerPlayer)this.thisPlayer).playerNetServerHandler.SendPacket(new Packet53BlockChange(i1, i2, i3, this.thisWorld));
            }

            return z6;
        }

        public virtual bool Func_6154_a(Player entityPlayer1, Level world2, ItemInstance itemStack3)
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

        public virtual bool ActiveBlockOrUseItem(Player entityPlayer1, Level world2, ItemInstance itemStack3, int i4, int i5, int i6, TileFace i7)
        {
            int i8 = world2.GetTile(i4, i5, i6);
            return i8 > 0 && Tile.tiles[i8].BlockActivated(world2, i4, i5, i6, entityPlayer1) ? true : (itemStack3 == null ? false : itemStack3.UseItem(entityPlayer1, world2, i4, i5, i6, i7));
        }
    }
}
