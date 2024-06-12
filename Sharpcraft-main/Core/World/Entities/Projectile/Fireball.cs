using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Projectile
{
    public class Fireball : Entity
    {
        private int xTile = -1;
        private int yTile = -1;
        private int zTile = -1;
        private int inTile = 0;
        private bool inGround = false;
        public int shake = 0;
        public Mob owner;
        private int field_k;
        private int ticksInAir = 0;
        public double xOff;
        public double yOff;
        public double zOff;
        public Fireball(Level world1) : base(world1)
        {
            this.SetSize(1F, 1F);
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

        public Fireball(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1)
        {
            this.SetSize(1F, 1F);
            this.SetLocationAndAngles(d2, d4, d6, this.yaw, this.pitch);
            this.SetPosition(d2, d4, d6);
            double d14 = Mth.Sqrt(d8 * d8 + d10 * d10 + d12 * d12);
            this.xOff = d8 / d14 * 0.1;
            this.yOff = d10 / d14 * 0.1;
            this.zOff = d12 / d14 * 0.1;
        }

        public Fireball(Level world1, Mob entityLiving2, double d3, double d5, double d7) : base(world1)
        {
            this.owner = entityLiving2;
            this.SetSize(1F, 1F);
            this.SetLocationAndAngles(entityLiving2.x, entityLiving2.y, entityLiving2.z, entityLiving2.yaw, entityLiving2.pitch);
            this.SetPosition(this.x, this.y, this.z);
            this.yOffset = 0F;
            this.motionX = this.motionY = this.motionZ = 0;
            d3 += this.rand.NextGaussian() * 0.4;
            d5 += this.rand.NextGaussian() * 0.4;
            d7 += this.rand.NextGaussian() * 0.4;
            double d9 = Mth.Sqrt(d3 * d3 + d5 * d5 + d7 * d7);
            this.xOff = d3 / d9 * 0.1;
            this.yOff = d5 / d9 * 0.1;
            this.zOff = d7 / d9 * 0.1;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.fire = 10;
            if (this.shake > 0)
            {
                --this.shake;
            }

            if (this.inGround)
            {
                int i1 = this.worldObj.GetTile(this.xTile, this.yTile, this.zTile);
                if (i1 == this.inTile)
                {
                    ++this.field_k;
                    if (this.field_k == 1200)
                    {
                        this.SetEntityDead();
                    }

                    return;
                }

                this.inGround = false;
                this.motionX *= this.rand.NextFloat() * 0.2F;
                this.motionY *= this.rand.NextFloat() * 0.2F;
                this.motionZ *= this.rand.NextFloat() * 0.2F;
                this.field_k = 0;
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

            Entity entity4 = null;
            IList<Entity> list5 = this.worldObj.GetEntities(this, this.boundingBox.AddCoord(this.motionX, this.motionY, this.motionZ).Expand(1, 1, 1));
            double d6 = 0;
            for (int i8 = 0; i8 < list5.Count; ++i8)
            {
                Entity entity9 = list5[i8];
                if (entity9.CanBeCollidedWith() && (entity9 != this.owner || this.ticksInAir >= 25))
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

            if (movingObjectPosition3 != null)
            {
                if (!this.worldObj.isRemote)
                {
                    if (movingObjectPosition3.EntityHit != null && movingObjectPosition3.EntityHit.AttackEntityFrom(this.owner, 0))
                    {
                    }

                    this.worldObj.Explode((Entity)null, this.x, this.y, this.z, 1F, true);
                }

                this.SetEntityDead();
            }

            this.x += this.motionX;
            this.y += this.motionY;
            this.z += this.motionZ;
            float f16 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
            this.yaw = (float)(Math.Atan2(this.motionX, this.motionZ) * 180 / Mth.PI);
            for (this.pitch = (float)(Math.Atan2(this.motionY, f16) * 180 / Mth.PI); this.pitch - this.prevPitch < -180F; this.prevPitch -= 360F)
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
            float f17 = 0.95F;
            if (this.IsInWater())
            {
                for (int i18 = 0; i18 < 4; ++i18)
                {
                    float f19 = 0.25F;
                    this.worldObj.AddParticle("bubble", this.x - this.motionX * f19, this.y - this.motionY * f19, this.z - this.motionZ * f19, this.motionX, this.motionY, this.motionZ);
                }

                f17 = 0.8F;
            }

            this.motionX += this.xOff;
            this.motionY += this.yOff;
            this.motionZ += this.zOff;
            this.motionX *= f17;
            this.motionY *= f17;
            this.motionZ *= f17;
            this.worldObj.AddParticle("smoke", this.x, this.y + 0.5, this.z, 0, 0, 0);
            this.SetPosition(this.x, this.y, this.z);
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

        public override bool CanBeCollidedWith()
        {
            return true;
        }

        public override float GetCollisionBorderSize()
        {
            return 1F;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            this.SetBeenAttacked();
            if (entity1 != null)
            {
                Vec3 vec3D3 = entity1.GetLookVec();
                if (vec3D3 != null)
                {
                    this.motionX = vec3D3.x;
                    this.motionY = vec3D3.y;
                    this.motionZ = vec3D3.z;
                    this.xOff = this.motionX * 0.1;
                    this.yOff = this.motionY * 0.1;
                    this.zOff = this.motionZ * 0.1;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public override float GetShadowSize()
        {
            return 0F;
        }
    }
}