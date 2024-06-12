using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public abstract class Model
    {
        public float OnGround;
        public bool IsRiding;

        public virtual void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
        }

        public virtual void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
        }

        public virtual void SetLivingAnimations(Mob entityLiving1, float f2, float f3, float f4)
        {
        }
    }
}
