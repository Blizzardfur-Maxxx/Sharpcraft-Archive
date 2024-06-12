using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class JukeboxTile : EntityTile
    {
        public JukeboxTile(int i1, int i2) : base(i1, i2, Material.wood)
        {
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.texture + (faceIdx == TileFace.UP ? 1 : 0);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.GetData(i2, i3, i4) == 0)
            {
                return false;
            }
            else
            {
                this.Activate(world1, i2, i3, i4);
                return true;
            }
        }

        public virtual void EjectRecord(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote)
            {
                TileEntityRecordPlayer tileEntityRecordPlayer6 = (TileEntityRecordPlayer)world1.GetTileEntity(i2, i3, i4);
                tileEntityRecordPlayer6.record = i5;
                tileEntityRecordPlayer6.SetChanged();
                world1.SetData(i2, i3, i4, 1);
            }
        }

        public virtual void Activate(Level world1, int i2, int i3, int i4)
        {
            if (!world1.isRemote)
            {
                TileEntityRecordPlayer tileEntityRecordPlayer5 = (TileEntityRecordPlayer)world1.GetTileEntity(i2, i3, i4);
                int i6 = tileEntityRecordPlayer5.record;
                if (i6 != 0)
                {
                    world1.LevelEvent(LevelEventType.RECORD, i2, i3, i4, 0);
                    world1.PlayStreamingMusic((string)null, i2, i3, i4);
                    tileEntityRecordPlayer5.record = 0;
                    tileEntityRecordPlayer5.SetChanged();
                    world1.SetData(i2, i3, i4, 0);
                    float f8 = 0.7F;
                    double d9 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.5;
                    double d11 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.2 + 0.6;
                    double d13 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.5;
                    ItemEntity entityItem15 = new ItemEntity(world1, i2 + d9, i3 + d11, i4 + d13, new ItemInstance(i6, 1, 0));
                    entityItem15.delayBeforeCanPickup = 10;
                    world1.AddEntity(entityItem15);
                }
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            this.Activate(world1, i2, i3, i4);
            base.OnBlockRemoval(world1, i2, i3, i4);
        }

        public override void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            if (!world1.isRemote)
            {
                base.SpawnResources(world1, i2, i3, i4, i5, f6);
            }
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityRecordPlayer();
        }
    }
}