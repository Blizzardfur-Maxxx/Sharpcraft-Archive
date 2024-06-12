using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PressurePlateTile : Tile
    {
        public enum Sensitivity
        {
            everything,
            mobs,
            players
        }

        private Sensitivity triggerMobType;
        public PressurePlateTile(int i1, int i2, Sensitivity enumMobType3, Material material4) : base(i1, i2, material4)
        {
            this.triggerMobType = enumMobType3;
            this.SetTicking(true);
            float f5 = 0.0625F;
            this.SetShape(f5, 0F, f5, 1F - f5, 0.03125F, 1F - f5);
        }

        public override int GetTickDelay()
        {
            return 20;
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2, i3 - 1, i4);
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            bool z6 = false;
            if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4))
            {
                z6 = true;
            }

            if (z6)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                if (world1.GetData(i2, i3, i4) != 0)
                {
                    this.SetStateIfMobInteractsWithPlate(world1, i2, i3, i4);
                }
            }
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            if (!world1.isRemote)
            {
                if (world1.GetData(i2, i3, i4) != 1)
                {
                    this.SetStateIfMobInteractsWithPlate(world1, i2, i3, i4);
                }
            }
        }

        private void SetStateIfMobInteractsWithPlate(Level world1, int i2, int i3, int i4)
        {
            bool z5 = world1.GetData(i2, i3, i4) == 1;
            bool z6 = false;
            float f7 = 0.125F;
            IList<Entity> list8 = null;
            if (this.triggerMobType == Sensitivity.everything)
            {
                list8 = world1.GetEntities((Entity)null, AABB.Of(i2 + f7, i3, i4 + f7, i2 + 1 - f7, i3 + 0.25, i4 + 1 - f7));
            }

            if (this.triggerMobType == Sensitivity.mobs)
            {
                list8 = world1.GetEntitiesOfClass<Entity>(typeof(Mob), AABB.Of(i2 + f7, i3, i4 + f7, i2 + 1 - f7, i3 + 0.25, i4 + 1 - f7));
            }

            if (this.triggerMobType == Sensitivity.players)
            {
                list8 = world1.GetEntitiesOfClass<Entity>(typeof(Player), AABB.Of(i2 + f7, i3, i4 + f7, i2 + 1 - f7, i3 + 0.25, i4 + 1 - f7));
            }

            if (list8.Count > 0)
            {
                z6 = true;
            }

            if (z6 && !z5)
            {
                world1.SetData(i2, i3, i4, 1);
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                world1.PlaySound(i2 + 0.5, i3 + 0.1, i4 + 0.5, "random.click", 0.3F, 0.6F);
            }

            if (!z6 && z5)
            {
                world1.SetData(i2, i3, i4, 0);
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                world1.PlaySound(i2 + 0.5, i3 + 0.1, i4 + 0.5, "random.click", 0.3F, 0.5F);
            }

            if (z6)
            {
                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            if (i5 > 0)
            {
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
            }

            base.OnBlockRemoval(world1, i2, i3, i4);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            bool z5 = iBlockAccess1.GetData(i2, i3, i4) == 1;
            float f6 = 0.0625F;
            if (z5)
            {
                this.SetShape(f6, 0F, f6, 1F - f6, 0.03125F, 1F - f6);
            }
            else
            {
                this.SetShape(f6, 0F, f6, 1F - f6, 0.0625F, 1F - f6);
            }
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return iBlockAccess1.GetData(i2, i3, i4) > 0;
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return world1.GetData(i2, i3, i4) == 0 ? false : i5 == 1;
        }

        public override bool IsSignalSource()
        {
            return true;
        }

        public override void SetBlockBoundsForItemRender()
        {
            float f1 = 0.5F;
            float f2 = 0.125F;
            float f3 = 0.5F;
            this.SetShape(0.5F - f1, 0.5F - f2, 0.5F - f3, 0.5F + f1, 0.5F + f2, 0.5F + f3);
        }

        public override int GetPistonPushReaction()
        {
            return 1;
        }
    }
}