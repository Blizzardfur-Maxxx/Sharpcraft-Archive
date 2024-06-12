using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using System;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Squid : WaterCreature
    {
        public float field_21089 = 0F;
        public float field_21088 = 0F;
        public float field_21087 = 0F;
        public float field_21086 = 0F;
        public float field_21085 = 0F;
        public float field_21084 = 0F;
        public float field_21083 = 0F;
        public float field_21082 = 0F;
        private float randomMotionSpeed = 0F;
        private float randDir = 0F;
        private float field_21079 = 0F;
        private float randomMotionVecX = 0F;
        private float randomMotionVecY = 0F;
        private float randomMotionVecZ = 0F;
        public Squid(Level world1) : base(world1)
        {
            this.texture = "/mob/squid.png";
            this.SetSize(0.95F, 0.95F);
            this.randDir = 1F / (this.rand.NextFloat() + 1F) * 0.2F;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
        }

        protected override string GetLivingSound()
        {
            return null;
        }

        protected override string GetHurtSound()
        {
            return null;
        }

        protected override string GetDeathSound()
        {
            return null;
        }

        protected override float GetSoundVolume()
        {
            return 0.4F;
        }

        protected override int GetDropItemId()
        {
            return 0;
        }

        protected override void DropFewItems()
        {
            int i1 = this.rand.NextInt(3) + 1;
            for (int i2 = 0; i2 < i1; ++i2)
            {
                this.EntityDropItem(new ItemInstance(Item.dyePowder, 1, 0), 0F);
            }
        }

        public override bool Interact(Player entityPlayer1)
        {
            return false;
        }

        public override bool IsInWater()
        {
            return this.worldObj.CheckAndHandleWater(this.boundingBox.Expand(0, -0.6000000238418579, 0), Material.water, this);
        }

        public override void OnLivingUpdate()
        {
            base.OnLivingUpdate();
            this.field_21088 = this.field_21089;
            this.field_21086 = this.field_21087;
            this.field_21084 = this.field_21085;
            this.field_21082 = this.field_21083;
            this.field_21085 += this.randDir;
            if (this.field_21085 > 6.2831855F)
            {
                this.field_21085 -= 6.2831855F;
                if (this.rand.NextInt(10) == 0)
                {
                    this.randDir = 1F / (this.rand.NextFloat() + 1F) * 0.2F;
                }
            }

            if (this.IsInWater())
            {
                float f1;
                if (this.field_21085 < Mth.PI)
                {
                    f1 = this.field_21085 / Mth.PI;
                    this.field_21083 = Mth.Sin(f1 * f1 * Mth.PI) * Mth.PI * 0.25F;
                    if (f1 > 0.75)
                    {
                        this.randomMotionSpeed = 1F;
                        this.field_21079 = 1F;
                    }
                    else
                    {
                        this.field_21079 *= 0.8F;
                    }
                }
                else
                {
                    this.field_21083 = 0F;
                    this.randomMotionSpeed *= 0.9F;
                    this.field_21079 *= 0.99F;
                }

                if (!this.isMultiplayerEntity)
                {
                    this.motionX = this.randomMotionVecX * this.randomMotionSpeed;
                    this.motionY = this.randomMotionVecY * this.randomMotionSpeed;
                    this.motionZ = this.randomMotionVecZ * this.randomMotionSpeed;
                }

                f1 = Mth.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                this.renderYawOffset += (-((float)Math.Atan2(this.motionX, this.motionZ)) * 180F / Mth.PI - this.renderYawOffset) * 0.1F;
                this.yaw = this.renderYawOffset;
                this.field_21087 += Mth.PI * this.field_21079 * 1.5F;
                this.field_21089 += (-((float)Math.Atan2(f1, this.motionY)) * 180F / Mth.PI - this.field_21089) * 0.1F;
            }
            else
            {
                this.field_21083 = Mth.Abs(Mth.Sin(this.field_21085)) * Mth.PI * 0.25F;
                if (!this.isMultiplayerEntity)
                {
                    this.motionX = 0;
                    this.motionY -= 0.08;
                    this.motionY *= 0.98F;
                    this.motionZ = 0;
                }

                this.field_21089 = (float)(this.field_21089 + (-90F - this.field_21089) * 0.02);
            }
        }

        public override void MoveEntityWithHeading(float f1, float f2)
        {
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
        }

        protected override void UpdatePlayerActionState()
        {
            if (this.rand.NextInt(50) == 0 || !this.inWater || this.randomMotionVecX == 0F && this.randomMotionVecY == 0F && this.randomMotionVecZ == 0F)
            {
                float f1 = this.rand.NextFloat() * Mth.PI * 2F;
                this.randomMotionVecX = Mth.Cos(f1) * 0.2F;
                this.randomMotionVecY = -0.1F + this.rand.NextFloat() * 0.2F;
                this.randomMotionVecZ = Mth.Sin(f1) * 0.2F;
            }

            this.DistanceDespawn();
        }
    }
}