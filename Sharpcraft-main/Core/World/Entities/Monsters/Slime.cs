using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Slime : Mob, IEnemy
    {
        public float field_a;
        public float field_b;
        private int slimeJumpDelay = 0;
        public Slime(Level world1) : base(world1)
        {
            this.texture = "/mob/slime.png";
            int i2 = 1 << this.rand.NextInt(3);
            this.yOffset = 0F;
            this.slimeJumpDelay = this.rand.NextInt(20) + 10;
            this.SetSlimeSize(i2);
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)1);
        }

        public virtual void SetSlimeSize(int i1)
        {
            this.dataWatcher.UpdateObject(16, (sbyte)i1);
            this.SetSize(0.6F * i1, 0.6F * i1);
            this.health = i1 * i1;
            this.SetPosition(this.x, this.y, this.z);
        }

        public virtual int GetSlimeSize()
        {
            return this.dataWatcher.GetWatchableObjectByte(16);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetInteger("Size", this.GetSlimeSize() - 1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.SetSlimeSize(nBTTagCompound1.GetInteger("Size") + 1);
        }

        public override void OnUpdate()
        {
            this.field_b = this.field_a;
            bool z1 = this.onGround;
            base.OnUpdate();
            if (this.onGround && !z1)
            {
                int i2 = this.GetSlimeSize();
                for (int i3 = 0; i3 < i2 * 8; ++i3)
                {
                    float f4 = this.rand.NextFloat() * Mth.PI * 2F;
                    float f5 = this.rand.NextFloat() * 0.5F + 0.5F;
                    float f6 = Mth.Sin(f4) * i2 * 0.5F * f5;
                    float f7 = Mth.Cos(f4) * i2 * 0.5F * f5;
                    this.worldObj.AddParticle("slime", this.x + f6, this.boundingBox.y0, this.z + f7, 0, 0, 0);
                }

                if (i2 > 2)
                {
                    this.worldObj.PlaySound(this, "mob.slime", this.GetSoundVolume(), ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F) / 0.8F);
                }

                this.field_a = -0.5F;
            }

            this.field_a *= 0.6F;
        }

        protected override void UpdatePlayerActionState()
        {
            this.DistanceDespawn();
            Player entityPlayer1 = this.worldObj.GetClosestPlayerToEntity(this, 16);
            if (entityPlayer1 != null)
            {
                this.FaceEntity(entityPlayer1, 10F, 20F);
            }

            if (this.onGround && this.slimeJumpDelay-- <= 0)
            {
                this.slimeJumpDelay = this.rand.NextInt(20) + 10;
                if (entityPlayer1 != null)
                {
                    this.slimeJumpDelay /= 3;
                }

                this.isJumping = true;
                if (this.GetSlimeSize() > 1)
                {
                    this.worldObj.PlaySound(this, "mob.slime", this.GetSoundVolume(), ((this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F) * 0.8F);
                }

                this.field_a = 1F;
                this.moveStrafing = 1F - this.rand.NextFloat() * 2F;
                this.moveForward = 1 * this.GetSlimeSize();
            }
            else
            {
                this.isJumping = false;
                if (this.onGround)
                {
                    this.moveStrafing = this.moveForward = 0F;
                }
            }
        }

        public override void SetEntityDead()
        {
            int i1 = this.GetSlimeSize();
            if (!this.worldObj.isRemote && i1 > 1 && this.health == 0)
            {
                for (int i2 = 0; i2 < 4; ++i2)
                {
                    float f3 = (i2 % 2 - 0.5F) * i1 / 4F;
                    float f4 = (i2 / 2 - 0.5F) * i1 / 4F;
                    Slime entitySlime5 = new Slime(this.worldObj);
                    entitySlime5.SetSlimeSize(i1 / 2);
                    entitySlime5.SetLocationAndAngles(this.x + f3, this.y + 0.5, this.z + f4, this.rand.NextFloat() * 360F, 0F);
                    this.worldObj.AddEntity(entitySlime5);
                }
            }

            base.SetEntityDead();
        }

        public override void OnCollideWithPlayer(Player entityPlayer1)
        {
            int i2 = this.GetSlimeSize();
            if (i2 > 1 && this.CanEntityBeSeen(entityPlayer1) && this.GetDistanceToEntity(entityPlayer1) < 0.6 * i2 && entityPlayer1.AttackEntityFrom(this, i2))
            {
                this.worldObj.PlaySound(this, "mob.slimeattack", 1F, (this.rand.NextFloat() - this.rand.NextFloat()) * 0.2F + 1F);
            }
        }

        protected override string GetHurtSound()
        {
            return "mob.slime";
        }

        protected override string GetDeathSound()
        {
            return "mob.slime";
        }

        protected override int GetDropItemId()
        {
            return this.GetSlimeSize() == 1 ? Item.slimeBall.id : 0;
        }

        public override bool GetCanSpawnHere()
        {
            LevelChunk chunk1 = this.worldObj.GetChunkAt(Mth.Floor(this.x), Mth.Floor(this.z));
            return (this.GetSlimeSize() == 1 || this.worldObj.difficultySetting > 0) && this.rand.NextInt(10) == 0 && chunk1.GetRandom(987234911).NextInt(10) == 0 && this.y < 16;
        }

        protected override float GetSoundVolume()
        {
            return 0.6F;
        }
    }
}