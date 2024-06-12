using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using System;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class NoteTile : EntityTile
    {
        public NoteTile(int i1) : base(i1, 74, Material.wood)
        {
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.texture;
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (i5 > 0 && Tile.tiles[i5].IsSignalSource())
            {
                bool z6 = world1.IsBlockGettingPowered(i2, i3, i4);
                TileEntityNote tileEntityNote7 = (TileEntityNote)world1.GetTileEntity(i2, i3, i4);
                if (tileEntityNote7.previousRedstoneState != z6)
                {
                    if (z6)
                    {
                        tileEntityNote7.TriggerNote(world1, i2, i3, i4);
                    }

                    tileEntityNote7.previousRedstoneState = z6;
                }
            }
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                TileEntityNote tileEntityNote6 = (TileEntityNote)world1.GetTileEntity(i2, i3, i4);
                tileEntityNote6.ChangePitch();
                tileEntityNote6.TriggerNote(world1, i2, i3, i4);
                return true;
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (!world1.isRemote)
            {
                TileEntityNote tileEntityNote6 = (TileEntityNote)world1.GetTileEntity(i2, i3, i4);
                tileEntityNote6.TriggerNote(world1, i2, i3, i4);
            }
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityNote();
        }

        public override void TileEvent(Level world1, int i2, int i3, int i4, int i5, int i6)
        {
            float f7 = (float)Math.Pow(2, (i6 - 12) / 12);
            string string8 = "harp";
            if (i5 == 1)
            {
                string8 = "bd";
            }

            if (i5 == 2)
            {
                string8 = "snare";
            }

            if (i5 == 3)
            {
                string8 = "hat";
            }

            if (i5 == 4)
            {
                string8 = "bassattack";
            }

            world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "note." + string8, 3F, f7);
            world1.AddParticle("note", i2 + 0.5, i3 + 1.2, i4 + 0.5, i6 / 24, 0, 0);
        }
    }
}