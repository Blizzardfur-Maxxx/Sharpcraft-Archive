using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class TileItem : Item
    {
        private int blockID;
        public TileItem(int i1) : base(i1)
        {
            this.blockID = i1 + 256;
            this.SetIconIndex(Tile.tiles[i1 + 256].GetTexture(TileFace.NORTH));
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (world3.GetTile(i4, i5, i6) == Tile.topSnow.id)
            {
                i7 = TileFace.DOWN;
            }
            else
            {
                if (i7 == TileFace.DOWN)
                {
                    --i5;
                }

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
            }

            if (itemStack1.stackSize == 0)
            {
                return false;
            }
            else if (i5 == 127 && Tile.tiles[this.blockID].material.IsSolid())
            {
                return false;
            }
            else if (world3.CanBlockBePlacedAt(this.blockID, i4, i5, i6, false, i7))
            {
                Tile block8 = Tile.tiles[this.blockID];
                if (world3.SetTileAndData(i4, i5, i6, this.blockID, this.GetMetadata(itemStack1.GetItemDamage())))
                {
                    Tile.tiles[this.blockID].OnBlockPlaced(world3, i4, i5, i6, i7);
                    Tile.tiles[this.blockID].OnBlockPlacedBy(world3, i4, i5, i6, entityPlayer2);
                    world3.PlaySound(i4 + 0.5F, i5 + 0.5F, i6 + 0.5F, block8.soundType.GetStepSound(), (block8.soundType.GetVolume() + 1F) / 2F, block8.soundType.GetPitch() * 0.8F);
                    --itemStack1.stackSize;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public override string GetItemNameIS(ItemInstance itemStack1)
        {
            return Tile.tiles[this.blockID].GetDescriptionId();
        }

        public override string GetItemName()
        {
            return Tile.tiles[this.blockID].GetDescriptionId();
        }
    }
}