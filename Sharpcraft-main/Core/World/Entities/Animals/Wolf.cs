using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Wolf : Animal
    {
        private bool looksWithInterest = false;
        private float field_b;
        private float field_c;
        private bool isWet;
        private bool field_g;
        private float timeWolfIsShaking;
        private float prevTimeWolfIsShaking;
        public Wolf(Level world1) : base(world1)
        {
            this.texture = "/mob/wolf.png";
            this.SetSize(0.8F, 0.8F);
            this.moveSpeed = 1.1F;
            this.health = 8;
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)0);
            this.dataWatcher.AddObject(17, "");
            this.dataWatcher.AddObject(18, this.health);
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        public override string GetEntityTexture()
        {
            return this.IsWolfTamed() ? "/mob/wolf_tame.png" : (this.IsWolfAngry() ? "/mob/wolf_angry.png" : base.GetEntityTexture());
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetBoolean("Angry", this.IsWolfAngry());
            nBTTagCompound1.SetBoolean("Sitting", this.IsSitting());
            if (this.GetOwner() == null)
            {
                nBTTagCompound1.SetString("Owner", "");
            }
            else
            {
                nBTTagCompound1.SetString("Owner", this.GetOwner());
            }
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.SetAngry(nBTTagCompound1.GetBoolean("Angry"));
            this.SetSitting(nBTTagCompound1.GetBoolean("Sitting"));
            string string2 = nBTTagCompound1.GetString("Owner");
            if (string2.Length > 0)
            {
                this.SetOwner(string2);
                this.SetTamed(true);
            }
        }

        protected override bool CanDespawn()
        {
            return !this.IsWolfTamed();
        }

        protected override string GetLivingSound()
        {
            return this.IsWolfAngry() ? "mob.wolf.growl" : (this.rand.NextInt(3) == 0 ? (this.IsWolfTamed() && this.dataWatcher.GetWatchableObjectInt(18) < 10 ? "mob.wolf.whine" : "mob.wolf.panting") : "mob.wolf.bark");
        }

        protected override string GetHurtSound()
        {
            return "mob.wolf.hurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.wolf.death";
        }

        protected override float GetSoundVolume()
        {
            return 0.4F;
        }

        protected override int GetDropItemId()
        {
            return -1;
        }

        protected override void UpdatePlayerActionState()
        {
            base.UpdatePlayerActionState();
            if (!this.hasAttacked && !this.HasPath() && this.IsWolfTamed() && this.ridingEntity == null)
            {
                Player entityPlayer3 = this.worldObj.GetPlayerEntityByName(this.GetOwner());
                if (entityPlayer3 != null)
                {
                    float f2 = entityPlayer3.GetDistanceToEntity(this);
                    if (f2 > 5F)
                    {
                        this.SetPathEntity(entityPlayer3, f2);
                    }
                }
                else if (!this.IsInWater())
                {
                    this.SetSitting(true);
                }
            }
            else if (this.playerToAttack == null && !this.HasPath() && !this.IsWolfTamed() && this.worldObj.rand.NextInt(100) == 0)
            {
                List<Sheep> list1 = this.worldObj.GetEntitiesOfClass<Sheep>(typeof(Sheep), AABB.Of(this.x, this.y, this.z, this.x + 1, this.y + 1, this.z + 1).Expand(16, 4, 16));
                if (list1.Count != 0)
                {
                    this.SetTarget(list1[this.worldObj.rand.NextInt(list1.Count)]);
                }
            }

            if (this.IsInWater())
            {
                this.SetSitting(false);
            }

            if (!this.worldObj.isRemote)
            {
                this.dataWatcher.UpdateObject(18, this.health);
            }
        }

        public override void OnLivingUpdate()
        {
            base.OnLivingUpdate();
            this.looksWithInterest = false;
            if (this.HasCurrentTarget() && !this.HasPath() && !this.IsWolfAngry())
            {
                Entity entity1 = this.GetCurrentTarget();
                if (entity1 is Player)
                {
                    Player entityPlayer2 = (Player)entity1;
                    ItemInstance itemStack3 = entityPlayer2.inventory.GetCurrentItem();
                    if (itemStack3 != null)
                    {
                        if (!this.IsWolfTamed() && itemStack3.itemID == Item.bone.id)
                        {
                            this.looksWithInterest = true;
                        }
                        else if (this.IsWolfTamed() && Item.items[itemStack3.itemID] is ItemFood)
                        {
                            this.looksWithInterest = ((ItemFood)Item.items[itemStack3.itemID]).IsWolfFavorite();
                        }
                    }
                }
            }

            if (!this.isMultiplayerEntity && this.isWet && !this.field_g && !this.HasPath() && this.onGround)
            {
                this.field_g = true;
                this.timeWolfIsShaking = 0F;
                this.prevTimeWolfIsShaking = 0F;
                this.worldObj.SendTrackedEntityStatusUpdatePacket(this, (byte)8);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.field_c = this.field_b;
            if (this.looksWithInterest)
            {
                this.field_b += (1F - this.field_b) * 0.4F;
            }
            else
            {
                this.field_b += (0F - this.field_b) * 0.4F;
            }

            if (this.looksWithInterest)
            {
                this.numTicksToChaseTarget = 10;
            }

            if (this.IsWet())
            {
                this.isWet = true;
                this.field_g = false;
                this.timeWolfIsShaking = 0F;
                this.prevTimeWolfIsShaking = 0F;
            }
            else if ((this.isWet || this.field_g) && this.field_g)
            {
                if (this.timeWolfIsShaking == 0F)
                {
                    this.worldObj.PlaySound(this, "mob.wolf.shake", this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                }

                this.prevTimeWolfIsShaking = this.timeWolfIsShaking;
                this.timeWolfIsShaking += 0.05F;
                if (this.prevTimeWolfIsShaking >= 2F)
                {
                    this.isWet = false;
                    this.field_g = false;
                    this.prevTimeWolfIsShaking = 0F;
                    this.timeWolfIsShaking = 0F;
                }

                if (this.timeWolfIsShaking > 0.4F)
                {
                    float f1 = (float)this.boundingBox.y0;
                    int i2 = (int)(Mth.Sin((this.timeWolfIsShaking - 0.4F) * Mth.PI) * 7F);
                    for (int i3 = 0; i3 < i2; ++i3)
                    {
                        float f4 = (this.rand.NextFloat() * 2F - 1F) * this.width * 0.5F;
                        float f5 = (this.rand.NextFloat() * 2F - 1F) * this.width * 0.5F;
                        this.worldObj.AddParticle("splash", this.x + f4, f1 + 0.8F, this.z + f5, this.motionX, this.motionY, this.motionZ);
                    }
                }
            }
        }

        public virtual bool GetWolfShaking()
        {
            return this.isWet;
        }

        public virtual float GetShadingWhileShaking(float f1)
        {
            return 0.75F + (this.prevTimeWolfIsShaking + (this.timeWolfIsShaking - this.prevTimeWolfIsShaking) * f1) / 2F * 0.25F;
        }

        public virtual float GetShakeAngle(float f1, float f2)
        {
            float f3 = (this.prevTimeWolfIsShaking + (this.timeWolfIsShaking - this.prevTimeWolfIsShaking) * f1 + f2) / 1.8F;
            if (f3 < 0F)
            {
                f3 = 0F;
            }
            else if (f3 > 1F)
            {
                f3 = 1F;
            }

            return Mth.Sin(f3 * Mth.PI) * Mth.Sin(f3 * Mth.PI * 11F) * 0.15F * Mth.PI;
        }

        public virtual float GetInterestedAngle(float f1)
        {
            return (this.field_c + (this.field_b - this.field_c) * f1) * 0.15F * Mth.PI;
        }

        public override float GetEyeHeight()
        {
            return this.height * 0.8F;
        }

        protected override int GetFaceEntityDist()
        {
            return this.IsSitting() ? 20 : base.GetFaceEntityDist();
        }

        private void SetPathEntity(Entity entity1, float f2)
        {
            GameLevel.PathFinding.Path pathEntity3 = this.worldObj.GetPathToEntity(this, entity1, 16F);
            if (pathEntity3 == null && f2 > 12F)
            {
                int i4 = Mth.Floor(entity1.x) - 2;
                int i5 = Mth.Floor(entity1.z) - 2;
                int i6 = Mth.Floor(entity1.boundingBox.y0);
                for (int i7 = 0; i7 <= 4; ++i7)
                {
                    for (int i8 = 0; i8 <= 4; ++i8)
                    {
                        if ((i7 < 1 || i8 < 1 || i7 > 3 || i8 > 3) && this.worldObj.IsSolidBlockingTile(i4 + i7, i6 - 1, i5 + i8) && !this.worldObj.IsSolidBlockingTile(i4 + i7, i6, i5 + i8) && !this.worldObj.IsSolidBlockingTile(i4 + i7, i6 + 1, i5 + i8))
                        {
                            this.SetLocationAndAngles(i4 + i7 + 0.5F, i6, i5 + i8 + 0.5F, this.yaw, this.pitch);
                            return;
                        }
                    }
                }
            }
            else
            {
                this.SetPathToEntity(pathEntity3);
            }
        }

        protected override bool IsMovementCeased()
        {
            return this.IsSitting() || this.field_g;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            this.SetSitting(false);
            if (entity1 != null && !(entity1 is Player) && !(entity1 is Arrow))
            {
                i2 = (i2 + 1) / 2;
            }

            if (!base.AttackEntityFrom(entity1, i2))
            {
                return false;
            }
            else
            {
                if (!this.IsWolfTamed() && !this.IsWolfAngry())
                {
                    if (entity1 is Player)
                    {
                        this.SetAngry(true);
                        this.playerToAttack = entity1;
                    }

                    if (entity1 is Arrow && ((Arrow)entity1).owner != null)
                    {
                        entity1 = ((Arrow)entity1).owner;
                    }

                    if (entity1 is Mob)
                    {
                        IList<Wolf> list3 = this.worldObj.GetEntitiesOfClass<Wolf>(typeof(Wolf), AABB.Of(this.x, this.y, this.z, this.x + 1, this.y + 1, this.z + 1).Expand(16, 4, 16));
                        IEnumerator<Wolf> iterator4 = list3.GetEnumerator();
                        while (iterator4.MoveNext())
                        {
                            Wolf entityWolf6 = iterator4.Current;
                            if (!entityWolf6.IsWolfTamed() && entityWolf6.playerToAttack == null)
                            {
                                entityWolf6.playerToAttack = entity1;
                                if (entity1 is Player)
                                {
                                    entityWolf6.SetAngry(true);
                                }
                            }
                        }
                    }
                }
                else if (entity1 != this && entity1 != null)
                {
                    if (this.IsWolfTamed() && entity1 is Player && ((Player)entity1).username.ToLower().Equals(this.GetOwner()))
                    {
                        return true;
                    }

                    this.playerToAttack = entity1;
                }

                return true;
            }
        }

        protected override Entity FindPlayerToAttack()
        {
            return this.IsWolfAngry() ? this.worldObj.GetClosestPlayerToEntity(this, 16) : null;
        }

        protected override void AttackEntity(Entity entity1, float f2)
        {
            if (f2 > 2F && f2 < 6F && this.rand.NextInt(10) == 0)
            {
                if (this.onGround)
                {
                    double d8 = entity1.x - this.x;
                    double d5 = entity1.z - this.z;
                    float f7 = Mth.Sqrt(d8 * d8 + d5 * d5);
                    this.motionX = d8 / f7 * 0.5 * 0.8F + this.motionX * 0.2F;
                    this.motionZ = d5 / f7 * 0.5 * 0.8F + this.motionZ * 0.2F;
                    this.motionY = 0.4F;
                }
            }
            else if (f2 < 1.5 && entity1.boundingBox.y1 > this.boundingBox.y0 && entity1.boundingBox.y0 < this.boundingBox.y1)
            {
                this.attackTime = 20;
                byte b3 = 2;
                if (this.IsWolfTamed())
                {
                    b3 = 4;
                }

                entity1.AttackEntityFrom(this, b3);
            }
        }

        public override bool Interact(Player entityPlayer1)
        {
            ItemInstance itemStack2 = entityPlayer1.inventory.GetCurrentItem();
            if (!this.IsWolfTamed())
            {
                if (itemStack2 != null && itemStack2.itemID == Item.bone.id && !this.IsWolfAngry())
                {
                    --itemStack2.stackSize;
                    if (itemStack2.stackSize <= 0)
                    {
                        entityPlayer1.inventory.SetItem(entityPlayer1.inventory.currentItem, (ItemInstance)null);
                    }

                    if (!this.worldObj.isRemote)
                    {
                        if (this.rand.NextInt(3) == 0)
                        {
                            this.SetTamed(true);
                            this.SetPathToEntity((GameLevel.PathFinding.Path)null);
                            this.SetSitting(true);
                            this.health = 20;
                            this.SetOwner(entityPlayer1.username);
                            this.ShowParticleShit(true);
                            this.worldObj.SendTrackedEntityStatusUpdatePacket(this, (byte)7);
                        }
                        else
                        {
                            this.ShowParticleShit(false);
                            this.worldObj.SendTrackedEntityStatusUpdatePacket(this, (byte)6);
                        }
                    }

                    return true;
                }
            }
            else
            {
                if (itemStack2 != null && Item.items[itemStack2.itemID] is ItemFood)
                {
                    ItemFood itemFood3 = (ItemFood)Item.items[itemStack2.itemID];
                    if (itemFood3.IsWolfFavorite() && this.dataWatcher.GetWatchableObjectInt(18) < 20)
                    {
                        --itemStack2.stackSize;
                        if (itemStack2.stackSize <= 0)
                        {
                            entityPlayer1.inventory.SetItem(entityPlayer1.inventory.currentItem, (ItemInstance)null);
                        }

                        this.Heal(((ItemFood)Item.porkRaw).GetHealAmount());
                        return true;
                    }
                }

                if (entityPlayer1.username.ToLower().Equals(this.GetOwner()))
                {
                    if (!this.worldObj.isRemote)
                    {
                        this.SetSitting(!this.IsSitting());
                        this.isJumping = false;
                        this.SetPathToEntity((GameLevel.PathFinding.Path)null);
                    }

                    return true;
                }
            }

            return false;
        }

        internal virtual void ShowParticleShit(bool z1)
        {
            string string2 = "heart";
            if (!z1)
            {
                string2 = "smoke";
            }

            for (int i3 = 0; i3 < 7; ++i3)
            {
                double d4 = this.rand.NextGaussian() * 0.02;
                double d6 = this.rand.NextGaussian() * 0.02;
                double d8 = this.rand.NextGaussian() * 0.02;
                this.worldObj.AddParticle(string2, this.x + this.rand.NextFloat() * this.width * 2F - this.width, this.y + 0.5 + this.rand.NextFloat() * this.height, this.z + this.rand.NextFloat() * this.width * 2F - this.width, d4, d6, d8);
            }
        }

        public override void HandleHealthUpdate(byte b1)
        {
            if (b1 == 7)
            {
                this.ShowParticleShit(true);
            }
            else if (b1 == 6)
            {
                this.ShowParticleShit(false);
            }
            else if (b1 == 8)
            {
                this.field_g = true;
                this.timeWolfIsShaking = 0F;
                this.prevTimeWolfIsShaking = 0F;
            }
            else
            {
                base.HandleHealthUpdate(b1);
            }
        }

        public virtual float SetTailRotation()
        {
            return this.IsWolfAngry() ? 1.5393804F : (this.IsWolfTamed() ? (0.55F - (20 - this.dataWatcher.GetWatchableObjectInt(18)) * 0.02F) * Mth.PI : 0.62831855F);
        }

        public override int GetMaxSpawnedInChunk()
        {
            return 8;
        }

        public virtual string GetOwner()
        {
            return this.dataWatcher.GetWatchableObjectString(17);
        }

        public virtual void SetOwner(string string1)
        {
            this.dataWatcher.UpdateObject(17, string1);
        }

        public virtual bool IsSitting()
        {
            return (this.dataWatcher.GetWatchableObjectByte(16) & 1) != 0;
        }

        public virtual void SetSitting(bool z1)
        {
            sbyte b2 = this.dataWatcher.GetWatchableObjectByte(16);
            if (z1)
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 | 1));
            }
            else
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 & -2));
            }
        }

        public virtual bool IsWolfAngry()
        {
            return (this.dataWatcher.GetWatchableObjectByte(16) & 2) != 0;
        }

        public virtual void SetAngry(bool z1)
        {
            sbyte b2 = this.dataWatcher.GetWatchableObjectByte(16);
            if (z1)
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 | 2));
            }
            else
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 & -3));
            }
        }

        public virtual bool IsWolfTamed()
        {
            return (this.dataWatcher.GetWatchableObjectByte(16) & 4) != 0;
        }

        public virtual void SetTamed(bool z1)
        {
            sbyte b2 = this.dataWatcher.GetWatchableObjectByte(16);
            if (z1)
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 | 4));
            }
            else
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 & -5));
            }
        }
    }
}