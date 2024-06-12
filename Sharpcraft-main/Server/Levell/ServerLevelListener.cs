using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Levell
{
    public class ServerLevelListener : ILevelListener
    {
        private Server server;
        private ServerLevel level;
        public ServerLevelListener(Server srv, ServerLevel worldServer2)
        {
            this.server = srv;
            this.level = worldServer2;
        }

        public virtual void AddParticle(string string1, double d2, double d4, double d6, double d8, double d10, double d12)
        {
        }

        public virtual void EntityAdded(Entity entity1)
        {
            this.server.GetEntityTracker(this.level.dimension.dimension).TrackEntity(entity1);
        }

        public virtual void EntityRemoved(Entity entity1)
        {
            this.server.GetEntityTracker(this.level.dimension.dimension).UntrackEntity(entity1);
        }

        public virtual void PlaySound(string string1, double d2, double d4, double d6, float f8, float f9)
        {
        }

        public virtual void SetTilesDirty(int i1, int i2, int i3, int i4, int i5, int i6)
        {
        }

        public virtual void AllChanged()
        {
        }

        public virtual void TileChanged(int i1, int i2, int i3)
        {
            this.server.configManager.MarkBlockNeedsUpdate(i1, i2, i3, this.level.dimension.dimension);
        }

        public virtual void PlayStreamingMusic(string string1, int i2, int i3, int i4)
        {
        }

        public virtual void TileEntityChanged(int i1, int i2, int i3, TileEntity tileEntity4)
        {
            this.server.configManager.SendTileEntityToPlayer(i1, i2, i3, tileEntity4);
        }

        public virtual void LevelEvent(Player entityPlayer1, LevelEventType i2, int i3, int i4, int i5, int i6)
        {
            this.server.configManager.SendPacketToPlayersAroundPoint(entityPlayer1, i3, i4, i5, 64, this.level.dimension.dimension, new Packet61WorldEvent(i2, i3, i4, i5, i6));
        }
    }
}
