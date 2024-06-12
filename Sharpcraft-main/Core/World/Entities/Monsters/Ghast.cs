using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Ghast : FlyingMob, IEnemy
    {
        public int courseChangeCooldown = 0;
        public double waypointX;
        public double waypointY;
        public double waypointZ;
        private Entity targetedEntity = null;
        private int aggroCooldown = 0;
        public int prevAttackCounter = 0;
        public int attackCounter = 0;
        public Ghast(Level world1) : base(world1)
        {
            this.texture = "/mob/ghast.png";
            this.SetSize(4F, 4F);
            this.isImmuneToFire = true;
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            sbyte b1 = this.dataWatcher.GetWatchableObjectByte(16);
            this.texture = b1 == 1 ? "/mob/ghast_fire.png" : "/mob/ghast.png";
        }

        protected override void UpdatePlayerActionState()
        {
            if (!this.worldObj.isRemote && this.worldObj.difficultySetting == 0)
            {
                this.SetEntityDead();
            }

            this.DistanceDespawn();
            this.prevAttackCounter = this.attackCounter;
            double d1 = this.waypointX - this.x;
            double d3 = this.waypointY - this.y;
            double d5 = this.waypointZ - this.z;
            double d7 = Mth.Sqrt(d1 * d1 + d3 * d3 + d5 * d5);
            if (d7 < 1 || d7 > 60)
            {
                this.waypointX = this.x + (this.rand.NextFloat() * 2F - 1F) * 16F;
                this.waypointY = this.y + (this.rand.NextFloat() * 2F - 1F) * 16F;
                this.waypointZ = this.z + (this.rand.NextFloat() * 2F - 1F) * 16F;
            }

            if (this.courseChangeCooldown-- <= 0)
            {
                this.courseChangeCooldown += this.rand.NextInt(5) + 2;
                if (this.IsCourseTraversable(this.waypointX, this.waypointY, this.waypointZ, d7))
                {
                    this.motionX += d1 / d7 * 0.1;
                    this.motionY += d3 / d7 * 0.1;
                    this.motionZ += d5 / d7 * 0.1;
                }
                else
                {
                    this.waypointX = this.x;
                    this.waypointY = this.y;
                    this.waypointZ = this.z;
                }
            }

            if (this.targetedEntity != null && this.targetedEntity.isDead)
            {
                this.targetedEntity = null;
            }

            if (this.targetedEntity == null || this.aggroCooldown-- <= 0)
            {
                this.targetedEntity = this.worldObj.GetClosestPlayerToEntity(this, 100);
                if (this.targetedEntity != null)
                {
                    this.aggroCooldown = 20;
                }
            }

            double d9 = 64;
            if (this.targetedEntity != null && this.targetedEntity.GetDistanceSqToEntity(this) < d9 * d9)
            {
                double d11 = this.targetedEntity.x - this.x;
                double d13 = this.targetedEntity.boundingBox.y0 + this.targetedEntity.height / 2F - (this.y + this.height / 2F);
                double d15 = this.targetedEntity.z - this.z;
                this.renderYawOffset = this.yaw = -((float)Math.Atan2(d11, d15)) * 180F / Mth.PI;
                if (this.CanEntityBeSeen(this.targetedEntity))
                {
                    if (this.attackCounter == 10)
                    {
                        this.worldObj.PlaySound(this, "mob.ghast.charge", this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                    }

                    ++this.attackCounter;
                    if (this.attackCounter == 20)
                    {
                        this.worldObj.PlaySound(this, "mob.ghast.fireball", this.GetSoundVolume(), (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
                        Fireball entityFireball17 = new Fireball(this.worldObj, this, d11, d13, d15);
                        double d18 = 4;
                        Vec3 vec3D20 = this.GetLook(1F);
                        entityFireball17.x = this.x + vec3D20.x * d18;
                        entityFireball17.y = this.y + this.height / 2F + 0.5;
                        entityFireball17.z = this.z + vec3D20.z * d18;
                        this.worldObj.AddEntity(entityFireball17);
                        this.attackCounter = -40;
                    }
                }
                else if (this.attackCounter > 0)
                {
                    --this.attackCounter;
                }
            }
            else
            {
                this.renderYawOffset = this.yaw = -((float)Math.Atan2(this.motionX, this.motionZ)) * 180F / Mth.PI;
                if (this.attackCounter > 0)
                {
                    --this.attackCounter;
                }
            }

            if (!this.worldObj.isRemote)
            {
                sbyte b21 = this.dataWatcher.GetWatchableObjectByte(16);
                sbyte b12 = (sbyte)(this.attackCounter > 10 ? 1 : 0);
                if (b21 != b12)
                {
                    this.dataWatcher.UpdateObject(16, b12);
                }
            }
        }

        private bool IsCourseTraversable(double d1, double d3, double d5, double d7)
        {
            double d9 = (this.waypointX - this.x) / d7;
            double d11 = (this.waypointY - this.y) / d7;
            double d13 = (this.waypointZ - this.z) / d7;
            AABB axisAlignedBB15 = this.boundingBox.Copy();
            for (int i16 = 1; i16 < d7; ++i16)
            {
                axisAlignedBB15.Offset(d9, d11, d13);
                if (this.worldObj.GetCubes(this, axisAlignedBB15).Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        protected override string GetLivingSound()
        {
            return "mob.ghast.moan";
        }

        protected override string GetHurtSound()
        {
            return "mob.ghast.scream";
        }

        protected override string GetDeathSound()
        {
            return "mob.ghast.death";
        }

        protected override int GetDropItemId()
        {
            return Item.gunpowder.id;
        }

        protected override float GetSoundVolume()
        {
            return 10F;
        }

        public override bool GetCanSpawnHere()
        {
            return this.rand.NextInt(20) == 0 && base.GetCanSpawnHere() && this.worldObj.difficultySetting > 0;
        }

        public override int GetMaxSpawnedInChunk()
        {
            return 1;
        }
    }
}