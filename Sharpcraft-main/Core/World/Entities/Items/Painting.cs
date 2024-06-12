using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Items
{
    public class Painting : Entity
    {
        private int field_ad;
        public int direction;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public Motive art;
        public Painting(Level world1) : base(world1)
        {
            this.field_ad = 0;
            this.direction = 0;
            this.yOffset = 0F;
            this.SetSize(0.5F, 0.5F);
        }

        public Painting(Level world1, int i2, int i3, int i4, int i5) : this(world1)
        {
            this.xPosition = i2;
            this.yPosition = i3;
            this.zPosition = i4;
            List<Motive> arrayList6 = new List<Motive>();
            Motive[] enumArt7 = Motive.Values();
            int i8 = enumArt7.Length;
            for (int i9 = 0; i9 < i8; ++i9)
            {
                Motive enumArt10 = enumArt7[i9];
                this.art = enumArt10;
                this.F412b(i5);
                if (this.OnValidSurface())
                {
                    arrayList6.Add(enumArt10);
                }
            }

            if (arrayList6.Count > 0)
            {
                this.art = arrayList6[this.rand.NextInt(arrayList6.Count)];
            }

            this.F412b(i5);
        }

        public Painting(Level world1, int i2, int i3, int i4, int i5, string string6) : this(world1)
        {
            this.xPosition = i2;
            this.yPosition = i3;
            this.zPosition = i4;
            Motive[] enumArt7 = Motive.Values();
            int i8 = enumArt7.Length;
            for (int i9 = 0; i9 < i8; ++i9)
            {
                Motive enumArt10 = enumArt7[i9];
                if (enumArt10.title.Equals(string6))
                {
                    this.art = enumArt10;
                    break;
                }
            }

            this.F412b(i5);
        }

        protected override void EntityInit()
        {
        }

        public virtual void F412b(int i1)
        {
            this.direction = i1;
            this.prevYaw = this.yaw = i1 * 90;
            float f2 = this.art.sizeX;
            float f3 = this.art.sizeY;
            float f4 = this.art.sizeX;
            if (i1 != 0 && i1 != 2)
            {
                f2 = 0.5F;
            }
            else
            {
                f4 = 0.5F;
            }

            f2 /= 32F;
            f3 /= 32F;
            f4 /= 32F;
            float f5 = this.xPosition + 0.5F;
            float f6 = this.yPosition + 0.5F;
            float f7 = this.zPosition + 0.5F;
            float f8 = 0.5625F;
            if (i1 == 0)
            {
                f7 -= f8;
            }

            if (i1 == 1)
            {
                f5 -= f8;
            }

            if (i1 == 2)
            {
                f7 += f8;
            }

            if (i1 == 3)
            {
                f5 += f8;
            }

            if (i1 == 0)
            {
                f5 -= this.Func_411_c(this.art.sizeX);
            }

            if (i1 == 1)
            {
                f7 += this.Func_411_c(this.art.sizeX);
            }

            if (i1 == 2)
            {
                f5 += this.Func_411_c(this.art.sizeX);
            }

            if (i1 == 3)
            {
                f7 -= this.Func_411_c(this.art.sizeX);
            }

            f6 += this.Func_411_c(this.art.sizeY);
            this.SetPosition(f5, f6, f7);
            float f9 = -0.00625F;
            this.boundingBox.SetBounds(f5 - f2 - f9, f6 - f3 - f9, f7 - f4 - f9, f5 + f2 + f9, f6 + f3 + f9, f7 + f4 + f9);
        }

        private float Func_411_c(int i1)
        {
            return i1 == 32 ? 0.5F : (i1 == 64 ? 0.5F : 0F);
        }

        public override void OnUpdate()
        {
            if (this.field_ad++ == 100 && !this.worldObj.isRemote)
            {
                this.field_ad = 0;
                if (!this.OnValidSurface())
                {
                    this.SetEntityDead();
                    this.worldObj.AddEntity(new ItemEntity(this.worldObj, this.x, this.y, this.z, new ItemInstance(Item.painting)));
                }
            }
        }

        public virtual bool OnValidSurface()
        {
            if (this.worldObj.GetCubes(this, this.boundingBox).Count > 0)
            {
                return false;
            }
            else
            {
                int i1 = this.art.sizeX / 16;
                int i2 = this.art.sizeY / 16;
                int i3 = this.xPosition;
                int i4 = this.yPosition;
                int i5 = this.zPosition;
                if (this.direction == 0)
                {
                    i3 = Mth.Floor(this.x - this.art.sizeX / 32F);
                }

                if (this.direction == 1)
                {
                    i5 = Mth.Floor(this.z - this.art.sizeX / 32F);
                }

                if (this.direction == 2)
                {
                    i3 = Mth.Floor(this.x - this.art.sizeX / 32F);
                }

                if (this.direction == 3)
                {
                    i5 = Mth.Floor(this.z - this.art.sizeX / 32F);
                }

                i4 = Mth.Floor(this.y - this.art.sizeY / 32F);
                int i7;
                for (int i6 = 0; i6 < i1; ++i6)
                {
                    for (i7 = 0; i7 < i2; ++i7)
                    {
                        Material material8;
                        if (this.direction != 0 && this.direction != 2)
                        {
                            material8 = this.worldObj.GetMaterial(this.xPosition, i4 + i7, i5 + i6);
                        }
                        else
                        {
                            material8 = this.worldObj.GetMaterial(i3 + i6, i4 + i7, this.zPosition);
                        }

                        if (!material8.IsSolid())
                        {
                            return false;
                        }
                    }
                }

                IList<Entity> list9 = this.worldObj.GetEntities(this, this.boundingBox);
                for (i7 = 0; i7 < list9.Count; ++i7)
                {
                    if (list9[i7] is Painting)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override bool CanBeCollidedWith()
        {
            return true;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            if (!this.isDead && !this.worldObj.isRemote)
            {
                this.SetEntityDead();
                this.SetBeenAttacked();
                this.worldObj.AddEntity(new ItemEntity(this.worldObj, this.x, this.y, this.z, new ItemInstance(Item.painting)));
            }

            return true;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetByte("Dir", (byte)this.direction);
            nBTTagCompound1.SetString("Motive", this.art.title);
            nBTTagCompound1.SetInteger("TileX", this.xPosition);
            nBTTagCompound1.SetInteger("TileY", this.yPosition);
            nBTTagCompound1.SetInteger("TileZ", this.zPosition);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            this.direction = nBTTagCompound1.GetByte("Dir");
            this.xPosition = nBTTagCompound1.GetInteger("TileX");
            this.yPosition = nBTTagCompound1.GetInteger("TileY");
            this.zPosition = nBTTagCompound1.GetInteger("TileZ");
            string string2 = nBTTagCompound1.GetString("Motive");
            Motive[] enumArt3 = Motive.Values();
            int i4 = enumArt3.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                Motive enumArt6 = enumArt3[i5];
                if (enumArt6.title.Equals(string2))
                {
                    this.art = enumArt6;
                }
            }

            if (this.art == null)
            {
                this.art = Motive.Kebab;
            }

            this.F412b(this.direction);
        }

        public override void MoveEntity(double d1, double d3, double d5)
        {
            if (!this.worldObj.isRemote && d1 * d1 + d3 * d3 + d5 * d5 > 0)
            {
                this.SetEntityDead();
                this.worldObj.AddEntity(new ItemEntity(this.worldObj, this.x, this.y, this.z, new ItemInstance(Item.painting)));
            }
        }

        public override void AddVelocity(double d1, double d3, double d5)
        {
            if (!this.worldObj.isRemote && d1 * d1 + d3 * d3 + d5 * d5 > 0)
            {
                this.SetEntityDead();
                this.worldObj.AddEntity(new ItemEntity(this.worldObj, this.x, this.y, this.z, new ItemInstance(Item.painting)));
            }
        }
    }
}