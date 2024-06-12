using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemMinecart : Item
    {
        public int minecartType;
        public ItemMinecart(int i1, int i2) : base(i1)
        {
            this.maxStackSize = 1;
            this.minecartType = i2;
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            int i8 = world3.GetTile(i4, i5, i6);
            if (RailTile.IsRailBlock(i8))
            {
                if (!world3.isRemote)
                {
                    world3.AddEntity(new Minecart(world3, i4 + 0.5F, i5 + 0.5F, i6 + 0.5F, this.minecartType));
                }

                --itemStack1.stackSize;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}