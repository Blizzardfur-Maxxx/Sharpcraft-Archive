using SharpCraft.Core.Util;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class NotGateTile : TorchTile
    {
        private bool torchActive = false;
        private static IList<RedstoneUpdateInfo> torchUpdates = new List<RedstoneUpdateInfo>();
        private class RedstoneUpdateInfo
        {
            public int x;
            public int y;
            public int z;
            public long updateTime;

            public RedstoneUpdateInfo(int i1, int i2, int i3, long j4)
            {
                this.x = i1;
                this.y = i2;
                this.z = i3;
                this.updateTime = j4;
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return faceIdx == TileFace.UP ? Tile.redstoneWire.GetTexture(faceIdx, i2) : base.GetTexture(faceIdx, i2);
        }

        private bool CheckForBurnout(Level world1, int i2, int i3, int i4, bool z5)
        {
            if (z5)
            {
                torchUpdates.Add(new RedstoneUpdateInfo(i2, i3, i4, world1.GetTime()));
            }

            int i6 = 0;
            for (int i7 = 0; i7 < torchUpdates.Count; ++i7)
            {
                RedstoneUpdateInfo redstoneUpdateInfo8 = torchUpdates[i7];
                if (redstoneUpdateInfo8.x == i2 && redstoneUpdateInfo8.y == i3 && redstoneUpdateInfo8.z == i4)
                {
                    ++i6;
                    if (i6 >= 8)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public NotGateTile(int i1, int i2, bool z3) : base(i1, i2)
        {
            this.torchActive = z3;
            this.SetTicking(true);
        }

        public override int GetTickDelay()
        {
            return 2;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetData(i2, i3, i4) == 0)
            {
                base.OnPlace(world1, i2, i3, i4);
            }

            if (this.torchActive)
            {
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
                world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            if (this.torchActive)
            {
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
                world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
            }
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            if (!this.torchActive)
            {
                return false;
            }
            else
            {
                int i6 = iBlockAccess1.GetData(i2, i3, i4);
                return i6 == 5 && i5 == 1 ? false : (i6 == 3 && i5 == 3 ? false : (i6 == 4 && i5 == 2 ? false : (i6 == 1 && i5 == 5 ? false : i6 != 2 || i5 != 4)));
            }
        }

        private bool Func_30003(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            return i5 == 5 && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 - 1, i4, 0) ? true : (i5 == 3 && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 - 1, 2) ? true : (i5 == 4 && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 + 1, 3) ? true : (i5 == 1 && world1.IsBlockIndirectlyProvidingPowerTo(i2 - 1, i3, i4, 4) ? true : i5 == 2 && world1.IsBlockIndirectlyProvidingPowerTo(i2 + 1, i3, i4, 5))));
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            bool z6 = this.Func_30003(world1, i2, i3, i4);
            while (torchUpdates.Count > 0 && world1.GetTime() - torchUpdates[0].updateTime > 100)
            {
                torchUpdates.RemoveAt(0);
            }

            if (this.torchActive)
            {
                if (z6)
                {
                    world1.SetTileAndData(i2, i3, i4, Tile.torchRedstoneIdle.id, world1.GetData(i2, i3, i4));
                    if (this.CheckForBurnout(world1, i2, i3, i4, true))
                    {
                        world1.PlaySound(i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, "random.fizz", 0.5F, 2.6F + (float)(world1.rand.NextFloat() - world1.rand.NextFloat()) * 0.8F);
                        for (int i7 = 0; i7 < 5; ++i7)
                        {
                            double d8 = i2 + random5.NextFloat() * 0.6 + 0.2;
                            double d10 = i3 + random5.NextFloat() * 0.6 + 0.2;
                            double d12 = i4 + random5.NextFloat() * 0.6 + 0.2;
                            world1.AddParticle("smoke", d8, d10, d12, 0, 0, 0);
                        }
                    }
                }
            }
            else if (!z6 && !this.CheckForBurnout(world1, i2, i3, i4, false))
            {
                world1.SetTileAndData(i2, i3, i4, Tile.torchRedstoneActive.id, world1.GetData(i2, i3, i4));
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            base.NeighborChanged(world1, i2, i3, i4, i5);
            world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return i5 == 0 ? this.GetDirectSignal(world1, i2, i3, i4, i5) : false;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.torchRedstoneActive.id;
        }

        public override bool IsSignalSource()
        {
            return true;
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.torchActive)
            {
                int i6 = world1.GetData(i2, i3, i4);
                double d7 = i2 + 0.5F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d9 = i3 + 0.7F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d11 = i4 + 0.5F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d13 = 0.22F;
                double d15 = 0.27F;
                if (i6 == 1)
                {
                    world1.AddParticle("reddust", d7 - d15, d9 + d13, d11, 0, 0, 0);
                }
                else if (i6 == 2)
                {
                    world1.AddParticle("reddust", d7 + d15, d9 + d13, d11, 0, 0, 0);
                }
                else if (i6 == 3)
                {
                    world1.AddParticle("reddust", d7, d9 + d13, d11 - d15, 0, 0, 0);
                }
                else if (i6 == 4)
                {
                    world1.AddParticle("reddust", d7, d9 + d13, d11 + d15, 0, 0, 0);
                }
                else
                {
                    world1.AddParticle("reddust", d7, d9, d11, 0, 0, 0);
                }
            }
        }
    }
}