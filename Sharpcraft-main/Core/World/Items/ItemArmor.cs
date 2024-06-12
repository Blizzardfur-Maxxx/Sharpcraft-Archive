namespace SharpCraft.Core.World.Items
{
    public class ItemArmor : Item
    {
        private static readonly int[] damageReduceAmountArray = new[]
        {
            3,
            8,
            6,
            3
        };
        private static readonly int[] maxDamageArray = new[]
        {
            11,
            16,
            15,
            13
        };
        public readonly int armorLevel;
        public readonly int armorType;
        public readonly int damageReduceAmount;
        public readonly int renderIndex;
        public ItemArmor(int i1, int i2, int i3, int i4) : base(i1)
        {
            this.armorLevel = i2;
            this.armorType = i4;
            this.renderIndex = i3;
            this.damageReduceAmount = damageReduceAmountArray[i4];
            this.SetMaxDamage(maxDamageArray[i4] * 3 << i2);
            this.maxStackSize = 1;
        }
    }
}