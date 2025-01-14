﻿using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Ptexture
{
    public class GreyscaleWaterSideTexture : DynamicTexture
    {
        protected float[] red = new float[256];
        protected float[] green = new float[256];
        protected float[] blue = new float[256];
        protected float[] alpha = new float[256];
        private int field_1134_k = 0;

        public GreyscaleWaterSideTexture() : base(Tile.water.texture + 1)
        {
            this.tileSize = 2;
        }

        public override void OnTick()
        {
            ++this.field_1134_k;

            int i1;
            int i2;
            float f3;
            int i5;
            int i6;
            for (i1 = 0; i1 < 16; ++i1)
            {
                for (i2 = 0; i2 < 16; ++i2)
                {
                    f3 = 0.0F;

                    for (int i4 = i2 - 2; i4 <= i2; ++i4)
                    {
                        i5 = i1 & 15;
                        i6 = i4 & 15;
                        f3 += this.red[i5 + i6 * 16];
                    }

                    this.green[i1 + i2 * 16] = f3 / 3.2F + this.blue[i1 + i2 * 16] * 0.8F;
                }
            }

            for (i1 = 0; i1 < 16; ++i1)
            {
                for (i2 = 0; i2 < 16; ++i2)
                {
                    this.blue[i1 + i2 * 16] += this.alpha[i1 + i2 * 16] * 0.05F;
                    if (this.blue[i1 + i2 * 16] < 0.0F)
                    {
                        this.blue[i1 + i2 * 16] = 0.0F;
                    }

                    this.alpha[i1 + i2 * 16] -= 0.3F;
                    if (Mth.Random() < 0.2D)
                    {
                        this.alpha[i1 + i2 * 16] = 0.5F;
                    }
                }
            }

            float[] f12 = this.green;
            this.green = this.red;
            this.red = f12;

            for (i2 = 0; i2 < 256; ++i2)
            {
                f3 = this.red[i2 - this.field_1134_k * 16 & 255];
                if (f3 > 1.0F)
                {
                    f3 = 1.0F;
                }

                if (f3 < 0.0F)
                {
                    f3 = 0.0F;
                }

                float f13 = f3 * f3;
                i5 = (int)(Enhancements.WATER_BIOME_COLOR_MUL + f13 * 50.0F);
                i6 = (int)(Enhancements.WATER_BIOME_COLOR_MUL + f13 * 50.0F);
                int i7 = (int)(Enhancements.WATER_BIOME_COLOR_MUL + f13 * 50.0F);
                int i8 = (int)(Enhancements.WATER_BIOME_COLOR_ALPHA + f13 * 50.0F);
                if (this.anaglyphEnabled)
                {
                    int i9 = (i5 * 30 + i6 * 59 + i7 * 11) / 100;
                    int i10 = (i5 * 30 + i6 * 70) / 100;
                    int i11 = (i5 * 30 + i7 * 70) / 100;
                    i5 = i9;
                    i6 = i10;
                    i7 = i11;
                }

                this.imageData[i2 * 4 + 0] = (byte)i5;
                this.imageData[i2 * 4 + 1] = (byte)i6;
                this.imageData[i2 * 4 + 2] = (byte)i7;
                this.imageData[i2 * 4 + 3] = (byte)i8;
            }

        }
    }
}
