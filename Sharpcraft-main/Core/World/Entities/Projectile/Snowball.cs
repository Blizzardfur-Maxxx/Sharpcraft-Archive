using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Projectile
{
    public class Snowball : Entity
    {
        private int xTileSnowball = -1;
        private int yTileSnowball = -1;
        private int zTileSnowball = -1;
        private int inTileSnowball = 0;
        private bool inGroundSnowball = false;
        public int shakeSnowball = 0;
        private Mob owner;
        private int ticksInGround;
        private int ticksInAir = 0;
        public Snowball(Level world1) : base(world1)
        {
            this.SetSize(0.25F, 0.25F);
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

        public Snowball(Level world1, Mob entityLiving2) : base(world1)
        {
            this.owner = entityLiving2;
            this.SetSize(0.25F, 0.25F);
            this.SetLocationAndAngles(entityLiving2.x, entityLiving2.y + entityLiving2.GetEyeHeight(), entityLiving2.z, entityLiving2.yaw, entityLiving2.pitch);
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

        public Snowball(Level world1, double d2, double d4, double d6) : base(world1)
        {
            this.ticksInGround = 0;
            this.SetSize(0.25F, 0.25F);
            this.SetPosition(d2, d4, d6);
            this.yOffset = 0F;
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
            }
        }

        public override void OnUpdate()
        {
            this.lastTickPosX = this.x;
            this.lastTickPosY = this.y;
            this.lastTickPosZ = this.z;
            base.OnUpdate();
            if (this.shakeSnowball > 0)
            {
                --this.shakeSnowball;
            }

            if (this.inGroundSnowball)
            {
                int i1 = this.worldObj.GetTile(this.xTileSnowball, this.yTileSnowball, this.zTileSnowball);
                if (i1 == this.inTileSnowball)
                {
                    ++this.ticksInGround;
                    if (this.ticksInGround == 1200)
                    {
                        this.SetEntityDead();
                    }

                    return;
                }

                this.inGroundSnowball = false;
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

            Vec3 vec3D15 = Vec3.Of(this.x, this.y, this.z);
            Vec3 vec3D2 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
            HitResult movingObjectPosition3 = this.worldObj.Clip(vec3D15, vec3D2);
            vec3D15 = Vec3.Of(this.x, this.y, this.z);
            vec3D2 = Vec3.Of(this.x + this.motionX, this.y + this.motionY, this.z + this.motionZ);
            if (movingObjectPosition3 != null)
            {
                vec3D2 = Vec3.Of(movingObjectPosition3.HitVec.x, movingObjectPosition3.HitVec.y, movingObjectPosition3.HitVec.z);
            }

            if (!this.worldObj.isRemote)
            {
                Entity entity4 = null;
                IList<Entity> list5 = this.worldObj.GetEntities(this, this.boundingBox.AddCoord(this.motionX, this.motionY, this.motionZ).Expand(1, 1, 1));
                double d6 = 0;
                for (int i8 = 0; i8 < list5.Count; ++i8)
                {
                    Entity entity9 = list5[i8];
                    if (entity9.CanBeCollidedWith() && (entity9 != this.owner || this.ticksInAir >= 5))
                    {
                        float f10 = 0.3F;
                        AABB axisAlignedBB11 = entity9.boundingBox.Expand(f10, f10, f10);
                        HitResult movingObjectPosition12 = axisAlignedBB11.Clip(vec3D15, vec3D2);
                        if (movingObjectPosition12 != null)
                        {
                            double d13 = vec3D15.DistanceTo(movingObjectPosition12.HitVec);
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
            }

            if (movingObjectPosition3 != null)
            {
                if (movingObjectPosition3.EntityHit != null && movingObjectPosition3.EntityHit.AttackEntityFrom(this.owner, 0))
                {
                }

                for (int i16 = 0; i16 < 8; ++i16)
                {
                    this.worldObj.AddParticle("snowballpoof", this.x, this.y, this.z, 0, 0, 0);
                }

                this.SetEntityDead();
            }

            this.x += this.motionX;
            this.y += this.motionY;
            this.z += this.motionZ;
            float f17 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
            this.yaw = (float)(Math.Atan2(this.motionX, this.motionZ) * 180 / Mth.PI);
            for (this.pitch = (float)(Math.Atan2(this.motionY, f17) * 180 / Mth.PI); this.pitch - this.prevPitch < -180F; this.prevPitch -= 360F)
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
            float f18 = 0.99F;
            float f19 = 0.03F;
            if (this.IsInWater())
            {
                for (int i7 = 0; i7 < 4; ++i7)
                {
                    float f20 = 0.25F;
                    this.worldObj.AddParticle("bubble", this.x - this.motionX * f20, this.y - this.motionY * f20, this.z - this.motionZ * f20, this.motionX, this.motionY, this.motionZ);
                }

                f18 = 0.8F;
            }

            this.motionX *= f18;
            this.motionY *= f18;
            this.motionZ *= f18;
            this.motionY -= f19;
            this.SetPosition(this.x, this.y, this.z);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("xTile", (short)this.xTileSnowball);
            nBTTagCompound1.SetShort("yTile", (short)this.yTileSnowball);
            nBTTagCompound1.SetShort("zTile", (short)this.zTileSnowball);
            nBTTagCompound1.SetByte("inTile", (byte)this.inTileSnowball);
            nBTTagCompound1.SetByte("shake", (byte)this.shakeSnowball);
            nBTTagCompound1.SetByte("inGround", (byte)(this.inGroundSnowball ? 1 : 0));
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.xTileSnowball = nBTTagCompound1.GetShort("xTile");
            this.yTileSnowball = nBTTagCompound1.GetShort("yTile");
            this.zTileSnowball = nBTTagCompound1.GetShort("zTile");
            this.inTileSnowball = nBTTagCompound1.GetByte("inTile") & 255;
            this.shakeSnowball = nBTTagCompound1.GetByte("shake") & 255;
            this.inGroundSnowball = nBTTagCompound1.GetByte("inGround") == 1;
        }

        public override void OnCollideWithPlayer(Player entityPlayer1)
        {
            if (this.inGroundSnowball && this.owner == entityPlayer1 && this.shakeSnowball <= 0 && entityPlayer1.inventory.AddItem(new ItemInstance(Item.arrow, 1)))
            {
                this.worldObj.PlaySound(this, "random.pop", 0.2F, ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.7F + 1F) * 2F);
                entityPlayer1.OnItemPickup(this, 1);
                this.SetEntityDead();
            }
        }

        public override float GetShadowSize()
        {
            return 0F;
        }
    }
}