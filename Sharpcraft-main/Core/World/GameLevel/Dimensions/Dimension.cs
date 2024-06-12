using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;
using System;

namespace SharpCraft.Core.World.GameLevel.Dimensions
{
    public abstract class Dimension
    {
        public Level level;
        public BiomeSource biomeSource;
        public bool isNether = false;
        public bool isHellWorld = false;
        public bool hasNoSky = false;
        public float[] lightBrightnessTable = new float[16];
        public int dimension = 0;
        private float[] colorsSunriseSunset = new float[4];
        public void Init(Level world1)
        {
            this.level = world1;
            this.RegisterWorldChunkManager();
            this.GenerateLightBrightnessTable();
        }

        protected virtual void GenerateLightBrightnessTable()
        {
            float f1 = 0.05F;
            for (int i2 = 0; i2 <= 15; ++i2)
            {
                float f3 = 1F - i2 / 15F;
                this.lightBrightnessTable[i2] = (1F - f3) / (f3 * 3F + 1F) * (1F - f1) + f1;
            }
        }

        protected virtual void RegisterWorldChunkManager()
        {
            this.biomeSource = new BiomeSource(this.level);
        }

        public virtual IChunkSource CreateRandomLevelSource()
        {
            if (Enhancements.FLAT_LEVEL)
            {
                return new FlatLevelSource(this.level, Enhancements.FLAT_LEVEL_VOID_WORLD);
            }
            else 
            {
                return new RandomLevelSource(this.level, this.level.GetRandomSeed());
            }
        }

        public virtual bool CanCoordinateBeSpawn(int i1, int i2)
        {
            int i3 = this.level.GetFirstUncoveredBlock(i1, i2);
            return i3 == Tile.sand.id;
        }

        public virtual float GetSunAngle(long j1, float f3)
        {
            int i4 = (int)(j1 % 24000);
            float f5 = (i4 + f3) / 24000F - 0.25F;
            if (f5 < 0F)
            {
                ++f5;
            }

            if (f5 > 1F)
            {
                --f5;
            }

            float f6 = f5;
            f5 = 1F - (float)((Math.Cos(f5 * Math.PI) + 1.0D) / 2.0D);
            f5 = f6 + (f5 - f6) / 3F;
            return f5;
        }

        public virtual float[] CalcSunriseSunsetColors(float f1, float f2)
        {
            float f3 = 0.4F;
            float f4 = Mth.Cos(f1 * Mth.PI * 2F) - 0F;
            float f5 = -0F;
            if (f4 >= f5 - f3 && f4 <= f5 + f3)
            {
                float f6 = (f4 - f5) / f3 * 0.5F + 0.5F;
                float f7 = 1F - (1F - Mth.Sin(f6 * Mth.PI)) * 0.99F;
                f7 *= f7;
                this.colorsSunriseSunset[0] = f6 * 0.3F + 0.7F;
                this.colorsSunriseSunset[1] = f6 * f6 * 0.7F + 0.2F;
                this.colorsSunriseSunset[2] = f6 * f6 * 0F + 0.2F;
                this.colorsSunriseSunset[3] = f7;
                return this.colorsSunriseSunset;
            }
            else
            {
                return null;
            }
        }

        public virtual Vec3 GetFogColor(float f1, float f2)
        {
            float f3 = Mth.Cos(f1 * Mth.PI * 2F) * 2F + 0.5F;
            if (f3 < 0F)
            {
                f3 = 0F;
            }

            if (f3 > 1F)
            {
                f3 = 1F;
            }

            float f4 = 0.7529412F;
            float f5 = 0.84705883F;
            float f6 = 1F;
            f4 *= f3 * 0.94F + 0.06F;
            f5 *= f3 * 0.94F + 0.06F;
            f6 *= f3 * 0.91F + 0.09F;
            return Vec3.Of(f4, f5, f6);
        }

        public virtual bool CanRespawnHere()
        {
            return true;
        }

        public static Dimension GetNew(int i0)
        {
            if (i0 == -1)
            {
                return new HellDimension();
            }
            else if (i0 == 0)
            {
                return new NormalDayCycleDimension();
            }
            else if (i0 == 1)
            {
                return new SkyDimension();
            }
            else
            {
                return null;
            }
        }

        public virtual float GetCloudHeight()
        {
            return 108F;
        }

        public virtual bool Func_28112_c()
        {
            return true;
        }
    }
}