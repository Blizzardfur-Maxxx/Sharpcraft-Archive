using SharpCraft.Core.NBT;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntityMobSpawner : TileEntity
    {
        public int delay = -1;
        private string mobID = "Pig";
        public double yaw;
        public double yaw2 = 0;
        public TileEntityMobSpawner()
        {
            this.delay = 20;
        }

        public virtual string GetMobID()
        {
            return this.mobID;
        }

        public virtual void SetEntityId(string string1)
        {
            this.mobID = string1;
        }

        public virtual bool AnyPlayerInRange()
        {
            return this.worldObj.GetClosestPlayer(this.xCoord + 0.5, this.yCoord + 0.5, this.zCoord + 0.5, 16) != null;
        }

        public override void UpdateEntity()
        {
            this.yaw2 = this.yaw;
            if (this.AnyPlayerInRange())
            {
                double d1 = this.xCoord + (float)worldObj.rand.NextFloat();
                double d3 = this.yCoord + (float)worldObj.rand.NextFloat();
                double d5 = this.zCoord + (float)worldObj.rand.NextFloat();
                this.worldObj.AddParticle("smoke", d1, d3, d5, 0, 0, 0);
                this.worldObj.AddParticle("flame", d1, d3, d5, 0, 0, 0);
                for (this.yaw += 1000F / (this.delay + 200F); this.yaw > 360; this.yaw2 -= 360)
                {
                    this.yaw -= 360;
                }

                if (!this.worldObj.isRemote)
                {
                    if (this.delay == -1)
                    {
                        this.UpdateDelay();
                    }

                    if (this.delay > 0)
                    {
                        --this.delay;
                        return;
                    }

                    byte b7 = 4;
                    for (int i8 = 0; i8 < b7; ++i8)
                    {
                        Mob entityLiving9 = ((Mob)EntityFactory.CreateEntity(this.mobID, this.worldObj));
                        if (entityLiving9 == null)
                        {
                            return;
                        }

                        int i10 = this.worldObj.GetEntitiesOfClass<Mob>(entityLiving9.GetType(), AABB.Of(this.xCoord, this.yCoord, this.zCoord, this.xCoord + 1, this.yCoord + 1, this.zCoord + 1).Expand(8, 4, 8)).Count;
                        if (i10 >= 6)
                        {
                            this.UpdateDelay();
                            return;
                        }

                        if (entityLiving9 != null)
                        {
                            double d11 = this.xCoord + (this.worldObj.rand.NextFloat() - this.worldObj.rand.NextFloat()) * 4;
                            double d13 = this.yCoord + this.worldObj.rand.NextInt(3) - 1;
                            double d15 = this.zCoord + (this.worldObj.rand.NextFloat() - this.worldObj.rand.NextFloat()) * 4;
                            entityLiving9.SetLocationAndAngles(d11, d13, d15, (float)worldObj.rand.NextFloat() * 360F, 0F);
                            if (entityLiving9.GetCanSpawnHere())
                            {
                                this.worldObj.AddEntity(entityLiving9);
                                for (int i17 = 0; i17 < 20; ++i17)
                                {
                                    d1 = this.xCoord + 0.5 + ((float)worldObj.rand.NextFloat() - 0.5) * 2;
                                    d3 = this.yCoord + 0.5 + ((float)worldObj.rand.NextFloat() - 0.5) * 2;
                                    d5 = this.zCoord + 0.5 + ((float)worldObj.rand.NextFloat() - 0.5) * 2;
                                    this.worldObj.AddParticle("smoke", d1, d3, d5, 0, 0, 0);
                                    this.worldObj.AddParticle("flame", d1, d3, d5, 0, 0, 0);
                                }

                                entityLiving9.SpawnExplosionParticle();
                                this.UpdateDelay();
                            }
                        }
                    }
                }

                base.UpdateEntity();
            }
        }

        private void UpdateDelay()
        {
            this.delay = 200 + this.worldObj.rand.NextInt(600);
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            this.mobID = nBTTagCompound1.GetString("EntityId");
            this.delay = nBTTagCompound1.GetShort("Delay");
        }

        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            nBTTagCompound1.SetString("EntityId", this.mobID);
            nBTTagCompound1.SetShort("Delay", (short)this.delay);
        }
    }
}