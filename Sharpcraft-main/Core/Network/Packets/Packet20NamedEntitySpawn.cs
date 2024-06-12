using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet20NamedEntitySpawn : Packet
    {
        public int entityId;
        public string name;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public sbyte rotation;
        public sbyte pitch;
        public int currentItem;

        public Packet20NamedEntitySpawn()
        {
        }

        public Packet20NamedEntitySpawn(Player entityPlayer1)
        {
            entityId = entityPlayer1.entityID;
            name = entityPlayer1.username;
            xPosition = Mth.Floor(entityPlayer1.x * 32d);
            yPosition = Mth.Floor(entityPlayer1.y * 32d);
            zPosition = Mth.Floor(entityPlayer1.z * 32d);
            rotation = (sbyte)((int)(entityPlayer1.yaw * 256F / 360F));
            pitch = (sbyte)((int)(entityPlayer1.pitch * 256F / 360F));
            ItemInstance itemStack2 = entityPlayer1.inventory.GetCurrentItem();
            currentItem = itemStack2 == null ? 0 : itemStack2.itemID;
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
            name = reader.ReadUTF16BE(16);
            xPosition = reader.ReadBEInt();
            yPosition = reader.ReadBEInt();
            zPosition = reader.ReadBEInt();
            rotation = reader.ReadSByte();
            pitch = reader.ReadSByte();
            currentItem = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
            writer.WriteUTF16BE(name);
            writer.WriteBEInt(xPosition);
            writer.WriteBEInt(yPosition);
            writer.WriteBEInt(zPosition);
            writer.Write(rotation);
            writer.Write(pitch);
            writer.WriteBEShort((short)currentItem);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleNamedEntitySpawn(this);
        }

        public override int Size()
        {
            return 28;
        }
    }
}