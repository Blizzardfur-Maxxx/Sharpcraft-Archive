using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel
{
    public class Explosion
    {
        public bool isFlaming = false;
        private JRandom ExplosionRNG = new JRandom();
        private Level worldObj;
        public double explosionX;
        public double explosionY;
        public double explosionZ;
        public Entity exploder;
        public float explosionSize;
        public HashSet<TilePos> destroyedBlockPositions = new HashSet<TilePos>();
        public Explosion(Level world1, Entity entity2, double d3, double d5, double d7, float f9)
        {
            this.worldObj = world1;
            this.exploder = entity2;
            this.explosionSize = f9;
            this.explosionX = d3;
            this.explosionY = d5;
            this.explosionZ = d7;
        }

        public virtual void Explode()
        {
            float f1 = this.explosionSize;
            byte b2 = 16;
            int i3;
            int i4;
            int i5;
            double d15;
            double d17;
            double d19;
            for (i3 = 0; i3 < b2; ++i3)
            {
                for (i4 = 0; i4 < b2; ++i4)
                {
                    for (i5 = 0; i5 < b2; ++i5)
                    {
                        if (i3 == 0 || i3 == b2 - 1 || i4 == 0 || i4 == b2 - 1 || i5 == 0 || i5 == b2 - 1)
                        {
                            double d6 = i3 / (b2 - 1F) * 2F - 1F;
                            double d8 = i4 / (b2 - 1F) * 2F - 1F;
                            double d10 = i5 / (b2 - 1F) * 2F - 1F;
                            double d12 = Math.Sqrt(d6 * d6 + d8 * d8 + d10 * d10);
                            d6 /= d12;
                            d8 /= d12;
                            d10 /= d12;
                            float f14 = this.explosionSize * (0.7F + this.worldObj.rand.NextFloat() * 0.6F);
                            d15 = this.explosionX;
                            d17 = this.explosionY;
                            d19 = this.explosionZ;
                            for (float f21 = 0.3F; f14 > 0F; f14 -= f21 * 0.75F)
                            {
                                int i22 = Mth.Floor(d15);
                                int i23 = Mth.Floor(d17);
                                int i24 = Mth.Floor(d19);
                                int i25 = this.worldObj.GetTile(i22, i23, i24);
                                if (i25 > 0)
                                {
                                    f14 -= (Tile.tiles[i25].GetExplosionResistance(this.exploder) + 0.3F) * f21;
                                }

                                if (f14 > 0F)
                                {
                                    this.destroyedBlockPositions.Add(new TilePos(i22, i23, i24));
                                }

                                d15 += d6 * f21;
                                d17 += d8 * f21;
                                d19 += d10 * f21;
                            }
                        }
                    }
                }
            }

            this.explosionSize *= 2F;
            i3 = Mth.Floor(this.explosionX - this.explosionSize - 1);
            i4 = Mth.Floor(this.explosionX + this.explosionSize + 1);
            i5 = Mth.Floor(this.explosionY - this.explosionSize - 1);
            int i29 = Mth.Floor(this.explosionY + this.explosionSize + 1);
            int i7 = Mth.Floor(this.explosionZ - this.explosionSize - 1);
            int i30 = Mth.Floor(this.explosionZ + this.explosionSize + 1);
            IList<Entity> list9 = this.worldObj.GetEntities(this.exploder, AABB.Of(i3, i5, i7, i4, i29, i30));
            Vec3 vec3D31 = Vec3.Of(this.explosionX, this.explosionY, this.explosionZ);
            for (int i11 = 0; i11 < list9.Count; ++i11)
            {
                Entity entity33 = list9[i11];
                double d13 = entity33.GetDistance(this.explosionX, this.explosionY, this.explosionZ) / this.explosionSize;
                if (d13 <= 1)
                {
                    d15 = entity33.x - this.explosionX;
                    d17 = entity33.y - this.explosionY;
                    d19 = entity33.z - this.explosionZ;
                    double d39 = Mth.Sqrt(d15 * d15 + d17 * d17 + d19 * d19);
                    d15 /= d39;
                    d17 /= d39;
                    d19 /= d39;
                    double d40 = this.worldObj.GetSeenPercent(vec3D31, entity33.boundingBox);
                    double d41 = (1 - d13) * d40;
                    entity33.AttackEntityFrom(this.exploder, (int)((d41 * d41 + d41) / 2 * 8 * this.explosionSize + 1));
                    entity33.motionX += d15 * d41;
                    entity33.motionY += d17 * d41;
                    entity33.motionZ += d19 * d41;
                }
            }

            this.explosionSize = f1;
            List<TilePos> arrayList32 = new List<TilePos>();

            foreach (TilePos p in destroyedBlockPositions) arrayList32.Add(p);
            if (this.isFlaming)
            {
                for (int i34 = arrayList32.Count - 1; i34 >= 0; --i34)
                {
                    TilePos chunkPosition35 = arrayList32[i34];
                    int i36 = chunkPosition35.x;
                    int i37 = chunkPosition35.y;
                    int i16 = chunkPosition35.z;
                    int i38 = this.worldObj.GetTile(i36, i37, i16);
                    int i18 = this.worldObj.GetTile(i36, i37 - 1, i16);
                    if (i38 == 0 && Tile.solid[i18] && this.ExplosionRNG.NextInt(3) == 0)
                    {
                        this.worldObj.SetTile(i36, i37, i16, Tile.fire.id);
                    }
                }
            }
        }

        public virtual void FinalizeExplosion(bool z1)
        {
            this.worldObj.PlaySound(this.explosionX, this.explosionY, this.explosionZ, "random.explode", 4F, (1F + (this.worldObj.rand.NextFloat() - this.worldObj.rand.NextFloat()) * 0.2F) * 0.7F);
            List<TilePos> arrayList2 = new List<TilePos>();
            foreach (TilePos p in destroyedBlockPositions) arrayList2.Add(p);
            for (int i3 = arrayList2.Count - 1; i3 >= 0; --i3)
            {
                TilePos chunkPosition4 = arrayList2[i3];
                int i5 = chunkPosition4.x;
                int i6 = chunkPosition4.y;
                int i7 = chunkPosition4.z;
                int i8 = this.worldObj.GetTile(i5, i6, i7);
                if (z1)
                {
                    double d9 = i5 + this.worldObj.rand.NextFloat();
                    double d11 = i6 + this.worldObj.rand.NextFloat();
                    double d13 = i7 + this.worldObj.rand.NextFloat();
                    double d15 = d9 - this.explosionX;
                    double d17 = d11 - this.explosionY;
                    double d19 = d13 - this.explosionZ;
                    double d21 = Mth.Sqrt(d15 * d15 + d17 * d17 + d19 * d19);
                    d15 /= d21;
                    d17 /= d21;
                    d19 /= d21;
                    double d23 = 0.5 / (d21 / this.explosionSize + 0.1);
                    d23 *= this.worldObj.rand.NextFloat() * this.worldObj.rand.NextFloat() + 0.3F;
                    d15 *= d23;
                    d17 *= d23;
                    d19 *= d23;
                    this.worldObj.AddParticle("explode", (d9 + this.explosionX * 1) / 2, (d11 + this.explosionY * 1) / 2, (d13 + this.explosionZ * 1) / 2, d15, d17, d19);
                    this.worldObj.AddParticle("smoke", d9, d11, d13, d15, d17, d19);
                }

                if (i8 > 0)
                {
                    Tile.tiles[i8].SpawnResources(this.worldObj, i5, i6, i7, this.worldObj.GetData(i5, i6, i7), 0.3F);
                    this.worldObj.SetTile(i5, i6, i7, 0);
                    Tile.tiles[i8].WasExploded(this.worldObj, i5, i6, i7);
                }
            }
        }
    }
}