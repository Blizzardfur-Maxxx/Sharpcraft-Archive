using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemMapBase : Item
    {
        protected ItemMapBase(int i1) : base(i1)
        {
        }

        public override bool MapItemFunc()
        {
            return true;
        }

        public virtual Packet Func_28022_b(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            return null;
        }
    }
}