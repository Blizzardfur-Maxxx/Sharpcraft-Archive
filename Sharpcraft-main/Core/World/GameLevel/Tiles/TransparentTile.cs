using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TransparentTile : Tile
    {
        private bool renderFace;
        protected TransparentTile(int i1, int i2, Material material3, bool z4) : base(i1, i2, material3)
        {
            this.renderFace = z4;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            int i6 = iBlockAccess1.GetTile(i2, i3, i4);
            return !this.renderFace && i6 == this.id ? false : base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5);
        }
    }
}