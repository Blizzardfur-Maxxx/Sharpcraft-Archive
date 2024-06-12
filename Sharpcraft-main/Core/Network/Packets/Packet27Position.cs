using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet27Position : Packet
    {
        private float strafe;
        private float forward;
        private bool field_c;
        private bool jump;
        private float pitchRot;
        private float yawRot;
        public override void Read(BinaryReader reader)
        {
            this.strafe = reader.ReadBEFloat();
            this.forward = reader.ReadBEFloat();
            this.pitchRot = reader.ReadBEFloat();
            this.yawRot = reader.ReadBEFloat();
            this.field_c = reader.ReadBoolean();
            this.jump = reader.ReadBoolean();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEFloat(this.strafe);
            writer.WriteBEFloat(this.forward);
            writer.WriteBEFloat(this.pitchRot);
            writer.WriteBEFloat(this.yawRot);
            writer.Write(this.field_c);
            writer.Write(this.jump);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleMovementType(this);
        }

        public override int Size()
        {
            return 18;
        }

        public virtual float GetStrafe()
        {
            return this.strafe;
        }

        public virtual float GetPitch()
        {
            return this.pitchRot;
        }

        public virtual float GetForward()
        {
            return this.forward;
        }

        public virtual float GetYaw()
        {
            return this.yawRot;
        }

        public virtual bool Func_g()
        {
            return this.field_c;
        }

        public virtual bool GetJump()
        {
            return this.jump;
        }
    }
}