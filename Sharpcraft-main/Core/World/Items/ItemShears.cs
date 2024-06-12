using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemShears : Item
    {
        public ItemShears(int i1) : base(i1)
        {
            this.SetMaxStackSize(1);
            this.SetMaxDamage(238);
        }

        public override bool OnBlockDestroyed(ItemInstance itemStack1, int i2, int i3, int i4, int i5, Mob entityLiving6)
        {
            if (i2 == Tile.leaves.id || i2 == Tile.web.id)
            {
                itemStack1.DamageItem(1, entityLiving6);
            }

            return base.OnBlockDestroyed(itemStack1, i2, i3, i4, i5, entityLiving6);
        }

        public override bool CanHarvestBlock(Tile block1)
        {
            return block1.id == Tile.web.id;
        }

        public override float GetStrVsBlock(ItemInstance itemStack1, Tile block2)
        {
            return block2.id != Tile.web.id && block2.id != Tile.leaves.id ? (block2.id == Tile.cloth.id ? 5F : base.GetStrVsBlock(itemStack1, block2)) : 15F;
        }
    }
}