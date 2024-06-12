namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class LiquidMaterial : Material
    {
        public LiquidMaterial(Color color) : base(color)
        {
            this.SetIsGroundCover();
            this.SetNoPushMobility();
        }

        public override bool IsLiquid()
        {
            return true;
        }

        public override bool BlocksMotion()
        {
            return false;
        }

        public override bool IsSolid()
        {
            return false;
        }
    }
}
