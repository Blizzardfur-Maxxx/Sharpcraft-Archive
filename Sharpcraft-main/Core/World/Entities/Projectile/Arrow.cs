using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Projectile
{
    public class Arrow : Entity
    {
        private int xTile = -1;
        private int yTile = -1;
        private int zTile = -1;
        private int inTile = 0;
        private int inData = 0;
        private bool inGround = false;
        public bool playerShot = false;
        public int arrowShake = 0;
        public Mob owner;
        private int ticksInGround;
        private int ticksInAir = 0;
        public Arrow(Level world1) : base(world1)
        {
            this.SetSize(0.5F, 0.5F);
        }

        public Arrow(Level world1, double d2, double d4, double d6) : base(world1)
        {
            this.SetSize(0.5F, 0.5F);
            this.SetPosition(d2, d4, d6);
            this.yOffset = 0F;
        }

        public Arrow(Level world1, Mob entityLiving2) : base(world1)
        {
            this.owner = entityLiving2;
            this.playerShot = entityLiving2 is Player;
            this.SetSize(0.5F, 0.5F);
            this.SetLocationAndAngles(entityLiving2.x, entityLiving2.y + entityLiving2.GetEyeHeight(), entityLiving2.z, entityLiving2.yaw, entityLiving2.pitch);
            this.x -= Mth.Cos(this.yaw / 180F * Mth.PI) * 0.16F;
            this.y -= 0.1F;
            this.z -= Mth.Sin(this.yaw / 180F * Mth.PI) * 0.16F;
            this.SetPosition(this.x, this.y, this.z);
            this.yOffset = 0F;
            this.motionX = -Mth.Sin(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI);
            this.motionZ = Mth.Cos(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI);
            this.motionY = (-Mth.Sin(this.pitch / 180F * Mth.PI));
            this.Shoot(this.motionX, this.motionY, this.motionZ, 1.5F, 1F);
        }

        protected override void EntityInit()
        {
        }

        public virtual void Shoot(double d1, double d3, double d5, float f7, float f8)
        {
            float f9 = Mth.Sqrt(d1 * d1 + d3 * d3 + d5 * d5);
            d1 /= f9;
            d3 /= f9;
            d5 /= f9;
            d1 += this.rand.NextGaussian() * 0.0075F * f8;
            d3 += this.rand.NextGaussian() * 0.0075F * f8;
            d5 += this.rand.NextGaussian() * 0.0075F * f8;
            d1 *= f7;
            d3 *= f7;
            d5 *= f7;
            this.motionX = d1;
            this.motionY = d3;
            this.motionZ = d5;
            float f10 = Mth.Sqrt(d1 * d1 + d5 * d5);
            this.prevYaw = this.yaw = (float)(Math.Atan2(d1, d5) * 180 / Mth.PI);
            this.prevPitch = this.pitch = (float)(Math.Atan2(d3, f10) * 180 / Mth.PI);
            this.ticksInGround = 0;
        }

        public override void SetVelocity(double d1, double d3, double d5)
        {
            this.motionX = d1;
            this.motionY = d3;
            this.motionZ = d5;
            if (this.prevPitch == 0F && this.prevYaw == 0F)
            {
                float f7 = Mth.Sqrt(d1 * d1 + d5 * d5);
                this.prevYaw = this.yaw = (float)(Math.Atan2(d1, d5) * 180 / Mth.PI);
                this.prevPitch = this.pitch = (float)(Math.Atan2(d3, f7) * 180 / Mth.PI);
                this.prevPitch = this.pitch;
                this.prevYaw = this.yaw;
                this.SetLocationAndAngles(this.x, this.y, this.z, this.yaw, this.pitch);
                this.ticksInGround = 0;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.prevPitch == 0F && this.prevYaw == 0F)
            {
                float f1 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                this.prevYaw = this.yaw = (float)(Math.Atan2(this.motionX, this.motionZ) * 180 / Mth.PI);
                this.prevPitch = this.pitch = (float)(Math.Atan2(this.motionY, f1) * 180 / Mth.PI);
            }

            int i15 = this.worldObj.GetTile(this.xTile, this.yTile, this.zTile);
            if (i15 > 0)
            {
                Tile.tiles[i15].SetBlockBoundsBasedOnState(this.worldObj, this.xTile, this.yTile, this.zTile);
                AABB axisAlignedBB2 = Tile.tiles[i15].GetAABB(this.worldObj, this.xTile, this.yTile, this.zTile);
                if (axisAlignedBB2 != null && axisAlignedBB2.IsVecInside(Vec3.Of(this.x, this.y, this.z)))
                {
                    this.inGround = true;
                }
            }

            if (this.arrowShake > 0)
            {
                --this.arrowShake;
            }

            if (this.inGround)
            {
                i15 = this.worldObj.GetTile(this.xTile, this.yTile, this.zTile);
                int i18 = this.worldObj.GetData(this.xTile, this.yTile, this.zTile);
                if (i15 == this.inTile && i18 == this.inData)
                {
                    ++this.ticksInGround;
                    if (this.ticksInGround == 1200)
                    {
                        this.SetEntityDead();
                    }
                }
                else
                {
                    this.inGround = false;
                    this.motionX *= this.rand.NextFloat() * 0.2F;
                    this.motionY *= this.rand.NextFloat() * 0.2F;
                    this.motionZ *= this.rand.NextFloat() * 0.2F;
                    this.ticksInGround = 0;
                    this.ticksInAir = 0;
                }
            }
            else
            {
                ++this.ticksInAir;
                Vec3 vec3D16 = Vec3.Of(this.x, this.y, this.z);
                Vec3 vec3D17 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
                HitResult movingObjectPosition3 = this.worldObj.Clip(vec3D16, vec3D17, false, true);
                vec3D16 = Vec3.Of(this.x, this.y, this.z);
                vec3D17 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
                if (movingObjectPosition3 != null)
                {
                    vec3D17 = Vec3.Of(movingObjectPosition3.HitVec.x, movingObjectPosition3.HitVec.y, movingObjectPosition3.HitVec.z);
                }

                Entity entity4 = null;
                IList<Entity> list5 = this.worldObj.GetEntities(this, this.boundingBox.AddCoord(this.motionX, this.motionY, this.motionZ).Expand(1, 1, 1));
                double d6 = 0;
                float f10;
                for (int i8 = 0; i8 < list5.Count; ++i8)
                {
                    Entity entity9 = list5[i8];
                    if (entity9.CanBeCollidedWith() && (entity9 != this.owner || this.ticksInAir >= 5))
                    {
                        f10 = 0.3F;
                        AABB axisAlignedBB11 = entity9.boundingBox.Expand(f10, f10, f10);
                        HitResult movingObjectPosition12 = axisAlignedBB11.Clip(vec3D16, vec3D17);
                        if (movingObjectPosition12 != null)
                        {
                            double d13 = vec3D16.DistanceTo(movingObjectPosition12.HitVec);
                            if (d13 < d6 || d6 == 0)
                            {
                                entity4 = entity9;
                                d6 = d13;
                            }
                        }
                    }
                }

                if (entity4 != null)
                {
                    movingObjectPosition3 = new HitResult(entity4);
                }

                float f19;
                if (movingObjectPosition3 != null)
                {
                    if (movingObjectPosition3.EntityHit != null)
                    {
                        if (movingObjectPosition3.EntityHit.AttackEntityFrom(this.owner, 4))
                        {
                            this.worldObj.PlaySound(this, "random.drr", 1F, 1.2F / (this.rand.NextFloat() * 0.2F + 0.9F));
                            this.SetEntityDead();
                        }
                        else
                        {
                            this.motionX *= -0.10000000149011612;
                            this.motionY *= -0.10000000149011612;
                            this.motionZ *= -0.10000000149011612;
                            this.yaw += 180F;
                            this.prevYaw += 180F;
                            this.ticksInAir = 0;
                        }
                    }
                    else
                    {
                        this.xTile = movingObjectPosition3.BlockX;
                        this.yTile = movingObjectPosition3.BlockY;
                        this.zTile = movingObjectPosition3.BlockZ;
                        this.inTile = this.worldObj.GetTile(this.xTile, this.yTile, this.zTile);
                        this.inData = this.worldObj.GetData(this.xTile, this.yTile, this.zTile);
                        this.motionX = ((float)(movingObjectPosition3.HitVec.x - this.x));
                        this.motionY = ((float)(movingObjectPosition3.HitVec.y - this.y));
                        this.motionZ = ((float)(movingObjectPosition3.HitVec.z - this.z));
                        f19 = Mth.Sqrt(this.motionX * this.motionX + this.motionY * this.motionY + this.motionZ * this.motionZ);
                        this.x -= this.motionX / f19 * 0.05F;
                        this.y -= this.motionY / f19 * 0.05F;
                        this.z -= this.motionZ / f19 * 0.05F;
                        this.worldObj.PlaySound(this, "random.drr", 1F, 1.2F / (this.rand.NextFloat() * 0.2F + 0.9F));
                        this.inGround = true;
                        this.arrowShake = 7;
                    }
                }

                this.x += this.motionX;
                this.y += this.motionY;
                this.z += this.motionZ;
                f19 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                this.yaw = (float)(Math.Atan2(this.motionX, this.motionZ) * 180 / Mth.PI);
                for (this.pitch = (float)(Math.Atan2(this.motionY, f19) * 180 / Mth.PI); this.pitch - this.prevPitch < -180F; this.prevPitch -= 360F)
                {
                }

                while (this.pitch - this.prevPitch >= 180F)
                {
                    this.prevPitch += 360F;
                }

                while (this.yaw - this.prevYaw < -180F)
                {
                    this.prevYaw -= 360F;
                }

                while (this.yaw - this.prevYaw >= 180F)
                {
                    this.prevYaw += 360F;
                }

                this.pitch = this.prevPitch + (this.pitch - this.prevPitch) * 0.2F;
                this.yaw = this.prevYaw + (this.yaw - this.prevYaw) * 0.2F;
                float f20 = 0.99F;
                f10 = 0.03F;
                if (this.IsInWater())
                {
                    for (int i21 = 0; i21 < 4; ++i21)
                    {
                        float f22 = 0.25F;
                        this.worldObj.AddParticle("bubble", this.x - this.motionX * f22, this.y - this.motionY * f22, this.z - this.motionZ * f22, this.motionX, this.motionY, this.motionZ);
                    }

                    f20 = 0.8F;
                }

                this.motionX *= f20;
                this.motionY *= f20;
                this.motionZ *= f20;
                this.motionY -= f10;
                this.SetPosition(this.x, this.y, this.z);
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("xTile", (short)this.xTile);
            nBTTagCompound1.SetShort("yTile", (short)this.yTile);
            nBTTagCompound1.SetShort("zTile", (short)this.zTile);
            nBTTagCompound1.SetByte("inTile", (byte)this.inTile);
            nBTTagCompound1.SetByte("inData", (byte)this.inData);
            nBTTagCompound1.SetByte("shake", (byte)this.arrowShake);
            nBTTagCompound1.SetByte("inGround", (byte)(this.inGround ? 1 : 0));
            nBTTagCompound1.SetBoolean("player", this.playerShot);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.xTile = nBTTagCompound1.GetShort("xTile");
            this.yTile = nBTTagCompound1.GetShort("yTile");
            this.zTile = nBTTagCompound1.GetShort("zTile");
            this.inTile = nBTTagCompound1.GetByte("inTile") & 255;
            this.inData = nBTTagCompound1.GetByte("inData") & 255;
            this.arrowShake = nBTTagCompound1.GetByte("shake") & 255;
            this.inGround = nBTTagCompound1.GetByte("inGround") == 1;
            this.playerShot = nBTTagCompound1.GetBoolean("player");
        }

        public override void OnCollideWithPlayer(Player entityPlayer1)
        {
            if (!this.worldObj.isRemote)
            {
                if (this.inGround && this.playerShot && this.arrowShake <= 0 && entityPlayer1.inventory.AddItem(new ItemInstance(Item.arrow, 1)))
                {
                    this.worldObj.PlaySound(this, "random.pop", 0.2F, ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.7F + 1F) * 2F);
                    entityPlayer1.OnItemPickup(this, 1);
                    this.SetEntityDead();
                }
            }
        }

        public override float GetShadowSize()
        {
            return 0F;
        }
    }
}