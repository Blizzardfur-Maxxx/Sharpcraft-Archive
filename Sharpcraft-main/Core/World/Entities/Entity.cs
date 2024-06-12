using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Weather;
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
    public abstract class Entity
    {
        private static int nextEntityID = 0;
        public int entityID = nextEntityID++;
        public double renderDistanceWeight = 1;
        public bool preventEntitySpawning = false;
        public Entity riddenByEntity;
        public Entity ridingEntity;
        public Level worldObj;
        public double prevX;
        public double prevY;
        public double prevZ;
        public double x;
        public double y;
        public double z;
        public double motionX;
        public double motionY;
        public double motionZ;
        public float yaw;
        public float pitch;
        public float prevYaw;
        public float prevPitch;
        public readonly AABB boundingBox = AABB.CreateAABB(0, 0, 0, 0, 0, 0);
        public bool onGround = false;
        public bool isCollidedHorizontally;
        public bool isCollidedVertically;
        public bool isCollided = false;
        public bool beenAttacked = false;
        public bool isInWeb;
        public bool field_9293 = true;
        public bool isDead = false;
        public float yOffset = 0F;
        public float width = 0.6F;
        public float height = 1.8F;
        public float prevDistanceWalkedModified = 0F;
        public float distanceWalkedModified = 0F;
        protected float fallDistance = 0F;
        private int nextStepDistance = 1;
        public double lastTickPosX;
        public double lastTickPosY;
        public double lastTickPosZ;
        public float ySize = 0F;
        public float stepHeight = 0F;
        public bool noClip = false;
        public float entityCollisionReduction = 0F;
        protected JRandom rand = new JRandom();
        public int ticksExisted = 0;
        public int fireResistance = 1;
        public int fire = 0;
        protected int maxAir = 300;
        protected bool inWater = false;
        public int heartsLife = 0;
        public int air = 300;
        private bool firstUpdate = true;
        public string skinUrl;
        public string cloakUrl;
        protected bool isImmuneToFire = false;
        protected SynchedEntityData dataWatcher = new SynchedEntityData();
        public float entityBrightness = 0F;
        private double entityRiderPitchDelta;
        private double entityRiderYawDelta;
        public bool addedToChunk = false;
        public int chunkCoordX;
        public int chunkCoordY;
        public int chunkCoordZ;
        public int serverPosX;
        public int serverPosY;
        public int serverPosZ;
        public bool ignoreFrustumCheck;
        public Entity(Level world1)
        {
            this.worldObj = world1;
            this.SetPosition(0, 0, 0);
            this.dataWatcher.AddObject(0, (sbyte)0);
            this.EntityInit();
        }

        protected abstract void EntityInit();
        public virtual SynchedEntityData GetDataWatcher()
        {
            return this.dataWatcher;
        }

        public override bool Equals(object object1)
        {
            return object1 is Entity ? ((Entity)object1).entityID == this.entityID : false;
        }

        public override int GetHashCode()
        {
            return this.entityID;
        }

        public virtual void PreparePlayerToSpawn()
        {
            if (this.worldObj != null)
            {
                while (this.y > 0)
                {
                    this.SetPosition(this.x, this.y, this.z);
                    if (this.worldObj.GetCubes(this, this.boundingBox).Count == 0)
                    {
                        break;
                    }

                    ++this.y;
                }

                this.motionX = this.motionY = this.motionZ = 0;
                this.pitch = 0F;
            }
        }

        public virtual void SetEntityDead()
        {
            this.isDead = true;
        }

        protected virtual void SetSize(float f1, float f2)
        {
            this.width = f1;
            this.height = f2;
        }

        protected virtual void SetRotation(float f1, float f2)
        {
            this.yaw = f1 % 360F;
            this.pitch = f2 % 360F;
        }

        public virtual void SetPosition(double d1, double d3, double d5)
        {
            this.x = d1;
            this.y = d3;
            this.z = d5;
            float f7 = this.width / 2F;
            float f8 = this.height;
            this.boundingBox.SetBounds(d1 - f7, d3 - this.yOffset + this.ySize, d5 - f7, d1 + f7, d3 - this.yOffset + this.ySize + f8, d5 + f7);
        }

        public virtual void Func_346(float f1, float f2)
        {
            float f3 = this.pitch;
            float f4 = this.yaw;
            this.yaw = (float)(this.yaw + f1 * 0.15);
            this.pitch = (float)(this.pitch - f2 * 0.15);
            if (this.pitch < -90F)
            {
                this.pitch = -90F;
            }

            if (this.pitch > 90F)
            {
                this.pitch = 90F;
            }

            this.prevPitch += this.pitch - f3;
            this.prevYaw += this.yaw - f4;
        }

        public virtual void OnUpdate()
        {
            this.OnEntityUpdate();
        }

        public virtual void OnEntityUpdate()
        {
            Profiler.StartSection("entityBaseTick");
            if (this.ridingEntity != null && this.ridingEntity.isDead)
            {
                this.ridingEntity = null;
            }

            ++this.ticksExisted;
            this.prevDistanceWalkedModified = this.distanceWalkedModified;
            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            this.prevPitch = this.pitch;
            this.prevYaw = this.yaw;
            if (this.HandleWaterMovement())
            {
                if (!this.inWater && !this.firstUpdate)
                {
                    float f1 = Mth.Sqrt(this.motionX * this.motionX * 0.2F + this.motionY * this.motionY + this.motionZ * this.motionZ * 0.2F) * 0.2F;
                    if (f1 > 1F)
                    {
                        f1 = 1F;
                    }

                    this.worldObj.PlaySound(this, "random.splash", f1, 1F + (this.rand.NextFloat() - this.rand.NextFloat()) * 0.4F);
                    float f2 = Mth.Floor(this.boundingBox.y0);
                    int i3;
                    float f4;
                    float f5;
                    for (i3 = 0; i3 < 1F + this.width * 20F; ++i3)
                    {
                        f4 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                        f5 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                        this.worldObj.AddParticle("bubble", this.x + f4, f2 + 1F, this.z + f5, this.motionX, this.motionY - this.rand.NextFloat() * 0.2F, this.motionZ);
                    }

                    for (i3 = 0; i3 < 1F + this.width * 20F; ++i3)
                    {
                        f4 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                        f5 = (this.rand.NextFloat() * 2F - 1F) * this.width;
                        this.worldObj.AddParticle("splash", this.x + f4, f2 + 1F, this.z + f5, this.motionX, this.motionY, this.motionZ);
                    }
                }

                this.fallDistance = 0F;
                this.inWater = true;
                this.fire = 0;
            }
            else
            {
                this.inWater = false;
            }

            if (this.worldObj.isRemote)
            {
                this.fire = 0;
            }
            else if (this.fire > 0)
            {
                if (this.isImmuneToFire)
                {
                    this.fire -= 4;
                    if (this.fire < 0)
                    {
                        this.fire = 0;
                    }
                }
                else
                {
                    if (this.fire % 20 == 0)
                    {
                        this.AttackEntityFrom((Entity)null, 1);
                    }

                    --this.fire;
                }
            }

            if (this.HandleLavaMovement())
            {
                this.SetOnFireFromLava();
            }

            if (this.y < -64)
            {
                this.Kill();
            }

            if (!this.worldObj.isRemote)
            {
                this.SetFlag(0, this.fire > 0);
                this.SetFlag(2, this.ridingEntity != null);
            }

            this.firstUpdate = false;
            Profiler.EndSection();
        }

        protected virtual void SetOnFireFromLava()
        {
            if (!this.isImmuneToFire)
            {
                this.AttackEntityFrom((Entity)null, 4);
                this.fire = 600;
            }
        }

        protected virtual void Kill()
        {
            this.SetEntityDead();
        }

        public virtual bool IsOffsetPositionInLiquid(double d1, double d3, double d5)
        {
            AABB axisAlignedBB7 = this.boundingBox.GetOffsetBoundingBox(d1, d3, d5);
            IList<AABB> list8 = this.worldObj.GetCubes(this, axisAlignedBB7);
            return list8.Count > 0 ? false : !this.worldObj.ContainsAnyLiquid(axisAlignedBB7);
        }

        public virtual void MoveEntity(double d1, double d3, double d5)
        {
            if (this.noClip)
            {
                this.boundingBox.Offset(d1, d3, d5);
                this.x = (this.boundingBox.x0 + this.boundingBox.x1) / 2;
                this.y = this.boundingBox.y0 + this.yOffset - this.ySize;
                this.z = (this.boundingBox.z0 + this.boundingBox.z1) / 2;
            }
            else
            {
                Profiler.StartSection("move");
                this.ySize *= 0.4F;
                double d7 = this.x;
                double d9 = this.z;
                if (this.isInWeb)
                {
                    this.isInWeb = false;
                    d1 *= 0.25;
                    d3 *= 0.05F;
                    d5 *= 0.25;
                    this.motionX = 0;
                    this.motionY = 0;
                    this.motionZ = 0;
                }

                double d11 = d1;
                double d13 = d3;
                double d15 = d5;
                AABB axisAlignedBB17 = this.boundingBox.Copy();
                bool z18 = this.onGround && this.IsSneaking();
                if (z18)
                {
                    double d19;
                    for (d19 = 0.05; d1 != 0 && this.worldObj.GetCubes(this, this.boundingBox.GetOffsetBoundingBox(d1, -1, 0)).Count == 0; d11 = d1)
                    {
                        if (d1 < d19 && d1 >= -d19)
                        {
                            d1 = 0;
                        }
                        else if (d1 > 0)
                        {
                            d1 -= d19;
                        }
                        else
                        {
                            d1 += d19;
                        }
                    }

                    for (; d5 != 0 && this.worldObj.GetCubes(this, this.boundingBox.GetOffsetBoundingBox(0, -1, d5)).Count == 0; d15 = d5)
                    {
                        if (d5 < d19 && d5 >= -d19)
                        {
                            d5 = 0;
                        }
                        else if (d5 > 0)
                        {
                            d5 -= d19;
                        }
                        else
                        {
                            d5 += d19;
                        }
                    }
                }

                IList<AABB> list35 = this.worldObj.GetCubes(this, this.boundingBox.AddCoord(d1, d3, d5));
                for (int i20 = 0; i20 < list35.Count; ++i20)
                {
                    d3 = list35[i20].CalculateYOffset(this.boundingBox, d3);
                }

                this.boundingBox.Offset(0, d3, 0);
                if (!this.field_9293 && d13 != d3)
                {
                    d5 = 0;
                    d3 = 0;
                    d1 = 0;
                }

                bool z36 = this.onGround || d13 != d3 && d13 < 0;
                int i21;
                for (i21 = 0; i21 < list35.Count; ++i21)
                {
                    d1 = list35[i21].CalculateXOffset(this.boundingBox, d1);
                }

                this.boundingBox.Offset(d1, 0, 0);
                if (!this.field_9293 && d11 != d1)
                {
                    d5 = 0;
                    d3 = 0;
                    d1 = 0;
                }

                for (i21 = 0; i21 < list35.Count; ++i21)
                {
                    d5 = list35[i21].CalculateZOffset(this.boundingBox, d5);
                }

                this.boundingBox.Offset(0, 0, d5);
                if (!this.field_9293 && d15 != d5)
                {
                    d5 = 0;
                    d3 = 0;
                    d1 = 0;
                }

                double d23;
                int i28;
                double d37;
                if (this.stepHeight > 0F && z36 && (z18 || this.ySize < 0.05F) && (d11 != d1 || d15 != d5))
                {
                    d37 = d1;
                    d23 = d3;
                    double d25 = d5;
                    d1 = d11;
                    d3 = this.stepHeight;
                    d5 = d15;
                    AABB axisAlignedBB27 = this.boundingBox.Copy();
                    this.boundingBox.SetBB(axisAlignedBB17);
                    list35 = this.worldObj.GetCubes(this, this.boundingBox.AddCoord(d11, d3, d15));
                    for (i28 = 0; i28 < list35.Count; ++i28)
                    {
                        d3 = list35[i28].CalculateYOffset(this.boundingBox, d3);
                    }

                    this.boundingBox.Offset(0, d3, 0);
                    if (!this.field_9293 && d13 != d3)
                    {
                        d5 = 0;
                        d3 = 0;
                        d1 = 0;
                    }

                    for (i28 = 0; i28 < list35.Count; ++i28)
                    {
                        d1 = list35[i28].CalculateXOffset(this.boundingBox, d1);
                    }

                    this.boundingBox.Offset(d1, 0, 0);
                    if (!this.field_9293 && d11 != d1)
                    {
                        d5 = 0;
                        d3 = 0;
                        d1 = 0;
                    }

                    for (i28 = 0; i28 < list35.Count; ++i28)
                    {
                        d5 = list35[i28].CalculateZOffset(this.boundingBox, d5);
                    }

                    this.boundingBox.Offset(0, 0, d5);
                    if (!this.field_9293 && d15 != d5)
                    {
                        d5 = 0;
                        d3 = 0;
                        d1 = 0;
                    }

                    if (!this.field_9293 && d13 != d3)
                    {
                        d5 = 0;
                        d3 = 0;
                        d1 = 0;
                    }
                    else
                    {
                        d3 = (-this.stepHeight);
                        for (i28 = 0; i28 < list35.Count; ++i28)
                        {
                            d3 = list35[i28].CalculateYOffset(this.boundingBox, d3);
                        }

                        this.boundingBox.Offset(0, d3, 0);
                    }

                    if (d37 * d37 + d25 * d25 >= d1 * d1 + d5 * d5)
                    {
                        d1 = d37;
                        d3 = d23;
                        d5 = d25;
                        this.boundingBox.SetBB(axisAlignedBB27);
                    }
                    else
                    {
                        double d41 = this.boundingBox.y0 - ((int)this.boundingBox.y0);
                        if (d41 > 0)
                        {
                            this.ySize = (float)(this.ySize + d41 + 0.01);
                        }
                    }
                }
                Profiler.EndSection();
                Profiler.StartSection("rest");
                this.x = (this.boundingBox.x0 + this.boundingBox.x1) / 2;
                this.y = this.boundingBox.y0 + this.yOffset - this.ySize;
                this.z = (this.boundingBox.z0 + this.boundingBox.z1) / 2;
                this.isCollidedHorizontally = d11 != d1 || d15 != d5;
                this.isCollidedVertically = d13 != d3;
                this.onGround = d13 != d3 && d13 < 0;
                this.isCollided = this.isCollidedHorizontally || this.isCollidedVertically;
                this.UpdateFallState(d3, this.onGround);
                if (d11 != d1)
                {
                    this.motionX = 0;
                }

                if (d13 != d3)
                {
                    this.motionY = 0;
                }

                if (d15 != d5)
                {
                    this.motionZ = 0;
                }

                d37 = this.x - d7;
                d23 = this.z - d9;
                int i26;
                int i38;
                int i39;
                if (this.CanTriggerWalking() && !z18 && this.ridingEntity == null)
                {
                    this.distanceWalkedModified = (float)(this.distanceWalkedModified + Mth.Sqrt(d37 * d37 + d23 * d23) * 0.6);
                    i38 = Mth.Floor(this.x);
                    i26 = Mth.Floor(this.y - 0.2F - this.yOffset);
                    i39 = Mth.Floor(this.z);
                    i28 = this.worldObj.GetTile(i38, i26, i39);
                    if (this.worldObj.GetTile(i38, i26 - 1, i39) == Tile.fence.id)
                    {
                        i28 = this.worldObj.GetTile(i38, i26 - 1, i39);
                    }

                    if (this.distanceWalkedModified > this.nextStepDistance && i28 > 0)
                    {
                        ++this.nextStepDistance;
                        SoundType stepSound29 = Tile.tiles[i28].soundType;
                        if (this.worldObj.GetTile(i38, i26 + 1, i39) == Tile.topSnow.id)
                        {
                            stepSound29 = Tile.topSnow.soundType;
                            this.worldObj.PlaySound(this, stepSound29.GetStepSound(), stepSound29.GetVolume() * 0.15F, stepSound29.GetPitch());
                        }
                        else if (!Tile.tiles[i28].material.IsLiquid())
                        {
                            this.worldObj.PlaySound(this, stepSound29.GetStepSound(), stepSound29.GetVolume() * 0.15F, stepSound29.GetPitch());
                        }

                        Tile.tiles[i28].StepOn(this.worldObj, i38, i26, i39, this);
                    }
                }

                i38 = Mth.Floor(this.boundingBox.x0 + 0.001);
                i26 = Mth.Floor(this.boundingBox.y0 + 0.001);
                i39 = Mth.Floor(this.boundingBox.z0 + 0.001);
                i28 = Mth.Floor(this.boundingBox.x1 - 0.001);
                int i40 = Mth.Floor(this.boundingBox.y1 - 0.001);
                int i30 = Mth.Floor(this.boundingBox.z1 - 0.001);
                if (this.worldObj.CheckChunksExist(i38, i26, i39, i28, i40, i30))
                {
                    for (int i31 = i38; i31 <= i28; ++i31)
                    {
                        for (int i32 = i26; i32 <= i40; ++i32)
                        {
                            for (int i33 = i39; i33 <= i30; ++i33)
                            {
                                int i34 = this.worldObj.GetTile(i31, i32, i33);
                                if (i34 > 0)
                                {
                                    Tile.tiles[i34].OnEntityCollidedWithBlock(this.worldObj, i31, i32, i33, this);
                                }
                            }
                        }
                    }
                }

                bool z42 = this.IsWet();
                if (this.worldObj.ContainsFireTile(this.boundingBox.GetInsetBoundingBox(0.001, 0.001, 0.001)))
                {
                    this.DealFireDamage(1);
                    if (!z42)
                    {
                        ++this.fire;
                        if (this.fire == 0)
                        {
                            this.fire = 300;
                        }
                    }
                }
                else if (this.fire <= 0)
                {
                    this.fire = -this.fireResistance;
                }

                if (z42 && this.fire > 0)
                {
                    this.worldObj.PlaySound(this, "random.fizz", 0.7F, 1.6F + (this.rand.NextFloat() - this.rand.NextFloat()) * 0.4F);
                    this.fire = -this.fireResistance;
                }
            }
            Profiler.EndSection();
        }

        protected virtual bool CanTriggerWalking()
        {
            return true;
        }

        protected virtual void UpdateFallState(double d1, bool z3)
        {
            if (z3)
            {
                if (this.fallDistance > 0F)
                {
                    this.Fall(this.fallDistance);
                    this.fallDistance = 0F;
                }
            }
            else if (d1 < 0)
            {
                this.fallDistance = (float)(this.fallDistance - d1);
            }
        }

        public virtual AABB GetBoundingBox()
        {
            return null;
        }

        protected virtual void DealFireDamage(int i1)
        {
            if (!this.isImmuneToFire)
            {
                this.AttackEntityFrom((Entity)null, i1);
            }
        }

        protected virtual void Fall(float f1)
        {
            if (this.riddenByEntity != null)
            {
                this.riddenByEntity.Fall(f1);
            }
        }

        public virtual bool IsWet()
        {
            return this.inWater || this.worldObj.CanLightningStrikeAt(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z));
        }

        public virtual bool IsInWater()
        {
            return this.inWater;
        }

        public virtual bool HandleWaterMovement()
        {
            return this.worldObj.CheckAndHandleWater(this.boundingBox.Expand(0, -0.4000000059604645, 0).GetInsetBoundingBox(0.001, 0.001, 0.001), Material.water, this);
        }

        public virtual bool IsInsideOfMaterial(Material material1)
        {
            double d2 = this.y + this.GetEyeHeight();
            int i4 = Mth.Floor(this.x);
            int i5 = Mth.Floor(Mth.Floor(d2));
            int i6 = Mth.Floor(this.z);
            int i7 = this.worldObj.GetTile(i4, i5, i6);
            if (i7 != 0 && Tile.tiles[i7].material == material1)
            {
                float f8 = LiquidTile.GetHeight(this.worldObj.GetData(i4, i5, i6)) - 0.11111111F;
                float f9 = i5 + 1 - f8;
                return d2 < f9;
            }
            else
            {
                return false;
            }
        }

        public virtual float GetEyeHeight()
        {
            return 0F;
        }

        public virtual bool HandleLavaMovement()
        {
            return this.worldObj.ContainsMaterial(this.boundingBox.Expand(-0.10000000149011612, -0.4000000059604645, -0.10000000149011612), Material.lava);
        }

        public virtual void MoveFlying(float f1, float f2, float f3)
        {
            float f4 = Mth.Sqrt(f1 * f1 + f2 * f2);
            if (f4 >= 0.01F)
            {
                if (f4 < 1F)
                {
                    f4 = 1F;
                }

                f4 = f3 / f4;
                f1 *= f4;
                f2 *= f4;
                float f5 = Mth.Sin(this.yaw * Mth.PI / 180F);
                float f6 = Mth.Cos(this.yaw * Mth.PI / 180F);
                this.motionX += f1 * f6 - f2 * f5;
                this.motionZ += f2 * f6 + f1 * f5;
            }
        }

        public virtual float GetEntityBrightness(float f1)
        {
            int i2 = Mth.Floor(this.x);
            double d3 = (this.boundingBox.y1 - this.boundingBox.y0) * 0.66;
            int i5 = Mth.Floor(this.y - this.yOffset + d3);
            int i6 = Mth.Floor(this.z);
            if (this.worldObj.CheckChunksExist(Mth.Floor(this.boundingBox.x0), Mth.Floor(this.boundingBox.y0), Mth.Floor(this.boundingBox.z0), Mth.Floor(this.boundingBox.x1), Mth.Floor(this.boundingBox.y1), Mth.Floor(this.boundingBox.z1)))
            {
                float f7 = this.worldObj.GetBrightness(i2, i5, i6);
                if (f7 < this.entityBrightness)
                {
                    f7 = this.entityBrightness;
                }

                return f7;
            }
            else
            {
                return this.entityBrightness;
            }
        }

        public virtual void SetWorld(Level world1)
        {
            this.worldObj = world1;
        }

        public virtual void SetPositionAndRotation(double d1, double d3, double d5, float f7, float f8)
        {
            this.prevX = this.x = d1;
            this.prevY = this.y = d3;
            this.prevZ = this.z = d5;
            this.prevYaw = this.yaw = f7;
            this.prevPitch = this.pitch = f8;
            this.ySize = 0F;
            double d9 = this.prevYaw - f7;
            if (d9 < -180)
            {
                this.prevYaw += 360F;
            }

            if (d9 >= 180)
            {
                this.prevYaw -= 360F;
            }

            this.SetPosition(this.x, this.y, this.z);
            this.SetRotation(f7, f8);
        }

        public virtual void SetLocationAndAngles(double d1, double d3, double d5, float f7, float f8)
        {
            this.lastTickPosX = this.prevX = this.x = d1;
            this.lastTickPosY = this.prevY = this.y = d3 + this.yOffset;
            this.lastTickPosZ = this.prevZ = this.z = d5;
            this.yaw = f7;
            this.pitch = f8;
            this.SetPosition(this.x, this.y, this.z);
        }

        public virtual float GetDistanceToEntity(Entity entity1)
        {
            float f2 = (float)(this.x - entity1.x);
            float f3 = (float)(this.y - entity1.y);
            float f4 = (float)(this.z - entity1.z);
            return Mth.Sqrt(f2 * f2 + f3 * f3 + f4 * f4);
        }

        public virtual double GetDistanceSq(double d1, double d3, double d5)
        {
            double d7 = this.x - d1;
            double d9 = this.y - d3;
            double d11 = this.z - d5;
            return d7 * d7 + d9 * d9 + d11 * d11;
        }

        public virtual double GetDistance(double d1, double d3, double d5)
        {
            double d7 = this.x - d1;
            double d9 = this.y - d3;
            double d11 = this.z - d5;
            return Mth.Sqrt(d7 * d7 + d9 * d9 + d11 * d11);
        }

        public virtual double GetDistanceSqToEntity(Entity entity1)
        {
            double d2 = this.x - entity1.x;
            double d4 = this.y - entity1.y;
            double d6 = this.z - entity1.z;
            return d2 * d2 + d4 * d4 + d6 * d6;
        }

        public virtual void OnCollideWithPlayer(Player entityPlayer1)
        {
        }

        public virtual void ApplyEntityCollision(Entity entity1)
        {
            if (entity1.riddenByEntity != this && entity1.ridingEntity != this)
            {
                double d2 = entity1.x - this.x;
                double d4 = entity1.z - this.z;
                double d6 = Mth.AbsMax(d2, d4);
                if (d6 >= 0.01F)
                {
                    d6 = Mth.Sqrt(d6);
                    d2 /= d6;
                    d4 /= d6;
                    double d8 = 1 / d6;
                    if (d8 > 1)
                    {
                        d8 = 1;
                    }

                    d2 *= d8;
                    d4 *= d8;
                    d2 *= 0.05F;
                    d4 *= 0.05F;
                    d2 *= 1F - this.entityCollisionReduction;
                    d4 *= 1F - this.entityCollisionReduction;
                    this.AddVelocity(-d2, 0, -d4);
                    entity1.AddVelocity(d2, 0, d4);
                }
            }
        }

        public virtual void AddVelocity(double d1, double d3, double d5)
        {
            this.motionX += d1;
            this.motionY += d3;
            this.motionZ += d5;
        }

        protected virtual void SetBeenAttacked()
        {
            this.beenAttacked = true;
        }

        public virtual bool AttackEntityFrom(Entity entity1, int i2)
        {
            this.SetBeenAttacked();
            return false;
        }

        public virtual bool CanBeCollidedWith()
        {
            return false;
        }

        public virtual bool CanBePushed()
        {
            return false;
        }

        public virtual void AddToPlayerScore(Entity entity1, int i2)
        {
        }

        public virtual bool IsInRangeToRenderVec3D(Vec3 vec3D1)
        {
            double d2 = this.x - vec3D1.x;
            double d4 = this.y - vec3D1.y;
            double d6 = this.z - vec3D1.z;
            double d8 = d2 * d2 + d4 * d4 + d6 * d6;
            return this.IsInRangeToRenderDist(d8);
        }

        public virtual bool IsInRangeToRenderDist(double d1)
        {
            double d3 = this.boundingBox.GetAverageEdgeLength();
            d3 *= 64 * this.renderDistanceWeight;
            return d1 < d3 * d3;
        }

        public virtual string GetEntityTexture()
        {
            return null;
        }

        public virtual bool AddEntityID(CompoundTag nBTTagCompound1)
        {
            string string2 = this.GetEntityString();
            if (!this.isDead && string2 != null)
            {
                nBTTagCompound1.SetString("id", string2);
                this.WriteToNBT(nBTTagCompound1);
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void WriteToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetTag("Pos", this.NewDoubleNBTList(this.x, this.y + this.ySize, this.z));
            nBTTagCompound1.SetTag("Motion", this.NewDoubleNBTList(this.motionX, this.motionY, this.motionZ));
            nBTTagCompound1.SetTag("Rotation", this.NewFloatNBTList(this.yaw, this.pitch));
            nBTTagCompound1.SetFloat("FallDistance", this.fallDistance);
            nBTTagCompound1.SetShort("Fire", (short)this.fire);
            nBTTagCompound1.SetShort("Air", (short)this.air);
            nBTTagCompound1.SetBoolean("OnGround", this.onGround);
            this.WriteEntityToNBT(nBTTagCompound1);
        }

        public virtual void ReadFromNBT(CompoundTag nBTTagCompound1)
        {
            ListTag<DoubleTag> nBTTagList2 = nBTTagCompound1.GetTagList<DoubleTag>("Pos");
            ListTag<DoubleTag> nBTTagList3 = nBTTagCompound1.GetTagList<DoubleTag>("Motion");
            ListTag<FloatTag> nBTTagList4 = nBTTagCompound1.GetTagList<FloatTag>("Rotation");
            this.motionX = nBTTagList3[0].Value;
            this.motionY = nBTTagList3[1].Value;
            this.motionZ = nBTTagList3[2].Value;
            if (Math.Abs(this.motionX) > 10)
            {
                this.motionX = 0;
            }

            if (Math.Abs(this.motionY) > 10)
            {
                this.motionY = 0;
            }

            if (Math.Abs(this.motionZ) > 10)
            {
                this.motionZ = 0;
            }

            this.prevX = this.lastTickPosX = this.x = nBTTagList2[0].Value;
            this.prevY = this.lastTickPosY = this.y = nBTTagList2[1].Value;
            this.prevZ = this.lastTickPosZ = this.z = nBTTagList2[2].Value;
            this.prevYaw = this.yaw = nBTTagList4[0].Value;
            this.prevPitch = this.pitch = nBTTagList4[1].Value;
            this.fallDistance = nBTTagCompound1.GetFloat("FallDistance");
            this.fire = nBTTagCompound1.GetShort("Fire");
            this.air = nBTTagCompound1.GetShort("Air");
            this.onGround = nBTTagCompound1.GetBoolean("OnGround");
            this.SetPosition(this.x, this.y, this.z);
            this.SetRotation(this.yaw, this.pitch);
            this.ReadEntityFromNBT(nBTTagCompound1);
        }

        protected string GetEntityString()
        {
            return EntityFactory.GetEncodeId(this);
        }

        protected abstract void ReadEntityFromNBT(CompoundTag nBTTagCompound1);
        protected abstract void WriteEntityToNBT(CompoundTag nBTTagCompound1);
        protected virtual ListTag<DoubleTag> NewDoubleNBTList(params double[] d1)
        {
            ListTag<DoubleTag> nBTTagList2 = new ListTag<DoubleTag>();
            double[] d3 = d1;
            int i4 = d1.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                double d6 = d3[i5];
                nBTTagList2.Add(new DoubleTag(d6));
            }

            return nBTTagList2;
        }

        protected virtual ListTag<FloatTag> NewFloatNBTList(params float[] f1)
        {
            ListTag<FloatTag> nBTTagList2 = new ListTag<FloatTag>();
            float[] f3 = f1;
            int i4 = f1.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                float f6 = f3[i5];
                nBTTagList2.Add(new FloatTag(f6));
            }

            return nBTTagList2;
        }

        public virtual float GetShadowSize()
        {
            return this.height / 2F;
        }

        public virtual ItemEntity DropItem(int i1, int i2)
        {
            return this.DropItemWithOffset(i1, i2, 0F);
        }

        public virtual ItemEntity DropItemWithOffset(int i1, int i2, float f3)
        {
            return this.EntityDropItem(new ItemInstance(i1, i2, 0), f3);
        }

        public virtual ItemEntity EntityDropItem(ItemInstance itemStack1, float f2)
        {
            ItemEntity entityItem3 = new ItemEntity(this.worldObj, this.x, this.y + f2, this.z, itemStack1);
            entityItem3.delayBeforeCanPickup = 10;
            this.worldObj.AddEntity(entityItem3);
            return entityItem3;
        }

        public virtual bool IsEntityAlive()
        {
            return !this.isDead;
        }

        public virtual bool IsEntityInsideOpaqueBlock()
        {
            for (int i1 = 0; i1 < 8; ++i1)
            {
                float f2 = ((i1 >> 0) % 2 - 0.5F) * this.width * 0.9F;
                float f3 = ((i1 >> 1) % 2 - 0.5F) * 0.1F;
                float f4 = ((i1 >> 2) % 2 - 0.5F) * this.width * 0.9F;
                int i5 = Mth.Floor(this.x + f2);
                int i6 = Mth.Floor(this.y + this.GetEyeHeight() + f3);
                int i7 = Mth.Floor(this.z + f4);
                if (this.worldObj.IsSolidBlockingTile(i5, i6, i7))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool Interact(Player entityPlayer1)
        {
            return false;
        }

        public virtual AABB GetCollisionBox(Entity entity1)
        {
            return null;
        }

        public virtual void UpdateRidden()
        {
            if (this.ridingEntity.isDead)
            {
                this.ridingEntity = null;
            }
            else
            {
                this.motionX = 0;
                this.motionY = 0;
                this.motionZ = 0;
                this.OnUpdate();
                if (this.ridingEntity != null)
                {
                    this.ridingEntity.UpdateRiderPosition();
                    this.entityRiderYawDelta += this.ridingEntity.yaw - this.ridingEntity.prevYaw;
                    for (this.entityRiderPitchDelta += this.ridingEntity.pitch - this.ridingEntity.prevPitch; this.entityRiderYawDelta >= 180; this.entityRiderYawDelta -= 360)
                    {
                    }

                    while (this.entityRiderYawDelta < -180)
                    {
                        this.entityRiderYawDelta += 360;
                    }

                    while (this.entityRiderPitchDelta >= 180)
                    {
                        this.entityRiderPitchDelta -= 360;
                    }

                    while (this.entityRiderPitchDelta < -180)
                    {
                        this.entityRiderPitchDelta += 360;
                    }

                    double d1 = this.entityRiderYawDelta * 0.5;
                    double d3 = this.entityRiderPitchDelta * 0.5;
                    float f5 = 10F;
                    if (d1 > f5)
                    {
                        d1 = f5;
                    }

                    if (d1 < (-f5))
                    {
                        d1 = (-f5);
                    }

                    if (d3 > f5)
                    {
                        d3 = f5;
                    }

                    if (d3 < (-f5))
                    {
                        d3 = (-f5);
                    }

                    this.entityRiderYawDelta -= d1;
                    this.entityRiderPitchDelta -= d3;
                    this.yaw = (float)(this.yaw + d1);
                    this.pitch = (float)(this.pitch + d3);
                }
            }
        }

        public virtual void UpdateRiderPosition()
        {
            this.riddenByEntity.SetPosition(this.x, this.y + this.GetMountedYOffset() + this.riddenByEntity.GetYOffset(), this.z);
        }

        public virtual double GetYOffset()
        {
            return this.yOffset;
        }

        public virtual double GetMountedYOffset()
        {
            return this.height * 0.75;
        }

        public virtual void MountEntity(Entity entity1)
        {
            this.entityRiderPitchDelta = 0;
            this.entityRiderYawDelta = 0;
            if (entity1 == null)
            {
                if (this.ridingEntity != null)
                {
                    this.SetLocationAndAngles(this.ridingEntity.x, this.ridingEntity.boundingBox.y0 + this.ridingEntity.height, this.ridingEntity.z, this.yaw, this.pitch);
                    this.ridingEntity.riddenByEntity = null;
                }

                this.ridingEntity = null;
            }
            else if (this.ridingEntity == entity1)
            {
                this.ridingEntity.riddenByEntity = null;
                this.ridingEntity = null;
                this.SetLocationAndAngles(entity1.x, entity1.boundingBox.y0 + entity1.height, entity1.z, this.yaw, this.pitch);
            }
            else
            {
                if (this.ridingEntity != null)
                {
                    this.ridingEntity.riddenByEntity = null;
                }

                if (entity1.riddenByEntity != null)
                {
                    entity1.riddenByEntity.ridingEntity = null;
                }

                this.ridingEntity = entity1;
                entity1.riddenByEntity = this;
            }
        }

        public virtual void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            this.SetPosition(d1, d3, d5);
            this.SetRotation(f7, f8);
            IList<AABB> list10 = this.worldObj.GetCubes(this, this.boundingBox.GetInsetBoundingBox(8 / 256, 0, 8 / 256));
            if (list10.Count > 0)
            {
                double d11 = 0;
                for (int i13 = 0; i13 < list10.Count; ++i13)
                {
                    AABB axisAlignedBB14 = list10[i13];
                    if (axisAlignedBB14.y1 > d11)
                    {
                        d11 = axisAlignedBB14.y1;
                    }
                }

                d3 += d11 - this.boundingBox.y0;
                this.SetPosition(d1, d3, d5);
            }
        }

        public virtual float GetCollisionBorderSize()
        {
            return 0.1F;
        }

        public virtual Vec3 GetLookVec()
        {
            return null;
        }

        public virtual void SetInPortal()
        {
        }

        public virtual ItemInstance[] GetInventory()
        {
            return null;
        }

        public virtual void SetVelocity(double d1, double d3, double d5)
        {
            this.motionX = d1;
            this.motionY = d3;
            this.motionZ = d5;
        }

        public virtual void HandleHealthUpdate(byte b1)
        {
        }

        public virtual void PerformHurtAnimation()
        {
        }

        public virtual void UpdateCloak()
        {
        }

        public virtual void OutfitWithItem(int i1, int i2, int i3)
        {
        }

        public virtual bool IsBurning()
        {
            return this.fire > 0 || this.GetFlag(0);
        }

        public virtual bool IsRiding()
        {
            return this.ridingEntity != null || this.GetFlag(2);
        }

        public virtual bool IsSneaking()
        {
            return this.GetFlag(1);
        }

        public virtual void SetSneaking(bool z1)
        {
            this.SetFlag(1, z1);
        }

        protected virtual bool GetFlag(int i1)
        {
            return (this.dataWatcher.GetWatchableObjectByte(0) & 1 << i1) != 0;
        }

        protected virtual void SetFlag(int i1, bool z2)
        {
            sbyte b3 = this.dataWatcher.GetWatchableObjectByte(0);
            if (z2)
            {
                this.dataWatcher.UpdateObject(0, (sbyte)(b3 | (sbyte)(1 << i1)));
            }
            else
            {
                this.dataWatcher.UpdateObject(0, (sbyte)(b3 & ~(1 << i1)));
            }
        }

        public virtual void OnStruckByLightning(LightningBolt entityLightningBolt1)
        {
            this.DealFireDamage(5);
            ++this.fire;
            if (this.fire == 0)
            {
                this.fire = 300;
            }
        }

        public virtual void OnKillEntity(Mob entityLiving1)
        {
        }

        protected virtual bool PushOutOfBlocks(double d1, double d3, double d5)
        {
            int i7 = Mth.Floor(d1);
            int i8 = Mth.Floor(d3);
            int i9 = Mth.Floor(d5);
            double d10 = d1 - i7;
            double d12 = d3 - i8;
            double d14 = d5 - i9;
            if (this.worldObj.IsSolidBlockingTile(i7, i8, i9))
            {
                bool z16 = !this.worldObj.IsSolidBlockingTile(i7 - 1, i8, i9);
                bool z17 = !this.worldObj.IsSolidBlockingTile(i7 + 1, i8, i9);
                bool z18 = !this.worldObj.IsSolidBlockingTile(i7, i8 - 1, i9);
                bool z19 = !this.worldObj.IsSolidBlockingTile(i7, i8 + 1, i9);
                bool z20 = !this.worldObj.IsSolidBlockingTile(i7, i8, i9 - 1);
                bool z21 = !this.worldObj.IsSolidBlockingTile(i7, i8, i9 + 1);
                sbyte b22 = -1;
                double d23 = 9999;
                if (z16 && d10 < d23)
                {
                    d23 = d10;
                    b22 = 0;
                }

                if (z17 && 1 - d10 < d23)
                {
                    d23 = 1 - d10;
                    b22 = 1;
                }

                if (z18 && d12 < d23)
                {
                    d23 = d12;
                    b22 = 2;
                }

                if (z19 && 1 - d12 < d23)
                {
                    d23 = 1 - d12;
                    b22 = 3;
                }

                if (z20 && d14 < d23)
                {
                    d23 = d14;
                    b22 = 4;
                }

                if (z21 && 1 - d14 < d23)
                {
                    d23 = 1 - d14;
                    b22 = 5;
                }

                float f25 = this.rand.NextFloat() * 0.2F + 0.1F;
                if (b22 == 0)
                {
                    this.motionX = (-f25);
                }

                if (b22 == 1)
                {
                    this.motionX = f25;
                }

                if (b22 == 2)
                {
                    this.motionY = (-f25);
                }

                if (b22 == 3)
                {
                    this.motionY = f25;
                }

                if (b22 == 4)
                {
                    this.motionZ = (-f25);
                }

                if (b22 == 5)
                {
                    this.motionZ = f25;
                }
            }

            return false;
        }
    }
}