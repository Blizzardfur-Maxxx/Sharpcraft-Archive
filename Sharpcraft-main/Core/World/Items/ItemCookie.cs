namespace SharpCraft.Core.World.Items
{
    public class ItemCookie : ItemFood
    {
        public ItemCookie(int i1, int i2, bool z3, int i4) : base(i1, i2, z3)
        {
            this.maxStackSize = i4;
        }
    }
}