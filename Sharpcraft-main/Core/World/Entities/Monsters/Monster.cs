using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Monster : Path, IEnemy
    {
        protected int attackStrength = 2;
        public Monster(Level world1) : base(world1)
        {
            this.health = 20;
        }

        public override void OnLivingUpdate()
        {
            float f1 = this.GetEntityBrightness(1F);
            if (f1 > 0.5F)
            {
                this.age += 2;
            }

            base.OnLivingUpdate();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!this.worldObj.isRemote && this.worldObj.difficultySetting == 0)
            {
                this.SetEntityDead();
            }
        }

        protected override Entity FindPlayerToAttack()
        {
            Player entityPlayer1 = this.worldObj.GetClosestPlayerToEntity(this, 16);
            return entityPlayer1 != null && this.CanEntityBeSeen(entityPlayer1) ? entityPlayer1 : null;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (base.AttackEntityFrom(entity1, i2))
            {
                if (this.riddenByEntity != entity1 && this.ridingEntity != entity1)
                {
                    if (entity1 != this)
                    {
                        this.playerToAttack = entity1;
                    }

                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        protected override void AttackEntity(Entity entity1, float f2)
        {
            if (this.attackTime <= 0 && f2 < 2F && entity1.boundingBox.y1 > this.boundingBox.y0 && entity1.boundingBox.y0 < this.boundingBox.y1)
            {
                this.attackTime = 20;
                entity1.AttackEntityFrom(this, this.attackStrength);
            }
        }

        protected override float GetBlockPathWeight(int i1, int i2, int i3)
        {
            return 0.5F - this.worldObj.GetBrightness(i1, i2, i3);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
        }

        public override bool GetCanSpawnHere()
        {
            int i1 = Mth.Floor(this.x);
            int i2 = Mth.Floor(this.boundingBox.y0);
            int i3 = Mth.Floor(this.z);
            if (this.worldObj.GetSavedLightValue(LightLayer.Sky, i1, i2, i3) > this.rand.NextInt(32))
            {
                return false;
            }
            else
            {
                int i4 = this.worldObj.GetRawBrightness(i1, i2, i3);
                if (this.worldObj.Func_B())
                {
                    int i5 = this.worldObj.skyDarken;
                    this.worldObj.skyDarken = 10;
                    i4 = this.worldObj.GetRawBrightness(i1, i2, i3);
                    this.worldObj.skyDarken = i5;
                }

                return i4 <= this.rand.NextInt(8) && base.GetCanSpawnHere();
            }
        }
    }
}