using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class DectectorRailTile : RailTile
    {
        public DectectorRailTile(int i1, int i2) : base(i1, i2, true)
        {
            this.SetTicking(true);
        }

        public override int GetTickDelay()
        {
            return 20;
        }

        public override bool IsSignalSource()
        {
            return true;
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if ((i6 & 8) == 0)
                {
                    this.SetDir(world1, i2, i3, i4, i6);
                }
            }
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if ((i6 & 8) != 0)
                {
                    this.SetDir(world1, i2, i3, i4, i6);
                }
            }
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return (iBlockAccess1.GetData(i2, i3, i4) & 8) != 0;
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return (world1.GetData(i2, i3, i4) & 8) == 0 ? false : i5 == 1;
        }

        private void SetDir(Level world1, int i2, int i3, int i4, int i5)
        {
            bool z6 = (i5 & 8) != 0;
            bool z7 = false;
            float f8 = 0.125F;
            IList<Minecart> list9 = world1.GetEntitiesOfClass<Minecart>(typeof(Minecart), AABB.Of(i2 + f8, i3, i4 + f8, i2 + 1 - f8, i3 + 0.25, i4 + 1 - f8));
            if (list9.Count > 0)
            {
                z7 = true;
            }

            if (z7 && !z6)
            {
                world1.SetData(i2, i3, i4, i5 | 8);
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
            }

            if (!z7 && z6)
            {
                world1.SetData(i2, i3, i4, i5 & 7);
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
            }

            if (z7)
            {
                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
            }
        }
    }
}