using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class Sheep : Animal
    {
        public static readonly float[][] COLOR = new[]
        {
            new[]
            {
                1F,
                1F,
                1F
            },
            new[]
            {
                0.95F,
                0.7F,
                0.2F
            },
            new[]
            {
                0.9F,
                0.5F,
                0.85F
            },
            new[]
            {
                0.6F,
                0.7F,
                0.95F
            },
            new[]
            {
                0.9F,
                0.9F,
                0.2F
            },
            new[]
            {
                0.5F,
                0.8F,
                0.1F
            },
            new[]
            {
                0.95F,
                0.7F,
                0.8F
            },
            new[]
            {
                0.3F,
                0.3F,
                0.3F
            },
            new[]
            {
                0.6F,
                0.6F,
                0.6F
            },
            new[]
            {
                0.3F,
                0.6F,
                0.7F
            },
            new[]
            {
                0.7F,
                0.4F,
                0.9F
            },
            new[]
            {
                0.2F,
                0.4F,
                0.8F
            },
            new[]
            {
                0.5F,
                0.4F,
                0.3F
            },
            new[]
            {
                0.4F,
                0.5F,
                0.2F
            },
            new[]
            {
                0.8F,
                0.3F,
                0.3F
            },
            new[]
            {
                0.1F,
                0.1F,
                0.1F
            }
        };
        public Sheep(Level world1) : base(world1)
        {
            this.texture = "/mob/sheep.png";
            this.SetSize(0.9F, 1.3F);
        }

        protected override void EntityInit()
        {
            base.EntityInit();
            this.dataWatcher.AddObject(16, (sbyte)0);
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            return base.AttackEntityFrom(entity1, i2);
        }

        protected override void DropFewItems()
        {
            if (!this.GetSheared())
            {
                this.EntityDropItem(new ItemInstance(Tile.cloth.id, 1, this.GetFleeceColor()), 0F);
            }
        }

        protected override int GetDropItemId()
        {
            return Tile.cloth.id;
        }

        public override bool Interact(Player entityPlayer1)
        {
            ItemInstance itemStack2 = entityPlayer1.inventory.GetCurrentItem();
            if (itemStack2 != null && itemStack2.itemID == Item.shears.id && !this.GetSheared())
            {
                if (!this.worldObj.isRemote)
                {
                    this.SetSheared(true);
                    int i3 = 2 + this.rand.NextInt(3);
                    for (int i4 = 0; i4 < i3; ++i4)
                    {
                        ItemEntity entityItem5 = this.EntityDropItem(new ItemInstance(Tile.cloth.id, 1, this.GetFleeceColor()), 1F);
                        entityItem5.motionY += this.rand.NextFloat() * 0.05F;
                        entityItem5.motionX += (this.rand.NextFloat() - this.rand.NextFloat()) * 0.1F;
                        entityItem5.motionZ += (this.rand.NextFloat() - this.rand.NextFloat()) * 0.1F;
                    }
                }

                itemStack2.DamageItem(1, entityPlayer1);
            }

            return false;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
            nBTTagCompound1.SetBoolean("Sheared", this.GetSheared());
            nBTTagCompound1.SetByte("Color", (byte)this.GetFleeceColor());
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
            this.SetSheared(nBTTagCompound1.GetBoolean("Sheared"));
            this.SetFleeceColor(nBTTagCompound1.GetByte("Color"));
        }

        protected override string GetLivingSound()
        {
            return "mob.sheep";
        }

        protected override string GetHurtSound()
        {
            return "mob.sheep";
        }

        protected override string GetDeathSound()
        {
            return "mob.sheep";
        }

        public virtual int GetFleeceColor()
        {
            return this.dataWatcher.GetWatchableObjectByte(16) & 15;
        }

        public virtual void SetFleeceColor(int i1)
        {
            sbyte b2 = this.dataWatcher.GetWatchableObjectByte(16);
            this.dataWatcher.UpdateObject(16, (sbyte)(b2 & 240 | i1 & 15));
        }

        public virtual bool GetSheared()
        {
            return (this.dataWatcher.GetWatchableObjectByte(16) & 16) != 0;
        }

        public virtual void SetSheared(bool z1)
        {
            sbyte b2 = this.dataWatcher.GetWatchableObjectByte(16);
            if (z1)
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 | 16));
            }
            else
            {
                this.dataWatcher.UpdateObject(16, (sbyte)(b2 & -17));
            }
        }

        public static int GetRandomColor(JRandom random0)
        {
            int i1 = random0.NextInt(100);
            return i1 < 5 ? 15 : (i1 < 10 ? 7 : (i1 < 15 ? 8 : (i1 < 18 ? 12 : (random0.NextInt(500) == 0 ? 6 : 0))));
        }
    }
}