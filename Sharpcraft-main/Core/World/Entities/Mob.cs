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
using static SharpCraft.Core.World.GameLevel.Tiles.Tile;

namespace SharpCraft.Core.World.Entities
{
    public abstract class Mob : Entity
    {
        public int heartsHalvesLife = 20;
        public float field_p;
        public float field_r;
        public float renderYawOffset = 0F;
        public float prevRenderYawOffset = 0F;
        protected float field_u;
        protected float field_v;
        protected float field_w;
        protected float field_x;
        protected bool field_y = true;
        protected string texture = "/mob/char.png";
        protected bool field_A = true;
        protected float field_B = 0F;
        protected string entityType = null;
        protected float field_D = 1F;
        protected int scoreValue = 0;
        protected float field_F = 0F;
        public bool isMultiplayerEntity = false;
        public float prevSwingProgress;
        public float swingProgress;
        public int health = 10;
        public int prevHealth;
        private int livingSoundTime;
        public int hurtTime;
        public int maxHurtTime;
        public float attackedAtYaw = 0F;
        public int deathTime = 0;
        public int attackTime = 0;
        public float cameraPitch;
        public float field_R;
        protected bool unused_flag = false;
        public int field_T = -1;
        public float field_U = (float)(Mth.Random() * 0.9F + 0.1F);
        public float field_Q;
        public float field_bd;
        public float field_ba;
        protected int newPosRotationIncrements;
        protected double newPosX;
        protected double newPosY;
        protected double newPosZ;
        protected double newRotationYaw;
        protected double newRotationPitch;
        protected int field_af = 0;
        protected int age = 0;
        protected float moveStrafing;
        protected float moveForward;
        protected float randomYawVelocity;
        protected bool isJumping = false;
        protected float defaultPitch = 0F;
        protected float moveSpeed = 0.7F;
        private Entity currentTarget;
        protected int numTicksToChaseTarget = 0;
        public Mob(Level world1) : base(world1)
        {
            this.preventEntitySpawning = true;
            this.field_r = (float)(Mth.Random() + 1) * 0.01F;
            this.SetPosition(this.x, this.y, this.z);
            this.field_p = (float)Mth.Random() * 12398F;
            this.yaw = (float)(Mth.Random() * Mth.PI * 2);
            this.stepHeight = 0.5F;
        }

        protected override void EntityInit()
        {
        }

        public virtual bool CanEntityBeSeen(Entity entity1)
        {
            return this.worldObj.Clip(Vec3.Of(this.x, this.y + this.GetEyeHeight(), this.z), Vec3.Of(entity1.x, entity1.y + entity1.GetEyeHeight(), entity1.z)) == null;
        }

        public override string GetEntityTexture()
        {
            return this.texture;
        }

        public override bool CanBeCollidedWith()
        {
            return !this.isDead;
        }

        public override bool CanBePushed()
        {
            return !this.isDead;
        }

        public override float GetEyeHeight()
        {
            return this.height * 0.85F;
        }

        public virtual int GetTalkInterval()
        {
            return 80;
        }

        public virtual void PlayLivingSound()
        {
            string string1 = this.GetLivingSound();
            if (string1 != null)
            {
                this.worldObj.PlaySound(this, string1, this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
            }
        }

        public override void OnEntityUpdate()
        {
            this.prevSwingProgress = this.swingProgress;
            base.OnEntityUpdate();
            Profiler.StartSection("mobBaseTick");
            if (this.rand.NextInt(1000) < this.livingSoundTime++)
            {
                this.livingSoundTime = -this.GetTalkInterval();
                this.PlayLivingSound();
            }

            if (this.IsEntityAlive() && this.IsEntityInsideOpaqueBlock())
            {
                this.AttackEntityFrom((Entity)null, 1);
            }

            if (this.isImmuneToFire || this.worldObj.isRemote)
            {
                this.fire = 0;
            }

            int i1;
            if (this.IsEntityAlive() && this.IsInsideOfMaterial(Material.water) && !this.CanBreatheUnderwater())
            {
                --this.air;
                if (this.air == -20)
                {
                    this.air = 0;
                    for (i1 = 0; i1 < 8; ++i1)
                    {
                        float f2 = this.rand.NextFloat() - this.rand.NextFloat();
                        float f3 = this.rand.NextFloat() - this.rand.NextFloat();
                        float f4 = this.rand.NextFloat() - this.rand.NextFloat();
                        this.worldObj.AddParticle("bubble", this.x + f2, this.y + f3, this.z + f4, this.motionX, this.motionY, this.motionZ);
                    }

                    this.AttackEntityFrom((Entity)null, 2);
                }

                this.fire = 0;
            }
            else
            {
                this.air = this.maxAir;
            }

            this.cameraPitch = this.field_R;
            if (this.attackTime > 0)
            {
                --this.attackTime;
            }

            if (this.hurtTime > 0)
            {
                --this.hurtTime;
            }

            if (this.heartsLife > 0)
            {
                --this.heartsLife;
            }

            if (this.health <= 0)
            {
                ++this.deathTime;
                if (this.deathTime > 20)
                {
                    this.OnEntityDeath();
                    this.SetEntityDead();
                    for (i1 = 0; i1 < 20; ++i1)
                    {
                        double d8 = this.rand.NextGaussian() * 0.02;
                        double d9 = this.rand.NextGaussian() * 0.02;
                        double d6 = this.rand.NextGaussian() * 0.02;
                        this.worldObj.AddParticle("explode", this.x + this.rand.NextFloat() * this.width * 2F - this.width, this.y + this.rand.NextFloat() * this.height, this.z + this.rand.NextFloat() * this.width * 2F - this.width, d8, d9, d6);
                    }
                }
            }

            this.field_x = this.field_w;
            this.prevRenderYawOffset = this.renderYawOffset;
            this.prevYaw = this.yaw;
            this.prevPitch = this.pitch;
            Profiler.EndSection();
        }

        public virtual void SpawnExplosionParticle()
        {
            for (int i1 = 0; i1 < 20; ++i1)
            {
                double d2 = this.rand.NextGaussian() * 0.02;
                double d4 = this.rand.NextGaussian() * 0.02;
                double d6 = this.rand.NextGaussian() * 0.02;
                double d8 = 10;
                this.worldObj.AddParticle("explode", this.x + this.rand.NextFloat() * this.width * 2F - this.width - d2 * d8, this.y + this.rand.NextFloat() * this.height - d4 * d8, this.z + this.rand.NextFloat() * this.width * 2F - this.width - d6 * d8, d2, d4, d6);
            }
        }

        public override void UpdateRidden()
        {
            base.UpdateRidden();
            this.field_u = this.field_v;
            this.field_v = 0F;
        }

        public override void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            this.yOffset = 0F;
            this.newPosX = d1;
            this.newPosY = d3;
            this.newPosZ = d5;
            this.newRotationYaw = f7;
            this.newRotationPitch = f8;
            this.newPosRotationIncrements = i9;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.OnLivingUpdate();
            double d1 = this.x - this.prevX;
            double d3 = this.z - this.prevZ;
            float f5 = Mth.Sqrt(d1 * d1 + d3 * d3);
            float f6 = this.renderYawOffset;
            float f7 = 0F;
            this.field_u = this.field_v;
            float f8 = 0F;
            if (f5 > 0.05F)
            {
                f8 = 1F;
                f7 = f5 * 3F;
                f6 = (float)Math.Atan2(d3, d1) * 180F / Mth.PI - 90F;
            }

            if (this.swingProgress > 0F)
            {
                f6 = this.yaw;
            }

            if (!this.onGround)
            {
                f8 = 0F;
            }

            this.field_v += (f8 - this.field_v) * 0.3F;
            float f9;
            for (f9 = f6 - this.renderYawOffset; f9 < -180F; f9 += 360F)
            {
            }

            while (f9 >= 180F)
            {
                f9 -= 360F;
            }

            this.renderYawOffset += f9 * 0.3F;
            float f10;
            for (f10 = this.yaw - this.renderYawOffset; f10 < -180F; f10 += 360F)
            {
            }

            while (f10 >= 180F)
            {
                f10 -= 360F;
            }

            bool z11 = f10 < -90F || f10 >= 90F;
            if (f10 < -75F)
            {
                f10 = -75F;
            }

            if (f10 >= 75F)
            {
                f10 = 75F;
            }

            this.renderYawOffset = this.yaw - f10;
            if (f10 * f10 > 2500F)
            {
                this.renderYawOffset += f10 * 0.2F;
            }

            if (z11)
            {
                f7 *= -1F;
            }

            while (this.yaw - this.prevYaw < -180F)
            {
                this.prevYaw -= 360F;
            }

            while (this.yaw - this.prevYaw >= 180F)
            {
                this.prevYaw += 360F;
            }

            while (this.renderYawOffset - this.prevRenderYawOffset < -180F)
            {
                this.prevRenderYawOffset -= 360F;
            }

            while (this.renderYawOffset - this.prevRenderYawOffset >= 180F)
            {
                this.prevRenderYawOffset += 360F;
            }

            while (this.pitch - this.prevPitch < -180F)
            {
                this.prevPitch -= 360F;
            }

            while (this.pitch - this.prevPitch >= 180F)
            {
                this.prevPitch += 360F;
            }

            this.field_w += f7;
        }

        protected override void SetSize(float f1, float f2)
        {
            base.SetSize(f1, f2);
        }

        public virtual void Heal(int i1)
        {
            if (this.health > 0)
            {
                this.health += i1;
                if (this.health > 20)
                {
                    this.health = 20;
                }

                this.heartsLife = this.heartsHalvesLife / 2;
            }
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (this.worldObj.isRemote)
            {
                return false;
            }
            else
            {
                this.age = 0;
                if (this.health <= 0)
                {
                    return false;
                }
                else
                {
                    this.field_bd = 1.5F;
                    bool z3 = true;
                    if (this.heartsLife > this.heartsHalvesLife / 2F)
                    {
                        if (i2 <= this.field_af)
                        {
                            return false;
                        }

                        this.DamageEntity(i2 - this.field_af);
                        this.field_af = i2;
                        z3 = false;
                    }
                    else
                    {
                        this.field_af = i2;
                        this.prevHealth = this.health;
                        this.heartsLife = this.heartsHalvesLife;
                        this.DamageEntity(i2);
                        this.hurtTime = this.maxHurtTime = 10;
                    }

                    this.attackedAtYaw = 0F;
                    if (z3)
                    {
                        this.worldObj.SendTrackedEntityStatusUpdatePacket(this, (byte)2);
                        this.SetBeenAttacked();
                        if (entity1 != null)
                        {
                            double d4 = entity1.x - this.x;
                            double d6;
                            for (d6 = entity1.z - this.z; d4 * d4 + d6 * d6 < 0.0001; d6 = (Mth.Random() - Mth.Random()) * 0.01)
                            {
                                d4 = (Mth.Random() - Mth.Random()) * 0.01;
                            }

                            this.attackedAtYaw = (float)(Math.Atan2(d6, d4) * 180 / Mth.PI) - this.yaw;
                            this.KnockBack(entity1, i2, d4, d6);
                        }
                        else
                        {
                            this.attackedAtYaw = (int)(Mth.Random() * 2) * 180;
                        }
                    }

                    if (this.health <= 0)
                    {
                        if (z3)
                        {
                            this.worldObj.PlaySound(this, this.GetDeathSound(), this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                        }

                        this.OnDeath(entity1);
                    }
                    else if (z3)
                    {
                        this.worldObj.PlaySound(this, this.GetHurtSound(), this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                    }

                    return true;
                }
            }
        }

        public override void PerformHurtAnimation()
        {
            this.hurtTime = this.maxHurtTime = 10;
            this.attackedAtYaw = 0F;
        }

        protected virtual void DamageEntity(int i1)
        {
            this.health -= i1;
        }

        protected virtual float GetSoundVolume()
        {
            return 1F;
        }

        protected virtual string GetLivingSound()
        {
            return null;
        }

        protected virtual string GetHurtSound()
        {
            return "random.hurt";
        }

        protected virtual string GetDeathSound()
        {
            return "random.hurt";
        }

        public virtual void KnockBack(Entity entity1, int i2, double d3, double d5)
        {
            float f7 = Mth.Sqrt(d3 * d3 + d5 * d5);
            float f8 = 0.4F;
            this.motionX /= 2;
            this.motionY /= 2;
            this.motionZ /= 2;
            this.motionX -= d3 / f7 * f8;
            this.motionY += 0.4F;
            this.motionZ -= d5 / f7 * f8;
            if (this.motionY > 0.4F)
            {
                this.motionY = 0.4F;
            }
        }

        public virtual void OnDeath(Entity entity1)
        {
            if (this.scoreValue >= 0 && entity1 != null)
            {
                entity1.AddToPlayerScore(this, this.scoreValue);
            }

            if (entity1 != null)
            {
                entity1.OnKillEntity(this);
            }

            this.unused_flag = true;
            if (!this.worldObj.isRemote)
            {
                this.DropFewItems();
            }

            this.worldObj.SendTrackedEntityStatusUpdatePacket(this, (byte)3);
        }

        protected virtual void DropFewItems()
        {
            int i1 = this.GetDropItemId();
            if (i1 > 0)
            {
                int i2 = this.rand.NextInt(3);
                for (int i3 = 0; i3 < i2; ++i3)
                {
                    this.DropItem(i1, 1);
                }
            }
        }

        protected virtual int GetDropItemId()
        {
            return 0;
        }

        protected override void Fall(float f1)
        {
            base.Fall(f1);
            int i2 = (int)Math.Ceiling(f1 - 3F);
            if (i2 > 0)
            {
                this.AttackEntityFrom((Entity)null, i2);
                int i3 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.y - 0.2F - this.yOffset), Mth.Floor(this.z));
                if (i3 > 0)
                {
                    SoundType stepSound4 = Tile.tiles[i3].soundType;
                    this.worldObj.PlaySound(this, stepSound4.GetStepSound(), stepSound4.GetVolume() * 0.5F, stepSound4.GetPitch() * 0.75F);
                }
            }
        }

        public virtual void MoveEntityWithHeading(float f1, float f2)
        {
            double d3;
            if (this.IsInWater())
            {
                d3 = this.y;
                this.MoveFlying(f1, f2, 0.02F);
                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= 0.8F;
                this.motionY *= 0.8F;
                this.motionZ *= 0.8F;
                this.motionY -= 0.02;
                if (this.isCollidedHorizontally && this.IsOffsetPositionInLiquid(this.motionX, this.motionY + 0.6F - this.y + d3, this.motionZ))
                {
                    this.motionY = 0.3F;
                }
            }
            else if (this.HandleLavaMovement())
            {
                d3 = this.y;
                this.MoveFlying(f1, f2, 0.02F);
                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= 0.5;
                this.motionY *= 0.5;
                this.motionZ *= 0.5;
                this.motionY -= 0.02;
                if (this.isCollidedHorizontally && this.IsOffsetPositionInLiquid(this.motionX, this.motionY + 0.6F - this.y + d3, this.motionZ))
                {
                    this.motionY = 0.3F;
                }
            }
            else
            {
                float f8 = 0.91F;
                if (this.onGround)
                {
                    f8 = 0.54600006F;
                    int i4 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.boundingBox.y0) - 1, Mth.Floor(this.z));
                    if (i4 > 0)
                    {
                        f8 = Tile.tiles[i4].slipperiness * 0.91F;
                    }
                }

                float f9 = 0.16277136F / (f8 * f8 * f8);
                this.MoveFlying(f1, f2, this.onGround ? 0.1F * f9 : 0.02F);
                f8 = 0.91F;
                if (this.onGround)
                {
                    f8 = 0.54600006F;
                    int i5 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.boundingBox.y0) - 1, Mth.Floor(this.z));
                    if (i5 > 0)
                    {
                        f8 = Tile.tiles[i5].slipperiness * 0.91F;
                    }
                }

                if (this.IsOnLadder())
                {
                    float f10 = 0.15F;
                    if (this.motionX < (-f10))
                    {
                        this.motionX = (-f10);
                    }

                    if (this.motionX > f10)
                    {
                        this.motionX = f10;
                    }

                    if (this.motionZ < (-f10))
                    {
                        this.motionZ = (-f10);
                    }

                    if (this.motionZ > f10)
                    {
                        this.motionZ = f10;
                    }

                    this.fallDistance = 0F;
                    if (this.motionY < -0.15)
                    {
                        this.motionY = -0.15;
                    }

                    if (this.IsSneaking() && this.motionY < 0)
                    {
                        this.motionY = 0;
                    }
                }

                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                if (this.isCollidedHorizontally && this.IsOnLadder())
                {
                    this.motionY = 0.2;
                }

                this.motionY -= 0.08;
                this.motionY *= 0.98F;
                this.motionX *= f8;
                this.motionZ *= f8;
            }

            this.field_Q = this.field_bd;
            d3 = this.x - this.prevX;
            double d11 = this.z - this.prevZ;
            float f7 = Mth.Sqrt(d3 * d3 + d11 * d11) * 4F;
            if (f7 > 1F)
            {
                f7 = 1F;
            }

            this.field_bd += (f7 - this.field_bd) * 0.4F;
            this.field_ba += this.field_bd;
        }

        public virtual bool IsOnLadder()
        {
            int i1 = Mth.Floor(this.x);
            int i2 = Mth.Floor(this.boundingBox.y0);
            int i3 = Mth.Floor(this.z);
            return this.worldObj.GetTile(i1, i2, i3) == Tile.ladder.id;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("Health", (short)this.health);
            nBTTagCompound1.SetShort("HurtTime", (short)this.hurtTime);
            nBTTagCompound1.SetShort("DeathTime", (short)this.deathTime);
            nBTTagCompound1.SetShort("AttackTime", (short)this.attackTime);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.health = nBTTagCompound1.GetShort("Health");
            if (!nBTTagCompound1.HasKey("Health"))
            {
                this.health = 10;
            }

            this.hurtTime = nBTTagCompound1.GetShort("HurtTime");
            this.deathTime = nBTTagCompound1.GetShort("DeathTime");
            this.attackTime = nBTTagCompound1.GetShort("AttackTime");
        }

        public override bool IsEntityAlive()
        {
            return !this.isDead && this.health > 0;
        }

        public virtual bool CanBreatheUnderwater()
        {
            return false;
        }

        public virtual void OnLivingUpdate()
        {
            if (this.newPosRotationIncrements > 0)
            {
                double d1 = this.x + (this.newPosX - this.x) / this.newPosRotationIncrements;
                double d3 = this.y + (this.newPosY - this.y) / this.newPosRotationIncrements;
                double d5 = this.z + (this.newPosZ - this.z) / this.newPosRotationIncrements;
                double d7;
                for (d7 = this.newRotationYaw - this.yaw; d7 < -180; d7 += 360)
                {
                }

                while (d7 >= 180)
                {
                    d7 -= 360;
                }

                this.yaw = (float)(this.yaw + d7 / this.newPosRotationIncrements);
                this.pitch = (float)(this.pitch + (this.newRotationPitch - this.pitch) / this.newPosRotationIncrements);
                --this.newPosRotationIncrements;
                this.SetPosition(d1, d3, d5);
                this.SetRotation(this.yaw, this.pitch);
                IList<AABB> list9 = this.worldObj.GetCubes(this, this.boundingBox.GetInsetBoundingBox(8 / 256, 0, 8 / 256));
                if (list9.Count > 0)
                {
                    double d10 = 0;
                    for (int i12 = 0; i12 < list9.Count; ++i12)
                    {
                        AABB axisAlignedBB13 = list9[i12];
                        if (axisAlignedBB13.y1 > d10)
                        {
                            d10 = axisAlignedBB13.y1;
                        }
                    }

                    d3 += d10 - this.boundingBox.y0;
                    this.SetPosition(d1, d3, d5);
                }
            }
            Profiler.StartSection("ai");
            if (this.IsMovementBlocked())
            {
                this.isJumping = false;
                this.moveStrafing = 0F;
                this.moveForward = 0F;
                this.randomYawVelocity = 0F;
            }
            else if (!this.isMultiplayerEntity)
            {
                Profiler.StartSection("oldAi");
                this.UpdatePlayerActionState();
                Profiler.EndSection();
            }

            Profiler.EndSection();
            bool z14 = this.IsInWater();
            bool z2 = this.HandleLavaMovement();
            if (this.isJumping)
            {
                if (z14)
                {
                    this.motionY += 0.04F;
                }
                else if (z2)
                {
                    this.motionY += 0.04F;
                }
                else if (this.onGround)
                {
                    this.Jump();
                }
            }

            this.moveStrafing *= 0.98F;
            this.moveForward *= 0.98F;
            this.randomYawVelocity *= 0.9F;
            this.MoveEntityWithHeading(this.moveStrafing, this.moveForward);
            Profiler.StartSection("push");
            IList<Entity> list15 = this.worldObj.GetEntities(this, this.boundingBox.Expand(0.2F, 0, 0.2F));
            if (list15 != null && list15.Count > 0)
            {
                for (int i4 = 0; i4 < list15.Count; ++i4)
                {
                    Entity entity16 = list15[i4];
                    if (entity16.CanBePushed())
                    {
                        entity16.ApplyEntityCollision(this);
                    }
                }
            }
            Profiler.EndSection();
        }

        protected virtual bool IsMovementBlocked()
        {
            return this.health <= 0;
        }

        protected virtual void Jump()
        {
            this.motionY = 0.42F;
        }

        protected virtual bool CanDespawn()
        {
            return true;
        }

        protected virtual void DistanceDespawn()
        {
            Player entityPlayer1 = this.worldObj.GetClosestPlayerToEntity(this, -1);
            if (this.CanDespawn() && entityPlayer1 != null)
            {
                double d2 = entityPlayer1.x - this.x;
                double d4 = entityPlayer1.y - this.y;
                double d6 = entityPlayer1.z - this.z;
                double d8 = d2 * d2 + d4 * d4 + d6 * d6;
                if (d8 > 16384)
                {
                    this.SetEntityDead();
                }

                if (this.age > 600 && this.rand.NextInt(800) == 0)
                {
                    if (d8 < 1024)
                    {
                        this.age = 0;
                    }
                    else
                    {
                        this.SetEntityDead();
                    }
                }
            }
        }

        protected virtual void UpdatePlayerActionState()
        {
            ++this.age;
            Player entityPlayer1 = this.worldObj.GetClosestPlayerToEntity(this, -1);
            this.DistanceDespawn();
            this.moveStrafing = 0F;
            this.moveForward = 0F;
            float f2 = 8F;
            if (this.rand.NextFloat() < 0.02F)
            {
                entityPlayer1 = this.worldObj.GetClosestPlayerToEntity(this, f2);
                if (entityPlayer1 != null)
                {
                    this.currentTarget = entityPlayer1;
                    this.numTicksToChaseTarget = 10 + this.rand.NextInt(20);
                }
                else
                {
                    this.randomYawVelocity = (this.rand.NextFloat() - 0.5F) * 20F;
                }
            }

            if (this.currentTarget != null)
            {
                this.FaceEntity(this.currentTarget, 10F, this.GetFaceEntityDist());
                if (this.numTicksToChaseTarget-- <= 0 || this.currentTarget.isDead || this.currentTarget.GetDistanceSqToEntity(this) > f2 * f2)
                {
                    this.currentTarget = null;
                }
            }
            else
            {
                if (this.rand.NextFloat() < 0.05F)
                {
                    this.randomYawVelocity = (this.rand.NextFloat() - 0.5F) * 20F;
                }

                this.yaw += this.randomYawVelocity;
                this.pitch = this.defaultPitch;
            }

            bool z3 = this.IsInWater();
            bool z4 = this.HandleLavaMovement();
            if (z3 || z4)
            {
                this.isJumping = this.rand.NextFloat() < 0.8F;
            }
        }

        protected virtual int GetFaceEntityDist()
        {
            return 40;
        }

        public virtual void FaceEntity(Entity entity1, float f2, float f3)
        {
            double d4 = entity1.x - this.x;
            double d8 = entity1.z - this.z;
            double d6;
            if (entity1 is Mob)
            {
                Mob entityLiving10 = (Mob)entity1;
                d6 = this.y + this.GetEyeHeight() - (entityLiving10.y + entityLiving10.GetEyeHeight());
            }
            else
            {
                d6 = (entity1.boundingBox.y0 + entity1.boundingBox.y1) / 2 - (this.y + this.GetEyeHeight());
            }

            double d14 = Mth.Sqrt(d4 * d4 + d8 * d8);
            float f12 = (float)(Math.Atan2(d8, d4) * 180 / Mth.PI) - 90F;
            float f13 = (float)(-(Math.Atan2(d6, d14) * 180 / Mth.PI));
            this.pitch = -this.UpdateRotation(this.pitch, f13, f3);
            this.yaw = this.UpdateRotation(this.yaw, f12, f2);
        }

        public virtual bool HasCurrentTarget()
        {
            return this.currentTarget != null;
        }

        public virtual Entity GetCurrentTarget()
        {
            return this.currentTarget;
        }

        private float UpdateRotation(float f1, float f2, float f3)
        {
            float f4;
            for (f4 = f2 - f1; f4 < -180F; f4 += 360F)
            {
            }

            while (f4 >= 180F)
            {
                f4 -= 360F;
            }

            if (f4 > f3)
            {
                f4 = f3;
            }

            if (f4 < -f3)
            {
                f4 = -f3;
            }

            return f1 + f4;
        }

        public virtual void OnEntityDeath()
        {
        }

        public virtual bool GetCanSpawnHere()
        {
            return this.worldObj.CheckIfAABBIsClear(this.boundingBox) && this.worldObj.GetCubes(this, this.boundingBox).Count == 0 && !this.worldObj.ContainsAnyLiquid(this.boundingBox);
        }

        protected override void Kill()
        {
            this.AttackEntityFrom((Entity)null, 4);
        }

        public virtual float GetSwingProgress(float f1)
        {
            float f2 = this.swingProgress - this.prevSwingProgress;
            if (f2 < 0F)
            {
                ++f2;
            }

            return this.prevSwingProgress + f2 * f1;
        }

        public virtual Vec3 GetPosition(float f1)
        {
            if (f1 == 1F)
            {
                return Vec3.Of(this.x, this.y, this.z);
            }
            else
            {
                double d2 = this.prevX + (this.x - this.prevX) * f1;
                double d4 = this.prevY + (this.y - this.prevY) * f1;
                double d6 = this.prevZ + (this.z - this.prevZ) * f1;
                return Vec3.Of(d2, d4, d6);
            }
        }

        public override Vec3 GetLookVec()
        {
            return this.GetLook(1F);
        }

        public virtual Vec3 GetLook(float f1)
        {
            float f2;
            float f3;
            float f4;
            float f5;
            if (f1 == 1F)
            {
                f2 = Mth.Cos(-this.yaw * 0.017453292F - Mth.PI);
                f3 = Mth.Sin(-this.yaw * 0.017453292F - Mth.PI);
                f4 = -Mth.Cos(-this.pitch * 0.017453292F);
                f5 = Mth.Sin(-this.pitch * 0.017453292F);
                return Vec3.Of(f3 * f4, f5, f2 * f4);
            }
            else
            {
                f2 = this.prevPitch + (this.pitch - this.prevPitch) * f1;
                f3 = this.prevYaw + (this.yaw - this.prevYaw) * f1;
                f4 = Mth.Cos(-f3 * 0.017453292F - Mth.PI);
                f5 = Mth.Sin(-f3 * 0.017453292F - Mth.PI);
                float f6 = -Mth.Cos(-f2 * 0.017453292F);
                float f7 = Mth.Sin(-f2 * 0.017453292F);
                return Vec3.Of(f5 * f6, f7, f4 * f6);
            }
        }

        public virtual HitResult RayTrace(double d1, float f3)
        {
            Vec3 vec3D4 = this.GetPosition(f3);
            Vec3 vec3D5 = this.GetLook(f3);
            Vec3 vec3D6 = vec3D4.AddVector(vec3D5.x * d1, vec3D5.y * d1, vec3D5.z * d1);
            return this.worldObj.Clip(vec3D4, vec3D6);
        }

        public virtual int GetMaxSpawnedInChunk()
        {
            return 4;
        }

        public virtual ItemInstance GetHeldItem()
        {
            return null;
        }

        public override void HandleHealthUpdate(byte b1)
        {
            if (b1 == 2)
            {
                this.field_bd = 1.5F;
                this.heartsLife = this.heartsHalvesLife;
                this.hurtTime = this.maxHurtTime = 10;
                this.attackedAtYaw = 0F;
                this.worldObj.PlaySound(this, this.GetHurtSound(), this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                this.AttackEntityFrom((Entity)null, 0);
            }
            else if (b1 == 3)
            {
                this.worldObj.PlaySound(this, this.GetDeathSound(), this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                this.health = 0;
                this.OnDeath((Entity)null);
            }
            else
            {
                base.HandleHealthUpdate(b1);
            }
        }

        public virtual bool IsSleeping()
        {
            return false;
        }

        public virtual int GetItemIcon(ItemInstance itemStack1)
        {
            return itemStack1.GetIconIndex();
        }
    }
}