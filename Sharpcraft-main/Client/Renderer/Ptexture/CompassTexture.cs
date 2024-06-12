using SharpCraft.Core;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LWCSGL;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Client.Renderer.Ptexture
{
    public class CompassTexture : DynamicTexture
    {
        private Client mc;
        private int[] compassIconImageData = new int[256];
        private double field_4229_i;
        private double field_4228_j;

        public CompassTexture(Client instance) : base(Item.compass.GetIconFromDamage(0))
        {
            this.mc = instance;
            this.tileImage = 1;

            try
            {
                Bitmap bufferedImage2 = Textures.GetAssetsBitmap("/gui/items.png");
                int i3 = this.iconIndex % 16 * 16;
                int i4 = this.iconIndex / 16 * 16;
                bufferedImage2.GetARGB(i3, i4, 16, 16, this.compassIconImageData, 0, 16);
                bufferedImage2.Dispose();
    
        }
            catch (Exception ioe)
            {
                ioe.PrintStackTrace();
            }

        }

        public override void OnTick()
        {
            for (int i1 = 0; i1 < 256; ++i1)
            {
                int i2 = this.compassIconImageData[i1] >> 24 & 255;
                int i3 = this.compassIconImageData[i1] >> 16 & 255;
                int i4 = this.compassIconImageData[i1] >> 8 & 255;
                int i5 = this.compassIconImageData[i1] >> 0 & 255;
                if (this.anaglyphEnabled)
                {
                    int i6 = (i3 * 30 + i4 * 59 + i5 * 11) / 100;
                    int i7 = (i3 * 30 + i4 * 70) / 100;
                    int i8 = (i3 * 30 + i5 * 70) / 100;
                    i3 = i6;
                    i4 = i7;
                    i5 = i8;
                }

                this.imageData[i1 * 4 + 0] = (byte)i3;
                this.imageData[i1 * 4 + 1] = (byte)i4;
                this.imageData[i1 * 4 + 2] = (byte)i5;
                this.imageData[i1 * 4 + 3] = (byte)i2;
            }

            double d20 = 0.0D;
            if (this.mc.level != null && this.mc.player != null)
            {
                Pos chunkCoordinates21 = this.mc.level.GetSpawnPos();
                double d23 = chunkCoordinates21.x - this.mc.player.x;
                double d25 = chunkCoordinates21.z - this.mc.player.z;
                d20 = (this.mc.player.yaw - 90.0F) * Math.PI / 180.0D - Math.Atan2(d25, d23);
                if (this.mc.level.dimension.isNether)
                {
                    d20 = Mth.Random() * Mth.PI * 2.0D;
                }
            }

            double d22;
            for (d22 = d20 - this.field_4229_i; d22 < -3.141592653589793D; d22 += Math.PI * 2D)
            {
            }

            while (d22 >= Math.PI)
            {
                d22 -= Math.PI * 2D;
            }

            if (d22 < -1.0D)
            {
                d22 = -1.0D;
            }

            if (d22 > 1.0D)
            {
                d22 = 1.0D;
            }

            this.field_4228_j += d22 * 0.1D;
            this.field_4228_j *= 0.8D;
            this.field_4229_i += this.field_4228_j;
            double d24 = Math.Sin(this.field_4229_i);
            double d26 = Math.Cos(this.field_4229_i);

            int i9;
            int i10;
            int i11;
            int i12;
            int i13;
            int i14;
            int i15;
            short s16;
            int i17;
            int i18;
            int i19;
            for (i9 = -4; i9 <= 4; ++i9)
            {
                i10 = (int)(8.5D + d26 * i9 * 0.3D);
                i11 = (int)(7.5D - d24 * i9 * 0.3D * 0.5D);
                i12 = i11 * 16 + i10;
                i13 = 100;
                i14 = 100;
                i15 = 100;
                s16 = 255;
                if (this.anaglyphEnabled)
                {
                    i17 = (i13 * 30 + i14 * 59 + i15 * 11) / 100;
                    i18 = (i13 * 30 + i14 * 70) / 100;
                    i19 = (i13 * 30 + i15 * 70) / 100;
                    i13 = i17;
                    i14 = i18;
                    i15 = i19;
                }

                this.imageData[i12 * 4 + 0] = (byte)i13;
                this.imageData[i12 * 4 + 1] = (byte)i14;
                this.imageData[i12 * 4 + 2] = (byte)i15;
                this.imageData[i12 * 4 + 3] = (byte)s16;
            }

            for (i9 = -8; i9 <= 16; ++i9)
            {
                i10 = (int)(8.5D + d24 * i9 * 0.3D);
                i11 = (int)(7.5D + d26 * i9 * 0.3D * 0.5D);
                i12 = i11 * 16 + i10;
                i13 = i9 >= 0 ? 255 : 100;
                i14 = i9 >= 0 ? 20 : 100;
                i15 = i9 >= 0 ? 20 : 100;
                s16 = 255;
                if (this.anaglyphEnabled)
                {
                    i17 = (i13 * 30 + i14 * 59 + i15 * 11) / 100;
                    i18 = (i13 * 30 + i14 * 70) / 100;
                    i19 = (i13 * 30 + i15 * 70) / 100;
                    i13 = i17;
                    i14 = i18;
                    i15 = i19;
                }

                this.imageData[i12 * 4 + 0] = (byte)i13;
                this.imageData[i12 * 4 + 1] = (byte)i14;
                this.imageData[i12 * 4 + 2] = (byte)i15;
                this.imageData[i12 * 4 + 3] = (byte)s16;
            }

        }
    }
}
