using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemSword : Item
    {
        private int weaponDamage;
        public ItemSword(int i1, Tier enumToolMaterial2) : base(i1)
        {
            this.maxStackSize = 1;
            this.SetMaxDamage(enumToolMaterial2.GetMaxUses());
            this.weaponDamage = 4 + enumToolMaterial2.GetDamageVsEntity() * 2;
        }

        public override float GetStrVsBlock(ItemInstance itemStack1, Tile block2)
        {
            return block2.id == Tile.web.id ? 15F : 1.5F;
        }

        public override bool HitEntity(ItemInstance itemStack1, Mob entityLiving2, Mob entityLiving3)
        {
            itemStack1.DamageItem(1, entityLiving3);
            return true;
        }

        public override bool OnBlockDestroyed(ItemInstance itemStack1, int i2, int i3, int i4, int i5, Mob entityLiving6)
        {
            itemStack1.DamageItem(2, entityLiving6);
            return true;
        }

        public override int GetDamageVsEntity(Entity entity1)
        {
            return this.weaponDamage;
        }

        public override bool IsFull3D()
        {
            return true;
        }

        public override bool CanHarvestBlock(Tile block1)
        {
            return block1.id == Tile.web.id;
        }
    }
}