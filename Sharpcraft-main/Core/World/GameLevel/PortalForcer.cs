using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel
{
    public class PortalForcer
    {
        private JRandom random = new JRandom();
        public virtual void SetExitLocation(Level world1, Entity entity2)
        {
            if (!this.FindExitLocation(world1, entity2))
            {
                this.CreatePortal(world1, entity2);
                this.FindExitLocation(world1, entity2);
            }
        }

        public virtual bool FindExitLocation(Level world1, Entity entity2)
        {
            short s3 = 128;
            double d4 = -1;
            int i6 = 0;
            int i7 = 0;
            int i8 = 0;
            int i9 = Mth.Floor(entity2.x);
            int i10 = Mth.Floor(entity2.z);
            double d18;
            for (int i11 = i9 - s3; i11 <= i9 + s3; ++i11)
            {
                double d12 = i11 + 0.5 - entity2.x;
                for (int i14 = i10 - s3; i14 <= i10 + s3; ++i14)
                {
                    double d15 = i14 + 0.5 - entity2.z;
                    for (int i17 = 127; i17 >= 0; --i17)
                    {
                        if (world1.GetTile(i11, i17, i14) == Tile.portal.id)
                        {
                            while (world1.GetTile(i11, i17 - 1, i14) == Tile.portal.id)
                            {
                                --i17;
                            }

                            d18 = i17 + 0.5 - entity2.y;
                            double d20 = d12 * d12 + d18 * d18 + d15 * d15;
                            if (d4 < 0 || d20 < d4)
                            {
                                d4 = d20;
                                i6 = i11;
                                i7 = i17;
                                i8 = i14;
                            }
                        }
                    }
                }
            }

            if (d4 >= 0)
            {
                double d22 = i6 + 0.5;
                double d16 = i7 + 0.5;
                d18 = i8 + 0.5;
                if (world1.GetTile(i6 - 1, i7, i8) == Tile.portal.id)
                {
                    d22 -= 0.5;
                }

                if (world1.GetTile(i6 + 1, i7, i8) == Tile.portal.id)
                {
                    d22 += 0.5;
                }

                if (world1.GetTile(i6, i7, i8 - 1) == Tile.portal.id)
                {
                    d18 -= 0.5;
                }

                if (world1.GetTile(i6, i7, i8 + 1) == Tile.portal.id)
                {
                    d18 += 0.5;
                }

                entity2.SetLocationAndAngles(d22, d16, d18, entity2.yaw, 0F);
                entity2.motionX = entity2.motionY = entity2.motionZ = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CreatePortal(Level world1, Entity entity2)
        {
            byte b3 = 16;
            double d4 = -1;
            int i6 = Mth.Floor(entity2.x);
            int i7 = Mth.Floor(entity2.y);
            int i8 = Mth.Floor(entity2.z);
            int i9 = i6;
            int i10 = i7;
            int i11 = i8;
            int i12 = 0;
            int i13 = this.random.NextInt(4);
            int i14;
            double d15;
            int i17;
            double d18;
            int i20;
            int i21;
            int i22;
            int i23;
            int i24;
            int i25;
            int i26;
            int i27;
            int i28;
            double d32;
            double d33;
            for (i14 = i6 - b3; i14 <= i6 + b3; ++i14)
            {
                d15 = i14 + 0.5 - entity2.x;
                for (i17 = i8 - b3; i17 <= i8 + b3; ++i17)
                {
                    d18 = i17 + 0.5 - entity2.z;
                    //label293:
                    bool continue293 = false;
                    for (i20 = 127; i20 >= 0; --i20)
                    {
                        if (world1.IsAirBlock(i14, i20, i17))
                        {
                            while (i20 > 0 && world1.IsAirBlock(i14, i20 - 1, i17))
                            {
                                --i20;
                            }

                            for (i21 = i13; i21 < i13 + 4; ++i21)
                            {
                                i22 = i21 % 2;
                                i23 = 1 - i22;
                                if (i21 % 4 >= 2)
                                {
                                    i22 = -i22;
                                    i23 = -i23;
                                }

                                for (i24 = 0; i24 < 3; ++i24)
                                {
                                    for (i25 = 0; i25 < 4; ++i25)
                                    {
                                        for (i26 = -1; i26 < 4; ++i26)
                                        {
                                            i27 = i14 + (i25 - 1) * i22 + i24 * i23;
                                            i28 = i20 + i26;
                                            int i29 = i17 + (i25 - 1) * i23 - i24 * i22;
                                            if (i26 < 0 && !world1.GetMaterial(i27, i28, i29).IsSolid() || i26 >= 0 && !world1.IsAirBlock(i27, i28, i29))
                                            {
                                                //continue label293;
                                                continue293 = true;
                                                break;
                                            }
                                        }
                                        if (continue293) break;
                                    }
                                    if (continue293) break;
                                }
                                if (continue293) break;

                                d32 = i20 + 0.5 - entity2.y;
                                d33 = d15 * d15 + d32 * d32 + d18 * d18;
                                if (d4 < 0 || d33 < d4)
                                {
                                    d4 = d33;
                                    i9 = i14;
                                    i10 = i20;
                                    i11 = i17;
                                    i12 = i21 % 4;
                                }
                            }
                            if (continue293)
                            {
                                continue293 = false;
                                continue;
                            }
                        }
                    }
                }
            }

            if (d4 < 0)
            {
                for (i14 = i6 - b3; i14 <= i6 + b3; ++i14)
                {
                    d15 = i14 + 0.5 - entity2.x;
                    for (i17 = i8 - b3; i17 <= i8 + b3; ++i17)
                    {
                        d18 = i17 + 0.5 - entity2.z;
                        //label231:
                        bool continue231 = false;
                        for (i20 = 127; i20 >= 0; --i20)
                        {
                            if (world1.IsAirBlock(i14, i20, i17))
                            {
                                while (world1.IsAirBlock(i14, i20 - 1, i17))
                                {
                                    --i20;
                                }

                                for (i21 = i13; i21 < i13 + 2; ++i21)
                                {
                                    i22 = i21 % 2;
                                    i23 = 1 - i22;
                                    for (i24 = 0; i24 < 4; ++i24)
                                    {
                                        for (i25 = -1; i25 < 4; ++i25)
                                        {
                                            i26 = i14 + (i24 - 1) * i22;
                                            i27 = i20 + i25;
                                            i28 = i17 + (i24 - 1) * i23;
                                            if (i25 < 0 && !world1.GetMaterial(i26, i27, i28).IsSolid() || i25 >= 0 && !world1.IsAirBlock(i26, i27, i28))
                                            {
                                                continue231 = true;
                                                break;
                                                //continue label231;
                                            }
                                        }
                                        if (continue231) break;
                                    }
                                    if (continue231) break;

                                    d32 = i20 + 0.5 - entity2.y;
                                    d33 = d15 * d15 + d32 * d32 + d18 * d18;
                                    if (d4 < 0 || d33 < d4)
                                    {
                                        d4 = d33;
                                        i9 = i14;
                                        i10 = i20;
                                        i11 = i17;
                                        i12 = i21 % 2;
                                    }
                                }

                                if (continue231)
                                {
                                    continue231 = false;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            int i30 = i9;
            int i16 = i10;
            i17 = i11;
            int i31 = i12 % 2;
            int i19 = 1 - i31;
            if (i12 % 4 >= 2)
            {
                i31 = -i31;
                i19 = -i19;
            }

            bool z34;
            if (d4 < 0)
            {
                if (i10 < 70)
                {
                    i10 = 70;
                }

                if (i10 > 118)
                {
                    i10 = 118;
                }

                i16 = i10;
                for (i20 = -1; i20 <= 1; ++i20)
                {
                    for (i21 = 1; i21 < 3; ++i21)
                    {
                        for (i22 = -1; i22 < 3; ++i22)
                        {
                            i23 = i30 + (i21 - 1) * i31 + i20 * i19;
                            i24 = i16 + i22;
                            i25 = i17 + (i21 - 1) * i19 - i20 * i31;
                            z34 = i22 < 0;
                            world1.SetTile(i23, i24, i25, z34 ? Tile.obsidian.id : 0);
                        }
                    }
                }
            }

            for (i20 = 0; i20 < 4; ++i20)
            {
                world1.editingBlocks = true;
                for (i21 = 0; i21 < 4; ++i21)
                {
                    for (i22 = -1; i22 < 4; ++i22)
                    {
                        i23 = i30 + (i21 - 1) * i31;
                        i24 = i16 + i22;
                        i25 = i17 + (i21 - 1) * i19;
                        z34 = i21 == 0 || i21 == 3 || i22 == -1 || i22 == 3;
                        world1.SetTile(i23, i24, i25, z34 ? Tile.obsidian.id : Tile.portal.id);
                    }
                }

                world1.editingBlocks = false;
                for (i21 = 0; i21 < 4; ++i21)
                {
                    for (i22 = -1; i22 < 4; ++i22)
                    {
                        i23 = i30 + (i21 - 1) * i31;
                        i24 = i16 + i22;
                        i25 = i17 + (i21 - 1) * i19;
                        world1.UpdateNeighborsAt(i23, i24, i25, world1.GetTile(i23, i24, i25));
                    }
                }
            }

            return true;
        }
    }
}