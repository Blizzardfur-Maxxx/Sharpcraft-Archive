using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class StairTile : Tile
    {
        private Tile modelBlock;
        public StairTile(int i1, Tile block2) : base(i1, block2.texture, block2.material)
        {
            this.modelBlock = block2;
            this.SetDestroyTime(block2.destroyTime);
            this.SetExplodeable(block2.explosionResistance / 3F);
            this.SetSoundType(block2.soundType);
            this.SetLightBlock(255);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return base.GetAABB(world1, i2, i3, i4);
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.STAIR;
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5);
        }

        public override void AddAABBs(Level world1, int i2, int i3, int i4, AABB axisAlignedBB5, List<AABB> arrayList6)
        {
            int i7 = world1.GetData(i2, i3, i4);
            if (i7 == 0)
            {
                this.SetShape(0F, 0F, 0F, 0.5F, 0.5F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                this.SetShape(0.5F, 0F, 0F, 1F, 1F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
            }
            else if (i7 == 1)
            {
                this.SetShape(0F, 0F, 0F, 0.5F, 1F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                this.SetShape(0.5F, 0F, 0F, 1F, 0.5F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
            }
            else if (i7 == 2)
            {
                this.SetShape(0F, 0F, 0F, 1F, 0.5F, 0.5F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                this.SetShape(0F, 0F, 0.5F, 1F, 1F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
            }
            else if (i7 == 3)
            {
                this.SetShape(0F, 0F, 0F, 1F, 1F, 0.5F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
                this.SetShape(0F, 0F, 0.5F, 1F, 0.5F, 1F);
                base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
            }

            this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            this.modelBlock.AnimateTick(world1, i2, i3, i4, random5);
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.modelBlock.OnBlockClicked(world1, i2, i3, i4, entityPlayer5);
        }

        public override void OnBlockDestroyedByPlayer(Level world1, int i2, int i3, int i4, int i5)
        {
            this.modelBlock.OnBlockDestroyedByPlayer(world1, i2, i3, i4, i5);
        }

        public override float GetBrightness(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            return this.modelBlock.GetBrightness(iBlockAccess1, i2, i3, i4);
        }

        public override float GetExplosionResistance(Entity entity1)
        {
            return this.modelBlock.GetExplosionResistance(entity1);
        }

        public override RenderLayer GetRenderLayer()
        {
            return this.modelBlock.GetRenderLayer();
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return this.modelBlock.GetResource(i1, random2);
        }

        public override int ResourceCount(JRandom random1)
        {
            return this.modelBlock.ResourceCount(random1);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return this.modelBlock.GetTexture(faceIdx, i2);
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.modelBlock.GetTexture(faceIdx);
        }

        public override int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            return this.modelBlock.GetBlockTexture(iBlockAccess1, i2, i3, i4, i5);
        }

        public override int GetTickDelay()
        {
            return this.modelBlock.GetTickDelay();
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            return this.modelBlock.GetTileAABB(world1, i2, i3, i4);
        }

        public override void HandleEntityInside(Level world1, int i2, int i3, int i4, Entity entity5, Vec3 vec3D6)
        {
            this.modelBlock.HandleEntityInside(world1, i2, i3, i4, entity5, vec3D6);
        }

        public override bool IsCollidable()
        {
            return this.modelBlock.IsCollidable();
        }

        public override bool MayPick(int i1, bool z2)
        {
            return this.modelBlock.MayPick(i1, z2);
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return this.modelBlock.CanPlaceBlockAt(world1, i2, i3, i4);
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            this.NeighborChanged(world1, i2, i3, i4, 0);
            this.modelBlock.OnPlace(world1, i2, i3, i4);
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            this.modelBlock.OnBlockRemoval(world1, i2, i3, i4);
        }

        public override void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            this.modelBlock.SpawnResources(world1, i2, i3, i4, i5, f6);
        }

        public override void StepOn(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            this.modelBlock.StepOn(world1, i2, i3, i4, entity5);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            this.modelBlock.Tick(world1, i2, i3, i4, random5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            return this.modelBlock.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        public override void WasExploded(Level world1, int i2, int i3, int i4)
        {
            this.modelBlock.WasExploded(world1, i2, i3, i4);
        }

        public override void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
            int i6 = Mth.Floor(entityLiving5.yaw * 4F / 360F + 0.5) & 3;
            if (i6 == 0)
            {
                world1.SetData(i2, i3, i4, 2);
            }

            if (i6 == 1)
            {
                world1.SetData(i2, i3, i4, 1);
            }

            if (i6 == 2)
            {
                world1.SetData(i2, i3, i4, 3);
            }

            if (i6 == 3)
            {
                world1.SetData(i2, i3, i4, 0);
            }
        }
    }
}