using SharpCraft.Core.World.Entities.Players;

namespace SharpCraft.Core.World.GameLevel
{
    public interface IPlayerIO
    {
        void Write(Player entityPlayer1);
        void Read(Player entityPlayer1);
    }
}