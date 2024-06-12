using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Items
{
    public class Minecart : Entity, IContainer
    {
        private ItemInstance[] cargoItems;
        public int minecartCurrentDamage;
        public int minecartTimeSinceHit;
        public int minecartRockDirection;
        private bool field_856;
        public int minecartType;
        public int fuel;
        public double pushX;
        public double pushZ;
        private static readonly int[][][] ROTATION = new[]
        {
            new[]
            {
                new[]
                {
                    0,
                    0,
                    -1
                },
                new[]
                {
                    0,
                    0,
                    1
                }
            },
            new[]
            {
                new[]
                {
                    -1,
                    0,
                    0
                },
                new[]
                {
                    1,
                    0,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    -1,
                    -1,
                    0
                },
                new[]
                {
                    1,
                    0,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    -1,
                    0,
                    0
                },
                new[]
                {
                    1,
                    -1,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    0,
                    -1
                },
                new[]
                {
                    0,
                    -1,
                    1
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    -1,
                    -1
                },
                new[]
                {
                    0,
                    0,
                    1
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    0,
                    1
                },
                new[]
                {
                    1,
                    0,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    0,
                    1
                },
                new[]
                {
                    -1,
                    0,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    0,
                    -1
                },
                new[]
                {
                    -1,
                    0,
                    0
                }
            },
            new[]
            {
                new[]
                {
                    0,
                    0,
                    -1
                },
                new[]
                {
                    1,
                    0,
                    0
                }
            }
        };
        private int field_9415;
        private double field_9414;
        private double field_9413;
        private double field_9412;
        private double field_9411;
        private double field_9410;
        private double field_9409;
        private double field_9408;
        private double field_9407;
        public Minecart(Level world1) : base(world1)
        {
            this.cargoItems = new ItemInstance[36];
            this.minecartCurrentDamage = 0;
            this.minecartTimeSinceHit = 0;
            this.minecartRockDirection = 1;
            this.field_856 = false;
            this.preventEntitySpawning = true;
            this.SetSize(0.98F, 0.7F);
            this.yOffset = this.height / 2F;
        }

        protected override bool CanTriggerWalking()
        {
            return false;
        }

        protected override void EntityInit()
        {
        }

        public override AABB GetCollisionBox(Entity entity1)
        {
            return entity1.boundingBox;
        }

        public override AABB GetBoundingBox()
        {
            return null;
        }

        public override bool CanBePushed()
        {
            return true;
        }

        public Minecart(Level world1, double d2, double d4, double d6, int i8) : this(world1)
        {
            this.SetPosition(d2, d4 + this.yOffset, d6);
            this.motionX = 0;
            this.motionY = 0;
            this.motionZ = 0;
            this.prevX = d2;
            this.prevY = d4;
            this.prevZ = d6;
            this.minecartType = i8;
        }

        public override double GetMountedYOffset()
        {
            return this.height * 0 - 0.3F;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (!this.worldObj.isRemote && !this.isDead)
            {
                this.minecartRockDirection = -this.minecartRockDirection;
                this.minecartTimeSinceHit = 10;
                this.SetBeenAttacked();
                this.minecartCurrentDamage += i2 * 10;
                if (this.minecartCurrentDamage > 40)
                {
                    if (this.riddenByEntity != null)
                    {
                        this.riddenByEntity.MountEntity(this);
                    }

                    this.SetEntityDead();
                    this.DropItemWithOffset(Item.minecartEmpty.id, 1, 0F);
                    if (this.minecartType == 1)
                    {
                        Minecart entityMinecart3 = this;
                        for (int i4 = 0; i4 < entityMinecart3.GetContainerSize(); ++i4)
                        {
                            ItemInstance itemStack5 = entityMinecart3.GetItem(i4);
                            if (itemStack5 != null)
                            {
                                float f6 = this.rand.NextFloat() * 0.8F + 0.1F;
                                float f7 = this.rand.NextFloat() * 0.8F + 0.1F;
                                float f8 = this.rand.NextFloat() * 0.8F + 0.1F;
                                while (itemStack5.stackSize > 0)
                                {
                                    int i9 = this.rand.NextInt(21) + 10;
                                    if (i9 > itemStack5.stackSize)
                                    {
                                        i9 = itemStack5.stackSize;
                                    }

                                    itemStack5.stackSize -= i9;
                                    ItemEntity entityItem10 = new ItemEntity(this.worldObj, this.x + f6, this.y + f7, this.z + f8, new ItemInstance(itemStack5.itemID, i9, itemStack5.GetItemDamage()));
                                    float f11 = 0.05F;
                                    entityItem10.motionX = (float)this.rand.NextGaussian() * f11;
                                    entityItem10.motionY = (float)this.rand.NextGaussian() * f11 + 0.2F;
                                    entityItem10.motionZ = (float)this.rand.NextGaussian() * f11;
                                    this.worldObj.AddEntity(entityItem10);
                                }
                            }
                        }

                        this.DropItemWithOffset(Tile.chest.id, 1, 0F);
                    }
                    else if (this.minecartType == 2)
                    {
                        this.DropItemWithOffset(Tile.furnace.id, 1, 0F);
                    }
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public override void PerformHurtAnimation()
        {
            System.Console.WriteLine("Animating hurt");
            this.minecartRockDirection = -this.minecartRockDirection;
            this.minecartTimeSinceHit = 10;
            this.minecartCurrentDamage += this.minecartCurrentDamage * 10;
        }

        public override bool CanBeCollidedWith()
        {
            return !this.isDead;
        }

        public override void SetEntityDead()
        {
            for (int i1 = 0; i1 < this.GetContainerSize(); ++i1)
            {
                ItemInstance itemStack2 = this.GetItem(i1);
                if (itemStack2 != null)
                {
                    float f3 = this.rand.NextFloat() * 0.8F + 0.1F;
                    float f4 = this.rand.NextFloat() * 0.8F + 0.1F;
                    float f5 = this.rand.NextFloat() * 0.8F + 0.1F;
                    while (itemStack2.stackSize > 0)
                    {
                        int i6 = this.rand.NextInt(21) + 10;
                        if (i6 > itemStack2.stackSize)
                        {
                            i6 = itemStack2.stackSize;
                        }

                        itemStack2.stackSize -= i6;
                        ItemEntity entityItem7 = new ItemEntity(this.worldObj, this.x + f3, this.y + f4, this.z + f5, new ItemInstance(itemStack2.itemID, i6, itemStack2.GetItemDamage()));
                        float f8 = 0.05F;
                        entityItem7.motionX = (float)this.rand.NextGaussian() * f8;
                        entityItem7.motionY = (float)this.rand.NextGaussian() * f8 + 0.2F;
                        entityItem7.motionZ = (float)this.rand.NextGaussian() * f8;
                        this.worldObj.AddEntity(entityItem7);
                    }
                }
            }

            base.SetEntityDead();
        }

        public override void OnUpdate()
        {
            if (this.minecartTimeSinceHit > 0)
            {
                --this.minecartTimeSinceHit;
            }

            if (this.minecartCurrentDamage > 0)
            {
                --this.minecartCurrentDamage;
            }

            double d7;
            if (this.worldObj.isRemote && this.field_9415 > 0)
            {
                if (this.field_9415 > 0)
                {
                    double d46 = this.x + (this.field_9414 - this.x) / this.field_9415;
                    double d47 = this.y + (this.field_9413 - this.y) / this.field_9415;
                    double d5 = this.z + (this.field_9412 - this.z) / this.field_9415;
                    for (d7 = this.field_9411 - this.yaw; d7 < -180; d7 += 360)
                    {
                    }

                    while (d7 >= 180)
                    {
                        d7 -= 360;
                    }

                    this.yaw = (float)(this.yaw + d7 / this.field_9415);
                    this.pitch = (float)(this.pitch + (this.field_9410 - this.pitch) / this.field_9415);
                    --this.field_9415;
                    this.SetPosition(d46, d47, d5);
                    this.SetRotation(this.yaw, this.pitch);
                }
                else
                {
                    this.SetPosition(this.x, this.y, this.z);
                    this.SetRotation(this.yaw, this.pitch);
                }
            }
            else
            {
                this.prevX = this.x;
                this.prevY = this.y;
                this.prevZ = this.z;
                this.motionY -= 0.04F;
                int i1 = Mth.Floor(this.x);
                int i2 = Mth.Floor(this.y);
                int i3 = Mth.Floor(this.z);
                if (RailTile.IsRailBlockAt(this.worldObj, i1, i2 - 1, i3))
                {
                    --i2;
                }

                double d4 = 0.4;
                bool z6 = false;
                d7 = 2 / 256;
                int i9 = this.worldObj.GetTile(i1, i2, i3);
                if (RailTile.IsRailBlock(i9))
                {
                    Vec3 vec3D10 = this.Func_g(this.x, this.y, this.z);
                    int i11 = this.worldObj.GetData(i1, i2, i3);
                    this.y = i2;
                    bool z12 = false;
                    bool z13 = false;
                    if (i9 == Tile.railPowered.id)
                    {
                        z12 = (i11 & 8) != 0;
                        z13 = !z12;
                    }

                    if (((RailTile)Tile.tiles[i9]).IsPowered())
                    {
                        i11 &= 7;
                    }

                    if (i11 >= 2 && i11 <= 5)
                    {
                        this.y = i2 + 1;
                    }

                    if (i11 == 2)
                    {
                        this.motionX -= d7;
                    }

                    if (i11 == 3)
                    {
                        this.motionX += d7;
                    }

                    if (i11 == 4)
                    {
                        this.motionZ += d7;
                    }

                    if (i11 == 5)
                    {
                        this.motionZ -= d7;
                    }

                    int[][] i14 = ROTATION[i11];
                    double d15 = i14[1][0] - i14[0][0];
                    double d17 = i14[1][2] - i14[0][2];
                    double d19 = Math.Sqrt(d15 * d15 + d17 * d17);
                    double d21 = this.motionX * d15 + this.motionZ * d17;
                    if (d21 < 0)
                    {
                        d15 = -d15;
                        d17 = -d17;
                    }

                    double d23 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                    this.motionX = d23 * d15 / d19;
                    this.motionZ = d23 * d17 / d19;
                    double d25;
                    if (z13)
                    {
                        d25 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                        if (d25 < 0.03)
                        {
                            this.motionX *= 0;
                            this.motionY *= 0;
                            this.motionZ *= 0;
                        }
                        else
                        {
                            this.motionX *= 0.5;
                            this.motionY *= 0;
                            this.motionZ *= 0.5;
                        }
                    }

                    d25 = 0;
                    double d27 = i1 + 0.5 + i14[0][0] * 0.5;
                    double d29 = i3 + 0.5 + i14[0][2] * 0.5;
                    double d31 = i1 + 0.5 + i14[1][0] * 0.5;
                    double d33 = i3 + 0.5 + i14[1][2] * 0.5;
                    d15 = d31 - d27;
                    d17 = d33 - d29;
                    double d35;
                    double d37;
                    double d39;
                    if (d15 == 0)
                    {
                        this.x = i1 + 0.5;
                        d25 = this.z - i3;
                    }
                    else if (d17 == 0)
                    {
                        this.z = i3 + 0.5;
                        d25 = this.x - i1;
                    }
                    else
                    {
                        d35 = this.x - d27;
                        d37 = this.z - d29;
                        d39 = (d35 * d15 + d37 * d17) * 2;
                        d25 = d39;
                    }

                    this.x = d27 + d15 * d25;
                    this.z = d29 + d17 * d25;
                    this.SetPosition(this.x, this.y + this.yOffset, this.z);
                    d35 = this.motionX;
                    d37 = this.motionZ;
                    if (this.riddenByEntity != null)
                    {
                        d35 *= 0.75;
                        d37 *= 0.75;
                    }

                    if (d35 < -d4)
                    {
                        d35 = -d4;
                    }

                    if (d35 > d4)
                    {
                        d35 = d4;
                    }

                    if (d37 < -d4)
                    {
                        d37 = -d4;
                    }

                    if (d37 > d4)
                    {
                        d37 = d4;
                    }

                    this.MoveEntity(d35, 0, d37);
                    if (i14[0][1] != 0 && Mth.Floor(this.x) - i1 == i14[0][0] && Mth.Floor(this.z) - i3 == i14[0][2])
                    {
                        this.SetPosition(this.x, this.y + i14[0][1], this.z);
                    }
                    else if (i14[1][1] != 0 && Mth.Floor(this.x) - i1 == i14[1][0] && Mth.Floor(this.z) - i3 == i14[1][2])
                    {
                        this.SetPosition(this.x, this.y + i14[1][1], this.z);
                    }

                    if (this.riddenByEntity != null)
                    {
                        this.motionX *= 0.997F;
                        this.motionY *= 0;
                        this.motionZ *= 0.997F;
                    }
                    else
                    {
                        if (this.minecartType == 2)
                        {
                            d39 = Mth.Sqrt(this.pushX * this.pushX + this.pushZ * this.pushZ);
                            if (d39 > 0.01)
                            {
                                z6 = true;
                                this.pushX /= d39;
                                this.pushZ /= d39;
                                double d41 = 0.04;
                                this.motionX *= 0.8F;
                                this.motionY *= 0;
                                this.motionZ *= 0.8F;
                                this.motionX += this.pushX * d41;
                                this.motionZ += this.pushZ * d41;
                            }
                            else
                            {
                                this.motionX *= 0.9F;
                                this.motionY *= 0;
                                this.motionZ *= 0.9F;
                            }
                        }

                        this.motionX *= 0.96F;
                        this.motionY *= 0;
                        this.motionZ *= 0.96F;
                    }

                    Vec3 vec3D52 = this.Func_g(this.x, this.y, this.z);
                    if (vec3D52 != null && vec3D10 != null)
                    {
                        double d40 = (vec3D10.y - vec3D52.y) * 0.05;
                        d23 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                        if (d23 > 0)
                        {
                            this.motionX = this.motionX / d23 * (d23 + d40);
                            this.motionZ = this.motionZ / d23 * (d23 + d40);
                        }

                        this.SetPosition(this.x, vec3D52.y, this.z);
                    }

                    int i53 = Mth.Floor(this.x);
                    int i54 = Mth.Floor(this.z);
                    if (i53 != i1 || i54 != i3)
                    {
                        d23 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                        this.motionX = d23 * (i53 - i1);
                        this.motionZ = d23 * (i54 - i3);
                    }

                    double d42;
                    if (this.minecartType == 2)
                    {
                        d42 = Mth.Sqrt(this.pushX * this.pushX + this.pushZ * this.pushZ);
                        if (d42 > 0.01 && this.motionX * this.motionX + this.motionZ * this.motionZ > 0.001)
                        {
                            this.pushX /= d42;
                            this.pushZ /= d42;
                            if (this.pushX * this.motionX + this.pushZ * this.motionZ < 0)
                            {
                                this.pushX = 0;
                                this.pushZ = 0;
                            }
                            else
                            {
                                this.pushX = this.motionX;
                                this.pushZ = this.motionZ;
                            }
                        }
                    }

                    if (z12)
                    {
                        d42 = Math.Sqrt(this.motionX * this.motionX + this.motionZ * this.motionZ);
                        if (d42 > 0.01)
                        {
                            double d44 = 0.06;
                            this.motionX += this.motionX / d42 * d44;
                            this.motionZ += this.motionZ / d42 * d44;
                        }
                        else if (i11 == 1)
                        {
                            if (this.worldObj.IsSolidBlockingTile(i1 - 1, i2, i3))
                            {
                                this.motionX = 0.02;
                            }
                            else if (this.worldObj.IsSolidBlockingTile(i1 + 1, i2, i3))
                            {
                                this.motionX = -0.02;
                            }
                        }
                        else if (i11 == 0)
                        {
                            if (this.worldObj.IsSolidBlockingTile(i1, i2, i3 - 1))
                            {
                                this.motionZ = 0.02;
                            }
                            else if (this.worldObj.IsSolidBlockingTile(i1, i2, i3 + 1))
                            {
                                this.motionZ = -0.02;
                            }
                        }
                    }
                }
                else
                {
                    if (this.motionX < -d4)
                    {
                        this.motionX = -d4;
                    }

                    if (this.motionX > d4)
                    {
                        this.motionX = d4;
                    }

                    if (this.motionZ < -d4)
                    {
                        this.motionZ = -d4;
                    }

                    if (this.motionZ > d4)
                    {
                        this.motionZ = d4;
                    }

                    if (this.onGround)
                    {
                        this.motionX *= 0.5;
                        this.motionY *= 0.5;
                        this.motionZ *= 0.5;
                    }

                    this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                    if (!this.onGround)
                    {
                        this.motionX *= 0.95F;
                        this.motionY *= 0.95F;
                        this.motionZ *= 0.95F;
                    }
                }

                this.pitch = 0F;
                double d48 = this.prevX - this.x;
                double d49 = this.prevZ - this.z;
                if (d48 * d48 + d49 * d49 > 0.001)
                {
                    this.yaw = (float)(Math.Atan2(d49, d48) * 180 / Math.PI);
                    if (this.field_856)
                    {
                        this.yaw += 180F;
                    }
                }

                double d50;
                for (d50 = this.yaw - this.prevYaw; d50 >= 180; d50 -= 360)
                {
                }

                while (d50 < -180)
                {
                    d50 += 360;
                }

                if (d50 < -170 || d50 >= 170)
                {
                    this.yaw += 180F;
                    this.field_856 = !this.field_856;
                }

                this.SetRotation(this.yaw, this.pitch);
                IList<Entity> list16 = this.worldObj.GetEntities(this, this.boundingBox.Expand(0.2F, 0, 0.2F));
                if (list16 != null && list16.Count > 0)
                {
                    for (int i51 = 0; i51 < list16.Count; ++i51)
                    {
                        Entity entity18 = list16[i51];
                        if (entity18 != this.riddenByEntity && entity18.CanBePushed() && entity18 is Minecart)
                        {
                            entity18.ApplyEntityCollision(this);
                        }
                    }
                }

                if (this.riddenByEntity != null && this.riddenByEntity.isDead)
                {
                    this.riddenByEntity = null;
                }

                if (z6 && this.rand.NextInt(4) == 0)
                {
                    --this.fuel;
                    if (this.fuel < 0)
                    {
                        this.pushX = this.pushZ = 0;
                    }

                    this.worldObj.AddParticle("largesmoke", this.x, this.y + 0.8, this.z, 0, 0, 0);
                }
            }
        }

        public virtual Vec3 Func_515_a(double d1, double d3, double d5, double d7)
        {
            int i9 = Mth.Floor(d1);
            int i10 = Mth.Floor(d3);
            int i11 = Mth.Floor(d5);
            if (RailTile.IsRailBlockAt(this.worldObj, i9, i10 - 1, i11))
            {
                --i10;
            }

            int i12 = this.worldObj.GetTile(i9, i10, i11);
            if (!RailTile.IsRailBlock(i12))
            {
                return null;
            }
            else
            {
                int i13 = this.worldObj.GetData(i9, i10, i11);
                if (((RailTile)Tile.tiles[i12]).IsPowered())
                {
                    i13 &= 7;
                }

                d3 = i10;
                if (i13 >= 2 && i13 <= 5)
                {
                    d3 = i10 + 1;
                }

                int[][] i14 = ROTATION[i13];
                double d15 = i14[1][0] - i14[0][0];
                double d17 = i14[1][2] - i14[0][2];
                double d19 = Math.Sqrt(d15 * d15 + d17 * d17);
                d15 /= d19;
                d17 /= d19;
                d1 += d15 * d7;
                d5 += d17 * d7;
                if (i14[0][1] != 0 && Mth.Floor(d1) - i9 == i14[0][0] && Mth.Floor(d5) - i11 == i14[0][2])
                {
                    d3 += i14[0][1];
                }
                else if (i14[1][1] != 0 && Mth.Floor(d1) - i9 == i14[1][0] && Mth.Floor(d5) - i11 == i14[1][2])
                {
                    d3 += i14[1][1];
                }

                return this.Func_g(d1, d3, d5);
            }
        }

        public virtual Vec3 Func_g(double d1, double d3, double d5)
        {
            int i7 = Mth.Floor(d1);
            int i8 = Mth.Floor(d3);
            int i9 = Mth.Floor(d5);
            if (RailTile.IsRailBlockAt(this.worldObj, i7, i8 - 1, i9))
            {
                --i8;
            }

            int i10 = this.worldObj.GetTile(i7, i8, i9);
            if (RailTile.IsRailBlock(i10))
            {
                int i11 = this.worldObj.GetData(i7, i8, i9);
                d3 = i8;
                if (((RailTile)Tile.tiles[i10]).IsPowered())
                {
                    i11 &= 7;
                }

                if (i11 >= 2 && i11 <= 5)
                {
                    d3 = i8 + 1;
                }

                int[][] i12 = ROTATION[i11];
                double d13 = 0;
                double d15 = i7 + 0.5 + i12[0][0] * 0.5;
                double d17 = i8 + 0.5 + i12[0][1] * 0.5;
                double d19 = i9 + 0.5 + i12[0][2] * 0.5;
                double d21 = i7 + 0.5 + i12[1][0] * 0.5;
                double d23 = i8 + 0.5 + i12[1][1] * 0.5;
                double d25 = i9 + 0.5 + i12[1][2] * 0.5;
                double d27 = d21 - d15;
                double d29 = (d23 - d17) * 2;
                double d31 = d25 - d19;
                if (d27 == 0)
                {
                    d1 = i7 + 0.5;
                    d13 = d5 - i9;
                }
                else if (d31 == 0)
                {
                    d5 = i9 + 0.5;
                    d13 = d1 - i7;
                }
                else
                {
                    double d33 = d1 - d15;
                    double d35 = d5 - d19;
                    double d37 = (d33 * d27 + d35 * d31) * 2;
                    d13 = d37;
                }

                d1 = d15 + d27 * d13;
                d3 = d17 + d29 * d13;
                d5 = d19 + d31 * d13;
                if (d29 < 0)
                {
                    ++d3;
                }

                if (d29 > 0)
                {
                    d3 += 0.5;
                }

                return Vec3.Of(d1, d3, d5);
            }
            else
            {
                return null;
            }
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetInteger("Type", this.minecartType);
            if (this.minecartType == 2)
            {
                nBTTagCompound1.SetDouble("PushX", this.pushX);
                nBTTagCompound1.SetDouble("PushZ", this.pushZ);
                nBTTagCompound1.SetShort("Fuel", (short)this.fuel);
            }
            else if (this.minecartType == 1)
            {
                ListTag<CompoundTag> nBTTagList2 = new ListTag<CompoundTag>();
                for (int i3 = 0; i3 < this.cargoItems.Length; ++i3)
                {
                    if (this.cargoItems[i3] != null)
                    {
                        CompoundTag nBTTagCompound4 = new CompoundTag();
                        nBTTagCompound4.SetByte("Slot", (byte)i3);
                        this.cargoItems[i3].WriteToNBT(nBTTagCompound4);
                        nBTTagList2.Add(nBTTagCompound4);
                    }
                }

                nBTTagCompound1.SetTag("Items", nBTTagList2);
            }
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.minecartType = nBTTagCompound1.GetInteger("Type");
            if (this.minecartType == 2)
            {
                this.pushX = nBTTagCompound1.GetDouble("PushX");
                this.pushZ = nBTTagCompound1.GetDouble("PushZ");
                this.fuel = nBTTagCompound1.GetShort("Fuel");
            }
            else if (this.minecartType == 1)
            {
                ListTag<CompoundTag> nBTTagList2 = nBTTagCompound1.GetTagList<CompoundTag>("Items");
                this.cargoItems = new ItemInstance[this.GetContainerSize()];
                for (int i3 = 0; i3 < nBTTagList2.Count; ++i3)
                {
                    CompoundTag nBTTagCompound4 = nBTTagList2[i3];
                    int i5 = nBTTagCompound4.GetByte("Slot") & 255;
                    if (i5 >= 0 && i5 < this.cargoItems.Length)
                    {
                        this.cargoItems[i5] = new ItemInstance(nBTTagCompound4);
                    }
                }
            }
        }

        public override float GetShadowSize()
        {
            return 0F;
        }

        public override void ApplyEntityCollision(Entity entity1)
        {
            if (!this.worldObj.isRemote)
            {
                if (entity1 != this.riddenByEntity)
                {
                    if (entity1 is Mob && !(entity1 is Player) && this.minecartType == 0 && this.motionX * this.motionX + this.motionZ * this.motionZ > 0.01 && this.riddenByEntity == null && entity1.ridingEntity == null)
                    {
                        entity1.MountEntity(this);
                    }

                    double d2 = entity1.x - this.x;
                    double d4 = entity1.z - this.z;
                    double d6 = d2 * d2 + d4 * d4;
                    if (d6 >= 9.999999747378752E-05)
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
                        d2 *= 0.1F;
                        d4 *= 0.1F;
                        d2 *= 1F - this.entityCollisionReduction;
                        d4 *= 1F - this.entityCollisionReduction;
                        d2 *= 0.5;
                        d4 *= 0.5;
                        if (entity1 is Minecart)
                        {
                            double d10 = entity1.x - this.x;
                            double d12 = entity1.z - this.z;
                            double d14 = d10 * entity1.motionZ + d12 * entity1.prevX;
                            d14 *= d14;
                            if (d14 > 5)
                            {
                                return;
                            }

                            double d16 = entity1.motionX + this.motionX;
                            double d18 = entity1.motionZ + this.motionZ;
                            if (((Minecart)entity1).minecartType == 2 && this.minecartType != 2)
                            {
                                this.motionX *= 0.2F;
                                this.motionZ *= 0.2F;
                                this.AddVelocity(entity1.motionX - d2, 0, entity1.motionZ - d4);
                                entity1.motionX *= 0.7F;
                                entity1.motionZ *= 0.7F;
                            }
                            else if (((Minecart)entity1).minecartType != 2 && this.minecartType == 2)
                            {
                                entity1.motionX *= 0.2F;
                                entity1.motionZ *= 0.2F;
                                entity1.AddVelocity(this.motionX + d2, 0, this.motionZ + d4);
                                this.motionX *= 0.7F;
                                this.motionZ *= 0.7F;
                            }
                            else
                            {
                                d16 /= 2;
                                d18 /= 2;
                                this.motionX *= 0.2F;
                                this.motionZ *= 0.2F;
                                this.AddVelocity(d16 - d2, 0, d18 - d4);
                                entity1.motionX *= 0.2F;
                                entity1.motionZ *= 0.2F;
                                entity1.AddVelocity(d16 + d2, 0, d18 + d4);
                            }
                        }
                        else
                        {
                            this.AddVelocity(-d2, 0, -d4);
                            entity1.AddVelocity(d2 / 4, 0, d4 / 4);
                        }
                    }
                }
            }
        }

        public int GetContainerSize()
        {
            return 27;
        }

        public ItemInstance GetItem(int i1)
        {
            return this.cargoItems[i1];
        }

        public ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.cargoItems[i1] != null)
            {
                ItemInstance itemStack3;
                if (this.cargoItems[i1].stackSize <= i2)
                {
                    itemStack3 = this.cargoItems[i1];
                    this.cargoItems[i1] = null;
                    return itemStack3;
                }
                else
                {
                    itemStack3 = this.cargoItems[i1].SplitStack(i2);
                    if (this.cargoItems[i1].stackSize == 0)
                    {
                        this.cargoItems[i1] = null;
                    }

                    return itemStack3;
                }
            }
            else
            {
                return null;
            }
        }

        public void SetItem(int i1, ItemInstance itemStack2)
        {
            this.cargoItems[i1] = itemStack2;
            if (itemStack2 != null && itemStack2.stackSize > this.GetMaxStackSize())
            {
                itemStack2.stackSize = this.GetMaxStackSize();
            }
        }

        public string GetName()
        {
            return "Minecart";
        }

        public int GetMaxStackSize()
        {
            return 64;
        }

        public void SetChanged()
        {
        }

        public override bool Interact(Player entityPlayer1)
        {
            if (this.minecartType == 0)
            {
                if (this.riddenByEntity != null && this.riddenByEntity is Player && this.riddenByEntity != entityPlayer1)
                {
                    return true;
                }

                if (!this.worldObj.isRemote)
                {
                    entityPlayer1.MountEntity(this);
                }
            }
            else if (this.minecartType == 1)
            {
                if (!this.worldObj.isRemote)
                {
                    entityPlayer1.DisplayGUIChest(this);
                }
            }
            else if (this.minecartType == 2)
            {
                ItemInstance itemStack2 = entityPlayer1.inventory.GetCurrentItem();
                if (itemStack2 != null && itemStack2.itemID == Item.coal.id)
                {
                    if (--itemStack2.stackSize == 0)
                    {
                        entityPlayer1.inventory.SetItem(entityPlayer1.inventory.currentItem, (ItemInstance)null);
                    }

                    this.fuel += 1200;
                }

                this.pushX = this.x - entityPlayer1.x;
                this.pushZ = this.z - entityPlayer1.z;
            }

            return true;
        }

        public override void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            this.field_9414 = d1;
            this.field_9413 = d3;
            this.field_9412 = d5;
            this.field_9411 = f7;
            this.field_9410 = f8;
            this.field_9415 = i9 + 2;
            this.motionX = this.field_9409;
            this.motionY = this.field_9408;
            this.motionZ = this.field_9407;
        }

        public override void SetVelocity(double d1, double d3, double d5)
        {
            this.field_9409 = this.motionX = d1;
            this.field_9408 = this.motionY = d3;
            this.field_9407 = this.motionZ = d5;
        }

        public bool StillValid(Player entityPlayer1)
        {
            return this.isDead ? false : entityPlayer1.GetDistanceSqToEntity(this) <= 64;
        }
    }
}