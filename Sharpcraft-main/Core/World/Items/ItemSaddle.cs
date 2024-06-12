using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Animals;

namespace SharpCraft.Core.World.Items
{
    public class ItemSaddle : Item
    {
        public ItemSaddle(int i1) : base(i1)
        {
            this.maxStackSize = 1;
        }

        public override void SaddleEntity(ItemInstance itemStack1, Mob entityLiving2)
        {
            if (entityLiving2 is Pig)
            {
                Pig entityPig3 = (Pig)entityLiving2;
                if (!entityPig3.GetSaddled())
                {
                    entityPig3.SetSaddled(true);
                    --itemStack1.stackSize;
                }
            }
        }

        public override bool HitEntity(ItemInstance itemStack1, Mob entityLiving2, Mob entityLiving3)
        {
            this.SaddleEntity(itemStack1, entityLiving2);
            return true;
        }
    }
}