using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Levell.Chonk
{
    public class ServerChunkDistance
    {
        public IList<ServerPlayer> players = new List<ServerPlayer>();
        private LongHashMap<LoadedChunk> loadedChunks = new LongHashMap<LoadedChunk>();
        private IList<LoadedChunk> chunksToUpdate = new List<LoadedChunk>();
        private Server mcServer;
        private int dimension;
        private int playerViewRadius;
        private static readonly int[][] facing = 
        {
            new[]
            {
                1,
                0
            },
            new[]
            {
                0,
                1
            },
            new[]
            {
                -1,
                0
            },
            new[]
            {
                0,
                -1
            }
        };

        public ServerChunkDistance(Server server, int dimension, int radius)
        {
            if (radius > 15)
            {
                throw new ArgumentException("Too big view radius!");
            }
            else if (radius < 3)
            {
                throw new ArgumentException("Too small view radius!");
            }
            else
            {
                this.mcServer = server;
                this.playerViewRadius = radius;
                this.dimension = dimension;
            }
        }

        public virtual ServerLevel GetWorld()
        {
            return this.mcServer.GetLevel(this.dimension);
        }

        public virtual void Tick()
        {
            for (int i1 = 0; i1 < this.chunksToUpdate.Count; ++i1)
            {
                this.chunksToUpdate[i1].Tick();
            }

            this.chunksToUpdate.Clear();
            if (this.players.Count == 0)
            {
                ServerLevel level = this.mcServer.GetLevel(this.dimension);
                Dimension dim = level.dimension;
                if (!dim.CanRespawnHere())
                {
                    level.serverChunkCache.UnloadAllChunks();
                }
            }
        }

        private LoadedChunk GetChunk(int i1, int i2, bool z3)
        {
            long j4 = i1 + 2147483647L | i2 + 2147483647L << 32;
            LoadedChunk playerInstance6 = this.loadedChunks.Get(j4);
            if (playerInstance6 == null && z3)
            {
                playerInstance6 = new LoadedChunk(this, i1, i2);
                this.loadedChunks.Put(j4, playerInstance6);
            }

            return playerInstance6;
        }

        public virtual void MarkBlockNeedsUpdate(int i1, int i2, int i3)
        {
            int i4 = i1 >> 4;
            int i5 = i3 >> 4;
            LoadedChunk playerInstance6 = this.GetChunk(i4, i5, false);
            if (playerInstance6 != null)
            {
                playerInstance6.MarkBlockNeedsUpdate(i1 & 15, i2, i3 & 15);
            }
        }

        public virtual void AddPlayer(ServerPlayer entityPlayerMP1)
        {
            int i2 = (int)entityPlayerMP1.x >> 4;
            int i3 = (int)entityPlayerMP1.z >> 4;
            entityPlayerMP1.field_9155_d = entityPlayerMP1.x;
            entityPlayerMP1.field_9154_e = entityPlayerMP1.z;
            int i4 = 0;
            int i5 = this.playerViewRadius;
            int i6 = 0;
            int i7 = 0;
            this.GetChunk(i2, i3, true).AddPlayer(entityPlayerMP1);
            int i8;
            for (i8 = 1; i8 <= i5 * 2; ++i8)
            {
                for (int i9 = 0; i9 < 2; ++i9)
                {
                    int[] idx = facing[i4++ % 4];
                    for (int i11 = 0; i11 < i8; ++i11)
                    {
                        i6 += idx[0];
                        i7 += idx[1];
                        this.GetChunk(i2 + i6, i3 + i7, true).AddPlayer(entityPlayerMP1);
                    }
                }
            }

            i4 %= 4;
            for (i8 = 0; i8 < i5 * 2; ++i8)
            {
                i6 += facing[i4][0];
                i7 += facing[i4][1];
                this.GetChunk(i2 + i6, i3 + i7, true).AddPlayer(entityPlayerMP1);
            }

            this.players.Add(entityPlayerMP1);
        }

        public virtual void RemovePlayer(ServerPlayer entityPlayerMP1)
        {
            int i2 = (int)entityPlayerMP1.field_9155_d >> 4;
            int i3 = (int)entityPlayerMP1.field_9154_e >> 4;
            for (int i4 = i2 - this.playerViewRadius; i4 <= i2 + this.playerViewRadius; ++i4)
            {
                for (int i5 = i3 - this.playerViewRadius; i5 <= i3 + this.playerViewRadius; ++i5)
                {
                    LoadedChunk playerInstance6 = this.GetChunk(i4, i5, false);
                    if (playerInstance6 != null)
                    {
                        playerInstance6.RemovePlayer(entityPlayerMP1);
                    }
                }
            }

            this.players.Remove(entityPlayerMP1);
        }

        private bool Func_544_a(int i1, int i2, int i3, int i4)
        {
            int i5 = i1 - i3;
            int i6 = i2 - i4;
            return i5 >= -this.playerViewRadius && i5 <= this.playerViewRadius ? i6 >= -this.playerViewRadius && i6 <= this.playerViewRadius : false;
        }

        public virtual void Func_543_c(ServerPlayer entityPlayerMP1)
        {
            int i2 = (int)entityPlayerMP1.x >> 4;
            int i3 = (int)entityPlayerMP1.z >> 4;
            double d4 = entityPlayerMP1.field_9155_d - entityPlayerMP1.x;
            double d6 = entityPlayerMP1.field_9154_e - entityPlayerMP1.z;
            double d8 = d4 * d4 + d6 * d6;
            if (d8 >= 64d)
            {
                int i10 = (int)entityPlayerMP1.field_9155_d >> 4;
                int i11 = (int)entityPlayerMP1.field_9154_e >> 4;
                int i12 = i2 - i10;
                int i13 = i3 - i11;
                if (i12 != 0 || i13 != 0)
                {
                    for (int i14 = i2 - this.playerViewRadius; i14 <= i2 + this.playerViewRadius; ++i14)
                    {
                        for (int i15 = i3 - this.playerViewRadius; i15 <= i3 + this.playerViewRadius; ++i15)
                        {
                            if (!this.Func_544_a(i14, i15, i10, i11))
                            {
                                this.GetChunk(i14, i15, true).AddPlayer(entityPlayerMP1);
                            }

                            if (!this.Func_544_a(i14 - i12, i15 - i13, i2, i3))
                            {
                                LoadedChunk playerInstance16 = this.GetChunk(i14 - i12, i15 - i13, false);
                                if (playerInstance16 != null)
                                {
                                    playerInstance16.RemovePlayer(entityPlayerMP1);
                                }
                            }
                        }
                    }

                    entityPlayerMP1.field_9155_d = entityPlayerMP1.x;
                    entityPlayerMP1.field_9154_e = entityPlayerMP1.z;
                }
            }
        }

        public virtual int GetMaxTrackingDistance()
        {
            return this.playerViewRadius * 16 - 16;
        }

        private class LoadedChunk
        {
            private readonly ServerChunkDistance SELF;
            private IList<ServerPlayer> players;
            private int chunkX;
            private int chunkZ;
            private ChunkPos currentChunk;
            private short[] blocksToUpdate;
            private int numBlocksToUpdate;
            private int minX;
            private int maxX;
            private int minY;
            private int maxY;
            private int minZ;
            private int maxZ;
            public LoadedChunk(ServerChunkDistance scd, int i2, int i3)
            {
                this.SELF = scd;
                this.players = new List<ServerPlayer>();
                this.blocksToUpdate = new short[10];
                this.numBlocksToUpdate = 0;
                this.chunkX = i2;
                this.chunkZ = i3;
                this.currentChunk = new ChunkPos(i2, i3);
                this.SELF.GetWorld().serverChunkCache.Create(i2, i3);
            }

            public virtual void AddPlayer(ServerPlayer player)
            {
                if (this.players.Contains(player))
                {
                    throw new Exception("Failed to add player. " + player + " already is in chunk " + this.chunkX + ", " + this.chunkZ);
                }
                else
                {
                    player.chunkPositions.Add(this.currentChunk);
                    player.playerNetServerHandler.SendPacket(new Packet50PreChunk(this.currentChunk.x, this.currentChunk.z, true));
                    this.players.Add(player);
                    player.loadedChunks.Add(this.currentChunk);
                }
            }

            public virtual void RemovePlayer(ServerPlayer entityPlayerMP1)
            {
                if (this.players.Contains(entityPlayerMP1))
                {
                    this.players.Remove(entityPlayerMP1);
                    if (this.players.Count == 0)
                    {
                        long j2 = this.chunkX + 2147483647L | this.chunkZ + 2147483647L << 32;
                        this.SELF.loadedChunks.Remove(j2);
                        if (this.numBlocksToUpdate > 0)
                        {
                            this.SELF.chunksToUpdate.Remove(this);
                        }

                        this.SELF.GetWorld().serverChunkCache.DropChunk(this.chunkX, this.chunkZ);
                    }

                    entityPlayerMP1.loadedChunks.Remove(this.currentChunk);
                    if (entityPlayerMP1.chunkPositions.Contains(this.currentChunk))
                    {
                        entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet50PreChunk(this.chunkX, this.chunkZ, false));
                    }
                }
            }

            public virtual void MarkBlockNeedsUpdate(int x0, int y0, int z0)
            {
                if (this.numBlocksToUpdate == 0)
                {
                    this.SELF.chunksToUpdate.Add(this);
                    this.minX = this.maxX = x0;
                    this.minY = this.maxY = y0;
                    this.minZ = this.maxZ = z0;
                }

                if (this.minX > x0)
                {
                    this.minX = x0;
                }

                if (this.maxX < x0)
                {
                    this.maxX = x0;
                }

                if (this.minY > y0)
                {
                    this.minY = y0;
                }

                if (this.maxY < y0)
                {
                    this.maxY = y0;
                }

                if (this.minZ > z0)
                {
                    this.minZ = z0;
                }

                if (this.maxZ < z0)
                {
                    this.maxZ = z0;
                }

                if (this.numBlocksToUpdate < 10)
                {
                    short shart = (short)(x0 << 12 | z0 << 8 | y0);
                    for (int i = 0; i < this.numBlocksToUpdate; ++i)
                    {
                        if (this.blocksToUpdate[i] == shart)
                        {
                            return;
                        }
                    }

                    this.blocksToUpdate[this.numBlocksToUpdate++] = shart;
                }
            }

            public virtual void SendPacketToPlayersInInstance(Packet packet1)
            {
                for (int i2 = 0; i2 < this.players.Count; ++i2)
                {
                    ServerPlayer entityPlayerMP3 = this.players[i2];
                    if (entityPlayerMP3.chunkPositions.Contains(this.currentChunk))
                    {
                        entityPlayerMP3.playerNetServerHandler.SendPacket(packet1);
                    }
                }
            }

            public virtual void Tick()
            {
                ServerLevel worldServer1 = this.SELF.GetWorld();
                if (this.numBlocksToUpdate != 0)
                {
                    int i2;
                    int i3;
                    int i4;
                    if (this.numBlocksToUpdate == 1)
                    {
                        i2 = this.chunkX * 16 + this.minX;
                        i3 = this.minY;
                        i4 = this.chunkZ * 16 + this.minZ;
                        this.SendPacketToPlayersInInstance(new Packet53BlockChange(i2, i3, i4, worldServer1));
                        if (Tile.isEntityTile[worldServer1.GetTile(i2, i3, i4)])
                        {
                            this.UpdateTileEntity(worldServer1.GetTileEntity(i2, i3, i4));
                        }
                    }
                    else
                    {
                        int i5;
                        if (this.numBlocksToUpdate == 10)
                        {
                            this.minY = this.minY / 2 * 2;
                            this.maxY = (this.maxY / 2 + 1) * 2;
                            i2 = this.minX + this.chunkX * 16;
                            i3 = this.minY;
                            i4 = this.minZ + this.chunkZ * 16;
                            i5 = this.maxX - this.minX + 1;
                            int i6 = this.maxY - this.minY + 2;
                            int i7 = this.maxZ - this.minZ + 1;
                            this.SendPacketToPlayersInInstance(new Packet51MapChunk(i2, i3, i4, i5, i6, i7, worldServer1));
                            IList<TileEntity> list8 = worldServer1.GetTileEntityList(i2, i3, i4, i2 + i5, i3 + i6, i4 + i7);
                            for (int i9 = 0; i9 < list8.Count; ++i9)
                            {
                                this.UpdateTileEntity(list8[i9]);
                            }
                        }
                        else
                        {
                            this.SendPacketToPlayersInInstance(new Packet52MultiBlockChange(this.chunkX, this.chunkZ, this.blocksToUpdate, this.numBlocksToUpdate, worldServer1));
                            for (i2 = 0; i2 < this.numBlocksToUpdate; ++i2)
                            {
                                i3 = this.chunkX * 16 + (this.numBlocksToUpdate >> 12 & 15);
                                i4 = this.numBlocksToUpdate & 255;
                                i5 = this.chunkZ * 16 + (this.numBlocksToUpdate >> 8 & 15);
                                if (Tile.isEntityTile[worldServer1.GetTile(i3, i4, i5)])
                                {
                                    Console.WriteLine("Sending!");
                                    this.UpdateTileEntity(worldServer1.GetTileEntity(i3, i4, i5));
                                }
                            }
                        }
                    }

                    this.numBlocksToUpdate = 0;
                }
            }

            private void UpdateTileEntity(TileEntity tileEntity1)
            {
                if (tileEntity1 != null)
                {
                    Packet packet2 = tileEntity1.GetDescriptionPacket();
                    if (packet2 != null)
                    {
                        this.SendPacketToPlayersInInstance(packet2);
                    }
                }
            }
        }
    }
}
