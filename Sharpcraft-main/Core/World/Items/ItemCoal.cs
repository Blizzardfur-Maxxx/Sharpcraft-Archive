namespace SharpCraft.Core.World.Items
{
    public class ItemCoal : Item
    {
        public ItemCoal(int i1) : base(i1)
        {
            this.SetHasSubtypes(true);
            this.SetMaxDamage(0);
        }

        public override string GetItemNameIS(ItemInstance itemStack1)
        {
            return itemStack1.GetItemDamage() == 1 ? "item.charcoal" : "item.coal";
        }
    }
}