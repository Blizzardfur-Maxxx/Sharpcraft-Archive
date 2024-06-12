using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using System;

namespace SharpCraft.Core.World.Entities
{
    public class Path : Mob
    {
        private GameLevel.PathFinding.Path pathToEntity;
        protected Entity playerToAttack;
        protected bool hasAttacked = false;
        public Path(Level world1) : base(world1)
        {
        }

        protected virtual bool IsMovementCeased()
        {
            return false;
        }

        protected override void UpdatePlayerActionState()
        {
            Profiler.StartSection("ai");
            this.hasAttacked = this.IsMovementCeased();
            float f1 = 16F;
            if (this.playerToAttack == null)
            {
                this.playerToAttack = this.FindPlayerToAttack();
                if (this.playerToAttack != null)
                {
                    this.pathToEntity = this.worldObj.GetPathToEntity(this, this.playerToAttack, f1);
                }
            }
            else if (!this.playerToAttack.IsEntityAlive())
            {
                this.playerToAttack = null;
            }
            else
            {
                float f2 = this.playerToAttack.GetDistanceToEntity(this);
                if (this.CanEntityBeSeen(this.playerToAttack))
                {
                    this.AttackEntity(this.playerToAttack, f2);
                }
                else
                {
                    this.AttackBlockedEntity(this.playerToAttack, f2);
                }
            }
            Profiler.EndSection();
            if (this.hasAttacked || this.playerToAttack == null || this.pathToEntity != null && this.rand.NextInt(20) != 0)
            {
                if (!this.hasAttacked && (this.pathToEntity == null && this.rand.NextInt(80) == 0 || this.rand.NextInt(80) == 0))
                {
                    this.Func_B();
                }
            }
            else
            {
                this.pathToEntity = this.worldObj.GetPathToEntity(this, this.playerToAttack, f1);
            }

            int i21 = Mth.Floor(this.boundingBox.y0 + 0.5);
            bool z3 = this.IsInWater();
            bool z4 = this.HandleLavaMovement();
            this.pitch = 0F;
            if (this.pathToEntity != null && this.rand.NextInt(100) != 0)
            {
                Profiler.StartSection("followpath");
                Vec3 vec3D5 = this.pathToEntity.CurrentPos(this);
                double d6 = this.width * 2F;
                while (vec3D5 != null && vec3D5.SquareDistanceTo(this.x, vec3D5.y, this.z) < d6 * d6)
                {
                    this.pathToEntity.Next();
                    if (this.pathToEntity.IsDone())
                    {
                        vec3D5 = null;
                        this.pathToEntity = null;
                    }
                    else
                    {
                        vec3D5 = this.pathToEntity.CurrentPos(this);
                    }
                }

                this.isJumping = false;
                if (vec3D5 != null)
                {
                    double d8 = vec3D5.x - this.x;
                    double d10 = vec3D5.z - this.z;
                    double d12 = vec3D5.y - i21;
                    float f14 = (float)(Math.Atan2(d10, d8) * 180 / Mth.PI) - 90F;
                    float f15 = f14 - this.yaw;
                    for (this.moveForward = this.moveSpeed; f15 < -180F; f15 += 360F)
                    {
                    }

                    while (f15 >= 180F)
                    {
                        f15 -= 360F;
                    }

                    if (f15 > 30F)
                    {
                        f15 = 30F;
                    }

                    if (f15 < -30F)
                    {
                        f15 = -30F;
                    }

                    this.yaw += f15;
                    if (this.hasAttacked && this.playerToAttack != null)
                    {
                        double d16 = this.playerToAttack.x - this.x;
                        double d18 = this.playerToAttack.z - this.z;
                        float f20 = this.yaw;
                        this.yaw = (float)(Math.Atan2(d18, d16) * 180 / Mth.PI) - 90F;
                        f15 = (f20 - this.yaw + 90F) * Mth.PI / 180F;
                        this.moveStrafing = -Mth.Sin(f15) * this.moveForward * 1F;
                        this.moveForward = Mth.Cos(f15) * this.moveForward * 1F;
                    }

                    if (d12 > 0)
                    {
                        this.isJumping = true;
                    }
                }

                if (this.playerToAttack != null)
                {
                    this.FaceEntity(this.playerToAttack, 30F, 30F);
                }

                if (this.isCollidedHorizontally && !this.HasPath())
                {
                    this.isJumping = true;
                }

                if (this.rand.NextFloat() < 0.8F && (z3 || z4))
                {
                    this.isJumping = true;
                }
                Profiler.EndSection();
            }
            else
            {
                base.UpdatePlayerActionState();
                this.pathToEntity = null;
            }
        }

        protected virtual void Func_B()
        {
            Profiler.StartSection("stroll");
            bool z1 = false;
            int i2 = -1;
            int i3 = -1;
            int i4 = -1;
            float f5 = -99999F;
            for (int i6 = 0; i6 < 10; ++i6)
            {
                int i7 = Mth.Floor(this.x + this.rand.NextInt(13) - 6);
                int i8 = Mth.Floor(this.y + this.rand.NextInt(7) - 3);
                int i9 = Mth.Floor(this.z + this.rand.NextInt(13) - 6);
                float f10 = this.GetBlockPathWeight(i7, i8, i9);
                if (f10 > f5)
                {
                    f5 = f10;
                    i2 = i7;
                    i3 = i8;
                    i4 = i9;
                    z1 = true;
                }
            }

            if (z1)
            {
                this.pathToEntity = this.worldObj.GetEntityPathToXYZ(this, i2, i3, i4, 10F);
            }
            Profiler.EndSection();
        }

        protected virtual void AttackEntity(Entity entity1, float f2)
        {
        }

        protected virtual void AttackBlockedEntity(Entity entity1, float f2)
        {
        }

        protected virtual float GetBlockPathWeight(int i1, int i2, int i3)
        {
            return 0F;
        }

        protected virtual Entity FindPlayerToAttack()
        {
            return null;
        }

        public override bool GetCanSpawnHere()
        {
            int i1 = Mth.Floor(this.x);
            int i2 = Mth.Floor(this.boundingBox.y0);
            int i3 = Mth.Floor(this.z);
            return base.GetCanSpawnHere() && this.GetBlockPathWeight(i1, i2, i3) >= 0F;
        }

        public virtual bool HasPath()
        {
            return this.pathToEntity != null;
        }

        public virtual void SetPathToEntity(GameLevel.PathFinding.Path pathEntity1)
        {
            this.pathToEntity = pathEntity1;
        }

        public virtual Entity GetTarget()
        {
            return this.playerToAttack;
        }

        public virtual void SetTarget(Entity entity1)
        {
            this.playerToAttack = entity1;
        }
    }
}