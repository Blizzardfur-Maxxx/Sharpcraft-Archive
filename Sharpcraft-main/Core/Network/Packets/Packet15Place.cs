using SharpCraft.Core.World.Items;
using System.IO;
using static SharpCraft.Core.Util.Facing;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet15Place : Packet
    {
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public TileFace direction;
        public ItemInstance itemStack;
        public Packet15Place()
        {
        }

        public Packet15Place(int i1, int i2, int i3, TileFace i4, ItemInstance itemStack5)
        {
            this.xPosition = i1;
            this.yPosition = i2;
            this.zPosition = i3;
            this.direction = i4;
            this.itemStack = itemStack5;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadByte();
            this.zPosition = reader.ReadBEInt();
            this.direction = (TileFace)reader.ReadByte();
            short s2 = reader.ReadBEShort();
            if (s2 >= 0)
            {
                byte b3 = reader.ReadByte();
                short s4 = reader.ReadBEShort();
                this.itemStack = new ItemInstance(s2, b3, s4);
            }
            else
            {
                this.itemStack = null;
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.Write((byte)this.yPosition);
            writer.WriteBEInt(this.zPosition);
            writer.Write((byte)this.direction);
            if (this.itemStack == null)
            {
                writer.WriteBEShort(-1);
            }
            else
            {
                writer.WriteBEShort((short)this.itemStack.itemID);
                writer.Write((byte)this.itemStack.stackSize);
                writer.WriteBEShort((short)this.itemStack.GetItemDamage());
            }
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandlePlace(this);
        }

        public override int Size()
        {
            return 15;
        }
    }
}