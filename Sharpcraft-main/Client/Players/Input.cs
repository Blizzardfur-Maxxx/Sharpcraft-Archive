using LWCSGL.Input;
using SharpCraft.Core.World.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Players
{
    public class Input
    {
        public float moveStrafe = 0.0F;
        public float moveForward = 0.0F;
        public bool field_1177_c = false;
        public bool jump = false;
        public bool sneak = false;

        public virtual void UpdatePlayerMoveState(Player entityPlayer1)
        {
        }
        public virtual void ResetKeyState()
        {
        }

        public virtual void CheckKeyForMovementInput(VirtualKey i1, bool z2)
        {
        }
    }
}
