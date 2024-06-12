using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemHoe : Item
    {
        public ItemHoe(int i1, Tier enumToolMaterial2) : base(i1)
        {
            this.maxStackSize = 1;
            this.SetMaxDamage(enumToolMaterial2.GetMaxUses());
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            int i8 = world3.GetTile(i4, i5, i6);
            int i9 = world3.GetTile(i4, i5 + 1, i6);
            if ((i7 == TileFace.DOWN || i9 != 0 || i8 != Tile.grass.id) && i8 != Tile.dirt.id)
            {
                return false;
            }
            else
            {
                Tile block10 = Tile.farmland;
                world3.PlaySound(i4 + 0.5F, i5 + 0.5F, i6 + 0.5F, block10.soundType.GetStepSound(), (block10.soundType.GetVolume() + 1F) / 2F, block10.soundType.GetPitch() * 0.8F);
                if (world3.isRemote)
                {
                    return true;
                }
                else
                {
                    world3.SetTile(i4, i5, i6, block10.id);
                    itemStack1.DamageItem(1, entityPlayer2);
                    return true;
                }
            }
        }

        public override bool IsFull3D()
        {
            return true;
        }
    }
}