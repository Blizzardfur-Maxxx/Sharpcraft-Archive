using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Entities.Weather
{
    public class LightningBolt : WeatherMob
    {
        private int field_b;
        public long randomSeed = 0;
        private int field_c;
        public LightningBolt(Level world1, double d2, double d4, double d6) : base(world1)
        {
            this.SetLocationAndAngles(d2, d4, d6, 0F, 0F);
            this.field_b = 2;
            this.randomSeed = this.rand.NextLong();
            this.field_c = this.rand.NextInt(3) + 1;
            if (world1.difficultySetting >= 2 && world1.DoChunksNearChunkExist(Mth.Floor(d2), Mth.Floor(d4), Mth.Floor(d6), 10))
            {
                int i8 = Mth.Floor(d2);
                int i9 = Mth.Floor(d4);
                int i10 = Mth.Floor(d6);
                if (world1.GetTile(i8, i9, i10) == 0 && Tile.fire.CanPlaceBlockAt(world1, i8, i9, i10))
                {
                    world1.SetTile(i8, i9, i10, Tile.fire.id);
                }

                for (i8 = 0; i8 < 4; ++i8)
                {
                    i9 = Mth.Floor(d2) + this.rand.NextInt(3) - 1;
                    i10 = Mth.Floor(d4) + this.rand.NextInt(3) - 1;
                    int i11 = Mth.Floor(d6) + this.rand.NextInt(3) - 1;
                    if (world1.GetTile(i9, i10, i11) == 0 && Tile.fire.CanPlaceBlockAt(world1, i9, i10, i11))
                    {
                        world1.SetTile(i9, i10, i11, Tile.fire.id);
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.field_b == 2)
            {
                this.worldObj.PlaySound(this.x, this.y, this.z, "ambient.weather.thunder", 10000F, 0.8F + this.rand.NextFloat() * 0.2F);
                this.worldObj.PlaySound(this.x, this.y, this.z, "random.explode", 2F, 0.5F + this.rand.NextFloat() * 0.2F);
            }

            --this.field_b;
            if (this.field_b < 0)
            {
                if (this.field_c == 0)
                {
                    this.SetEntityDead();
                }
                else if (this.field_b < -this.rand.NextInt(10))
                {
                    --this.field_c;
                    this.field_b = 1;
                    this.randomSeed = this.rand.NextLong();
                    if (this.worldObj.DoChunksNearChunkExist(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z), 10))
                    {
                        int i1 = Mth.Floor(this.x);
                        int i2 = Mth.Floor(this.y);
                        int i3 = Mth.Floor(this.z);
                        if (this.worldObj.GetTile(i1, i2, i3) == 0 && Tile.fire.CanPlaceBlockAt(this.worldObj, i1, i2, i3))
                        {
                            this.worldObj.SetTile(i1, i2, i3, Tile.fire.id);
                        }
                    }
                }
            }

            if (this.field_b >= 0)
            {
                double d6 = 3;
                IList<Entity> list7 = this.worldObj.GetEntities(this, AABB.Of(this.x - d6, this.y - d6, this.z - d6, this.x + d6, this.y + 6 + d6, this.z + d6));
                for (int i4 = 0; i4 < list7.Count; ++i4)
                {
                    Entity entity5 = list7[i4];
                    entity5.OnStruckByLightning(this);
                }

                this.worldObj.field_i = 2;
            }
        }

        protected override void EntityInit()
        {
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
        }

        public override bool IsInRangeToRenderVec3D(Vec3 vec3D1)
        {
            return this.field_b >= 0;
        }
    }
}