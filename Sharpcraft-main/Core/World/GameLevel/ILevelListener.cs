using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.GameLevel
{
    public interface ILevelListener
    {
        void TileChanged(int i1, int i2, int i3);
        void SetTilesDirty(int i1, int i2, int i3, int i4, int i5, int i6);
        void PlaySound(string string1, double d2, double d4, double d6, float f8, float f9);
        void AddParticle(string string1, double d2, double d4, double d6, double d8, double d10, double d12);
        void EntityAdded(Entity entity1);
        void EntityRemoved(Entity entity1);
        void AllChanged();
        void PlayStreamingMusic(string string1, int i2, int i3, int i4);
        void TileEntityChanged(int i1, int i2, int i3, TileEntity tileEntity4);
        void LevelEvent(Player player, LevelEventType type, int x, int y, int z, int data);
    }
}