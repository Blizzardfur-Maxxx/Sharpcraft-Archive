using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemDoor : Item
    {
        private Material doorMaterial;
        public ItemDoor(int i1, Material material2) : base(i1)
        {
            this.doorMaterial = material2;
            this.maxStackSize = 1;
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (i7 != TileFace.UP)
            {
                return false;
            }
            else
            {
                ++i5;
                Tile block8;
                if (this.doorMaterial == Material.wood)
                {
                    block8 = Tile.doorWood;
                }
                else
                {
                    block8 = Tile.door_iron;
                }

                if (!block8.CanPlaceBlockAt(world3, i4, i5, i6))
                {
                    return false;
                }
                else
                {
                    int i9 = Mth.Floor((entityPlayer2.yaw + 180F) * 4F / 360F - 0.5) & 3;
                    sbyte b10 = 0;
                    sbyte b11 = 0;
                    if (i9 == 0)
                    {
                        b11 = 1;
                    }

                    if (i9 == 1)
                    {
                        b10 = -1;
                    }

                    if (i9 == 2)
                    {
                        b11 = -1;
                    }

                    if (i9 == 3)
                    {
                        b10 = 1;
                    }

                    int i12 = (world3.IsSolidBlockingTile(i4 - b10, i5, i6 - b11) ? 1 : 0) + (world3.IsSolidBlockingTile(i4 - b10, i5 + 1, i6 - b11) ? 1 : 0);
                    int i13 = (world3.IsSolidBlockingTile(i4 + b10, i5, i6 + b11) ? 1 : 0) + (world3.IsSolidBlockingTile(i4 + b10, i5 + 1, i6 + b11) ? 1 : 0);
                    bool z14 = world3.GetTile(i4 - b10, i5, i6 - b11) == block8.id || world3.GetTile(i4 - b10, i5 + 1, i6 - b11) == block8.id;
                    bool z15 = world3.GetTile(i4 + b10, i5, i6 + b11) == block8.id || world3.GetTile(i4 + b10, i5 + 1, i6 + b11) == block8.id;
                    bool z16 = false;
                    if (z14 && !z15)
                    {
                        z16 = true;
                    }
                    else if (i13 > i12)
                    {
                        z16 = true;
                    }

                    if (z16)
                    {
                        i9 = i9 - 1 & 3;
                        i9 += 4;
                    }

                    world3.editingBlocks = true;
                    world3.SetTileAndData(i4, i5, i6, block8.id, i9);
                    world3.SetTileAndData(i4, i5 + 1, i6, block8.id, i9 + 8);
                    world3.editingBlocks = false;
                    world3.UpdateNeighborsAt(i4, i5, i6, block8.id);
                    world3.UpdateNeighborsAt(i4, i5 + 1, i6, block8.id);
                    --itemStack1.stackSize;
                    return true;
                }
            }
        }
    }
}