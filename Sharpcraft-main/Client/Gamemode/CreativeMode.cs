using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Gamemode
{
    public class CreativeMode : GameMode
    {
        public CreativeMode(Client instance) : base(instance)
        {
            this.field_1064_b = true;
        }

        public override void Func_6473_b(Player entityPlayer1)
        {
            for (int i2 = 0; i2 < 9; ++i2)
            {
                if (entityPlayer1.inventory.mainInventory[i2] == null)
                {
                    this.mc.player.inventory.mainInventory[i2] = new ItemInstance(User.creativeTiles[i2]);
                }
                else
                {
                    this.mc.player.inventory.mainInventory[i2].stackSize = 1;
                }
            }
        }

        public override bool ShouldDrawHUD()
        {
            return false;
        }

        public override void Func_717_a(Level world1)
        {
            base.Func_717_a(world1);
        }

        public override void UpdateController()
        {
        }
    }
}
