using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.Inventory
{
    public class TrapMenu : AbstractContainerMenu
    {
        private TileEntityDispenser field_21149_a;
        public TrapMenu(IContainer iInventory1, TileEntityDispenser tileEntityDispenser2)
        {
            this.field_21149_a = tileEntityDispenser2;
            int i3;
            int i4;
            for (i3 = 0; i3 < 3; ++i3)
            {
                for (i4 = 0; i4 < 3; ++i4)
                {
                    this.AddSlot(new Slot(tileEntityDispenser2, i4 + i3 * 3, 62 + i4 * 18, 17 + i3 * 18));
                }
            }

            for (i3 = 0; i3 < 3; ++i3)
            {
                for (i4 = 0; i4 < 9; ++i4)
                {
                    this.AddSlot(new Slot(iInventory1, i4 + i3 * 9 + 9, 8 + i4 * 18, 84 + i3 * 18));
                }
            }

            for (i3 = 0; i3 < 9; ++i3)
            {
                this.AddSlot(new Slot(iInventory1, i3, 8 + i3 * 18, 142));
            }
        }

        public override bool StillValid(Player entityPlayer1)
        {
            return this.field_21149_a.StillValid(entityPlayer1);
        }
    }
}