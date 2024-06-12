using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class PistonPieceEntity : TileEntity
    {
        private int storedBlockID;
        private int storedMetadata;
        private Facing.TileFace storedOrientation;
        private bool isExtending;
        private bool field;
        private float progress;
        private float lastProgress;
        private static IList<Entity> pushedEntities = new List<Entity>();
        public PistonPieceEntity()
        {
        }

        public PistonPieceEntity(int i1, int i2, Facing.TileFace i3, bool z4, bool z5)
        {
            this.storedBlockID = i1;
            this.storedMetadata = i2;
            this.storedOrientation = i3;
            this.isExtending = z4;
            this.field = z5;
        }

        public virtual int GetStoredBlockID()
        {
            return this.storedBlockID;
        }

        public override int GetBlockMetadata()
        {
            return this.storedMetadata;
        }

        public virtual bool Func_31015()
        {
            return this.isExtending;
        }

        public virtual Facing.TileFace Func_31009()
        {
            return this.storedOrientation;
        }

        public virtual bool Func_31012_k()
        {
            return this.field;
        }

        public virtual float Func_31008(float f1)
        {
            if (f1 > 1F)
            {
                f1 = 1F;
            }

            return this.lastProgress + (this.progress - this.lastProgress) * f1;
        }

        public virtual float Func_31017_b(float f1)
        {
            return this.isExtending ? (this.Func_31008(f1) - 1F) * Facing.xOffset(this.storedOrientation) : (1F - this.Func_31008(f1)) * Facing.xOffset(this.storedOrientation);
        }

        public virtual float Func_31014_c(float f1)
        {
            return this.isExtending ? (this.Func_31008(f1) - 1F) * Facing.yOffset(this.storedOrientation) : (1F - this.Func_31008(f1)) * Facing.yOffset(this.storedOrientation);
        }

        public virtual float Func_31013_d(float f1)
        {
            return this.isExtending ? (this.Func_31008(f1) - 1F) * Facing.zOffset(this.storedOrientation) : (1F - this.Func_31008(f1)) * Facing.zOffset(this.storedOrientation);
        }

        private void Func_31010(float f1, float f2)
        {
            if (!this.isExtending)
            {
                --f1;
            }
            else
            {
                f1 = 1F - f1;
            }

            AABB axisAlignedBB3 = Tile.pistonMoving.Func_31035(this.worldObj, this.xCoord, this.yCoord, this.zCoord, this.storedBlockID, f1, this.storedOrientation);
            if (axisAlignedBB3 != null)
            {
                IList<Entity> list4 = this.worldObj.GetEntities((Entity)null, axisAlignedBB3);
                if (list4.Count > 0)
                {
                    foreach (Entity e in list4) pushedEntities.Add(e);
                    IEnumerator<Entity> iterator5 = pushedEntities.GetEnumerator();
                    while (iterator5.MoveNext())
                    {
                        Entity entity6 = iterator5.Current;
                        entity6.MoveEntity(f2 * Facing.xOffset(this.storedOrientation), f2 * Facing.yOffset(this.storedOrientation), f2 * Facing.zOffset(this.storedOrientation));
                    }

                    pushedEntities.Clear();
                }
            }
        }

        public virtual void ClearPistonTileEntity()
        {
            if (this.lastProgress < 1F)
            {
                this.lastProgress = this.progress = 1F;
                this.worldObj.RemoveTileEntity(this.xCoord, this.yCoord, this.zCoord);
                this.Invalidate();
                if (this.worldObj.GetTile(this.xCoord, this.yCoord, this.zCoord) == Tile.pistonMoving.id)
                {
                    this.worldObj.SetTileAndData(this.xCoord, this.yCoord, this.zCoord, this.storedBlockID, this.storedMetadata);
                }
            }
        }

        public override void UpdateEntity()
        {
            this.lastProgress = this.progress;
            if (this.lastProgress >= 1F)
            {
                this.Func_31010(1F, 0.25F);
                this.worldObj.RemoveTileEntity(this.xCoord, this.yCoord, this.zCoord);
                this.Invalidate();
                if (this.worldObj.GetTile(this.xCoord, this.yCoord, this.zCoord) == Tile.pistonMoving.id)
                {
                    this.worldObj.SetTileAndData(this.xCoord, this.yCoord, this.zCoord, this.storedBlockID, this.storedMetadata);
                }
            }
            else
            {
                this.progress += 0.5F;
                if (this.progress >= 1F)
                {
                    this.progress = 1F;
                }

                if (this.isExtending)
                {
                    this.Func_31010(this.progress, this.progress - this.lastProgress + 0.0625F);
                }
            }
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            this.storedBlockID = nBTTagCompound1.GetInteger("blockId");
            this.storedMetadata = nBTTagCompound1.GetInteger("blockData");
            this.storedOrientation = (Facing.TileFace)nBTTagCompound1.GetInteger("facing");
            this.lastProgress = this.progress = nBTTagCompound1.GetFloat("progress");
            this.isExtending = nBTTagCompound1.GetBoolean("extending");
        }

        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            nBTTagCompound1.SetInteger("blockId", this.storedBlockID);
            nBTTagCompound1.SetInteger("blockData", this.storedMetadata);
            nBTTagCompound1.SetInteger("facing", (int)this.storedOrientation);
            nBTTagCompound1.SetFloat("progress", this.lastProgress);
            nBTTagCompound1.SetBoolean("extending", this.isExtending);
        }
    }
}