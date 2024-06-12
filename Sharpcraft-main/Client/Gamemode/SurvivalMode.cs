using SharpCraft.Core.World.Entities.Players;
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
    public class SurvivalMode : GameMode
    {
        private int field_1074_c = -1;
        private int field_1073_d = -1;
        private int field_1072_e = -1;
        private float curBlockDamage = 0F;
        private float prevBlockDamage = 0F;
        private float field_1069_h = 0F;
        private int blockHitWait = 0;
        public SurvivalMode(Client instance) : base(instance)
        {
        }

        public override void FlipPlayer(Player entityPlayer1)
        {
            entityPlayer1.yaw = -180F;
        }

        public override bool SendBlockRemoved(int i1, int i2, int i3, TileFace i4)
        {
            int i5 = this.mc.level.GetTile(i1, i2, i3);
            int i6 = this.mc.level.GetData(i1, i2, i3);
            bool z7 = base.SendBlockRemoved(i1, i2, i3, i4);
            ItemInstance itemStack8 = this.mc.player.GetCurrentEquippedItem();
            bool z9 = this.mc.player.CanHarvestBlock(Tile.tiles[i5]);
            if (itemStack8 != null)
            {
                itemStack8.OnDestroyBlock(i5, i1, i2, i3, this.mc.player);
                if (itemStack8.stackSize == 0)
                {
                    itemStack8.Func_1097(this.mc.player);
                    this.mc.player.DestroyCurrentEquippedItem();
                }
            }

            if (z7 && z9)
            {
                Tile.tiles[i5].HarvestBlock(this.mc.level, this.mc.player, i1, i2, i3, i6);
            }

            return z7;
        }

        public override void ClickBlock(int i1, int i2, int i3, TileFace i4)
        {
            this.mc.level.ExtinguishFire(this.mc.player, i1, i2, i3, i4);
            int i5 = this.mc.level.GetTile(i1, i2, i3);
            if (i5 > 0 && this.curBlockDamage == 0F)
            {
                Tile.tiles[i5].OnBlockClicked(this.mc.level, i1, i2, i3, this.mc.player);
            }

            if (i5 > 0 && Tile.tiles[i5].BlockStrength(this.mc.player) >= 1F)
            {
                this.SendBlockRemoved(i1, i2, i3, i4);
            }
        }

        public override void ResetBlockRemoving()
        {
            this.curBlockDamage = 0F;
            this.blockHitWait = 0;
        }

        public override void SendBlockRemoving(int i1, int i2, int i3, TileFace i4)
        {
            if (this.blockHitWait > 0)
            {
                --this.blockHitWait;
            }
            else
            {
                if (i1 == this.field_1074_c && i2 == this.field_1073_d && i3 == this.field_1072_e)
                {
                    int i5 = this.mc.level.GetTile(i1, i2, i3);
                    if (i5 == 0)
                    {
                        return;
                    }

                    Tile block6 = Tile.tiles[i5];
                    this.curBlockDamage += block6.BlockStrength(this.mc.player);
                    if (this.field_1069_h % 4F == 0F && block6 != null)
                    {
                        this.mc.soundEngine.PlaySound(block6.soundType.GetStepSound(), i1 + 0.5F, i2 + 0.5F, i3 + 0.5F, (block6.soundType.GetVolume() + 1F) / 8F, block6.soundType.GetPitch() * 0.5F);
                    }

                    ++this.field_1069_h;
                    if (this.curBlockDamage >= 1F)
                    {
                        this.SendBlockRemoved(i1, i2, i3, i4);
                        this.curBlockDamage = 0F;
                        this.prevBlockDamage = 0F;
                        this.field_1069_h = 0F;
                        this.blockHitWait = 5;
                    }
                }
                else
                {
                    this.curBlockDamage = 0F;
                    this.prevBlockDamage = 0F;
                    this.field_1069_h = 0F;
                    this.field_1074_c = i1;
                    this.field_1073_d = i2;
                    this.field_1072_e = i3;
                }
            }
        }

        public override void SetPartialTime(float f1)
        {
            if (this.curBlockDamage <= 0F)
            {
                this.mc.ingameGUI.damageGuiPartialTime = 0F;
                this.mc.renderGlobal.damagePartialTime = 0F;
            }
            else
            {
                float f2 = this.prevBlockDamage + (this.curBlockDamage - this.prevBlockDamage) * f1;
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
            this.prevBlockDamage = this.curBlockDamage;
            this.mc.soundEngine.PlayRandomMusicIfReady();
        }
    }
}
