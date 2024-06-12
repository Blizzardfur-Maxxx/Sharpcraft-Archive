using SharpCraft.Core;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Players
{
    public class RemotePlayer : Player
    {
        private int field_785_bg;
        private double field_784_bh;
        private double field_783_bi;
        private double field_782_bj;
        private double field_780_bk;
        private double field_786_bl;

        public RemotePlayer(Level world1, string string2) : base(world1)
        {
            username = string2;
            yOffset = 0F;
            stepHeight = 0F;
            if (string2 != null && string2.Length > 0)
            {
                skinUrl = SharedConstants.SKIN_URL + string2 + ".png";
            }

            noClip = true;
            field_766 = 0.25F;
            renderDistanceWeight = 10;
        }

        protected override void ResetHeight()
        {
            yOffset = 0F;
        }

        public override bool AttackEntityFrom(Entity entity1, int i2)
        {
            return true;
        }

        public override void SetPositionAndRotation2(double d1, double d3, double d5, float f7, float f8, int i9)
        {
            field_784_bh = d1;
            field_783_bi = d3;
            field_782_bj = d5;
            field_780_bk = f7;
            field_786_bl = f8;
            field_785_bg = i9;
        }

        public override void OnUpdate()
        {
            field_766 = 0F;
            base.OnUpdate();
            field_Q = field_bd;
            double d1 = x - prevX;
            double d3 = z - prevZ;
            float f5 = Mth.Sqrt(d1 * d1 + d3 * d3) * 4F;
            if (f5 > 1F)
            {
                f5 = 1F;
            }

            field_bd += (f5 - field_bd) * 0.4F;
            field_ba += field_bd;
        }

        public override float GetShadowSize()
        {
            return 0F;
        }

        public override void OnLivingUpdate()
        {
            base.UpdatePlayerActionState();
            if (field_785_bg > 0)
            {
                double d1 = x + (field_784_bh - x) / field_785_bg;
                double d3 = y + (field_783_bi - y) / field_785_bg;
                double d5 = z + (field_782_bj - z) / field_785_bg;
                double d7;
                for (d7 = field_780_bk - yaw; d7 < -180; d7 += 360)
                {
                }

                while (d7 >= 180)
                {
                    d7 -= 360;
                }

                yaw = (float)(yaw + d7 / field_785_bg);
                pitch = (float)(pitch + (field_786_bl - pitch) / field_785_bg);
                --field_785_bg;
                SetPosition(d1, d3, d5);
                SetRotation(yaw, pitch);
            }

            field_775 = field_774;
            float f9 = Mth.Sqrt(motionX * motionX + motionZ * motionZ);
            float f2 = (float)Math.Atan(-motionY * 0.2F) * 15F;
            if (f9 > 0.1F)
            {
                f9 = 0.1F;
            }

            if (!onGround || health <= 0)
            {
                f9 = 0F;
            }

            if (onGround || health <= 0)
            {
                f2 = 0F;
            }

            field_774 += (f9 - field_774) * 0.4F;
            field_R += (f2 - field_R) * 0.8F;
        }

        public override void OutfitWithItem(int i1, int i2, int i3)
        {
            ItemInstance itemStack4 = null;
            if (i2 >= 0)
            {
                itemStack4 = new ItemInstance(i2, 1, i3);
            }

            if (i1 == 0)
            {
                inventory.mainInventory[inventory.currentItem] = itemStack4;
            }
            else
            {
                inventory.armorInventory[i1 - 1] = itemStack4;
            }
        }

        public override void Fun_o()
        {
        }
    }
}
