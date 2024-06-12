using LWCSGL.Input;
using SharpCraft.Core.World.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Players
{
    public class KeyboardInput : Input
    {
        private bool[] keys = new bool[10];
        private Options options;

        public KeyboardInput(Options gameSettings1)
        {
            this.options = gameSettings1;
        }

        public override void CheckKeyForMovementInput(VirtualKey i1, bool z2)
        {
            sbyte b3 = -1;
            if (i1 == this.options.keyBindForward.keyCode)
            {
                b3 = 0;
            }

            if (i1 == this.options.keyBindBack.keyCode)
            {
                b3 = 1;
            }

            if (i1 == this.options.keyBindLeft.keyCode)
            {
                b3 = 2;
            }

            if (i1 == this.options.keyBindRight.keyCode)
            {
                b3 = 3;
            }

            if (i1 == this.options.keyBindJump.keyCode)
            {
                b3 = 4;
            }

            if (i1 == this.options.keyBindSneak.keyCode)
            {
                b3 = 5;
            }

            if (b3 >= 0)
            {
                this.keys[b3] = z2;
            }

        }

        public override void ResetKeyState()
        {
            for (int i1 = 0; i1 < 10; ++i1)
            {
                this.keys[i1] = false;
            }

        }

        public override void UpdatePlayerMoveState(Player entityPlayer1)
        {
            this.moveStrafe = 0.0F;
            this.moveForward = 0.0F;
            if (this.keys[0])
            {
                ++this.moveForward;
            }

            if (this.keys[1])
            {
                --this.moveForward;
            }

            if (this.keys[2])
            {
                ++this.moveStrafe;
            }

            if (this.keys[3])
            {
                --this.moveStrafe;
            }

            this.jump = this.keys[4];
            this.sneak = this.keys[5];
            if (this.sneak)
            {
                this.moveStrafe = (float)(this.moveStrafe * 0.3D);
                this.moveForward = (float)(this.moveForward * 0.3D);
            }

        }
    }
}
