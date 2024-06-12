using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemSign : Item
    {
        public ItemSign(int i1) : base(i1)
        {
            this.maxStackSize = 1;
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (i7 == TileFace.DOWN)
            {
                return false;
            }
            else if (!world3.GetMaterial(i4, i5, i6).IsSolid())
            {
                return false;
            }
            else
            {
                if (i7 == TileFace.UP)
                {
                    ++i5;
                }

                if (i7 == TileFace.NORTH)
                {
                    --i6;
                }

                if (i7 == TileFace.SOUTH)
                {
                    ++i6;
                }

                if (i7 == TileFace.WEST)
                {
                    --i4;
                }

                if (i7 == TileFace.EAST)
                {
                    ++i4;
                }

                if (!Tile.signPost.CanPlaceBlockAt(world3, i4, i5, i6))
                {
                    return false;
                }
                else
                {
                    if (i7 == TileFace.UP)
                    {
                        world3.SetTileAndData(i4, i5, i6, Tile.signPost.id, Mth.Floor((entityPlayer2.yaw + 180F) * 16F / 360F + 0.5) & 15);
                    }
                    else
                    {
                        world3.SetTileAndData(i4, i5, i6, Tile.signWall.id, (int)i7);
                    }

                    --itemStack1.stackSize;
                    TileEntitySign tileEntitySign8 = (TileEntitySign)world3.GetTileEntity(i4, i5, i6);
                    if (tileEntitySign8 != null)
                    {
                        entityPlayer2.DisplayGUIEditSign(tileEntitySign8);
                    }

                    return true;
                }
            }
        }
    }
}