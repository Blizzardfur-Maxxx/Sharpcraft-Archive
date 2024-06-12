using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Client.Network;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Network;

namespace SharpCraft.Client.Players
{
    public class MultiplayerLocalPlayer : LocalPlayer
    {
        public ClientConnection sendQueue;
        private int field_9380_bx = 0;
        private bool field_21093_bH = false;
        private double oldPosX;
        private double field_9378_bz;
        private double oldPosY;
        private double oldPosZ;
        private float oldRotationYaw;
        private float oldRotationPitch;
        private bool field_9382_bF = false;
        private bool wasSneaking = false;
        private int field_12242_bI = 0;

        public MultiplayerLocalPlayer(Client instance, Level world2, User session3, ClientConnection netClientHandler4) :
            base(instance, world2, session3, 0)
        {
            this.sendQueue = netClientHandler4;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            return false;
        }

        public override void Heal(int i1)
        {
        }

        public override void OnUpdate()
        {
            if (this.worldObj.HasChunkAt(Mth.Floor(this.x), 64, Mth.Floor(this.z)))
            {
                base.OnUpdate();
                this.Func_4056_N();
            }
        }

        public virtual void Func_4056_N()
        {
            if (this.field_9380_bx++ == 20)
            {
                this.SendInventoryChanged();
                this.field_9380_bx = 0;
            }

            bool z1 = this.IsSneaking();
            if (z1 != this.wasSneaking)
            {
                if (z1)
                {
                    this.sendQueue.AddToSendQueue(new Packet19EntityAction(this, EntityActionType.SNEAK));
                }
                else
                {
                    this.sendQueue.AddToSendQueue(new Packet19EntityAction(this, EntityActionType.UNSNEAK));
                }

                this.wasSneaking = z1;
            }

            double d2 = this.x - this.oldPosX;
            double d4 = this.boundingBox.y0 - this.field_9378_bz;
            double d6 = this.y - this.oldPosY;
            double d8 = this.z - this.oldPosZ;
            double d10 = this.yaw - this.oldRotationYaw;
            double d12 = this.pitch - this.oldRotationPitch;
            bool z14 = d4 != 0 || d6 != 0 || d2 != 0 || d8 != 0;
            bool z15 = d10 != 0 || d12 != 0;
            if (this.ridingEntity != null)
            {
                if (z15)
                {
                    this.sendQueue.AddToSendQueue(new Packet11PlayerPosition(this.motionX, -999, -999, this.motionZ, this.onGround));
                }
                else
                {
                    this.sendQueue.AddToSendQueue(new Packet13PlayerLookMove(this.motionX, -999, -999, this.motionZ, this.yaw, this.pitch, this.onGround));
                }

                z14 = false;
            }
            else if (z14 && z15)
            {
                this.sendQueue.AddToSendQueue(new Packet13PlayerLookMove(this.x, this.boundingBox.y0, this.y, this.z, this.yaw, this.pitch, this.onGround));
                this.field_12242_bI = 0;
            }
            else if (z14)
            {
                this.sendQueue.AddToSendQueue(new Packet11PlayerPosition(this.x, this.boundingBox.y0, this.y, this.z, this.onGround));
                this.field_12242_bI = 0;
            }
            else if (z15)
            {
                this.sendQueue.AddToSendQueue(new Packet12PlayerLook(this.yaw, this.pitch, this.onGround));
                this.field_12242_bI = 0;
            }
            else
            {
                this.sendQueue.AddToSendQueue(new Packet10Flying(this.onGround));
                if (this.field_9382_bF == this.onGround && this.field_12242_bI <= 200)
                {
                    ++this.field_12242_bI;
                }
                else
                {
                    this.field_12242_bI = 0;
                }
            }

            this.field_9382_bF = this.onGround;
            if (z14)
            {
                this.oldPosX = this.x;
                this.field_9378_bz = this.boundingBox.y0;
                this.oldPosY = this.y;
                this.oldPosZ = this.z;
            }

            if (z15)
            {
                this.oldRotationYaw = this.yaw;
                this.oldRotationPitch = this.pitch;
            }
        }

        public override void DropCurrentItem()
        {
            this.sendQueue.AddToSendQueue(new Packet14BlockDig(PlayerDigActionType.DROP_ITEM, 0, 0, 0, Facing.TileFace.DOWN));
        }

        private void SendInventoryChanged()
        {
        }

        protected override void JoinEntityItemWithWorld(ItemEntity entityItem1)
        {
        }

        public override void SendChatMessage(string string1)
        {
            this.sendQueue.AddToSendQueue(new Packet3Chat(string1));
        }

        public override void SwingItem()
        {
            base.SwingItem();
            this.sendQueue.AddToSendQueue(new Packet18Animation(this, EntityAnimationType.SWING));
        }

        public override void RespawnPlayer()
        {
            this.SendInventoryChanged();
            this.sendQueue.AddToSendQueue(new Packet9Respawn((sbyte)this.dimension));
        }

        protected override void DamageEntity(int i1)
        {
            this.health -= i1;
        }

        protected override void CloseScreen()
        {
            this.sendQueue.AddToSendQueue(new Packet101CloseWindow(this.curCraftingInventory.windowId));
            this.inventory.SetItem(null);
            base.CloseScreen();
        }

        public override void SetHealth(int i1)
        {
            if (this.field_21093_bH)
            {
                base.SetHealth(i1);
            }
            else
            {
                this.health = i1;
                this.field_21093_bH = true;
            }
        }

        public override void AddStat(Stat statBase1, int i2)
        {
            if (statBase1 != null)
            {
                if (statBase1.field_g)
                {
                    base.AddStat(statBase1, i2);
                }
            }
        }

        public virtual void Func_27027_b(Stat statBase1, int i2)
        {
            if (statBase1 != null)
            {
                if (!statBase1.field_g)
                {
                    base.AddStat(statBase1, i2);
                }
            }
        }
    }
}
