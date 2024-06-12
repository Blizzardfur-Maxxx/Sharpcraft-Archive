using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpCraft.Client.Particles
{
    public class EntityBubbleFX : Particle
    {
        public EntityBubbleFX(Level world1, double d2, double d4, double d6, double d8, double d10, double d12) : base(world1, d2, d4, d6, d8, d10, d12)
        {
            this.particleRed = 1F;
            this.particleGreen = 1F;
            this.particleBlue = 1F;
            this.particleTextureIndex = 32;
            this.SetSize(0.02F, 0.02F);
            this.particleScale *= this.rand.NextFloat() * 0.6F + 0.2F;
            this.motionX = d8 * 0.2F + (float)(Mth.Random() * 2 - 1) * 0.02F;
            this.motionY = d10 * 0.2F + (float)(Mth.Random() * 2 - 1) * 0.02F;
            this.motionZ = d12 * 0.2F + (float)(Mth.Random() * 2 - 1) * 0.02F;
            this.particleMaxAge = (int)(8 / (Mth.Random() * 0.8 + 0.2));
        }

        public override void OnUpdate()
        {
            this.prevX = this.x;
            this.prevY = this.y;
            this.prevZ = this.z;
            this.motionY += 0.002;
            this.MoveEntity(this.motionX, this.motionY, this.motionZ);
            this.motionX *= 0.85F;
            this.motionY *= 0.85F;
            this.motionZ *= 0.85F;
            if (this.worldObj.GetMaterial(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)) != Material.water)
            {
                this.SetEntityDead();
            }

            if (this.particleMaxAge-- <= 0)
            {
                this.SetEntityDead();
            }
        }
    }
}