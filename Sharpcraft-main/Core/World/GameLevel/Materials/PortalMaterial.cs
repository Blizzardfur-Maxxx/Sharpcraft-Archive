namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class PortalMaterial : Material
    {
        public PortalMaterial(Color color) : base(color)
        {
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
