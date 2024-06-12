using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemTool : Item
    {
        private Tile[] blocksEffectiveAgainst;
        private float efficiencyOnProperMaterial = 4F;
        private int damageVsEntity;
        protected Tier toolMaterial;
        protected ItemTool(int i1, int i2, Tier enumToolMaterial3, Tile[] block4) : base(i1)
        {
            this.toolMaterial = enumToolMaterial3;
            this.blocksEffectiveAgainst = block4;
            this.maxStackSize = 1;
            this.SetMaxDamage(enumToolMaterial3.GetMaxUses());
            this.efficiencyOnProperMaterial = enumToolMaterial3.GetEfficiencyOnProperMaterial();
            this.damageVsEntity = i2 + enumToolMaterial3.GetDamageVsEntity();
        }

        public override float GetStrVsBlock(ItemInstance itemStack1, Tile block2)
        {
            for (int i3 = 0; i3 < this.blocksEffectiveAgainst.Length; ++i3)
            {
                if (this.blocksEffectiveAgainst[i3] == block2)
                {
                    return this.efficiencyOnProperMaterial;
                }
            }

            return 1F;
        }

        public override bool HitEntity(ItemInstance itemStack1, Mob entityLiving2, Mob entityLiving3)
        {
            itemStack1.DamageItem(2, entityLiving3);
            return true;
        }

        public override bool OnBlockDestroyed(ItemInstance itemStack1, int i2, int i3, int i4, int i5, Mob entityLiving6)
        {
            itemStack1.DamageItem(1, entityLiving6);
            return true;
        }

        public override int GetDamageVsEntity(Entity entity1)
        {
            return this.damageVsEntity;
        }

        public override bool IsFull3D()
        {
            return true;
        }
    }
}