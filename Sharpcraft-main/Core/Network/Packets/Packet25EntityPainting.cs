using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet25EntityPainting : Packet
    {
        public int entityId;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public int direction;
        public string title;
        public Packet25EntityPainting()
        {
        }

        public Packet25EntityPainting(Painting entityPainting1)
        {
            this.entityId = entityPainting1.entityID;
            this.xPosition = entityPainting1.xPosition;
            this.yPosition = entityPainting1.yPosition;
            this.zPosition = entityPainting1.zPosition;
            this.direction = entityPainting1.direction;
            this.title = entityPainting1.art.title;
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.title = reader.ReadUTF16BE(Motive.maxArtTitleLength);
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadBEInt();
            this.zPosition = reader.ReadBEInt();
            this.direction = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.WriteUTF16BE(this.title);
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEInt(this.yPosition);
            writer.WriteBEInt(this.zPosition);
            writer.WriteBEInt(this.direction);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandlePainting(this);
        }

        public override int Size()
        {
            return 24;
        }
    }
}