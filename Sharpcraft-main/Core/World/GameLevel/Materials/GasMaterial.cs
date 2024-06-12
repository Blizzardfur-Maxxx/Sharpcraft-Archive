namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class GasMaterial : Material
    {
        public GasMaterial(Color color) : base(color)
        {
            this.SetIsGroundCover();
        }

        public override bool IsSolid()
        {
            return false;
        }

        public override bool BlocksLight()
        {
            return false;
        }

        public override bool BlocksMotion()
        {
            return false;
        }
    }
}
