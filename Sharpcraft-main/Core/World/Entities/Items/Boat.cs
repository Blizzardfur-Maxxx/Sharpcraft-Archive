using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Items
{
    public class Boat : Entity
    {
        public int boatCurrentDamage;
        public int boatTimeSinceHit;
        public int boatRockDirection;
        private int field_9394;
        private double field_9393;
        private double field_9392;
        private double field_9391;
        private double field_9390;
        private double field_9389;
        private double field_9388;
        private double field_9387;
        private double field_9386;
        public Boat(Level world1) : base(world1)
        {
            this.boatCurrentDamage = 0;
            this.boatTimeSinceHit = 0;
            this.boatRockDirection = 1;
            this.preventEntitySpawning = true;
            this.SetSize(1.5F, 0.6F);
            this.yOffset = this.height / 2F;
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        protected override void EntityInit()
        {
        }

        public override AABB GetCollisionBox(Entity entity1)
        {
            return entity1.boundingBox;
        }

        public override AABB GetBoundingBox()
        {
            return this.boundingBox;
        }

        public override bool CanBePushed()
        {
            return true;
        }

        public Boat(Level world1, double d2, double d4, double d6) : this(world1)
        {
            this.SetPosition(d2, d4 + this.yOffset, d6);
            this.motionX = 0;
            this.motionY = 0;
            this.motionZ = 0;
            this.prevX = d2;
            this.prevY = d4;
            this.prevZ = d6;
        }

        public override double GetMountedYOffset()
        {
            return this.height * 0 - 0.3F;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (!this.worldObj.isRemote && !this.isDead)
            {
                this.boatRockDirection = -this.boatRockDirection;
                this.boatTimeSinceHit = 10;
                this.boatCurrentDamage += i2 * 10;
                this.SetBeenAttacked();
                if (this.boatCurrentDamage > 40)
                {
                    if (this.riddenByEntity != null)
                    {
                        this.riddenByEntity.MountEntity(this);
                    }

                    int i3;
                    for (i3 = 0; i3 < 3; ++i3)
                    {
                        this.DropItemWithOffset(Tile.wood.id, 1, 0F);
                    }

                    for (i3 = 0; i3 < 2; ++i3)
                    {
                        this.DropItemWithOffset(Item.stick.id, 1, 0F);
                    }

                    this.SetEntityDead();
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public override void PerformHurtAnimation()
        {
            this.boatRockDirection = -this.boatRockDirection;
            this.boatTimeSinceHit = 10;
            this.boatCurrentDamage += this.boatCurrentDamage * 10;
        }

        public override bool CanBeCollidedWith()
        {
            return !this.isDead;
        }

        public override void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            this.field_9393 = d1;
            this.field_9392 = d3;
            this.field_9391 = d5;
            this.field_9390 = f7;
            this.field_9389 = f8;
            this.field_9394 = i9 + 4;
            this.motionX = this.field_9388;
            this.motionY = this.field_9387;
            this.motionZ = this.field_9386;
        }

        public override void SetVelocity(double d1, double d3, double d5)
        {
            this.field_9388 = this.motionX = d1;
            this.field_9387 = this.motionY = d3;
            this.field_9386 = this.motionZ = d5;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.boatTimeSinceHit > 0)
            {
                --this.boatTimeSinceHit;
            }

            if (this.boatCurrentDamage > 0)
            {
                --this.boatCurrentDamage;
            }

            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            byte b1 = 5;
            double d2 = 0;
            for (int i4 = 0; i4 < b1; ++i4)
            {
                double d5 = this.boundingBox.y0 + (this.boundingBox.y1 - this.boundingBox.y0) * (i4 + 0) / b1 - 0.125;
                double d7 = this.boundingBox.y0 + (this.boundingBox.y1 - this.boundingBox.y0) * (i4 + 1) / b1 - 0.125;
                AABB axisAlignedBB9 = AABB.Of(this.boundingBox.x0, d5, this.boundingBox.z0, this.boundingBox.x1, d7, this.boundingBox.z1);
                if (this.worldObj.ContainsLiquid(axisAlignedBB9, Material.water))
                {
                    d2 += 1.0D / b1;
                }
            }

            double d6;
            double d8;
            double d10;
            double d21;
            if (this.worldObj.isRemote)
            {
                if (this.field_9394 > 0)
                {
                    d21 = this.x + (this.field_9393 - this.x) / this.field_9394;
                    d6 = this.y + (this.field_9392 - this.y) / this.field_9394;
                    d8 = this.z + (this.field_9391 - this.z) / this.field_9394;
                    for (d10 = this.field_9390 - this.yaw; d10 < -180; d10 += 360)
                    {
                    }

                    while (d10 >= 180)
                    {
                        d10 -= 360;
                    }

                    this.yaw = (float)(this.yaw + d10 / this.field_9394);
                    this.pitch = (float)(this.pitch + (this.field_9389 - this.pitch) / this.field_9394);
                    --this.field_9394;
                    this.SetPosition(d21, d6, d8);
                    this.SetRotation(this.yaw, this.pitch);
                }
                else
                {
                    d21 = this.x + this.motionX;
                    d6 = this.y + this.motionY;
                    d8 = this.z + this.motionZ;
                    this.SetPosition(d21, d6, d8);
                    if (this.onGround)
                    {
                        this.motionX *= 0.5;
                        this.motionY *= 0.5;
                        this.motionZ *= 0.5;
                    }

                    this.motionX *= 0.99F;
                    this.motionY *= 0.95F;
                    this.motionZ *= 0.99F;
                }
            }
            else
            {
                if (d2 < 1)
                {
                    d21 = d2 * 2 - 1;
                    this.motionY += 0.04F * d21;
                }
                else
                {
                    if (this.motionY < 0)
                    {
                        this.motionY /= 2;
                    }

                    this.motionY += 0.007000000216066837;
                }

                if (this.riddenByEntity != null)
                {
                    this.motionX += this.riddenByEntity.motionX * 0.2;
                    this.motionZ += this.riddenByEntity.motionZ * 0.2;
                }

                d21 = 0.4;
                if (this.motionX < -d21)
                {
                    this.motionX = -d21;
                }

                if (this.motionX > d21)
                {
                    this.motionX = d21;
                }

                if (this.motionZ < -d21)
                {
                    this.motionZ = -d21;
                }

                if (this.motionZ > d21)
                {
                    this.motionZ = d21;
                }

                if (this.onGround)
                {
                    this.motionX *= 0.5;
                    this.motionY *= 0.5;
                    this.motionZ *= 0.5;
                }

                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                d6 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                if (d6 > 0.15)
                {
                    d8 = Math.Cos(this.yaw * Math.PI / 180);
                    d10 = Math.Sin(this.yaw * Math.PI / 180);
                    for (int i12 = 0; i12 < 1 + d6 * 60; ++i12)
                    {
                        double d13 = this.rand.NextFloat() * 2F - 1F;
                        double d15 = (this.rand.NextInt(2) * 2 - 1) * 0.7;
                        double d17;
                        double d19;
                        if (this.rand.NextBoolean())
                        {
                            d17 = this.x - d8 * d13 * 0.8 + d10 * d15;
                            d19 = this.z - d10 * d13 * 0.8 - d8 * d15;
                            this.worldObj.AddParticle("splash", d17, this.y - 0.125, d19, this.motionX, this.motionY, this.motionZ);
                        }
                        else
                        {
                            d17 = this.x + d8 + d10 * d13 * 0.7;
                            d19 = this.z + d10 - d8 * d13 * 0.7;
                            this.worldObj.AddParticle("splash", d17, this.y - 0.125, d19, this.motionX, this.motionY, this.motionZ);
                        }
                    }
                }

                if (this.isCollidedHorizontally && d6 > 0.15)
                {
                    if (!this.worldObj.isRemote)
                    {
                        this.SetEntityDead();
                        int i22;
                        for (i22 = 0; i22 < 3; ++i22)
                        {
                            this.DropItemWithOffset(Tile.wood.id, 1, 0F);
                        }

                        for (i22 = 0; i22 < 2; ++i22)
                        {
                            this.DropItemWithOffset(Item.stick.id, 1, 0F);
                        }
                    }
                }
                else
                {
                    this.motionX *= 0.99F;
                    this.motionY *= 0.95F;
                    this.motionZ *= 0.99F;
                }

                this.pitch = 0F;
                d8 = this.yaw;
                d10 = this.prevX - this.x;
                double d23 = this.prevZ - this.z;
                if (d10 * d10 + d23 * d23 > 0.001)
                {
                    d8 = ((float)(Math.Atan2(d23, d10) * 180 / Math.PI));
                }

                double d14;
                for (d14 = d8 - this.yaw; d14 >= 180; d14 -= 360)
                {
                }

                while (d14 < -180)
                {
                    d14 += 360;
                }

                if (d14 > 20)
                {
                    d14 = 20;
                }

                if (d14 < -20)
                {
                    d14 = -20;
                }

                this.yaw = (float)(this.yaw + d14);
                this.SetRotation(this.yaw, this.pitch);
                IList<Entity> list16 = this.worldObj.GetEntities(this, this.boundingBox.Expand(0.2F, 0, 0.2F));
                int i24;
                if (list16 != null && list16.Count > 0)
                {
                    for (i24 = 0; i24 < list16.Count; ++i24)
                    {
                        Entity entity18 = list16[i24];
                        if (entity18 != this.riddenByEntity && entity18.CanBePushed() && entity18 is Boat)
                        {
                            entity18.ApplyEntityCollision(this);
                        }
                    }
                }

                for (i24 = 0; i24 < 4; ++i24)
                {
                    int i25 = Mth.Floor(this.x + (i24 % 2 - 0.5) * 0.8);
                    int i26 = Mth.Floor(this.y);
                    int i20 = Mth.Floor(this.z + (i24 / 2 - 0.5) * 0.8);
                    if (this.worldObj.GetTile(i25, i26, i20) == Tile.topSnow.id)
                    {
                        this.worldObj.SetTile(i25, i26, i20, 0);
                    }
                }

                if (this.riddenByEntity != null && this.riddenByEntity.isDead)
                {
                    this.riddenByEntity = null;
                }
            }
        }

        public override void UpdateRiderPosition()
        {
            if (this.riddenByEntity != null)
            {
                double d1 = Math.Cos(this.yaw * Math.PI / 180) * 0.4;
                double d3 = Math.Sin(this.yaw * Math.PI / 180) * 0.4;
                this.riddenByEntity.SetPosition(this.x + d1, this.y + this.GetMountedYOffset() + this.riddenByEntity.GetYOffset(), this.z + d3);
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
        }

        public override float GetShadowSize()
        {
            return 0F;
        }

        public override bool Interact(Player entityPlayer1)
        {
            if (this.riddenByEntity != null && this.riddenByEntity is Player && this.riddenByEntity != entityPlayer1)
            {
                return true;
            }
            else
            {
                if (!this.worldObj.isRemote)
                {
                    entityPlayer1.MountEntity(this);
                }

                return true;
            }
        }
    }
}