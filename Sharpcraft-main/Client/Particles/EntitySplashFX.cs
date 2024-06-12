using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntitySplashFX : EntityRainFX
    {
        public EntitySplashFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1, d2, d4, d6)
        {
            this.particleGravity = 0.04F;
            ++this.particleTextureIndex;
            if (d10 == 0 && (d8 != 0 || d12 != 0))
            {
                this.motionX = d8;
                this.motionY = d10 + 0.1;
                this.motionZ = d12;
            }
        }
    }
}