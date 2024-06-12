

using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World
{
    public interface IContainer
    {
        int GetContainerSize();
        ItemInstance GetItem(int i1);
        ItemInstance RemoveItem(int i1, int i2);
        void SetItem(int count, ItemInstance item);
        string GetName();
        int GetMaxStackSize();
        void SetChanged();
        bool StillValid(Player entityPlayer1);
    }
}