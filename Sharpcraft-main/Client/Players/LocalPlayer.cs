using Microsoft.VisualBasic.ApplicationServices;
using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World;
using LWCSGL.Input;
using SharpCraft.Client.Particles;
using SharpCraft.Client.GUI.Screens.Inventories;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Client.Players
{
    public class LocalPlayer : Player
    {
        public Input movementInput;
        protected Client mc;

        public LocalPlayer(Client instance, Level world2, User session3, int i4) : base(world2)
        {
            this.mc = instance;
            this.dimension = i4;
            if (session3 != null && session3.name != null && session3.name.Length > 0)
            {
                this.skinUrl = SharedConstants.SKIN_URL + session3.name + ".png";
            }

            this.username = session3.name;
            this.inventory.AddItem(new Core.World.Items.ItemInstance(Tile.rock.id, 64, 0));
            this.inventory.AddItem(new Core.World.Items.ItemInstance(Tile.rock.id, 64, 0));
            this.inventory.AddItem(new Core.World.Items.ItemInstance(Tile.rock.id, 64, 0));
            this.inventory.AddItem(new Core.World.Items.ItemInstance(Tile.rock.id, 64, 0));
        }

        public override void MoveEntity(double x, double y, double z)
        {
            if (Enhancements.client_fly_hack)
            {
                if (!this.worldObj.isRemote)
                {
                    if (y < 0)
                    {
                        y = 0;
                    }

                    x *= 5;
                    z *= 5;
                    if (this.movementInput.jump)
                    {
                        y += 0.5;
                    }

                    if (this.movementInput.sneak)
                    {
                        y -= 0.5;
                    }
                }
            }

            base.MoveEntity(x, y, z);
        }

        protected override void UpdatePlayerActionState()
        {
            base.UpdatePlayerActionState();
            this.moveStrafing = this.movementInput.moveStrafe;
            this.moveForward = this.movementInput.moveForward;
            this.isJumping = this.movementInput.jump;
        }

        public override void OnLivingUpdate()
        {
            if (!this.mc.statFileWriter.HasAchievementUnlocked(AchievementList.openInventory))
            {
                this.mc.guiAchievement.QueueAchievementInformation(AchievementList.openInventory);
            }

            this.prevTimeInPortal = this.timeInPortal;
            if (this.inPortal)
            {
                if (!this.worldObj.isRemote && this.ridingEntity != null)
                {
                    this.MountEntity((Entity)null);
                }

                if (this.mc.currentScreen != null)
                {
                    this.mc.SetScreen(null);
                }

                if (this.timeInPortal == 0F)
                {
                    this.mc.soundEngine.PlaySoundFX("portal.trigger", 1F, this.rand.NextFloat() * 0.4F + 0.8F);
                }

                this.timeInPortal += 0.0125F;
                if (this.timeInPortal >= 1F)
                {
                    this.timeInPortal = 1F;
                    if (!this.worldObj.isRemote)
                    {
                        this.timeUntilPortal = 10;
                        this.mc.soundEngine.PlaySoundFX("portal.travel", 1F, this.rand.NextFloat() * 0.4F + 0.8F);
                        this.mc.ToggleDimension();
                    }
                }

                this.inPortal = false;
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

            if (Enhancements.client_fly_hack)
            {
                if (!this.worldObj.isRemote)
                {
                    this.fallDistance = 0;
                    this.motionY = 0;
                }
            }

            this.movementInput.UpdatePlayerMoveState(this);
            if (this.movementInput.sneak && this.ySize < 0.2F)
            {
                this.ySize = 0.2F;
            }

            this.PushOutOfBlocks(this.x - this.width * 0.35, this.boundingBox.y0 + 0.5, this.z + this.width * 0.35);
            this.PushOutOfBlocks(this.x - this.width * 0.35, this.boundingBox.y0 + 0.5, this.z - this.width * 0.35);
            this.PushOutOfBlocks(this.x + this.width * 0.35, this.boundingBox.y0 + 0.5, this.z - this.width * 0.35);
            this.PushOutOfBlocks(this.x + this.width * 0.35, this.boundingBox.y0 + 0.5, this.z + this.width * 0.35);
            base.OnLivingUpdate();
        }

        public virtual void ResetPlayerKeyState()
        {
            this.movementInput.ResetKeyState();
        }

        public virtual void HandleKeyPress(VirtualKey i1, bool z2)
        {
            this.movementInput.CheckKeyForMovementInput(i1, z2);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetInteger("Score", this.score);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.score = nBTTagCompound1.GetInteger("Score");
        }

        //public accessor to call CloseScreen cause not making the entire hierachy public for one method call. -pnp
        public void AccessorCloseScreen() => CloseScreen();

        protected override void CloseScreen()
        {
            base.CloseScreen();
            this.mc.SetScreen(null);
        }

        public override void DisplayGUIEditSign(TileEntitySign tileEntitySign1)
        {
            this.mc.SetScreen(new GuiEditSign(tileEntitySign1));
        }

        public override void DisplayGUIChest(IContainer iInventory1)
        {
            this.mc.SetScreen(new GuiChest(this.inventory, iInventory1));
        }

        public override void DisplayWorkbenchGUI(int i1, int i2, int i3)
        {
            this.mc.SetScreen(new GuiCrafting(this.inventory, this.worldObj, i1, i2, i3));
        }

        public override void DisplayGUIFurnace(TileEntityFurnace tileEntityFurnace1)
        {
            this.mc.SetScreen(new GuiFurnace(this.inventory, tileEntityFurnace1));
        }

        public override void DisplayGUIDispenser(TileEntityDispenser tileEntityDispenser1)
        {
            this.mc.SetScreen(new GuiDispenser(this.inventory, tileEntityDispenser1));
        }

        public override void OnItemPickup(Entity entity1, int i2)
        {
            this.mc.effectRenderer.AddEffect(new EntityPickupFX(this.mc.level, entity1, this, -0.5F));
        }

        public virtual int GetPlayerArmorValue()
        {
            return this.inventory.GetTotalArmorValue();
        }

        public virtual void SendChatMessage(string string1)
        {
            this.mc.ingameGUI.AddChatMessage("Client Message: "+string1);
        }

        public override bool IsSneaking()
        {
            return this.movementInput.sneak && !this.sleeping;
        }

        public virtual void SetHealth(int i1)
        {
            int i2 = this.health - i1;
            if (i2 <= 0)
            {
                this.health = i1;
                if (i2 < 0)
                {
                    this.heartsLife = this.heartsHalvesLife / 2;
                }
            }
            else
            {
                this.field_af = i2;
                this.prevHealth = this.health;
                this.heartsLife = this.heartsHalvesLife;
                this.DamageEntity(i2);
                this.hurtTime = this.maxHurtTime = 10;
            }
        }

        public override void RespawnPlayer()
        {
            this.mc.Respawn(false, 0);
        }

        public override void Fun_o()
        {
        }

        public override void AddChatMessage(string string1)
        {
            this.mc.ingameGUI.AddChatMessageTranslate(string1);
        }

        public override void AddStat(Stat statBase1, int i2)
        {
            if (statBase1 != null)
            {
                if (statBase1.Fun_f())
                {
                    Achievement achievement3 = (Achievement)statBase1;
                    if (achievement3.parentAchievement == null || this.mc.statFileWriter.HasAchievementUnlocked(achievement3.parentAchievement))
                    {
                        if (!this.mc.statFileWriter.HasAchievementUnlocked(achievement3))
                        {
                            this.mc.guiAchievement.QueueTakenAchievement(achievement3);
                        }

                        this.mc.statFileWriter.ReadStat(statBase1, i2);
                    }
                }
                else
                {
                    this.mc.statFileWriter.ReadStat(statBase1, i2);
                }
            }
        }

        private bool IsBlockTranslucent(int i1, int i2, int i3)
        {
            return this.worldObj.IsSolidBlockingTile(i1, i2, i3);
        }

        protected override bool PushOutOfBlocks(double d1, double d3, double d5)
        {
            int i7 = Mth.Floor(d1);
            int i8 = Mth.Floor(d3);
            int i9 = Mth.Floor(d5);
            double d10 = d1 - i7;
            double d12 = d5 - i9;
            if (this.IsBlockTranslucent(i7, i8, i9) || this.IsBlockTranslucent(i7, i8 + 1, i9))
            {
                bool z14 = !this.IsBlockTranslucent(i7 - 1, i8, i9) && !this.IsBlockTranslucent(i7 - 1, i8 + 1, i9);
                bool z15 = !this.IsBlockTranslucent(i7 + 1, i8, i9) && !this.IsBlockTranslucent(i7 + 1, i8 + 1, i9);
                bool z16 = !this.IsBlockTranslucent(i7, i8, i9 - 1) && !this.IsBlockTranslucent(i7, i8 + 1, i9 - 1);
                bool z17 = !this.IsBlockTranslucent(i7, i8, i9 + 1) && !this.IsBlockTranslucent(i7, i8 + 1, i9 + 1);
                sbyte b18 = -1;
                double d19 = 9999;
                if (z14 && d10 < d19)
                {
                    d19 = d10;
                    b18 = 0;
                }

                if (z15 && 1 - d10 < d19)
                {
                    d19 = 1 - d10;
                    b18 = 1;
                }

                if (z16 && d12 < d19)
                {
                    d19 = d12;
                    b18 = 4;
                }

                if (z17 && 1 - d12 < d19)
                {
                    d19 = 1 - d12;
                    b18 = 5;
                }

                float f21 = 0.1F;
                if (b18 == 0)
                {
                    this.motionX = (-f21);
                }

                if (b18 == 1)
                {
                    this.motionX = f21;
                }

                if (b18 == 4)
                {
                    this.motionZ = (-f21);
                }

                if (b18 == 5)
                {
                    this.motionZ = f21;
                }
            }

            return false;
        }
    }
}
