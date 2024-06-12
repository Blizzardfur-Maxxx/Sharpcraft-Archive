using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet21PickupSpawn : Packet
    {
        public int entityId;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public sbyte rotation;
        public sbyte pitch;
        public sbyte roll;
        public int itemID;
        public int count;
        public int itemDamage;

        public Packet21PickupSpawn()
        {
        }

        public Packet21PickupSpawn(ItemEntity item)
        {
            entityId = item.entityID;
            itemID = item.item.itemID;
            count = item.item.stackSize;
            itemDamage = item.item.GetItemDamage();
            xPosition = Mth.Floor(item.x * 32d);
            yPosition = Mth.Floor(item.y * 32d);
            zPosition = Mth.Floor(item.z * 32d);
            rotation = (sbyte)((int)(item.motionX * 128d));
            pitch = (sbyte)((int)(item.motionY * 128d));
            roll = (sbyte)((int)(item.motionZ * 128d));
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
            itemID = reader.ReadBEShort();
            count = reader.ReadSByte();
            itemDamage = reader.ReadBEShort();
            xPosition = reader.ReadBEInt();
            yPosition = reader.ReadBEInt();
            zPosition = reader.ReadBEInt();
            rotation = reader.ReadSByte();
            pitch = reader.ReadSByte();
            roll = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
            writer.WriteBEShort((short)itemID);
            writer.Write((byte)count);
            writer.WriteBEShort((short)itemDamage);
            writer.WriteBEInt(xPosition);
            writer.WriteBEInt(yPosition);
            writer.WriteBEInt(zPosition);
            writer.Write((byte)rotation);
            writer.Write((byte)pitch);
            writer.Write((byte)roll);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandlePickupSpawn(this);
        }

        public override int Size()
        {
            return 24;
        }
    }
}