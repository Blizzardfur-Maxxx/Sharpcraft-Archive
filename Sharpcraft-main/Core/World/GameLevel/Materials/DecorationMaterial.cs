﻿namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class DecorationMaterial : Material
    {
        public DecorationMaterial(Color color) : base(color) { }

        public override bool IsSolid()
        {
            return false;
        }

        public override bool BlocksMotion()
        {
            return false;
        }

        public override bool BlocksLight()
        {
            return false;
        }
    }
}
