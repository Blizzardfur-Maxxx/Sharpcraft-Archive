using SharpCraft.Core.Util;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class DeadBushTile : Bush
    {
        public DeadBushTile(int i1, int i2) : base(i1, i2)
        {
            float f3 = 0.4F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, 0.8F, 0.5F + f3);
        }

        protected override bool CanThisPlantGrowOnThisBlockID(int i1)
        {
            return i1 == Tile.sand.id;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return this.texture;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return -1;
        }
    }
}