using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel
{
    public class LightUpdate
    {
        public readonly LightLayer layer;
        public int x0;
        public int y0;
        public int z0;
        public int x1;
        public int y1;
        public int z1;
        public LightUpdate(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            this.layer = enumSkyBlock1;
            this.x0 = i2;
            this.y0 = i3;
            this.z0 = i4;
            this.x1 = i5;
            this.y1 = i6;
            this.z1 = i7;
        }

        public virtual void Update(Level world1)
        {
            int i2 = this.x1 - this.x0 + 1;
            int i3 = this.y1 - this.y0 + 1;
            int i4 = this.z1 - this.z0 + 1;
            int i5 = i2 * i3 * i4;
            if (i5 > 32768)
            {
                System.Console.WriteLine("Light too large, skipping!");
            }
            else
            {
                int i6 = 0;
                int i7 = 0;
                bool z8 = false;
                bool z9 = false;
                for (int i10 = this.x0; i10 <= this.x1; ++i10)
                {
                    for (int i11 = this.z0; i11 <= this.z1; ++i11)
                    {
                        int i12 = i10 >> 4;
                        int i13 = i11 >> 4;
                        bool z14 = false;
                        if (z8 && i12 == i6 && i13 == i7)
                        {
                            z14 = z9;
                        }
                        else
                        {
                            z14 = world1.DoChunksNearChunkExist(i10, 0, i11, 1);
                            if (z14)
                            {
                                LevelChunk chunk15 = world1.GetChunk(i10 >> 4, i11 >> 4);
                                if (chunk15.IsEmpty())
                                {
                                    z14 = false;
                                }
                            }

                            z9 = z14;
                            i6 = i12;
                            i7 = i13;
                        }

                        if (z14)
                        {
                            if (this.y0 < 0)
                            {
                                this.y0 = 0;
                            }

                            if (this.y1 >= 128)
                            {
                                this.y1 = 127;
                            }

                            for (int i27 = this.y0; i27 <= this.y1; ++i27)
                            {
                                int i16 = world1.GetSavedLightValue(this.layer, i10, i27, i11);
                                int i18 = world1.GetTile(i10, i27, i11);
                                int i19 = Tile.lightBlock[i18];
                                if (i19 == 0)
                                {
                                    i19 = 1;
                                }

                                int i20 = 0;
                                if (this.layer == LightLayer.Sky)
                                {
                                    if (world1.CanExistingBlockSeeTheSky(i10, i27, i11))
                                    {
                                        i20 = 15;
                                    }
                                }
                                else if (this.layer == LightLayer.Block)
                                {
                                    i20 = Tile.lightEmission[i18];
                                }

                                int i21;
                                int i28;
                                if (i19 >= 15 && i20 == 0)
                                {
                                    i28 = 0;
                                }
                                else
                                {
                                    i21 = world1.GetSavedLightValue(this.layer, i10 - 1, i27, i11);
                                    int i22 = world1.GetSavedLightValue(this.layer, i10 + 1, i27, i11);
                                    int i23 = world1.GetSavedLightValue(this.layer, i10, i27 - 1, i11);
                                    int i24 = world1.GetSavedLightValue(this.layer, i10, i27 + 1, i11);
                                    int i25 = world1.GetSavedLightValue(this.layer, i10, i27, i11 - 1);
                                    int i26 = world1.GetSavedLightValue(this.layer, i10, i27, i11 + 1);
                                    i28 = i21;
                                    if (i22 > i21)
                                    {
                                        i28 = i22;
                                    }

                                    if (i23 > i28)
                                    {
                                        i28 = i23;
                                    }

                                    if (i24 > i28)
                                    {
                                        i28 = i24;
                                    }

                                    if (i25 > i28)
                                    {
                                        i28 = i25;
                                    }

                                    if (i26 > i28)
                                    {
                                        i28 = i26;
                                    }

                                    i28 -= i19;
                                    if (i28 < 0)
                                    {
                                        i28 = 0;
                                    }

                                    if (i20 > i28)
                                    {
                                        i28 = i20;
                                    }
                                }

                                if (i16 != i28)
                                {
                                    world1.SetLightValue(this.layer, i10, i27, i11, i28);
                                    i21 = i28 - 1;
                                    if (i21 < 0)
                                    {
                                        i21 = 0;
                                    }

                                    world1.NeighborLightPropagationChanged(this.layer, i10 - 1, i27, i11, i21);
                                    world1.NeighborLightPropagationChanged(this.layer, i10, i27 - 1, i11, i21);
                                    world1.NeighborLightPropagationChanged(this.layer, i10, i27, i11 - 1, i21);
                                    if (i10 + 1 >= this.x1)
                                    {
                                        world1.NeighborLightPropagationChanged(this.layer, i10 + 1, i27, i11, i21);
                                    }

                                    if (i27 + 1 >= this.y1)
                                    {
                                        world1.NeighborLightPropagationChanged(this.layer, i10, i27 + 1, i11, i21);
                                    }

                                    if (i11 + 1 >= this.z1)
                                    {
                                        world1.NeighborLightPropagationChanged(this.layer, i10, i27, i11 + 1, i21);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual bool Func866(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            if (i1 >= this.x0 && i2 >= this.y0 && i3 >= this.z0 && i4 <= this.x1 && i5 <= this.y1 && i6 <= this.z1)
            {
                return true;
            }
            else
            {
                byte b7 = 1;
                if (i1 >= this.x0 - b7 && i2 >= this.y0 - b7 && i3 >= this.z0 - b7 && i4 <= this.x1 + b7 && i5 <= this.y1 + b7 && i6 <= this.z1 + b7)
                {
                    int i8 = this.x1 - this.x0;
                    int i9 = this.y1 - this.y0;
                    int i10 = this.z1 - this.z0;
                    if (i1 > this.x0)
                    {
                        i1 = this.x0;
                    }

                    if (i2 > this.y0)
                    {
                        i2 = this.y0;
                    }

                    if (i3 > this.z0)
                    {
                        i3 = this.z0;
                    }

                    if (i4 < this.x1)
                    {
                        i4 = this.x1;
                    }

                    if (i5 < this.y1)
                    {
                        i5 = this.y1;
                    }

                    if (i6 < this.z1)
                    {
                        i6 = this.z1;
                    }

                    int i11 = i4 - i1;
                    int i12 = i5 - i2;
                    int i13 = i6 - i3;
                    int i14 = i8 * i9 * i10;
                    int i15 = i11 * i12 * i13;
                    if (i15 - i14 <= 2)
                    {
                        this.x0 = i1;
                        this.y0 = i2;
                        this.z0 = i3;
                        this.x1 = i4;
                        this.y1 = i5;
                        this.z1 = i6;
                        return true;
                    }
                }

                return false;
            }
        }
    }
}