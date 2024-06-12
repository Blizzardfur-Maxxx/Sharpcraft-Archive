using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using System;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public abstract class LiquidTile : Tile
    {
        protected LiquidTile(int i1, Material material2) : base(i1, (material2 == Material.lava ? 14 : 12) * 16 + 13, material2)
        {
            float f3 = 0F;
            float f4 = 0F;
            this.SetShape(0F + f4, 0F + f3, 0F + f4, 1F + f4, 1F + f3, 1F + f4);
            this.SetTicking(true);
        }

        public override int GetColor(ILevelSource ls, int x, int y, int z)
        {
            if (Enhancements.WATER_BIOME_COLOR)
            {
                if (this.material == Material.water)
                {
                    BiomeSource bs = ls.GetBiomeSource();
                    if (bs.GetBiomeGenAt(x, z) == Biome.hell)
                    {
                        return 0x7B4340;
                    }

                    bs.Func_a(x, z, 1, 1);
                    double temp = bs.temperature[0];
                    double humd = bs.humidity[0];
                    return WaterColor.GetColor(temp, humd);
                }
            }

            return 0xFFFFFF;
        }

        public static float GetHeight(int i0)
        {
            if (i0 >= 8)
            {
                i0 = 0;
            }

            float f1 = (i0 + 1) / 9F;
            return f1;
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx != TileFace.DOWN && faceIdx != TileFace.UP ? this.texture + 1 : this.texture;
        }

        protected virtual int GetDepth(Level world1, int i2, int i3, int i4)
        {
            return world1.GetMaterial(i2, i3, i4) != this.material ? -1 : world1.GetData(i2, i3, i4);
        }

        protected virtual int GetRenderedDepth(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            if (iBlockAccess1.GetMaterial(i2, i3, i4) != this.material)
            {
                return -1;
            }
            else
            {
                int i5 = iBlockAccess1.GetData(i2, i3, i4);
                if (i5 >= 8)
                {
                    i5 = 0;
                }

                return i5;
            }
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool MayPick(int i1, bool z2)
        {
            return z2 && i1 == 0;
        }

        public override bool GetIsBlockSolid(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            Material material6 = iBlockAccess1.GetMaterial(i2, i3, i4);
            return material6 == this.material ? false : (material6 == Material.ice ? false : (i5 == 1 ? true : base.GetIsBlockSolid(iBlockAccess1, i2, i3, i4, i5)));
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            Material material6 = iBlockAccess1.GetMaterial(i2, i3, i4);
            return material6 == this.material ? false : (material6 == Material.ice ? false : (i5 == 1 ? true : base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5)));
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.LIQUID;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return 0;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        private Vec3 GetFlow(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            Vec3 vec3D5 = Vec3.Of(0, 0, 0);
            int i6 = this.GetRenderedDepth(iBlockAccess1, i2, i3, i4);
            for (int i7 = 0; i7 < 4; ++i7)
            {
                int i8 = i2;
                int i10 = i4;
                if (i7 == 0)
                {
                    i8 = i2 - 1;
                }

                if (i7 == 1)
                {
                    i10 = i4 - 1;
                }

                if (i7 == 2)
                {
                    ++i8;
                }

                if (i7 == 3)
                {
                    ++i10;
                }

                int i11 = this.GetRenderedDepth(iBlockAccess1, i8, i3, i10);
                int i12;
                if (i11 < 0)
                {
                    if (!iBlockAccess1.GetMaterial(i8, i3, i10).BlocksMotion())
                    {
                        i11 = this.GetRenderedDepth(iBlockAccess1, i8, i3 - 1, i10);
                        if (i11 >= 0)
                        {
                            i12 = i11 - (i6 - 8);
                            vec3D5 = vec3D5.AddVector((i8 - i2) * i12, (i3 - i3) * i12, (i10 - i4) * i12);
                        }
                    }
                }
                else if (i11 >= 0)
                {
                    i12 = i11 - i6;
                    vec3D5 = vec3D5.AddVector((i8 - i2) * i12, (i3 - i3) * i12, (i10 - i4) * i12);
                }
            }

            if (iBlockAccess1.GetData(i2, i3, i4) >= 8)
            {
                bool z13 = false;
                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2, i3, i4 - 1, 2))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2, i3, i4 + 1, 3))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2 - 1, i3, i4, 4))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2 + 1, i3, i4, 5))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2, i3 + 1, i4 - 1, 2))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2, i3 + 1, i4 + 1, 3))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2 - 1, i3 + 1, i4, 4))
                {
                    z13 = true;
                }

                if (z13 || this.GetIsBlockSolid(iBlockAccess1, i2 + 1, i3 + 1, i4, 5))
                {
                    z13 = true;
                }

                if (z13)
                {
                    vec3D5 = vec3D5.Normalize().AddVector(0, -6, 0);
                }
            }

            vec3D5 = vec3D5.Normalize();
            return vec3D5;
        }

        public override void HandleEntityInside(Level world1, int i2, int i3, int i4, Entity entity5, Vec3 vec3D6)
        {
            Vec3 vec3D7 = this.GetFlow(world1, i2, i3, i4);
            vec3D6.x += vec3D7.x;
            vec3D6.y += vec3D7.y;
            vec3D6.z += vec3D7.z;
        }

        public override int GetTickDelay()
        {
            return this.material == Material.water ? 5 : (this.material == Material.lava ? 30 : 0);
        }

        public override float GetBrightness(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            float f5 = iBlockAccess1.GetBrightness(i2, i3, i4);
            float f6 = iBlockAccess1.GetBrightness(i2, i3 + 1, i4);
            return f5 > f6 ? f5 : f6;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            base.Tick(world1, i2, i3, i4, random5);
        }

        public override RenderLayer GetRenderLayer()
        {
            return this.material == Material.water ? RenderLayer.RENDERLAYER_ALPHATEST : RenderLayer.RENDERLAYER_OPAQUE;
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.material == Material.water && random5.NextInt(64) == 0)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if (i6 > 0 && i6 < 8)
                {
                    world1.PlaySound(i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, "liquid.water", (float)random5.NextFloat() * 0.25F + 0.75F, (float)random5.NextFloat() * 1F + 0.5F);
                }
            }

            if (this.material == Material.lava && world1.GetMaterial(i2, i3 + 1, i4) == Material.air && !world1.IsSolidRenderTile(i2, i3 + 1, i4) && random5.NextInt(100) == 0)
            {
                double d12 = i2 + random5.NextFloat();
                double d8 = i3 + this.maxY;
                double d10 = i4 + random5.NextFloat();
                world1.AddParticle("lava", d12, d8, d10, 0, 0, 0);
            }
        }

        public static double GetSlopeAngle(ILevelSource iBlockAccess0, int i1, int i2, int i3, Material material4)
        {
            Vec3 vec3D5 = null;
            if (material4 == Material.water)
            {
                vec3D5 = ((LiquidTile)Tile.water).GetFlow(iBlockAccess0, i1, i2, i3);
            }

            if (material4 == Material.lava)
            {
                vec3D5 = ((LiquidTile)Tile.lava).GetFlow(iBlockAccess0, i1, i2, i3);
            }

            return vec3D5.x == 0 && vec3D5.z == 0 ? -1000 : Math.Atan2(vec3D5.z, vec3D5.x) - Math.PI / 2;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            this.UpdateLiquid(world1, i2, i3, i4);
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            this.UpdateLiquid(world1, i2, i3, i4);
        }

        private void UpdateLiquid(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetTile(i2, i3, i4) == this.id)
            {
                if (this.material == Material.lava)
                {
                    bool z5 = false;
                    if (z5 || world1.GetMaterial(i2, i3, i4 - 1) == Material.water)
                    {
                        z5 = true;
                    }

                    if (z5 || world1.GetMaterial(i2, i3, i4 + 1) == Material.water)
                    {
                        z5 = true;
                    }

                    if (z5 || world1.GetMaterial(i2 - 1, i3, i4) == Material.water)
                    {
                        z5 = true;
                    }

                    if (z5 || world1.GetMaterial(i2 + 1, i3, i4) == Material.water)
                    {
                        z5 = true;
                    }

                    if (z5 || world1.GetMaterial(i2, i3 + 1, i4) == Material.water)
                    {
                        z5 = true;
                    }

                    if (z5)
                    {
                        int i6 = world1.GetData(i2, i3, i4);
                        if (i6 == 0)
                        {
                            world1.SetTile(i2, i3, i4, Tile.obsidian.id);
                        }
                        else if (i6 <= 4)
                        {
                            world1.SetTile(i2, i3, i4, Tile.stoneBrick.id);
                        }

                        this.Fizz(world1, i2, i3, i4);
                    }
                }
            }
        }

        protected virtual void Fizz(Level world1, int i2, int i3, int i4)
        {
            world1.PlaySound(i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, "random.fizz", 0.5F, 2.6F + (float)(world1.rand.NextFloat() - world1.rand.NextFloat()) * 0.8F);
            for (int i5 = 0; i5 < 8; ++i5)
            {
                world1.AddParticle("largesmoke", i2 + world1.rand.NextFloat(), i3 + 1.2, i4 + world1.rand.NextFloat(), 0, 0, 0);
            }
        }
    }
}