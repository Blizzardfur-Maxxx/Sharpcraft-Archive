using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Items
{
    public class FishingHook : Entity
    {
        private int xTile = -1;
        private int yTile = -1;
        private int zTile = -1;
        private int inTile = 0;
        private bool inGround = false;
        public int shake = 0;
        public Player owner;
        private int ticksInGround;
        private int ticksInAir = 0;
        private int ticksCatchable = 0;
        public Entity bobber = null;
        private int field_L;
        private double field_M;
        private double field_N;
        private double field_O;
        private double field_P;
        private double field_Q;
        private double velocityX;
        private double velocityY;
        private double velocityZ;
        public FishingHook(Level world1) : base(world1)
        {
            this.SetSize(0.25F, 0.25F);
            this.ignoreFrustumCheck = true;
        }

        public FishingHook(Level world1, double d2, double d4, double d6) : this(world1)
        {
            this.SetPosition(d2, d4, d6);
            this.ignoreFrustumCheck = true;
        }

        public FishingHook(Level world1, Player entityPlayer2) : base(world1)
        {
            this.ignoreFrustumCheck = true;
            this.owner = entityPlayer2;
            this.owner.fishEntity = this;
            this.SetSize(0.25F, 0.25F);
            this.SetLocationAndAngles(entityPlayer2.x, entityPlayer2.y + 1.62 - entityPlayer2.yOffset, entityPlayer2.z, entityPlayer2.yaw, entityPlayer2.pitch);
            this.x -= Mth.Cos(this.yaw / 180F * Mth.PI) * 0.16F;
            this.y -= 0.1F;
            this.z -= Mth.Sin(this.yaw / 180F * Mth.PI) * 0.16F;
            this.SetPosition(this.x, this.y, this.z);
            this.yOffset = 0F;
            float f3 = 0.4F;
            this.motionX = -Mth.Sin(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI) * f3;
            this.motionZ = Mth.Cos(this.yaw / 180F * Mth.PI) * Mth.Cos(this.pitch / 180F * Mth.PI) * f3;
            this.motionY = -Mth.Sin(this.pitch / 180F * Mth.PI) * f3;
            this.Shoot(this.motionX, this.motionY, this.motionZ, 1.5F, 1F);
        }

        protected override void EntityInit()
        {
        }

        public override bool IsInRangeToRenderDist(double d1)
        {
            double d3 = this.boundingBox.GetAverageEdgeLength() * 4;
            d3 *= 64;
            return d1 < d3 * d3;
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

        public override void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            this.field_M = d1;
            this.field_N = d3;
            this.field_O = d5;
            this.field_P = f7;
            this.field_Q = f8;
            this.field_L = i9;
            this.motionX = this.velocityX;
            this.motionY = this.velocityY;
            this.motionZ = this.velocityZ;
        }

        public override void SetVelocity(double d1, double d3, double d5)
        {
            this.velocityX = this.motionX = d1;
            this.velocityY = this.motionY = d3;
            this.velocityZ = this.motionZ = d5;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.field_L > 0)
            {
                double d21 = this.x + (this.field_M - this.x) / this.field_L;
                double d22 = this.y + (this.field_N - this.y) / this.field_L;
                double d23 = this.z + (this.field_O - this.z) / this.field_L;
                double d7;
                for (d7 = this.field_P - this.yaw; d7 < -180; d7 += 360)
                {
                }

                while (d7 >= 180)
                {
                    d7 -= 360;
                }

                this.yaw = (float)(this.yaw + d7 / this.field_L);
                this.pitch = (float)(this.pitch + (this.field_Q - this.pitch) / this.field_L);
                --this.field_L;
                this.SetPosition(d21, d22, d23);
                this.SetRotation(this.yaw, this.pitch);
            }
            else
            {
                if (!this.worldObj.isRemote)
                {
                    ItemInstance itemStack1 = this.owner.GetCurrentEquippedItem();
                    if (this.owner.isDead || !this.owner.IsEntityAlive() || itemStack1 == null || itemStack1.GetItem() != Item.fishingRod || this.GetDistanceSqToEntity(this.owner) > 1024)
                    {
                        this.SetEntityDead();
                        this.owner.fishEntity = null;
                        return;
                    }

                    if (this.bobber != null)
                    {
                        if (!this.bobber.isDead)
                        {
                            this.x = this.bobber.x;
                            this.y = this.bobber.boundingBox.y0 + this.bobber.height * 0.8;
                            this.z = this.bobber.z;
                            return;
                        }

                        this.bobber = null;
                    }
                }

                if (this.shake > 0)
                {
                    --this.shake;
                }

                if (this.inGround)
                {
                    int i19 = this.worldObj.GetTile(this.xTile, this.yTile, this.zTile);
                    if (i19 == this.inTile)
                    {
                        ++this.ticksInGround;
                        if (this.ticksInGround == 1200)
                        {
                            this.SetEntityDead();
                        }

                        return;
                    }

                    this.inGround = false;
                    this.motionX *= this.rand.NextFloat() * 0.2F;
                    this.motionY *= this.rand.NextFloat() * 0.2F;
                    this.motionZ *= this.rand.NextFloat() * 0.2F;
                    this.ticksInGround = 0;
                    this.ticksInAir = 0;
                }
                else
                {
                    ++this.ticksInAir;
                }

                Vec3 vec3D20 = Vec3.Of(this.x, this.y, this.z);
                Vec3 vec3D2 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
                HitResult movingObjectPosition3 = this.worldObj.Clip(vec3D20, vec3D2);
                vec3D20 = Vec3.Of(this.x, this.y, this.z);
                vec3D2 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
                if (movingObjectPosition3 != null)
                {
                    vec3D2 = Vec3.Of(movingObjectPosition3.HitVec.x, movingObjectPosition3.HitVec.y, movingObjectPosition3.HitVec.z);
                }

                Entity entity4 = null;
                IList<Entity> list5 = this.worldObj.GetEntities(this, this.boundingBox.AddCoord(this.motionX, this.motionY, this.motionZ).Expand(1, 1, 1));
                double d6 = 0;
                double d13;
                for (int i8 = 0; i8 < list5.Count; ++i8)
                {
                    Entity entity9 = list5[i8];
                    if (entity9.CanBeCollidedWith() && (entity9 != this.owner || this.ticksInAir >= 5))
                    {
                        float f10 = 0.3F;
                        AABB axisAlignedBB11 = entity9.boundingBox.Expand(f10, f10, f10);
                        HitResult movingObjectPosition12 = axisAlignedBB11.Clip(vec3D20, vec3D2);
                        if (movingObjectPosition12 != null)
                        {
                            d13 = vec3D20.DistanceTo(movingObjectPosition12.HitVec);
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

                if (movingObjectPosition3 != null)
                {
                    if (movingObjectPosition3.EntityHit != null)
                    {
                        if (movingObjectPosition3.EntityHit.AttackEntityFrom(this.owner, 0))
                        {
                            this.bobber = movingObjectPosition3.EntityHit;
                        }
                    }
                    else
                    {
                        this.inGround = true;
                    }
                }

                if (!this.inGround)
                {
                    this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                    float f24 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                    this.yaw = (float)(Math.Atan2(this.motionX, this.motionZ) * 180 / Mth.PI);
                    for (this.pitch = (float)(Math.Atan2(this.motionY, f24) * 180 / Mth.PI); this.pitch - this.prevPitch < -180F; this.prevPitch -= 360F)
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
                    float f25 = 0.92F;
                    if (this.onGround || this.isCollidedHorizontally)
                    {
                        f25 = 0.5F;
                    }

                    byte b26 = 5;
                    double d27 = 0;
                    for (int i28 = 0; i28 < b26; ++i28)
                    {
                        double d14 = this.boundingBox.y0 + (this.boundingBox.y1 - this.boundingBox.y0) * (i28 + 0) / b26 - 0.125 + 0.125;
                        double d16 = this.boundingBox.y0 + (this.boundingBox.y1 - this.boundingBox.y0) * (i28 + 1) / b26 - 0.125 + 0.125;
                        AABB axisAlignedBB18 = AABB.Of(this.boundingBox.x0, d14, this.boundingBox.z0, this.boundingBox.x1, d16, this.boundingBox.z1);
                        if (this.worldObj.ContainsLiquid(axisAlignedBB18, Material.water))
                        {
                            d27 += 1 / b26;
                        }
                    }

                    if (d27 > 0)
                    {
                        if (this.ticksCatchable > 0)
                        {
                            --this.ticksCatchable;
                        }
                        else
                        {
                            short s29 = 500;
                            if (this.worldObj.CanLightningStrikeAt(Mth.Floor(this.x), Mth.Floor(this.y) + 1, Mth.Floor(this.z)))
                            {
                                s29 = 300;
                            }

                            if (this.rand.NextInt(s29) == 0)
                            {
                                this.ticksCatchable = this.rand.NextInt(30) + 10;
                                this.motionY -= 0.2F;
                                this.worldObj.PlaySound(this, "random.splash", 0.25F, 1F + (this.rand.NextFloat() - this.rand.NextFloat()) * 0.4F);
                                float f30 = Mth.Floor(this.boundingBox.y0);
                                int i15;
                                float f17;
                                float f31;
                                for (i15 = 0; i15 < 1F + this.width * 20F; ++i15)
                                {
                                    f31 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                                    f17 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                                    this.worldObj.AddParticle("bubble", this.x + f31, f30 + 1F, this.z + f17, this.motionX, this.motionY - this.rand.NextFloat() * 0.2F, this.motionZ);
                                }

                                for (i15 = 0; i15 < 1F + this.width * 20F; ++i15)
                                {
                                    f31 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                                    f17 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                                    this.worldObj.AddParticle("splash", this.x + f31, f30 + 1F, this.z + f17, this.motionX, this.motionY, this.motionZ);
                                }
                            }
                        }
                    }

                    if (this.ticksCatchable > 0)
                    {
                        this.motionY -= this.rand.NextFloat() * this.rand.NextFloat() * this.rand.NextFloat() * 0.2;
                    }

                    d13 = d27 * 2 - 1;
                    this.motionY += 0.04F * d13;
                    if (d27 > 0)
                    {
                        f25 = (float)(f25 * 0.9);
                        this.motionY *= 0.8;
                    }

                    this.motionX *= f25;
                    this.motionY *= f25;
                    this.motionZ *= f25;
                    this.SetPosition(this.x, this.y, this.z);
                }
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("xTile", (short)this.xTile);
            nBTTagCompound1.SetShort("yTile", (short)this.yTile);
            nBTTagCompound1.SetShort("zTile", (short)this.zTile);
            nBTTagCompound1.SetByte("inTile", (byte)this.inTile);
            nBTTagCompound1.SetByte("shake", (byte)this.shake);
            nBTTagCompound1.SetByte("inGround", (byte)(this.inGround ? 1 : 0));
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.xTile = nBTTagCompound1.GetShort("xTile");
            this.yTile = nBTTagCompound1.GetShort("yTile");
            this.zTile = nBTTagCompound1.GetShort("zTile");
            this.inTile = nBTTagCompound1.GetByte("inTile") & 255;
            this.shake = nBTTagCompound1.GetByte("shake") & 255;
            this.inGround = nBTTagCompound1.GetByte("inGround") == 1;
        }

        public override float GetShadowSize()
        {
            return 0F;
        }

        public virtual int CatchFish()
        {
            byte b1 = 0;
            if (this.bobber != null)
            {
                double d2 = this.owner.x - this.x;
                double d4 = this.owner.y - this.y;
                double d6 = this.owner.z - this.z;
                double d8 = Mth.Sqrt(d2 * d2 + d4 * d4 + d6 * d6);
                double d10 = 0.1;
                this.bobber.motionX += d2 * d10;
                this.bobber.motionY += d4 * d10 + Mth.Sqrt(d8) * 0.08;
                this.bobber.motionZ += d6 * d10;
                b1 = 3;
            }
            else if (this.ticksCatchable > 0)
            {
                ItemEntity entityItem13 = new ItemEntity(this.worldObj, this.x, this.y, this.z, new ItemInstance(Item.fishRaw));
                double d3 = this.owner.x - this.x;
                double d5 = this.owner.y - this.y;
                double d7 = this.owner.z - this.z;
                double d9 = Mth.Sqrt(d3 * d3 + d5 * d5 + d7 * d7);
                double d11 = 0.1;
                entityItem13.motionX = d3 * d11;
                entityItem13.motionY = d5 * d11 + Mth.Sqrt(d9) * 0.08;
                entityItem13.motionZ = d7 * d11;
                this.worldObj.AddEntity(entityItem13);
                this.owner.AddStat(StatList.fishCaught, 1);
                b1 = 1;
            }

            if (this.inGround)
            {
                b1 = 2;
            }

            this.SetEntityDead();
            this.owner.fishEntity = null;
            return b1;
        }
    }
}