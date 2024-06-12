using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public abstract class Packet
    {
        private static NullDictionary<int, Type> packetIdToClassMap = new();
        private static NullDictionary<Type, int> packetClassToIdMap = new();
        private static HashSet<int> clientPacketIdList = new HashSet<int>();
        private static HashSet<int> serverPacketIdList = new HashSet<int>();
        public readonly long creationTimeMillis = TimeUtil.MilliTime;
        public bool isChunkDataPacket = false;

        static Packet()
        {
            AddIdClassMapping(0, true, true, typeof(Packet0KeepAlive));
            AddIdClassMapping(1, true, true, typeof(Packet1Login));
            AddIdClassMapping(2, true, true, typeof(Packet2Handshake));
            AddIdClassMapping(3, true, true, typeof(Packet3Chat));
            AddIdClassMapping(4, true, false, typeof(Packet4UpdateTime));
            AddIdClassMapping(5, true, false, typeof(Packet5PlayerInventory));
            AddIdClassMapping(6, true, false, typeof(Packet6SpawnPosition));
            AddIdClassMapping(7, false, true, typeof(Packet7UseEntity));
            AddIdClassMapping(8, true, false, typeof(Packet8UpdateHealth));
            AddIdClassMapping(9, true, true, typeof(Packet9Respawn));
            AddIdClassMapping(10, true, true, typeof(Packet10Flying));
            AddIdClassMapping(11, true, true, typeof(Packet11PlayerPosition));
            AddIdClassMapping(12, true, true, typeof(Packet12PlayerLook));
            AddIdClassMapping(13, true, true, typeof(Packet13PlayerLookMove));
            AddIdClassMapping(14, false, true, typeof(Packet14BlockDig));
            AddIdClassMapping(15, false, true, typeof(Packet15Place));
            AddIdClassMapping(16, false, true, typeof(Packet16BlockItemSwitch));
            AddIdClassMapping(17, true, false, typeof(Packet17Sleep));
            AddIdClassMapping(18, true, true, typeof(Packet18Animation));
            AddIdClassMapping(19, false, true, typeof(Packet19EntityAction));
            AddIdClassMapping(20, true, false, typeof(Packet20NamedEntitySpawn));
            AddIdClassMapping(21, true, false, typeof(Packet21PickupSpawn));
            AddIdClassMapping(22, true, false, typeof(Packet22Collect));
            AddIdClassMapping(23, true, false, typeof(Packet23VehicleSpawn));
            AddIdClassMapping(24, true, false, typeof(Packet24MobSpawn));
            AddIdClassMapping(25, true, false, typeof(Packet25EntityPainting));
            AddIdClassMapping(27, false, true, typeof(Packet27Position));
            AddIdClassMapping(28, true, false, typeof(Packet28EntityVelocity));
            AddIdClassMapping(29, true, false, typeof(Packet29DestroyEntity));
            AddIdClassMapping(30, true, false, typeof(Packet30Entity));
            AddIdClassMapping(31, true, false, typeof(Packet31RelEntityMove));
            AddIdClassMapping(32, true, false, typeof(Packet32EntityLook));
            AddIdClassMapping(33, true, false, typeof(Packet33RelEntityMoveLook));
            AddIdClassMapping(34, true, false, typeof(Packet34EntityTeleport));
            AddIdClassMapping(38, true, false, typeof(Packet38EntityHealth));
            AddIdClassMapping(39, true, false, typeof(Packet39AttachEntity));
            AddIdClassMapping(40, true, false, typeof(Packet40EntityMetadata));
            AddIdClassMapping(50, true, false, typeof(Packet50PreChunk));
            AddIdClassMapping(51, true, false, typeof(Packet51MapChunk));
            AddIdClassMapping(52, true, false, typeof(Packet52MultiBlockChange));
            AddIdClassMapping(53, true, false, typeof(Packet53BlockChange));
            AddIdClassMapping(54, true, false, typeof(Packet54PlayNoteBlock));
            AddIdClassMapping(60, true, false, typeof(Packet60Explosion));
            AddIdClassMapping(61, true, false, typeof(Packet61WorldEvent));
            AddIdClassMapping(70, true, false, typeof(Packet70Bed));
            AddIdClassMapping(71, true, false, typeof(Packet71Weather));
            AddIdClassMapping(100, true, false, typeof(Packet100OpenWindow));
            AddIdClassMapping(101, true, true, typeof(Packet101CloseWindow));
            AddIdClassMapping(102, false, true, typeof(Packet102WindowClick));
            AddIdClassMapping(103, true, false, typeof(Packet103SetSlot));
            AddIdClassMapping(104, true, false, typeof(Packet104WindowItems));
            AddIdClassMapping(105, true, false, typeof(Packet105UpdateProgressbar));
            AddIdClassMapping(106, true, true, typeof(Packet106Transaction));
            AddIdClassMapping(130, true, true, typeof(Packet130UpdateSign));
            AddIdClassMapping(131, true, false, typeof(Packet131MapData));
            AddIdClassMapping(200, true, false, typeof(Packet200Statistic));
            AddIdClassMapping(255, true, true, typeof(Packet255KickDisconnect));
        }

        private static void AddIdClassMapping(int i0, bool clnt, bool srvr, Type class3)
        {
            if (packetIdToClassMap.ContainsKey(i0))
            {
                throw new ArgumentException("Duplicate packet id:" + i0);
            }
            else if (packetClassToIdMap.ContainsKey(class3))
            {
                throw new ArgumentException("Duplicate packet class:" + class3);
            }
            else
            {
                packetIdToClassMap[i0] = class3;
                packetClassToIdMap[class3] = i0;
                if (clnt)
                {
                    clientPacketIdList.Add(i0);
                }

                if (srvr)
                {
                    serverPacketIdList.Add(i0);
                }
            }
        }

        public static Packet GetNewPacket(int i0)
        {
            try
            {
                Type cls = packetIdToClassMap[i0];
                if (cls == null)
                {
                    return null;
                }
                Packet pack = (Packet)Activator.CreateInstance(cls);
                return pack;
            }
            catch (Exception exception2)
            {
                exception2.PrintStackTrace();
                Console.WriteLine("Skipping packet with id " + i0);
                return null;
            }
        }

        public int GetId()
        {
            return packetClassToIdMap[this.GetType()];
        }

        public static Packet ReadPacket(BinaryReader dataInputStream0, bool z1)
        {
            Packet packet3;
            int id;
            try
            {
                try
                {
                    id = dataInputStream0.ReadByte();
                }
                catch (EndOfStreamException) //separate catch here so we return early
                {
                    return null;
                }

                if (z1 && !serverPacketIdList.Contains(id) || !z1 && !clientPacketIdList.Contains(id))
                {
                    throw new IOException("Bad packet id " + id);
                }

                packet3 = GetNewPacket(id);
                if (packet3 == null)
                {
                    throw new IOException("Bad packet id " + id);
                }

                packet3.Read(dataInputStream0);
            }
            catch (EndOfStreamException)
            {
                Console.Error.WriteLine("Reached end of stream");
                return null;
            }

            return packet3;
        }

        public static void WritePacket(Packet packet0, BinaryWriter dos)
        {
            dos.Write((byte)packet0.GetId());
            packet0.Write(dos);
        }

        public abstract void Read(BinaryReader stream);

        public abstract void Write(BinaryWriter stream);

        public abstract void Handle(PacketListener netHandler);

        public abstract int Size();
    }
}