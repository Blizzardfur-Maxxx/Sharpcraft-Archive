using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemRecord : Item
    {
        public readonly string recordName;
        public ItemRecord(int i1, string string2) : base(i1)
        {
            this.recordName = string2;
            this.maxStackSize = 1;
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (world3.GetTile(i4, i5, i6) == Tile.jukebox.id && world3.GetData(i4, i5, i6) == 0)
            {
                if (world3.isRemote)
                {
                    return true;
                }
                else
                {
                    ((JukeboxTile)Tile.jukebox).EjectRecord(world3, i4, i5, i6, this.id);
                    world3.LevelEvent((Player)null, LevelEventType.RECORD, i4, i5, i6, this.id);
                    --itemStack1.stackSize;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}