using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class IceTile : TransparentTile
    {
        public IceTile(int i1, int i2) : base(i1, i2, Material.ice, false)
        {
            this.slipperiness = 0.98F;
            this.SetTicking(true);
        }

        public override RenderLayer GetRenderLayer()
        {
            return RenderLayer.RENDERLAYER_ALPHATEST;
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, 1 - i5);
        }

        public override void HarvestBlock(Level world1, Player entityPlayer2, int i3, int i4, int i5, int i6)
        {
            base.HarvestBlock(world1, entityPlayer2, i3, i4, i5, i6);
            Material material7 = world1.GetMaterial(i3, i4 - 1, i5);
            if (material7.BlocksMotion() || material7.IsLiquid())
            {
                world1.SetTile(i3, i4, i5, Tile.water.id);
            }
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.GetSavedLightValue(LightLayer.Block, i2, i3, i4) > 11 - Tile.lightBlock[this.id])
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, Tile.calmWater.id);
            }
        }

        public override int GetPistonPushReaction()
        {
            return 0;
        }
    }
}