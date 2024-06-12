using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Entities.Items
{
    public class FallingTile : Entity
    {
        public int blockID;
        public int fallTime = 0;
        public FallingTile(Level world1) : base(world1)
        {
        }

        public FallingTile(Level world1, double d2, double d4, double d6, int i8) : base(world1)
        {
            this.blockID = i8;
            this.preventEntitySpawning = true;
            this.SetSize(0.98F, 0.98F);
            this.yOffset = this.height / 2F;
            this.SetPosition(d2, d4, d6);
            this.motionX = 0;
            this.motionY = 0;
            this.motionZ = 0;
            this.prevX = d2;
            this.prevY = d4;
            this.prevZ = d6;
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        protected override void EntityInit()
        {
        }

        public override bool CanBeCollidedWith()
        {
            return !this.isDead;
        }

        public override void OnUpdate()
        {
            if (this.blockID == 0)
            {
                this.SetEntityDead();
            }
            else
            {
                this.prevX = this.x;
                this.prevY = this.y;
                this.prevZ = this.z;
                ++this.fallTime;
                this.motionY -= 0.04F;
                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= 0.98F;
                this.motionY *= 0.98F;
                this.motionZ *= 0.98F;
                int i1 = Mth.Floor(this.x);
                int i2 = Mth.Floor(this.y);
                int i3 = Mth.Floor(this.z);
                if (this.worldObj.GetTile(i1, i2, i3) == this.blockID)
                {
                    this.worldObj.SetTile(i1, i2, i3, 0);
                }

                if (this.onGround)
                {
                    this.motionX *= 0.7F;
                    this.motionZ *= 0.7F;
                    this.motionY *= -0.5;
                    this.SetEntityDead();
                    if ((!this.worldObj.CanBlockBePlacedAt(this.blockID, i1, i2, i3, true, Facing.TileFace.UP) || SandTile.CanFallBelow(this.worldObj, i1, i2 - 1, i3) || !this.worldObj.SetTile(i1, i2, i3, this.blockID)) && !this.worldObj.isRemote)
                    {
                        this.DropItem(this.blockID, 1);
                    }
                }
                else if (this.fallTime > 100 && !this.worldObj.isRemote)
                {
                    this.DropItem(this.blockID, 1);
                    this.SetEntityDead();
                }
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetByte("Tile", (byte)this.blockID);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.blockID = nBTTagCompound1.GetByte("Tile") & 255;
        }

        public override float GetShadowSize()
        {
            return 0F;
        }

        public virtual Level GetWorld()
        {
            return this.worldObj;
        }
    }
}