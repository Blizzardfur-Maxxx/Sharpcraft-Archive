using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.GameSavedData.maps;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using System.Text;

namespace SharpCraft.Core.World.Items
{
    public class ItemMap : ItemMapBase
    {
        public ItemMap(int i1) : base(i1)
        {
            this.SetMaxStackSize(1);
        }

        public static MapItemSavedData GetShortMapData(short s0, Level world1)
        {
            (new StringBuilder()).Append("map_").Append(s0).ToString();
            MapItemSavedData mapData3 = (MapItemSavedData)world1.LoadItemData(typeof(MapItemSavedData), "map_" + s0);
            if (mapData3 == null)
            {
                int i4 = world1.GetUniqueDataId("map");
                string string2 = "map_" + i4;
                mapData3 = new MapItemSavedData(string2);
                world1.SetItemData(string2, mapData3);
            }

            return mapData3;
        }

        public virtual MapItemSavedData GetItemMapData(ItemInstance itemStack1, Level world2)
        {
            (new StringBuilder()).Append("map_").Append(itemStack1.GetItemDamage()).ToString();
            MapItemSavedData mapData4 = (MapItemSavedData)world2.LoadItemData(typeof(MapItemSavedData), "map_" + itemStack1.GetItemDamage());
            if (mapData4 == null)
            {
                itemStack1.SetItemDamage(world2.GetUniqueDataId("map"));
                string string3 = "map_" + itemStack1.GetItemDamage();
                mapData4 = new MapItemSavedData(string3);
                mapData4.field_b = world2.GetLevelData().GetSpawnX();
                mapData4.field_c = world2.GetLevelData().GetSpawnZ();
                mapData4.field_e = 3;
                mapData4.field_d = (byte)world2.dimension.dimension;
                mapData4.MarkDirty();
                world2.SetItemData(string3, mapData4);
            }

            return mapData4;
        }

        public virtual void F28011(Level world1, Entity entity2, MapItemSavedData mapData3)
        {
            if (world1.dimension.dimension == mapData3.field_d)
            {
                short s4 = 128;
                short s5 = 128;
                int i6 = 1 << mapData3.field_e;
                int i7 = mapData3.field_b;
                int i8 = mapData3.field_c;
                int i9 = Mth.Floor(entity2.x - i7) / i6 + s4 / 2;
                int i10 = Mth.Floor(entity2.z - i8) / i6 + s5 / 2;
                int i11 = 128 / i6;
                if (world1.dimension.hasNoSky)
                {
                    i11 /= 2;
                }

                ++mapData3.field_g;
                for (int i12 = i9 - i11 + 1; i12 < i9 + i11; ++i12)
                {
                    if ((i12 & 15) == (mapData3.field_g & 15))
                    {
                        int i13 = 255;
                        int i14 = 0;
                        double d15 = 0;
                        for (int i17 = i10 - i11 - 1; i17 < i10 + i11; ++i17)
                        {
                            if (i12 >= 0 && i17 >= -1 && i12 < s4 && i17 < s5)
                            {
                                int i18 = i12 - i9;
                                int i19 = i17 - i10;
                                bool z20 = i18 * i18 + i19 * i19 > (i11 - 2) * (i11 - 2);
                                int i21 = (i7 / i6 + i12 - s4 / 2) * i6;
                                int i22 = (i8 / i6 + i17 - s5 / 2) * i6;
                                int[] i26 = new int[256];
                                LevelChunk chunk27 = world1.GetChunkAt(i21, i22);
                                int i28 = i21 & 15;
                                int i29 = i22 & 15;
                                int i30 = 0;
                                double d31 = 0;
                                int i33;
                                int i34;
                                int i35;
                                int i38;
                                if (world1.dimension.hasNoSky)
                                {
                                    i33 = i21 + i22 * 231871;
                                    i33 = i33 * i33 * 31287121 + i33 * 11;
                                    if ((i33 >> 20 & 1) == 0)
                                    {
                                        i26[Tile.dirt.id] += 10;
                                    }
                                    else
                                    {
                                        i26[Tile.rock.id] += 10;
                                    }

                                    d31 = 100;
                                }
                                else
                                {
                                    for (i33 = 0; i33 < i6; ++i33)
                                    {
                                        for (i34 = 0; i34 < i6; ++i34)
                                        {
                                            i35 = chunk27.GetHeightValue(i33 + i28, i34 + i29) + 1;
                                            int i36 = 0;
                                            if (i35 > 1)
                                            {
                                                bool z37 = false;
                                                bool break164 = false;
                                            //label164:
                                                while (true)
                                                {
                                                    z37 = true;
                                                    i36 = chunk27.GetBlockID(i33 + i28, i35 - 1, i34 + i29);
                                                    if (i36 == 0)
                                                    {
                                                        z37 = false;
                                                    }
                                                    else if (i35 > 0 && i36 > 0 && Tile.tiles[i36].material.color == Color.air)
                                                    {
                                                        z37 = false;
                                                    }

                                                    if (!z37)
                                                    {
                                                        --i35;
                                                        i36 = chunk27.GetBlockID(i33 + i28, i35 - 1, i34 + i29);
                                                    }

                                                    if (z37)
                                                    {
                                                        if (i36 == 0 || !Tile.tiles[i36].material.IsLiquid())
                                                        {
                                                            break;
                                                        }

                                                        i38 = i35 - 1;
                                                        while (true)
                                                        {
                                                            int i43 = chunk27.GetBlockID(i33 + i28, i38--, i34 + i29);
                                                            ++i30;
                                                            if (i38 <= 0 || i43 == 0 || !Tile.tiles[i43].material.IsLiquid())
                                                            {
                                                                //break label164;
                                                                break164 = true;
                                                                break;
                                                            }
                                                        }
                                                        if (break164) 
                                                            break;
                                                    }
                                                }
                                            }

                                            d31 += (double)i35 / (double)(i6 * i6);
                                            ++i26[i36];
                                        }
                                    }
                                }

                                i30 /= i6 * i6;
                                i33 = 0;
                                i34 = 0;
                                for (i35 = 0; i35 < 256; ++i35)
                                {
                                    if (i26[i35] > i33)
                                    {
                                        i34 = i35;
                                        i33 = i26[i35];
                                    }
                                }

                                double d41 = (d31 - d15) * 4 / (i6 + 4) + ((i12 + i17 & 1) - 0.5) * 0.4;
                                byte b42 = 1;
                                if (d41 > 0.6)
                                {
                                    b42 = 2;
                                }

                                if (d41 < -0.6)
                                {
                                    b42 = 0;
                                }

                                i38 = 0;
                                if (i34 > 0)
                                {
                                    Color mapColor44 = Tile.tiles[i34].material.color;
                                    if (mapColor44 == Color.water)
                                    {
                                        d41 = i30 * 0.1 + (i12 + i17 & 1) * 0.2;
                                        b42 = 1;
                                        if (d41 < 0.5)
                                        {
                                            b42 = 2;
                                        }

                                        if (d41 > 0.9)
                                        {
                                            b42 = 0;
                                        }
                                    }

                                    i38 = mapColor44.id;
                                }

                                d15 = d31;
                                if (i17 >= 0 && i18 * i18 + i19 * i19 < i11 * i11 && (!z20 || (i12 + i17 & 1) != 0))
                                {
                                    byte b45 = mapData3.field_f[i12 + i17 * s4];
                                    byte b40 = (byte)(i38 * 4 + b42);
                                    if (b45 != b40)
                                    {
                                        if (i13 > i17)
                                        {
                                            i13 = i17;
                                        }

                                        if (i14 < i17)
                                        {
                                            i14 = i17;
                                        }

                                        mapData3.field_f[i12 + i17 * s4] = b40;
                                    }
                                }
                            }
                        }

                        if (i13 <= i14)
                        {
                            mapData3.Func_28170(i12, i13, i14);
                        }
                    }
                }
            }
        }

        public override void OnUpdate(ItemInstance itemStack1, Level world2, Entity entity3, int i4, bool z5)
        {
            if (!world2.isRemote)
            {
                MapItemSavedData mapData6 = this.GetItemMapData(itemStack1, world2);
                if (entity3 is Player)
                {
                    Player entityPlayer7 = (Player)entity3;
                    mapData6.Func_28155(entityPlayer7, itemStack1);
                }

                if (z5)
                {
                    this.F28011(world2, entity3, mapData6);
                }
            }
        }

        public override void OnCreated(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            itemStack1.SetItemDamage(world2.GetUniqueDataId("map"));
            string string4 = "map_" + itemStack1.GetItemDamage();
            MapItemSavedData mapData5 = new MapItemSavedData(string4);
            world2.SetItemData(string4, mapData5);
            mapData5.field_b = Mth.Floor(entityPlayer3.x);
            mapData5.field_c = Mth.Floor(entityPlayer3.z);
            mapData5.field_e = 3;
            mapData5.field_d = (byte)world2.dimension.dimension;
            mapData5.MarkDirty();
        }

        public override Packet Func_28022_b(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            byte[] b4 = this.GetItemMapData(itemStack1, world2).GetData(itemStack1, world2, entityPlayer3);
            return b4 == null ? null : new Packet131MapData((short)Item.mapItem.id, (short)itemStack1.GetItemDamage(), b4);
        }
    }
}