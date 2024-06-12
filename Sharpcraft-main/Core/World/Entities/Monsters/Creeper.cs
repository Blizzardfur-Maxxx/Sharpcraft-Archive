using SharpCraft.Core.NBT;
using SharpCraft.Core.World.Entities.Weather;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Creeper : Monster
    {
        int timeSinceIgnited;
        int lastActiveTime;
        public Creeper(Level world1) : base(world1)
        {
            this.texture = "/mob/creeper.png";
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)-1);
            this.dataWatcher.AddObject(17, (sbyte)0);
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            if (this.dataWatcher.GetWatchableObjectByte(17) == 1)
            {
                nBTTagCompound1.SetBoolean("powered", true);
            }
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.dataWatcher.UpdateObject(17, (sbyte)(nBTTagCompound1.GetBoolean("powered") ? 1 : 0));
        }

        protected override void AttackBlockedEntity(Entity entity1, float f2)
        {
            if (!this.worldObj.isRemote)
            {
                if (this.timeSinceIgnited > 0)
                {
                    this.SetCreeperState(-1);
                    --this.timeSinceIgnited;
                    if (this.timeSinceIgnited < 0)
                    {
                        this.timeSinceIgnited = 0;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.lastActiveTime = this.timeSinceIgnited;
            if (this.worldObj.isRemote)
            {
                int i1 = this.GetCreeperState();
                if (i1 > 0 && this.timeSinceIgnited == 0)
                {
                    this.worldObj.PlaySound(this, "random.fuse", 1F, 0.5F);
                }

                this.timeSinceIgnited += i1;
                if (this.timeSinceIgnited < 0)
                {
                    this.timeSinceIgnited = 0;
                }

                if (this.timeSinceIgnited >= 30)
                {
                    this.timeSinceIgnited = 30;
                }
            }

            base.OnUpdate();
            if (this.playerToAttack == null && this.timeSinceIgnited > 0)
            {
                this.SetCreeperState(-1);
                --this.timeSinceIgnited;
                if (this.timeSinceIgnited < 0)
                {
                    this.timeSinceIgnited = 0;
                }
            }
        }

        protected override string GetHurtSound()
        {
            return "mob.creeper";
        }

        protected override string GetDeathSound()
        {
            return "mob.creeperdeath";
        }

        public override void OnDeath(Entity entity1)
        {
            base.OnDeath(entity1);
            if (entity1 is Skeleton)
            {
                this.DropItem(Item.record13.id + this.rand.NextInt(2), 1);
            }
        }

        protected override void AttackEntity(Entity entity1, float f2)
        {
            if (!this.worldObj.isRemote)
            {
                int i3 = this.GetCreeperState();
                if (i3 <= 0 && f2 < 3F || i3 > 0 && f2 < 7F)
                {
                    if (this.timeSinceIgnited == 0)
                    {
                        this.worldObj.PlaySound(this, "random.fuse", 1F, 0.5F);
                    }

                    this.SetCreeperState(1);
                    ++this.timeSinceIgnited;
                    if (this.timeSinceIgnited >= 30)
                    {
                        if (this.GetPowered())
                        {
                            this.worldObj.Explode(this, this.x, this.y, this.z, 6F);
                        }
                        else
                        {
                            this.worldObj.Explode(this, this.x, this.y, this.z, 3F);
                        }

                        this.SetEntityDead();
                    }

                    this.hasAttacked = true;
                }
                else
                {
                    this.SetCreeperState(-1);
                    --this.timeSinceIgnited;
                    if (this.timeSinceIgnited < 0)
                    {
                        this.timeSinceIgnited = 0;
                    }
                }
            }
        }

        public virtual bool GetPowered()
        {
            return this.dataWatcher.GetWatchableObjectByte(17) == 1;
        }

        public virtual float SetCreeperFlashTime(float f1)
        {
            return (this.lastActiveTime + (this.timeSinceIgnited - this.lastActiveTime) * f1) / 28F;
        }

        protected override int GetDropItemId()
        {
            return Item.gunpowder.id;
        }

        private int GetCreeperState()
        {
            return this.dataWatcher.GetWatchableObjectByte(16);
        }

        private void SetCreeperState(int i1)
        {
            this.dataWatcher.UpdateObject(16, (sbyte)i1);
        }

        public override void OnStruckByLightning(LightningBolt entityLightningBolt1)
        {
            base.OnStruckByLightning(entityLightningBolt1);
            this.dataWatcher.UpdateObject(17, (sbyte)1);
        }
    }
}