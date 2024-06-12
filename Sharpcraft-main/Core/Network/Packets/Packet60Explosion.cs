using SharpCraft.Core.World.GameLevel;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet60Explosion : Packet
    {
        public double explosionX;
        public double explosionY;
        public double explosionZ;
        public float explosionSize;
        public HashSet<TilePos> destroyedBlockPositions;

        public Packet60Explosion()
        {
        }

        public Packet60Explosion(double d1, double d3, double d5, float f7, HashSet<TilePos> set8)
        {
            this.explosionX = d1;
            this.explosionY = d3;
            this.explosionZ = d5;
            this.explosionSize = f7;
            this.destroyedBlockPositions = new HashSet<TilePos>(set8);
        }

        public override void Read(BinaryReader reader)
        {
            this.explosionX = reader.ReadBEDouble();
            this.explosionY = reader.ReadBEDouble();
            this.explosionZ = reader.ReadBEDouble();
            this.explosionSize = reader.ReadBEFloat();
            int i2 = reader.ReadBEInt();
            this.destroyedBlockPositions = new HashSet<TilePos>();
            int i3 = (int)this.explosionX;
            int i4 = (int)this.explosionY;
            int i5 = (int)this.explosionZ;
            for (int i6 = 0; i6 < i2; ++i6)
            {
                int i7 = reader.ReadSByte() + i3;
                int i8 = reader.ReadSByte() + i4;
                int i9 = reader.ReadSByte() + i5;
                this.destroyedBlockPositions.Add(new TilePos(i7, i8, i9));
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEDouble(this.explosionX);
            writer.WriteBEDouble(this.explosionY);
            writer.WriteBEDouble(this.explosionZ);
            writer.WriteBEFloat(this.explosionSize);
            writer.WriteBEInt(this.destroyedBlockPositions.Count);
            int i2 = (int)this.explosionX;
            int i3 = (int)this.explosionY;
            int i4 = (int)this.explosionZ;

            foreach (TilePos pos in destroyedBlockPositions) 
            {
                int i7 = pos.x - i2;
                int i8 = pos.y - i3;
                int i9 = pos.z - i4;
                writer.Write((sbyte)i7);
                writer.Write((sbyte)i8);
                writer.Write((sbyte)i9);
            }
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleExplosion(this);
        }

        public override int Size()
        {
            return 32 + this.destroyedBlockPositions.Count * 3;
        }
    }
}