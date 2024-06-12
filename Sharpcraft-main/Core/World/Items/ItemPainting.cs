using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemPainting : Item
    {
        public ItemPainting(int i1) : base(i1)
        {
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (i7 == TileFace.DOWN)
            {
                return false;
            }
            else if (i7 == TileFace.UP)
            {
                return false;
            }
            else
            {
                byte b8 = 0;
                if (i7 == TileFace.WEST)
                {
                    b8 = 1;
                }

                if (i7 == TileFace.SOUTH)
                {
                    b8 = 2;
                }

                if (i7 == TileFace.EAST)
                {
                    b8 = 3;
                }

                Painting entityPainting9 = new Painting(world3, i4, i5, i6, b8);
                if (entityPainting9.OnValidSurface())
                {
                    if (!world3.isRemote)
                    {
                        world3.AddEntity(entityPainting9);
                    }

                    --itemStack1.stackSize;
                }

                return true;
            }
        }
    }
}