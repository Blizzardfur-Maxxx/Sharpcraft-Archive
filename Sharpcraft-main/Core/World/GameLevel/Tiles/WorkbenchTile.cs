using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class WorkbenchTile : Tile
    {
        public WorkbenchTile(int i1) : base(i1, Material.wood)
        {
            this.texture = 59;
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture - 16 : (faceIdx == 0 ? Tile.wood.GetTexture(0) : (faceIdx != TileFace.NORTH && faceIdx != TileFace.WEST ? this.texture : this.texture + 1));
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                entityPlayer5.DisplayWorkbenchGUI(i2, i3, i4);
                return true;
            }
        }
    }
}