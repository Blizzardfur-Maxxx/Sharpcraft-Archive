using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.GameSavedData.maps
{
    public class MapItemSavedData : SavedData
    {
        public int field_b;
        public int field_c;
        public byte field_d;
        public byte field_e;
        public byte[] field_f = new byte[16384];
        public int field_g;
        public IList<HoldingPlayer> field_h = new List<HoldingPlayer>();
        private NullDictionary<Player, HoldingPlayer> field_j = new NullDictionary<Player, HoldingPlayer>();
        public IList<MapDecoration> mapDecorations = new List<MapDecoration>();
        public class MapDecoration
        {
            public byte type;
            public byte x;
            public byte y;
            public byte rot;
            public MapDecoration(byte b2, byte b3, byte b4, byte b5)
            {
                this.type = b2;
                this.x = b3;
                this.y = b4;
                this.rot = b5;
            }
        }

        public class HoldingPlayer
        {
            public readonly Player player;
            public int[] field_b;
            public int[] field_c;
            private int field_e;
            private int field_f;
            private byte[] field_g;
            private readonly MapItemSavedData savedData;
            public HoldingPlayer(Player entityPlayer2, MapItemSavedData mapItemSavedData)
            {
                this.field_b = new int[128];
                this.field_c = new int[128];
                this.field_e = 0;
                this.field_f = 0;
                this.player = entityPlayer2;
                for (int i3 = 0; i3 < this.field_b.Length; ++i3)
                {
                    this.field_b[i3] = 0;
                    this.field_c[i3] = 127;
                }
                this.savedData = mapItemSavedData;
            }

            public virtual byte[] Func_28118_a(ItemInstance itemStack1)
            {
                int i3;
                int i10;
                if (--this.field_f < 0)
                {
                    this.field_f = 4;
                    byte[] b2 = new byte[savedData.mapDecorations.Count * 3 + 1];
                    b2[0] = 1;
                    for (i3 = 0; i3 < savedData.mapDecorations.Count; ++i3)
                    {
                        MapDecoration mapCoord4 = savedData.mapDecorations[i3];
                        b2[i3 * 3 + 1] = (byte)(mapCoord4.type + (mapCoord4.rot & 15) * 16);
                        b2[i3 * 3 + 2] = mapCoord4.x;
                        b2[i3 * 3 + 3] = mapCoord4.y;
                    }

                    bool z9 = true;
                    if (this.field_g != null && this.field_g.Length == b2.Length)
                    {
                        for (i10 = 0; i10 < b2.Length; ++i10)
                        {
                            if (b2[i10] != this.field_g[i10])
                            {
                                z9 = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        z9 = false;
                    }

                    if (!z9)
                    {
                        this.field_g = b2;
                        return b2;
                    }
                }

                for (int i8 = 0; i8 < 10; ++i8)
                {
                    i3 = this.field_e * 11 % 128;
                    ++this.field_e;
                    if (this.field_b[i3] >= 0)
                    {
                        i10 = this.field_c[i3] - this.field_b[i3] + 1;
                        int i5 = this.field_b[i3];
                        byte[] b6 = new byte[i10 + 3];
                        b6[0] = 0;
                        b6[1] = (byte)i3;
                        b6[2] = (byte)i5;
                        for (int i7 = 0; i7 < b6.Length - 3; ++i7)
                        {
                            b6[i7 + 3] = savedData.field_f[(i7 + i5) * 128 + i3];
                        }

                        this.field_c[i3] = -1;
                        this.field_b[i3] = -1;
                        return b6;
                    }
                }

                return null;
            }
        }

        public MapItemSavedData(string string1) : base(string1)
        {
        }

        public override void Read(CompoundTag nBTTagCompound1)
        {
            this.field_d = nBTTagCompound1.GetByte("dimension");
            this.field_b = nBTTagCompound1.GetInteger("xCenter");
            this.field_c = nBTTagCompound1.GetInteger("zCenter");
            this.field_e = nBTTagCompound1.GetByte("scale");
            if (this.field_e < 0)
            {
                this.field_e = 0;
            }

            if (this.field_e > 4)
            {
                this.field_e = 4;
            }

            short s2 = nBTTagCompound1.GetShort("width");
            short s3 = nBTTagCompound1.GetShort("height");
            if (s2 == 128 && s3 == 128)
            {
                this.field_f = nBTTagCompound1.GetByteArray("colors");
            }
            else
            {
                byte[] b4 = nBTTagCompound1.GetByteArray("colors");
                this.field_f = new byte[16384];
                int i5 = (128 - s2) / 2;
                int i6 = (128 - s3) / 2;
                for (int i7 = 0; i7 < s3; ++i7)
                {
                    int i8 = i7 + i6;
                    if (i8 >= 0 || i8 < 128)
                    {
                        for (int i9 = 0; i9 < s2; ++i9)
                        {
                            int i10 = i9 + i5;
                            if (i10 >= 0 || i10 < 128)
                            {
                                this.field_f[i10 + i8 * 128] = b4[i9 + i7 * s2];
                            }
                        }
                    }
                }
            }
        }

        public override void Write(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetByte("dimension", this.field_d);
            nBTTagCompound1.SetInteger("xCenter", this.field_b);
            nBTTagCompound1.SetInteger("zCenter", this.field_c);
            nBTTagCompound1.SetByte("scale", this.field_e);
            nBTTagCompound1.SetShort("width", (short)128);
            nBTTagCompound1.SetShort("height", (short)128);
            nBTTagCompound1.SetByteArray("colors", this.field_f);
        }

        public virtual void Func_28155(Player entityPlayer1, ItemInstance itemStack2)
        {
            if (!this.field_j.ContainsKey(entityPlayer1))
            {
                HoldingPlayer mapInfo3 = new HoldingPlayer(entityPlayer1, this);
                this.field_j[entityPlayer1] = mapInfo3;
                this.field_h.Add(mapInfo3);
            }

            this.mapDecorations.Clear();
            for (int i14 = 0; i14 < this.field_h.Count; ++i14)
            {
                HoldingPlayer mapInfo4 = this.field_h[i14];
                if (!mapInfo4.player.isDead && mapInfo4.player.inventory.ContainsEqual(itemStack2))
                {
                    float f5 = (float)(mapInfo4.player.x - this.field_b) / (1 << this.field_e);
                    float f6 = (float)(mapInfo4.player.z - this.field_c) / (1 << this.field_e);
                    byte b7 = 64;
                    byte b8 = 64;
                    if (f5 >= (-b7) && f6 >= (-b8) && f5 <= b7 && f6 <= b8)
                    {
                        byte b9 = 0;
                        byte b10 = (byte)((int)(f5 * 2F + 0.5));
                        byte b11 = (byte)((int)(f6 * 2F + 0.5));
                        byte b12 = (byte)((int)(entityPlayer1.yaw * 16F / 360F + 0.5));
                        if (this.field_d < 0)
                        {
                            int i13 = this.field_g / 10;
                            b12 = (byte)(i13 * i13 * 34187121 + i13 * 121 >> 15 & 15);
                        }

                        if (mapInfo4.player.dimension == this.field_d)
                        {
                            this.mapDecorations.Add(new MapDecoration(b9, b10, b11, b12));
                        }
                    }
                }
                else
                {
                    this.field_j.Remove(mapInfo4.player);
                    this.field_h.Remove(mapInfo4);
                }
            }
        }

        public virtual void SetData(byte[] b1)
        {
            int i2;
            if (b1[0] == 0)
            {
                i2 = b1[1] & 255;
                int i3 = b1[2] & 255;
                for (int i4 = 0; i4 < b1.Length - 3; ++i4)
                {
                    this.field_f[(i4 + i3) * 128 + i2] = b1[i4 + 3];
                }

                this.MarkDirty();
            }
            else if (b1[0] == 1)
            {
                this.mapDecorations.Clear();
                for (i2 = 0; i2 < (b1.Length - 1) / 3; ++i2)
                {
                    byte b7 = (byte)(b1[i2 * 3 + 1] % 16);
                    byte b8 = b1[i2 * 3 + 2];
                    byte b5 = b1[i2 * 3 + 3];
                    byte b6 = (byte)(b1[i2 * 3 + 1] / 16);
                    this.mapDecorations.Add(new MapDecoration(b7, b8, b5, b6));
                }
            }
        }

        public virtual byte[] GetData(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            HoldingPlayer mapInfo4 = this.field_j[entityPlayer3];
            if (mapInfo4 == null)
            {
                return null;
            }
            else
            {
                byte[] b5 = mapInfo4.Func_28118_a(itemStack1);
                return b5;
            }
        }

        public virtual void Func_28170(int i1, int i2, int i3)
        {
            base.MarkDirty();
            for (int i4 = 0; i4 < this.field_h.Count; ++i4)
            {
                HoldingPlayer mapInfo5 = this.field_h[i4];
                if (mapInfo5.field_b[i1] < 0 || mapInfo5.field_b[i1] > i2)
                {
                    mapInfo5.field_b[i1] = i2;
                }

                if (mapInfo5.field_c[i1] < 0 || mapInfo5.field_c[i1] < i3)
                {
                    mapInfo5.field_c[i1] = i3;
                }
            }
        }
    }
}