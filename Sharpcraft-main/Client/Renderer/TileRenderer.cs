using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.Util.Facing;
using static SharpCraft.Core.World.GameLevel.Tiles.Tile;

namespace SharpCraft.Client.Renderer
{
    public class TileRenderer
    {
        private ILevelSource blockAccess;
        private int overrideBlockTexture = -1;
        private bool flipTexture = false;
        private bool renderAllFaces = false;
        public static bool fancyGrass = true;
        public bool field_31088_b = true;
        private int field_31087_g = 0;
        private int field_31086_h = 0;
        private int field_31085_i = 0;
        private int field_31084_j = 0;
        private int field_31083_k = 0;
        private int field_31082_l = 0;
        private bool enableAO;
        private float lightValueOwn;
        private float aoLightValueXNeg;
        private float aoLightValueYNeg;
        private float aoLightValueZNeg;
        private float aoLightValueXPos;
        private float aoLightValueYPos;
        private float aoLightValueZPos;
        private float field_22377_m;
        private float field_22376_n;
        private float field_22375_o;
        private float field_22374_p;
        private float field_22373_q;
        private float field_22372_r;
        private float field_22371_s;
        private float field_22370_t;
        private float field_22369_u;
        private float field_22368_v;
        private float field_22367_w;
        private float field_22366_x;
        private float field_22365_y;
        private float field_22364_z;
        private float field_22362_A;
        private float field_22360_B;
        private float field_22358_C;
        private float field_22356_D;
        private float field_22354_E;
        private float field_22353_F;
        private int field_22352_G = 1;
        private float colorRedTopLeft;
        private float colorRedBottomLeft;
        private float colorRedBottomRight;
        private float colorRedTopRight;
        private float colorGreenTopLeft;
        private float colorGreenBottomLeft;
        private float colorGreenBottomRight;
        private float colorGreenTopRight;
        private float colorBlueTopLeft;
        private float colorBlueBottomLeft;
        private float colorBlueBottomRight;
        private float colorBlueTopRight;
        private bool field_22339_T;
        private bool field_22338_U;
        private bool field_22337_V;
        private bool field_22336_W;
        private bool field_22335_X;
        private bool field_22334_Y;
        private bool field_22333_Z;
        private bool field_22363_aa;
        private bool field_22361_ab;
        private bool field_22359_ac;
        private bool field_22357_ad;
        private bool field_22355_ae;
        public TileRenderer(ILevelSource iBlockAccess1)
        {
            this.blockAccess = iBlockAccess1;
        }

        public TileRenderer()
        {
        }

        public virtual void RenderBlockUsingTexture(Tile block1, int i2, int i3, int i4, int i5)
        {
            this.overrideBlockTexture = i5;
            this.RenderBlockByRenderType(block1, i2, i3, i4);
            this.overrideBlockTexture = -1;
        }

        public virtual void Func_31075_a(Tile block1, int i2, int i3, int i4)
        {
            this.renderAllFaces = true;
            this.RenderBlockByRenderType(block1, i2, i3, i4);
            this.renderAllFaces = false;
        }

        public virtual bool RenderBlockByRenderType(Tile block1, int i2, int i3, int i4)
        {
            Tile.RenderShape i5 = block1.GetRenderShape();
            block1.SetBlockBoundsBasedOnState(this.blockAccess, i2, i3, i4);
            switch (i5)
            {
                case Tile.RenderShape.NORMAL:
                    return this.RenderStandardBlock(block1, i2, i3, i4);
                case Tile.RenderShape.CROSS:
                    return this.RenderBlockReed(block1, i2, i3, i4);
                case Tile.RenderShape.TORCH:
                    return this.RenderBlockTorch(block1, i2, i3, i4);
                case Tile.RenderShape.FIRE:
                    return this.RenderBlockFire(block1, i2, i3, i4);
                case Tile.RenderShape.LIQUID:
                    return this.RenderBlockFluids(block1, i2, i3, i4);
                case Tile.RenderShape.WIRE:
                    return this.RenderBlockRedstoneWire(block1, i2, i3, i4);
                case Tile.RenderShape.CROPS:
                    return this.RenderBlockCrops(block1, i2, i3, i4);
                case Tile.RenderShape.DOOR:
                    return this.RenderBlockDoor(block1, i2, i3, i4);
                case Tile.RenderShape.LADDER:
                    return this.RenderBlockLadder(block1, i2, i3, i4);
                case Tile.RenderShape.RAIL:
                    return this.RenderBlockMinecartTrack((RailTile)block1, i2, i3, i4);
                case Tile.RenderShape.STAIR:
                    return this.RenderBlockStairs(block1, i2, i3, i4);
                case Tile.RenderShape.FENCE:
                    return this.RenderBlockFence(block1, i2, i3, i4);
                case Tile.RenderShape.LEVER:
                    return this.RenderBlockLever(block1, i2, i3, i4);
                case Tile.RenderShape.CACTUS:
                    return this.RenderBlockCactus(block1, i2, i3, i4);
                case Tile.RenderShape.BED:
                    return this.RenderBlockBed(block1, i2, i3, i4);
                case Tile.RenderShape.REPEATER:
                    return this.RenderBlockRepeater(block1, i2, i3, i4);
                case Tile.RenderShape.PISTON_BASE:
                    return this.Func_31074_b(block1, i2, i3, i4, false);
                case Tile.RenderShape.PISTON_EXTENSION:
                    return this.Func_31080_c(block1, i2, i3, i4, true);
                case Tile.RenderShape.NONE:
                default:
                    return false;
            }
        }

        private bool RenderBlockBed(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            int direction = BedTile.GetDirectionFromMetadata(i6);
            bool isFoot = BedTile.IsBlockFootOfBed(i6);
            float f9 = 0.5F;
            float f10 = 1F;
            float f11 = 0.8F;
            float f12 = 0.6F;
            float f25 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            tessellator5.Color(f9 * f25, f9 * f25, f9 * f25);
            int rotation = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.DOWN);
            int i27 = (rotation & 15) << 4;
            int i28 = rotation & 240;
            double d29 = i27 / 256F;
            double d31 = (i27 + 16 - 0.01) / 256;
            double d33 = i28 / 256F;
            double d35 = (i28 + 16 - 0.01) / 256;
            double d37 = i2 + block1.minX;
            double d39 = i2 + block1.maxX;
            double d41 = i3 + block1.minY + 0.1875;
            double d43 = i4 + block1.minZ;
            double d45 = i4 + block1.maxZ;
            tessellator5.VertexUV(d37, d41, d45, d29, d35);
            tessellator5.VertexUV(d37, d41, d43, d29, d33);
            tessellator5.VertexUV(d39, d41, d43, d31, d33);
            tessellator5.VertexUV(d39, d41, d45, d31, d35);
            float f64 = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
            tessellator5.Color(f10 * f64, f10 * f64, f10 * f64);
            i27 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.UP);
            i28 = (i27 & 15) << 4;
            int i67 = i27 & 240;
            double d30 = i28 / 256F;
            double d32 = (i28 + 16 - 0.01) / 256;
            double d34 = i67 / 256F;
            double d36 = (i67 + 16 - 0.01) / 256;
            double d38 = d30;
            double d40 = d32;
            double d42 = d34;
            double d44 = d34;
            double d46 = d30;
            double d48 = d32;
            double d50 = d36;
            double d52 = d36;
            if (direction == 0)
            {
                d40 = d30;
                d42 = d36;
                d46 = d32;
                d52 = d34;
            }
            else if (direction == 2)
            {
                d38 = d32;
                d44 = d36;
                d48 = d30;
                d50 = d34;
            }
            else if (direction == 3)
            {
                d38 = d32;
                d44 = d36;
                d48 = d30;
                d50 = d34;
                d40 = d30;
                d42 = d36;
                d46 = d32;
                d52 = d34;
            }

            double d54 = i2 + block1.minX;
            double d56 = i2 + block1.maxX;
            double d58 = i3 + block1.maxY;
            double d60 = i4 + block1.minZ;
            double d62 = i4 + block1.maxZ;
            tessellator5.VertexUV(d56, d58, d62, d46, d50);
            tessellator5.VertexUV(d56, d58, d60, d38, d42);
            tessellator5.VertexUV(d54, d58, d60, d40, d44);
            tessellator5.VertexUV(d54, d58, d62, d48, d52);
            rotation = Direction.rotations[direction];
            if (isFoot)
            {
                rotation = Direction.rotations[Direction.opposite[direction]];
            }

            byte b65 = 4;
            switch (direction)
            {
                case 0:
                    b65 = 5;
                    break;
                case 1:
                    b65 = 3;
                    break;
                case 2:
                default:
                    break;
                case 3:
                    b65 = 2;
                    break;
            }

            float f66;
            if (rotation != 2 && (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 - 1, 2)))
            {
                f66 = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                if (block1.minZ > 0)
                {
                    f66 = f25;
                }

                tessellator5.Color(f11 * f66, f11 * f66, f11 * f66);
                this.flipTexture = b65 == 2;
                this.RenderEastFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.NORTH));
            }

            if (rotation != 3 && (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 + 1, 3)))
            {
                f66 = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                if (block1.maxZ < 1)
                {
                    f66 = f25;
                }

                tessellator5.Color(f11 * f66, f11 * f66, f11 * f66);
                this.flipTexture = b65 == 3;
                this.RenderWestFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH));
            }

            if (rotation != 4 && (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 - 1, i3, i4, 4)))
            {
                f66 = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                if (block1.minX > 0)
                {
                    f66 = f25;
                }

                tessellator5.Color(f12 * f66, f12 * f66, f12 * f66);
                this.flipTexture = b65 == 4;
                this.RenderNorthFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.WEST));
            }

            if (rotation != 5 && (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 + 1, i3, i4, 5)))
            {
                f66 = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                if (block1.maxX < 1)
                {
                    f66 = f25;
                }

                tessellator5.Color(f12 * f66, f12 * f66, f12 * f66);
                this.flipTexture = b65 == 5;
                this.RenderSouthFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.EAST));
            }

            this.flipTexture = false;
            return true;
        }

        public virtual bool RenderBlockTorch(Tile block1, int i2, int i3, int i4)
        {
            int i5 = this.blockAccess.GetData(i2, i3, i4);
            Tessellator tessellator6 = Tessellator.Instance;
            float f7 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            if (Tile.lightEmission[block1.id] > 0)
            {
                f7 = 1F;
            }

            tessellator6.Color(f7, f7, f7);
            double d8 = 0.4F;
            double d10 = 0.5 - d8;
            double d12 = 0.2F;
            if (i5 == 1)
            {
                this.RenderTorchAtAngle(block1, i2 - d10, i3 + d12, i4, -d8, 0);
            }
            else if (i5 == 2)
            {
                this.RenderTorchAtAngle(block1, i2 + d10, i3 + d12, i4, d8, 0);
            }
            else if (i5 == 3)
            {
                this.RenderTorchAtAngle(block1, i2, i3 + d12, i4 - d10, 0, -d8);
            }
            else if (i5 == 4)
            {
                this.RenderTorchAtAngle(block1, i2, i3 + d12, i4 + d10, 0, d8);
            }
            else
            {
                this.RenderTorchAtAngle(block1, i2, i3, i4, 0, 0);
            }

            return true;
        }

        private bool RenderBlockRepeater(Tile block1, int i2, int i3, int i4)
        {
            int i5 = this.blockAccess.GetData(i2, i3, i4);
            int i6 = i5 & 3;
            int i7 = (i5 & 12) >> 2;
            this.RenderStandardBlock(block1, i2, i3, i4);
            Tessellator tessellator8 = Tessellator.Instance;
            float f9 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            if (Tile.lightEmission[block1.id] > 0)
            {
                f9 = (f9 + 1F) * 0.5F;
            }

            tessellator8.Color(f9, f9, f9);
            double d10 = -0.1875;
            double d12 = 0;
            double d14 = 0;
            double d16 = 0;
            double d18 = 0;
            switch (i6)
            {
                case 0:
                    d18 = -0.3125;
                    d14 = DiodeTile.CONVERSION[i7];
                    break;
                case 1:
                    d16 = 0.3125;
                    d12 = -DiodeTile.CONVERSION[i7];
                    break;
                case 2:
                    d18 = 0.3125;
                    d14 = -DiodeTile.CONVERSION[i7];
                    break;
                case 3:
                    d16 = -0.3125;
                    d12 = DiodeTile.CONVERSION[i7];
                    break;
            }

            this.RenderTorchAtAngle(block1, i2 + d12, i3 + d10, i4 + d14, 0, 0);
            this.RenderTorchAtAngle(block1, i2 + d16, i3 + d10, i4 + d18, 0, 0);
            int i20 = block1.GetTexture(TileFace.UP);
            int i21 = (i20 & 15) << 4;
            int i22 = i20 & 240;
            double d23 = i21 / 256F;
            double d25 = (i21 + 15.99F) / 256F;
            double d27 = i22 / 256F;
            double d29 = (i22 + 15.99F) / 256F;
            float f31 = 0.125F;
            float f32 = i2 + 1;
            float f33 = i2 + 1;
            float f34 = i2 + 0;
            float f35 = i2 + 0;
            float f36 = i4 + 0;
            float f37 = i4 + 1;
            float f38 = i4 + 1;
            float f39 = i4 + 0;
            float f40 = i3 + f31;
            if (i6 == 2)
            {
                f32 = f33 = i2 + 0;
                f34 = f35 = i2 + 1;
                f36 = f39 = i4 + 1;
                f37 = f38 = i4 + 0;
            }
            else if (i6 == 3)
            {
                f32 = f35 = i2 + 0;
                f33 = f34 = i2 + 1;
                f36 = f37 = i4 + 0;
                f38 = f39 = i4 + 1;
            }
            else if (i6 == 1)
            {
                f32 = f35 = i2 + 1;
                f33 = f34 = i2 + 0;
                f36 = f37 = i4 + 1;
                f38 = f39 = i4 + 0;
            }

            tessellator8.VertexUV(f35, f40, f39, d23, d27);
            tessellator8.VertexUV(f34, f40, f38, d23, d29);
            tessellator8.VertexUV(f33, f40, f37, d25, d29);
            tessellator8.VertexUV(f32, f40, f36, d25, d27);
            return true;
        }

        public virtual void Func_31078_d(Tile block1, int i2, int i3, int i4)
        {
            this.renderAllFaces = true;
            this.Func_31074_b(block1, i2, i3, i4, true);
            this.renderAllFaces = false;
        }

        private bool Func_31074_b(Tile block1, int i2, int i3, int i4, bool z5)
        {
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            bool z7 = z5 || (i6 & 8) != 0;
            TileFace tf = PistonBaseTile.GetOrientation(i6);
            if (z7)
            {
                switch (tf)
                {
                    case TileFace.DOWN:
                        this.field_31087_g = 3;
                        this.field_31086_h = 3;
                        this.field_31085_i = 3;
                        this.field_31084_j = 3;
                        block1.SetShape(0F, 0.25F, 0F, 1F, 1F, 1F);
                        break;
                    case TileFace.UP:
                        block1.SetShape(0F, 0F, 0F, 1F, 0.75F, 1F);
                        break;
                    case TileFace.NORTH:
                        this.field_31085_i = 1;
                        this.field_31084_j = 2;
                        block1.SetShape(0F, 0F, 0.25F, 1F, 1F, 1F);
                        break;
                    case TileFace.SOUTH:
                        this.field_31085_i = 2;
                        this.field_31084_j = 1;
                        this.field_31083_k = 3;
                        this.field_31082_l = 3;
                        block1.SetShape(0F, 0F, 0F, 1F, 1F, 0.75F);
                        break;
                    case TileFace.WEST:
                        this.field_31087_g = 1;
                        this.field_31086_h = 2;
                        this.field_31083_k = 2;
                        this.field_31082_l = 1;
                        block1.SetShape(0.25F, 0F, 0F, 1F, 1F, 1F);
                        break;
                    case TileFace.EAST:
                        this.field_31087_g = 2;
                        this.field_31086_h = 1;
                        this.field_31083_k = 1;
                        this.field_31082_l = 2;
                        block1.SetShape(0F, 0F, 0F, 0.75F, 1F, 1F);
                        break;
                }

                this.RenderStandardBlock(block1, i2, i3, i4);
                this.field_31087_g = 0;
                this.field_31086_h = 0;
                this.field_31085_i = 0;
                this.field_31084_j = 0;
                this.field_31083_k = 0;
                this.field_31082_l = 0;
                block1.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            }
            else
            {
                switch (tf)
                {
                    case TileFace.DOWN:
                        this.field_31087_g = 3;
                        this.field_31086_h = 3;
                        this.field_31085_i = 3;
                        this.field_31084_j = 3;
                        break;
                    case TileFace.UP:
                    default:
                        break;
                    case TileFace.NORTH:
                        this.field_31085_i = 1;
                        this.field_31084_j = 2;
                        break;
                    case TileFace.SOUTH:
                        this.field_31085_i = 2;
                        this.field_31084_j = 1;
                        this.field_31083_k = 3;
                        this.field_31082_l = 3;
                        break;
                    case TileFace.WEST:
                        this.field_31087_g = 1;
                        this.field_31086_h = 2;
                        this.field_31083_k = 2;
                        this.field_31082_l = 1;
                        break;
                    case TileFace.EAST:
                        this.field_31087_g = 2;
                        this.field_31086_h = 1;
                        this.field_31083_k = 1;
                        this.field_31082_l = 2;
                        break;
                }

                this.RenderStandardBlock(block1, i2, i3, i4);
                this.field_31087_g = 0;
                this.field_31086_h = 0;
                this.field_31085_i = 0;
                this.field_31084_j = 0;
                this.field_31083_k = 0;
                this.field_31082_l = 0;
            }

            return true;
        }

        private void Func_31076_a(double d1, double d3, double d5, double d7, double d9, double d11, float f13, double d14)
        {
            int i16 = 108;
            if (this.overrideBlockTexture >= 0)
            {
                i16 = this.overrideBlockTexture;
            }

            int i17 = (i16 & 15) << 4;
            int i18 = i16 & 240;
            Tessellator tessellator19 = Tessellator.Instance;
            double d20 = (i17 + 0) / 256F;
            double d22 = (i18 + 0) / 256F;
            double d24 = (i17 + d14 - 0.01) / 256;
            double d26 = (i18 + 4F - 0.01) / 256;
            tessellator19.Color(f13, f13, f13);
            tessellator19.VertexUV(d1, d7, d9, d24, d22);
            tessellator19.VertexUV(d1, d5, d9, d20, d22);
            tessellator19.VertexUV(d3, d5, d11, d20, d26);
            tessellator19.VertexUV(d3, d7, d11, d24, d26);
        }

        private void Func_31081_b(double d1, double d3, double d5, double d7, double d9, double d11, float f13, double d14)
        {
            int i16 = 108;
            if (this.overrideBlockTexture >= 0)
            {
                i16 = this.overrideBlockTexture;
            }

            int i17 = (i16 & 15) << 4;
            int i18 = i16 & 240;
            Tessellator tessellator19 = Tessellator.Instance;
            double d20 = (i17 + 0) / 256F;
            double d22 = (i18 + 0) / 256F;
            double d24 = (i17 + d14 - 0.01) / 256;
            double d26 = (i18 + 4F - 0.01) / 256;
            tessellator19.Color(f13, f13, f13);
            tessellator19.VertexUV(d1, d5, d11, d24, d22);
            tessellator19.VertexUV(d1, d5, d9, d20, d22);
            tessellator19.VertexUV(d3, d7, d9, d20, d26);
            tessellator19.VertexUV(d3, d7, d11, d24, d26);
        }

        private void Func_31077_c(double d1, double d3, double d5, double d7, double d9, double d11, float f13, double d14)
        {
            int i16 = 108;
            if (this.overrideBlockTexture >= 0)
            {
                i16 = this.overrideBlockTexture;
            }

            int i17 = (i16 & 15) << 4;
            int i18 = i16 & 240;
            Tessellator tessellator19 = Tessellator.Instance;
            double d20 = (i17 + 0) / 256F;
            double d22 = (i18 + 0) / 256F;
            double d24 = (i17 + d14 - 0.01) / 256;
            double d26 = (i18 + 4F - 0.01) / 256;
            tessellator19.Color(f13, f13, f13);
            tessellator19.VertexUV(d3, d5, d9, d24, d22);
            tessellator19.VertexUV(d1, d5, d9, d20, d22);
            tessellator19.VertexUV(d1, d7, d11, d20, d26);
            tessellator19.VertexUV(d3, d7, d11, d24, d26);
        }

        public virtual void Func_31079_a(Tile block1, int i2, int i3, int i4, bool z5)
        {
            this.renderAllFaces = true;
            this.Func_31080_c(block1, i2, i3, i4, z5);
            this.renderAllFaces = false;
        }

        private bool Func_31080_c(Tile block1, int i2, int i3, int i4, bool z5)
        {
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            TileFace tf = PistonExtensionTile.Func_31050_c(i6);
            float f11 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            float f12 = z5 ? 1F : 0.5F;
            double d13 = z5 ? 16 : 8;
            switch (tf)
            {
                case TileFace.DOWN:
                    this.field_31087_g = 3;
                    this.field_31086_h = 3;
                    this.field_31085_i = 3;
                    this.field_31084_j = 3;
                    block1.SetShape(0F, 0F, 0F, 1F, 0.25F, 1F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31076_a(i2 + 0.375F, i2 + 0.625F, i3 + 0.25F, i3 + 0.25F + f12, i4 + 0.625F, i4 + 0.625F, f11 * 0.8F, d13);
                    this.Func_31076_a(i2 + 0.625F, i2 + 0.375F, i3 + 0.25F, i3 + 0.25F + f12, i4 + 0.375F, i4 + 0.375F, f11 * 0.8F, d13);
                    this.Func_31076_a(i2 + 0.375F, i2 + 0.375F, i3 + 0.25F, i3 + 0.25F + f12, i4 + 0.375F, i4 + 0.625F, f11 * 0.6F, d13);
                    this.Func_31076_a(i2 + 0.625F, i2 + 0.625F, i3 + 0.25F, i3 + 0.25F + f12, i4 + 0.625F, i4 + 0.375F, f11 * 0.6F, d13);
                    break;
                case TileFace.UP:
                    block1.SetShape(0F, 0.75F, 0F, 1F, 1F, 1F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31076_a(i2 + 0.375F, i2 + 0.625F, i3 - 0.25F + 1F - f12, i3 - 0.25F + 1F, i4 + 0.625F, i4 + 0.625F, f11 * 0.8F, d13);
                    this.Func_31076_a(i2 + 0.625F, i2 + 0.375F, i3 - 0.25F + 1F - f12, i3 - 0.25F + 1F, i4 + 0.375F, i4 + 0.375F, f11 * 0.8F, d13);
                    this.Func_31076_a(i2 + 0.375F, i2 + 0.375F, i3 - 0.25F + 1F - f12, i3 - 0.25F + 1F, i4 + 0.375F, i4 + 0.625F, f11 * 0.6F, d13);
                    this.Func_31076_a(i2 + 0.625F, i2 + 0.625F, i3 - 0.25F + 1F - f12, i3 - 0.25F + 1F, i4 + 0.625F, i4 + 0.375F, f11 * 0.6F, d13);
                    break;
                case TileFace.NORTH:
                    this.field_31085_i = 1;
                    this.field_31084_j = 2;
                    block1.SetShape(0F, 0F, 0F, 1F, 1F, 0.25F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31081_b(i2 + 0.375F, i2 + 0.375F, i3 + 0.625F, i3 + 0.375F, i4 + 0.25F, i4 + 0.25F + f12, f11 * 0.6F, d13);
                    this.Func_31081_b(i2 + 0.625F, i2 + 0.625F, i3 + 0.375F, i3 + 0.625F, i4 + 0.25F, i4 + 0.25F + f12, f11 * 0.6F, d13);
                    this.Func_31081_b(i2 + 0.375F, i2 + 0.625F, i3 + 0.375F, i3 + 0.375F, i4 + 0.25F, i4 + 0.25F + f12, f11 * 0.5F, d13);
                    this.Func_31081_b(i2 + 0.625F, i2 + 0.375F, i3 + 0.625F, i3 + 0.625F, i4 + 0.25F, i4 + 0.25F + f12, f11, d13);
                    break;
                case TileFace.SOUTH:
                    this.field_31085_i = 2;
                    this.field_31084_j = 1;
                    this.field_31083_k = 3;
                    this.field_31082_l = 3;
                    block1.SetShape(0F, 0F, 0.75F, 1F, 1F, 1F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31081_b(i2 + 0.375F, i2 + 0.375F, i3 + 0.625F, i3 + 0.375F, i4 - 0.25F + 1F - f12, i4 - 0.25F + 1F, f11 * 0.6F, d13);
                    this.Func_31081_b(i2 + 0.625F, i2 + 0.625F, i3 + 0.375F, i3 + 0.625F, i4 - 0.25F + 1F - f12, i4 - 0.25F + 1F, f11 * 0.6F, d13);
                    this.Func_31081_b(i2 + 0.375F, i2 + 0.625F, i3 + 0.375F, i3 + 0.375F, i4 - 0.25F + 1F - f12, i4 - 0.25F + 1F, f11 * 0.5F, d13);
                    this.Func_31081_b(i2 + 0.625F, i2 + 0.375F, i3 + 0.625F, i3 + 0.625F, i4 - 0.25F + 1F - f12, i4 - 0.25F + 1F, f11, d13);
                    break;
                case TileFace.WEST:
                    this.field_31087_g = 1;
                    this.field_31086_h = 2;
                    this.field_31083_k = 2;
                    this.field_31082_l = 1;
                    block1.SetShape(0F, 0F, 0F, 0.25F, 1F, 1F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31077_c(i2 + 0.25F, i2 + 0.25F + f12, i3 + 0.375F, i3 + 0.375F, i4 + 0.625F, i4 + 0.375F, f11 * 0.5F, d13);
                    this.Func_31077_c(i2 + 0.25F, i2 + 0.25F + f12, i3 + 0.625F, i3 + 0.625F, i4 + 0.375F, i4 + 0.625F, f11, d13);
                    this.Func_31077_c(i2 + 0.25F, i2 + 0.25F + f12, i3 + 0.375F, i3 + 0.625F, i4 + 0.375F, i4 + 0.375F, f11 * 0.6F, d13);
                    this.Func_31077_c(i2 + 0.25F, i2 + 0.25F + f12, i3 + 0.625F, i3 + 0.375F, i4 + 0.625F, i4 + 0.625F, f11 * 0.6F, d13);
                    break;
                case TileFace.EAST:
                    this.field_31087_g = 2;
                    this.field_31086_h = 1;
                    this.field_31083_k = 1;
                    this.field_31082_l = 2;
                    block1.SetShape(0.75F, 0F, 0F, 1F, 1F, 1F);
                    this.RenderStandardBlock(block1, i2, i3, i4);
                    this.Func_31077_c(i2 - 0.25F + 1F - f12, i2 - 0.25F + 1F, i3 + 0.375F, i3 + 0.375F, i4 + 0.625F, i4 + 0.375F, f11 * 0.5F, d13);
                    this.Func_31077_c(i2 - 0.25F + 1F - f12, i2 - 0.25F + 1F, i3 + 0.625F, i3 + 0.625F, i4 + 0.375F, i4 + 0.625F, f11, d13);
                    this.Func_31077_c(i2 - 0.25F + 1F - f12, i2 - 0.25F + 1F, i3 + 0.375F, i3 + 0.625F, i4 + 0.375F, i4 + 0.375F, f11 * 0.6F, d13);
                    this.Func_31077_c(i2 - 0.25F + 1F - f12, i2 - 0.25F + 1F, i3 + 0.625F, i3 + 0.375F, i4 + 0.625F, i4 + 0.625F, f11 * 0.6F, d13);
                    break;
            }

            this.field_31087_g = 0;
            this.field_31086_h = 0;
            this.field_31085_i = 0;
            this.field_31084_j = 0;
            this.field_31083_k = 0;
            this.field_31082_l = 0;
            block1.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            return true;
        }

        public virtual bool RenderBlockLever(Tile block1, int i2, int i3, int i4)
        {
            int i5 = this.blockAccess.GetData(i2, i3, i4);
            int i6 = i5 & 7;
            bool z7 = (i5 & 8) > 0;
            Tessellator tessellator8 = Tessellator.Instance;
            bool z9 = this.overrideBlockTexture >= 0;
            if (!z9)
            {
                this.overrideBlockTexture = Tile.stoneBrick.texture;
            }

            float f10 = 0.25F;
            float f11 = 0.1875F;
            float f12 = 0.1875F;
            if (i6 == 5)
            {
                block1.SetShape(0.5F - f11, 0F, 0.5F - f10, 0.5F + f11, f12, 0.5F + f10);
            }
            else if (i6 == 6)
            {
                block1.SetShape(0.5F - f10, 0F, 0.5F - f11, 0.5F + f10, f12, 0.5F + f11);
            }
            else if (i6 == 4)
            {
                block1.SetShape(0.5F - f11, 0.5F - f10, 1F - f12, 0.5F + f11, 0.5F + f10, 1F);
            }
            else if (i6 == 3)
            {
                block1.SetShape(0.5F - f11, 0.5F - f10, 0F, 0.5F + f11, 0.5F + f10, f12);
            }
            else if (i6 == 2)
            {
                block1.SetShape(1F - f12, 0.5F - f10, 0.5F - f11, 1F, 0.5F + f10, 0.5F + f11);
            }
            else if (i6 == 1)
            {
                block1.SetShape(0F, 0.5F - f10, 0.5F - f11, f12, 0.5F + f10, 0.5F + f11);
            }

            this.RenderStandardBlock(block1, i2, i3, i4);
            if (!z9)
            {
                this.overrideBlockTexture = -1;
            }

            float f13 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator8.Color(f13, f13, f13);
            int i14 = block1.GetTexture(0);
            if (this.overrideBlockTexture >= 0)
            {
                i14 = this.overrideBlockTexture;
            }

            int i15 = (i14 & 15) << 4;
            int i16 = i14 & 240;
            float f17 = i15 / 256F;
            float f18 = (i15 + 15.99F) / 256F;
            float f19 = i16 / 256F;
            float f20 = (i16 + 15.99F) / 256F;
            Vec3[] vec3D21 = new Vec3[8];
            float f22 = 0.0625F;
            float f23 = 0.0625F;
            float f24 = 0.625F;
            vec3D21[0] = Vec3.Of((-f22), 0, (-f23));
            vec3D21[1] = Vec3.Of(f22, 0, (-f23));
            vec3D21[2] = Vec3.Of(f22, 0, f23);
            vec3D21[3] = Vec3.Of((-f22), 0, f23);
            vec3D21[4] = Vec3.Of((-f22), f24, (-f23));
            vec3D21[5] = Vec3.Of(f22, f24, (-f23));
            vec3D21[6] = Vec3.Of(f22, f24, f23);
            vec3D21[7] = Vec3.Of((-f22), f24, f23);
            for (int i25 = 0; i25 < 8; ++i25)
            {
                if (z7)
                {
                    vec3D21[i25].z -= 0.0625;
                    vec3D21[i25].RotateAroundX(Mth.PI / 4.5F);
                }
                else
                {
                    vec3D21[i25].z += 0.0625;
                    vec3D21[i25].RotateAroundX(-0.69813174F);
                }

                if (i6 == 6)
                {
                    vec3D21[i25].RotateAroundY(Mth.PI / 2F);
                }

                if (i6 < 5)
                {
                    vec3D21[i25].y -= 0.375;
                    vec3D21[i25].RotateAroundX(Mth.PI / 2F);
                    if (i6 == 4)
                    {
                        vec3D21[i25].RotateAroundY(0F);
                    }

                    if (i6 == 3)
                    {
                        vec3D21[i25].RotateAroundY(Mth.PI);
                    }

                    if (i6 == 2)
                    {
                        vec3D21[i25].RotateAroundY(Mth.PI / 2F);
                    }

                    if (i6 == 1)
                    {
                        vec3D21[i25].RotateAroundY(-1.5707964F);
                    }

                    vec3D21[i25].x += i2 + 0.5;
                    vec3D21[i25].y += i3 + 0.5F;
                    vec3D21[i25].z += i4 + 0.5;
                }
                else
                {
                    vec3D21[i25].x += i2 + 0.5;
                    vec3D21[i25].y += i3 + 0.125F;
                    vec3D21[i25].z += i4 + 0.5;
                }
            }

            Vec3 vec3D30 = null;
            Vec3 vec3D26 = null;
            Vec3 vec3D27 = null;
            Vec3 vec3D28 = null;
            for (int i29 = 0; i29 < 6; ++i29)
            {
                if (i29 == 0)
                {
                    f17 = (i15 + 7) / 256F;
                    f18 = (i15 + 9 - 0.01F) / 256F;
                    f19 = (i16 + 6) / 256F;
                    f20 = (i16 + 8 - 0.01F) / 256F;
                }
                else if (i29 == 2)
                {
                    f17 = (i15 + 7) / 256F;
                    f18 = (i15 + 9 - 0.01F) / 256F;
                    f19 = (i16 + 6) / 256F;
                    f20 = (i16 + 16 - 0.01F) / 256F;
                }

                if (i29 == 0)
                {
                    vec3D30 = vec3D21[0];
                    vec3D26 = vec3D21[1];
                    vec3D27 = vec3D21[2];
                    vec3D28 = vec3D21[3];
                }
                else if (i29 == 1)
                {
                    vec3D30 = vec3D21[7];
                    vec3D26 = vec3D21[6];
                    vec3D27 = vec3D21[5];
                    vec3D28 = vec3D21[4];
                }
                else if (i29 == 2)
                {
                    vec3D30 = vec3D21[1];
                    vec3D26 = vec3D21[0];
                    vec3D27 = vec3D21[4];
                    vec3D28 = vec3D21[5];
                }
                else if (i29 == 3)
                {
                    vec3D30 = vec3D21[2];
                    vec3D26 = vec3D21[1];
                    vec3D27 = vec3D21[5];
                    vec3D28 = vec3D21[6];
                }
                else if (i29 == 4)
                {
                    vec3D30 = vec3D21[3];
                    vec3D26 = vec3D21[2];
                    vec3D27 = vec3D21[6];
                    vec3D28 = vec3D21[7];
                }
                else if (i29 == 5)
                {
                    vec3D30 = vec3D21[0];
                    vec3D26 = vec3D21[3];
                    vec3D27 = vec3D21[7];
                    vec3D28 = vec3D21[4];
                }

                tessellator8.VertexUV(vec3D30.x, vec3D30.y, vec3D30.z, f17, f20);
                tessellator8.VertexUV(vec3D26.x, vec3D26.y, vec3D26.z, f18, f20);
                tessellator8.VertexUV(vec3D27.x, vec3D27.y, vec3D27.z, f18, f19);
                tessellator8.VertexUV(vec3D28.x, vec3D28.y, vec3D28.z, f17, f19);
            }

            return true;
        }

        public virtual bool RenderBlockFire(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = block1.GetTexture(0);
            if (this.overrideBlockTexture >= 0)
            {
                i6 = this.overrideBlockTexture;
            }

            float f7 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            tessellator5.Color(f7, f7, f7);
            int i8 = (i6 & 15) << 4;
            int i9 = i6 & 240;
            double d10 = i8 / 256F;
            double d12 = (i8 + 15.99F) / 256F;
            double d14 = i9 / 256F;
            double d16 = (i9 + 15.99F) / 256F;
            float f18 = 1.4F;
            double d21;
            double d23;
            double d25;
            double d27;
            double d29;
            double d31;
            double d33;
            if (!this.blockAccess.IsSolidBlockingTile(i2, i3 - 1, i4) && !Tile.fire.CanBlockCatchFire(this.blockAccess, i2, i3 - 1, i4))
            {
                float f37 = 0.2F;
                float f20 = 0.0625F;
                if ((i2 + i3 + i4 & 1) == 1)
                {
                    d10 = i8 / 256F;
                    d12 = (i8 + 15.99F) / 256F;
                    d14 = (i9 + 16) / 256F;
                    d16 = (i9 + 15.99F + 16F) / 256F;
                }

                if ((i2 / 2 + i3 / 2 + i4 / 2 & 1) == 1)
                {
                    d21 = d12;
                    d12 = d10;
                    d10 = d21;
                }

                if (Tile.fire.CanBlockCatchFire(this.blockAccess, i2 - 1, i3, i4))
                {
                    tessellator5.VertexUV(i2 + f37, i3 + f18 + f20, i4 + 1, d12, d14);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 1, d12, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + f37, i3 + f18 + f20, i4 + 0, d10, d14);
                    tessellator5.VertexUV(i2 + f37, i3 + f18 + f20, i4 + 0, d10, d14);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 1, d12, d16);
                    tessellator5.VertexUV(i2 + f37, i3 + f18 + f20, i4 + 1, d12, d14);
                }

                if (Tile.fire.CanBlockCatchFire(this.blockAccess, i2 + 1, i3, i4))
                {
                    tessellator5.VertexUV(i2 + 1 - f37, i3 + f18 + f20, i4 + 0, d10, d14);
                    tessellator5.VertexUV(i2 + 1 - 0, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + 1 - 0, i3 + 0 + f20, i4 + 1, d12, d16);
                    tessellator5.VertexUV(i2 + 1 - f37, i3 + f18 + f20, i4 + 1, d12, d14);
                    tessellator5.VertexUV(i2 + 1 - f37, i3 + f18 + f20, i4 + 1, d12, d14);
                    tessellator5.VertexUV(i2 + 1 - 0, i3 + 0 + f20, i4 + 1, d12, d16);
                    tessellator5.VertexUV(i2 + 1 - 0, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + 1 - f37, i3 + f18 + f20, i4 + 0, d10, d14);
                }

                if (Tile.fire.CanBlockCatchFire(this.blockAccess, i2, i3, i4 - 1))
                {
                    tessellator5.VertexUV(i2 + 0, i3 + f18 + f20, i4 + f37, d12, d14);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 0, d12, d16);
                    tessellator5.VertexUV(i2 + 1, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + 1, i3 + f18 + f20, i4 + f37, d10, d14);
                    tessellator5.VertexUV(i2 + 1, i3 + f18 + f20, i4 + f37, d10, d14);
                    tessellator5.VertexUV(i2 + 1, i3 + 0 + f20, i4 + 0, d10, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 0, d12, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + f18 + f20, i4 + f37, d12, d14);
                }

                if (Tile.fire.CanBlockCatchFire(this.blockAccess, i2, i3, i4 + 1))
                {
                    tessellator5.VertexUV(i2 + 1, i3 + f18 + f20, i4 + 1 - f37, d10, d14);
                    tessellator5.VertexUV(i2 + 1, i3 + 0 + f20, i4 + 1 - 0, d10, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 1 - 0, d12, d16);
                    tessellator5.VertexUV(i2 + 0, i3 + f18 + f20, i4 + 1 - f37, d12, d14);
                    tessellator5.VertexUV(i2 + 0, i3 + f18 + f20, i4 + 1 - f37, d12, d14);
                    tessellator5.VertexUV(i2 + 0, i3 + 0 + f20, i4 + 1 - 0, d12, d16);
                    tessellator5.VertexUV(i2 + 1, i3 + 0 + f20, i4 + 1 - 0, d10, d16);
                    tessellator5.VertexUV(i2 + 1, i3 + f18 + f20, i4 + 1 - f37, d10, d14);
                }

                if (Tile.fire.CanBlockCatchFire(this.blockAccess, i2, i3 + 1, i4))
                {
                    d21 = i2 + 0.5 + 0.5;
                    d23 = i2 + 0.5 - 0.5;
                    d25 = i4 + 0.5 + 0.5;
                    d27 = i4 + 0.5 - 0.5;
                    d29 = i2 + 0.5 - 0.5;
                    d31 = i2 + 0.5 + 0.5;
                    d33 = i4 + 0.5 - 0.5;
                    double d35 = i4 + 0.5 + 0.5;
                    d10 = i8 / 256F;
                    d12 = (i8 + 15.99F) / 256F;
                    d14 = i9 / 256F;
                    d16 = (i9 + 15.99F) / 256F;
                    ++i3;
                    f18 = -0.2F;
                    if ((i2 + i3 + i4 & 1) == 0)
                    {
                        tessellator5.VertexUV(d29, i3 + f18, i4 + 0, d12, d14);
                        tessellator5.VertexUV(d21, i3 + 0, i4 + 0, d12, d16);
                        tessellator5.VertexUV(d21, i3 + 0, i4 + 1, d10, d16);
                        tessellator5.VertexUV(d29, i3 + f18, i4 + 1, d10, d14);
                        d10 = i8 / 256F;
                        d12 = (i8 + 15.99F) / 256F;
                        d14 = (i9 + 16) / 256F;
                        d16 = (i9 + 15.99F + 16F) / 256F;
                        tessellator5.VertexUV(d31, i3 + f18, i4 + 1, d12, d14);
                        tessellator5.VertexUV(d23, i3 + 0, i4 + 1, d12, d16);
                        tessellator5.VertexUV(d23, i3 + 0, i4 + 0, d10, d16);
                        tessellator5.VertexUV(d31, i3 + f18, i4 + 0, d10, d14);
                    }
                    else
                    {
                        tessellator5.VertexUV(i2 + 0, i3 + f18, d35, d12, d14);
                        tessellator5.VertexUV(i2 + 0, i3 + 0, d27, d12, d16);
                        tessellator5.VertexUV(i2 + 1, i3 + 0, d27, d10, d16);
                        tessellator5.VertexUV(i2 + 1, i3 + f18, d35, d10, d14);
                        d10 = i8 / 256F;
                        d12 = (i8 + 15.99F) / 256F;
                        d14 = (i9 + 16) / 256F;
                        d16 = (i9 + 15.99F + 16F) / 256F;
                        tessellator5.VertexUV(i2 + 1, i3 + f18, d33, d12, d14);
                        tessellator5.VertexUV(i2 + 1, i3 + 0, d25, d12, d16);
                        tessellator5.VertexUV(i2 + 0, i3 + 0, d25, d10, d16);
                        tessellator5.VertexUV(i2 + 0, i3 + f18, d33, d10, d14);
                    }
                }
            }
            else
            {
                double d19 = i2 + 0.5 + 0.2;
                d21 = i2 + 0.5 - 0.2;
                d23 = i4 + 0.5 + 0.2;
                d25 = i4 + 0.5 - 0.2;
                d27 = i2 + 0.5 - 0.3;
                d29 = i2 + 0.5 + 0.3;
                d31 = i4 + 0.5 - 0.3;
                d33 = i4 + 0.5 + 0.3;
                tessellator5.VertexUV(d27, i3 + f18, i4 + 1, d12, d14);
                tessellator5.VertexUV(d19, i3 + 0, i4 + 1, d12, d16);
                tessellator5.VertexUV(d19, i3 + 0, i4 + 0, d10, d16);
                tessellator5.VertexUV(d27, i3 + f18, i4 + 0, d10, d14);
                tessellator5.VertexUV(d29, i3 + f18, i4 + 0, d12, d14);
                tessellator5.VertexUV(d21, i3 + 0, i4 + 0, d12, d16);
                tessellator5.VertexUV(d21, i3 + 0, i4 + 1, d10, d16);
                tessellator5.VertexUV(d29, i3 + f18, i4 + 1, d10, d14);
                d10 = i8 / 256F;
                d12 = (i8 + 15.99F) / 256F;
                d14 = (i9 + 16) / 256F;
                d16 = (i9 + 15.99F + 16F) / 256F;
                tessellator5.VertexUV(i2 + 1, i3 + f18, d33, d12, d14);
                tessellator5.VertexUV(i2 + 1, i3 + 0, d25, d12, d16);
                tessellator5.VertexUV(i2 + 0, i3 + 0, d25, d10, d16);
                tessellator5.VertexUV(i2 + 0, i3 + f18, d33, d10, d14);
                tessellator5.VertexUV(i2 + 0, i3 + f18, d31, d12, d14);
                tessellator5.VertexUV(i2 + 0, i3 + 0, d23, d12, d16);
                tessellator5.VertexUV(i2 + 1, i3 + 0, d23, d10, d16);
                tessellator5.VertexUV(i2 + 1, i3 + f18, d31, d10, d14);
                d19 = i2 + 0.5 - 0.5;
                d21 = i2 + 0.5 + 0.5;
                d23 = i4 + 0.5 - 0.5;
                d25 = i4 + 0.5 + 0.5;
                d27 = i2 + 0.5 - 0.4;
                d29 = i2 + 0.5 + 0.4;
                d31 = i4 + 0.5 - 0.4;
                d33 = i4 + 0.5 + 0.4;
                tessellator5.VertexUV(d27, i3 + f18, i4 + 0, d10, d14);
                tessellator5.VertexUV(d19, i3 + 0, i4 + 0, d10, d16);
                tessellator5.VertexUV(d19, i3 + 0, i4 + 1, d12, d16);
                tessellator5.VertexUV(d27, i3 + f18, i4 + 1, d12, d14);
                tessellator5.VertexUV(d29, i3 + f18, i4 + 1, d10, d14);
                tessellator5.VertexUV(d21, i3 + 0, i4 + 1, d10, d16);
                tessellator5.VertexUV(d21, i3 + 0, i4 + 0, d12, d16);
                tessellator5.VertexUV(d29, i3 + f18, i4 + 0, d12, d14);
                d10 = i8 / 256F;
                d12 = (i8 + 15.99F) / 256F;
                d14 = i9 / 256F;
                d16 = (i9 + 15.99F) / 256F;
                tessellator5.VertexUV(i2 + 0, i3 + f18, d33, d10, d14);
                tessellator5.VertexUV(i2 + 0, i3 + 0, d25, d10, d16);
                tessellator5.VertexUV(i2 + 1, i3 + 0, d25, d12, d16);
                tessellator5.VertexUV(i2 + 1, i3 + f18, d33, d12, d14);
                tessellator5.VertexUV(i2 + 1, i3 + f18, d31, d10, d14);
                tessellator5.VertexUV(i2 + 1, i3 + 0, d23, d10, d16);
                tessellator5.VertexUV(i2 + 0, i3 + 0, d23, d12, d16);
                tessellator5.VertexUV(i2 + 0, i3 + f18, d31, d12, d14);
            }

            return true;
        }

        public virtual bool RenderBlockRedstoneWire(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            int i7 = block1.GetTexture(TileFace.UP, i6);
            if (this.overrideBlockTexture >= 0)
            {
                i7 = this.overrideBlockTexture;
            }

            float f8 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            float f9 = i6 / 15F;
            float f10 = f9 * 0.6F + 0.4F;
            if (i6 == 0)
            {
                f10 = 0.3F;
            }

            float f11 = f9 * f9 * 0.7F - 0.5F;
            float f12 = f9 * f9 * 0.6F - 0.7F;
            if (f11 < 0F)
            {
                f11 = 0F;
            }

            if (f12 < 0F)
            {
                f12 = 0F;
            }

            tessellator5.Color(f8 * f10, f8 * f11, f8 * f12);
            int i13 = (i7 & 15) << 4;
            int i14 = i7 & 240;
            double d15 = i13 / 256F;
            double d17 = (i13 + 15.99F) / 256F;
            double d19 = i14 / 256F;
            double d21 = (i14 + 15.99F) / 256F;
            bool z26 = WireTile.IsPowerProviderOrWire(this.blockAccess, i2 - 1, i3, i4, 1) || !this.blockAccess.IsSolidBlockingTile(i2 - 1, i3, i4) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2 - 1, i3 - 1, i4, -1);
            bool z27 = WireTile.IsPowerProviderOrWire(this.blockAccess, i2 + 1, i3, i4, 3) || !this.blockAccess.IsSolidBlockingTile(i2 + 1, i3, i4) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2 + 1, i3 - 1, i4, -1);
            bool z28 = WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3, i4 - 1, 2) || !this.blockAccess.IsSolidBlockingTile(i2, i3, i4 - 1) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3 - 1, i4 - 1, -1);
            bool z29 = WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3, i4 + 1, 0) || !this.blockAccess.IsSolidBlockingTile(i2, i3, i4 + 1) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3 - 1, i4 + 1, -1);
            if (!this.blockAccess.IsSolidBlockingTile(i2, i3 + 1, i4))
            {
                if (this.blockAccess.IsSolidBlockingTile(i2 - 1, i3, i4) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2 - 1, i3 + 1, i4, -1))
                {
                    z26 = true;
                }

                if (this.blockAccess.IsSolidBlockingTile(i2 + 1, i3, i4) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2 + 1, i3 + 1, i4, -1))
                {
                    z27 = true;
                }

                if (this.blockAccess.IsSolidBlockingTile(i2, i3, i4 - 1) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3 + 1, i4 - 1, -1))
                {
                    z28 = true;
                }

                if (this.blockAccess.IsSolidBlockingTile(i2, i3, i4 + 1) && WireTile.IsPowerProviderOrWire(this.blockAccess, i2, i3 + 1, i4 + 1, -1))
                {
                    z29 = true;
                }
            }

            float f31 = i2 + 0;
            float f32 = i2 + 1;
            float f33 = i4 + 0;
            float f34 = i4 + 1;
            byte b35 = 0;
            if ((z26 || z27) && !z28 && !z29)
            {
                b35 = 1;
            }

            if ((z28 || z29) && !z27 && !z26)
            {
                b35 = 2;
            }

            if (b35 != 0)
            {
                d15 = (i13 + 16) / 256F;
                d17 = (i13 + 16 + 15.99F) / 256F;
                d19 = i14 / 256F;
                d21 = (i14 + 15.99F) / 256F;
            }

            if (b35 == 0)
            {
                if (z27 || z28 || z29 || z26)
                {
                    if (!z26)
                    {
                        f31 += 0.3125F;
                    }

                    if (!z26)
                    {
                        d15 += 0.01953125;
                    }

                    if (!z27)
                    {
                        f32 -= 0.3125F;
                    }

                    if (!z27)
                    {
                        d17 -= 0.01953125;
                    }

                    if (!z28)
                    {
                        f33 += 0.3125F;
                    }

                    if (!z28)
                    {
                        d19 += 0.01953125;
                    }

                    if (!z29)
                    {
                        f34 -= 0.3125F;
                    }

                    if (!z29)
                    {
                        d21 -= 0.01953125;
                    }
                }

                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d17, d19);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d15, d21);
                tessellator5.Color(f8, f8, f8);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21 + 0.0625);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d17, d19 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d15, d21 + 0.0625);
            }
            else if (b35 == 1)
            {
                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d17, d19);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d15, d21);
                tessellator5.Color(f8, f8, f8);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21 + 0.0625);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d17, d19 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d15, d21 + 0.0625);
            }
            else if (b35 == 2)
            {
                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d15, d21);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d17, d19);
                tessellator5.Color(f8, f8, f8);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f34, d17, d21 + 0.0625);
                tessellator5.VertexUV(f32, i3 + 0.015625F, f33, d15, d21 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f33, d15, d19 + 0.0625);
                tessellator5.VertexUV(f31, i3 + 0.015625F, f34, d17, d19 + 0.0625);
            }

            if (!this.blockAccess.IsSolidBlockingTile(i2, i3 + 1, i4))
            {
                d15 = (i13 + 16) / 256F;
                d17 = (i13 + 16 + 15.99F) / 256F;
                d19 = i14 / 256F;
                d21 = (i14 + 15.99F) / 256F;
                if (this.blockAccess.IsSolidBlockingTile(i2 - 1, i3, i4) && this.blockAccess.GetTile(i2 - 1, i3 + 1, i4) == Tile.redstoneWire.id)
                {
                    tessellator5.Color(f8 * f10, f8 * f11, f8 * f12);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 1 + 0.021875F, i4 + 1, d17, d19);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 0, i4 + 1, d15, d19);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 0, i4 + 0, d15, d21);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 1 + 0.021875F, i4 + 0, d17, d21);
                    tessellator5.Color(f8, f8, f8);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 1 + 0.021875F, i4 + 1, d17, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 0, i4 + 1, d15, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 0, i4 + 0, d15, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 0.015625F, i3 + 1 + 0.021875F, i4 + 0, d17, d21 + 0.0625);
                }

                if (this.blockAccess.IsSolidBlockingTile(i2 + 1, i3, i4) && this.blockAccess.GetTile(i2 + 1, i3 + 1, i4) == Tile.redstoneWire.id)
                {
                    tessellator5.Color(f8 * f10, f8 * f11, f8 * f12);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 0, i4 + 1, d15, d21);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 1 + 0.021875F, i4 + 1, d17, d21);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 1 + 0.021875F, i4 + 0, d17, d19);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 0, i4 + 0, d15, d19);
                    tessellator5.Color(f8, f8, f8);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 0, i4 + 1, d15, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 1 + 0.021875F, i4 + 1, d17, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 1 + 0.021875F, i4 + 0, d17, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 1 - 0.015625F, i3 + 0, i4 + 0, d15, d19 + 0.0625);
                }

                if (this.blockAccess.IsSolidBlockingTile(i2, i3, i4 - 1) && this.blockAccess.GetTile(i2, i3 + 1, i4 - 1) == Tile.redstoneWire.id)
                {
                    tessellator5.Color(f8 * f10, f8 * f11, f8 * f12);
                    tessellator5.VertexUV(i2 + 1, i3 + 0, i4 + 0.015625F, d15, d21);
                    tessellator5.VertexUV(i2 + 1, i3 + 1 + 0.021875F, i4 + 0.015625F, d17, d21);
                    tessellator5.VertexUV(i2 + 0, i3 + 1 + 0.021875F, i4 + 0.015625F, d17, d19);
                    tessellator5.VertexUV(i2 + 0, i3 + 0, i4 + 0.015625F, d15, d19);
                    tessellator5.Color(f8, f8, f8);
                    tessellator5.VertexUV(i2 + 1, i3 + 0, i4 + 0.015625F, d15, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 1, i3 + 1 + 0.021875F, i4 + 0.015625F, d17, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 0, i3 + 1 + 0.021875F, i4 + 0.015625F, d17, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 0, i3 + 0, i4 + 0.015625F, d15, d19 + 0.0625);
                }

                if (this.blockAccess.IsSolidBlockingTile(i2, i3, i4 + 1) && this.blockAccess.GetTile(i2, i3 + 1, i4 + 1) == Tile.redstoneWire.id)
                {
                    tessellator5.Color(f8 * f10, f8 * f11, f8 * f12);
                    tessellator5.VertexUV(i2 + 1, i3 + 1 + 0.021875F, i4 + 1 - 0.015625F, d17, d19);
                    tessellator5.VertexUV(i2 + 1, i3 + 0, i4 + 1 - 0.015625F, d15, d19);
                    tessellator5.VertexUV(i2 + 0, i3 + 0, i4 + 1 - 0.015625F, d15, d21);
                    tessellator5.VertexUV(i2 + 0, i3 + 1 + 0.021875F, i4 + 1 - 0.015625F, d17, d21);
                    tessellator5.Color(f8, f8, f8);
                    tessellator5.VertexUV(i2 + 1, i3 + 1 + 0.021875F, i4 + 1 - 0.015625F, d17, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 1, i3 + 0, i4 + 1 - 0.015625F, d15, d19 + 0.0625);
                    tessellator5.VertexUV(i2 + 0, i3 + 0, i4 + 1 - 0.015625F, d15, d21 + 0.0625);
                    tessellator5.VertexUV(i2 + 0, i3 + 1 + 0.021875F, i4 + 1 - 0.015625F, d17, d21 + 0.0625);
                }
            }

            return true;
        }

        public virtual bool RenderBlockMinecartTrack(RailTile blockRail1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            int i7 = blockRail1.GetTexture(0, i6);
            if (this.overrideBlockTexture >= 0)
            {
                i7 = this.overrideBlockTexture;
            }

            if (blockRail1.IsPowered())
            {
                i6 &= 7;
            }

            float f8 = blockRail1.GetBrightness(this.blockAccess, i2, i3, i4);
            tessellator5.Color(f8, f8, f8);
            int i9 = (i7 & 15) << 4;
            int i10 = i7 & 240;
            double d11 = i9 / 256F;
            double d13 = (i9 + 15.99F) / 256F;
            double d15 = i10 / 256F;
            double d17 = (i10 + 15.99F) / 256F;
            float f19 = 0.0625F;
            float f20 = i2 + 1;
            float f21 = i2 + 1;
            float f22 = i2 + 0;
            float f23 = i2 + 0;
            float f24 = i4 + 0;
            float f25 = i4 + 1;
            float f26 = i4 + 1;
            float f27 = i4 + 0;
            float f28 = i3 + f19;
            float f29 = i3 + f19;
            float f30 = i3 + f19;
            float f31 = i3 + f19;
            if (i6 != 1 && i6 != 2 && i6 != 3 && i6 != 7)
            {
                if (i6 == 8)
                {
                    f20 = f21 = i2 + 0;
                    f22 = f23 = i2 + 1;
                    f24 = f27 = i4 + 1;
                    f25 = f26 = i4 + 0;
                }
                else if (i6 == 9)
                {
                    f20 = f23 = i2 + 0;
                    f21 = f22 = i2 + 1;
                    f24 = f25 = i4 + 0;
                    f26 = f27 = i4 + 1;
                }
            }
            else
            {
                f20 = f23 = i2 + 1;
                f21 = f22 = i2 + 0;
                f24 = f25 = i4 + 1;
                f26 = f27 = i4 + 0;
            }

            if (i6 != 2 && i6 != 4)
            {
                if (i6 == 3 || i6 == 5)
                {
                    ++f29;
                    ++f30;
                }
            }
            else
            {
                ++f28;
                ++f31;
            }

            tessellator5.VertexUV(f20, f28, f24, d13, d15);
            tessellator5.VertexUV(f21, f29, f25, d13, d17);
            tessellator5.VertexUV(f22, f30, f26, d11, d17);
            tessellator5.VertexUV(f23, f31, f27, d11, d15);
            tessellator5.VertexUV(f23, f31, f27, d11, d15);
            tessellator5.VertexUV(f22, f30, f26, d11, d17);
            tessellator5.VertexUV(f21, f29, f25, d13, d17);
            tessellator5.VertexUV(f20, f28, f24, d13, d15);
            return true;
        }

        public virtual bool RenderBlockLadder(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = block1.GetTexture(0);
            if (this.overrideBlockTexture >= 0)
            {
                i6 = this.overrideBlockTexture;
            }

            float f7 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            tessellator5.Color(f7, f7, f7);
            int i8 = (i6 & 15) << 4;
            int i9 = i6 & 240;
            double d10 = i8 / 256F;
            double d12 = (i8 + 15.99F) / 256F;
            double d14 = i9 / 256F;
            double d16 = (i9 + 15.99F) / 256F;
            int i18 = this.blockAccess.GetData(i2, i3, i4);
            float f19 = 0F;
            float f20 = 0.05F;
            if (i18 == 5)
            {
                tessellator5.VertexUV(i2 + f20, i3 + 1 + f19, i4 + 1 + f19, d10, d14);
                tessellator5.VertexUV(i2 + f20, i3 + 0 - f19, i4 + 1 + f19, d10, d16);
                tessellator5.VertexUV(i2 + f20, i3 + 0 - f19, i4 + 0 - f19, d12, d16);
                tessellator5.VertexUV(i2 + f20, i3 + 1 + f19, i4 + 0 - f19, d12, d14);
            }

            if (i18 == 4)
            {
                tessellator5.VertexUV(i2 + 1 - f20, i3 + 0 - f19, i4 + 1 + f19, d12, d16);
                tessellator5.VertexUV(i2 + 1 - f20, i3 + 1 + f19, i4 + 1 + f19, d12, d14);
                tessellator5.VertexUV(i2 + 1 - f20, i3 + 1 + f19, i4 + 0 - f19, d10, d14);
                tessellator5.VertexUV(i2 + 1 - f20, i3 + 0 - f19, i4 + 0 - f19, d10, d16);
            }

            if (i18 == 3)
            {
                tessellator5.VertexUV(i2 + 1 + f19, i3 + 0 - f19, i4 + f20, d12, d16);
                tessellator5.VertexUV(i2 + 1 + f19, i3 + 1 + f19, i4 + f20, d12, d14);
                tessellator5.VertexUV(i2 + 0 - f19, i3 + 1 + f19, i4 + f20, d10, d14);
                tessellator5.VertexUV(i2 + 0 - f19, i3 + 0 - f19, i4 + f20, d10, d16);
            }

            if (i18 == 2)
            {
                tessellator5.VertexUV(i2 + 1 + f19, i3 + 1 + f19, i4 + 1 - f20, d10, d14);
                tessellator5.VertexUV(i2 + 1 + f19, i3 + 0 - f19, i4 + 1 - f20, d10, d16);
                tessellator5.VertexUV(i2 + 0 - f19, i3 + 0 - f19, i4 + 1 - f20, d12, d16);
                tessellator5.VertexUV(i2 + 0 - f19, i3 + 1 + f19, i4 + 1 - f20, d12, d14);
            }

            return true;
        }

        public virtual bool RenderBlockReed(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            float f6 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            int i7 = block1.GetColor(this.blockAccess, i2, i3, i4);
            float f8 = (i7 >> 16 & 255) / 255F;
            float f9 = (i7 >> 8 & 255) / 255F;
            float f10 = (i7 & 255) / 255F;
            if (GameRenderer.field_28135_a)
            {
                float f11 = (f8 * 30F + f9 * 59F + f10 * 11F) / 100F;
                float f12 = (f8 * 30F + f9 * 70F) / 100F;
                float f13 = (f8 * 30F + f10 * 70F) / 100F;
                f8 = f11;
                f9 = f12;
                f10 = f13;
            }

            tessellator5.Color(f6 * f8, f6 * f9, f6 * f10);
            double d19 = i2;
            double d20 = i3;
            double d15 = i4;
            if (block1 == Tile.tallGrass)
            {
                long j17 = i2 * 3129871 ^ i4 * 116129781 ^ i3;
                j17 = j17 * j17 * 42317861 + j17 * 11;
                d19 += ((j17 >> 16 & 15) / 15F - 0.5) * 0.5;
                d20 += ((j17 >> 20 & 15) / 15F - 1) * 0.2;
                d15 += ((j17 >> 24 & 15) / 15F - 0.5) * 0.5;
            }

            this.RenderCrossedSquares(block1, this.blockAccess.GetData(i2, i3, i4), d19, d20, d15);
            return true;
        }

        public virtual bool RenderBlockCrops(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            float f6 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            tessellator5.Color(f6, f6, f6);
            this.Func_1245_b(block1, this.blockAccess.GetData(i2, i3, i4), i2, i3 - 0.0625F, i4);
            return true;
        }

        public virtual void RenderTorchAtAngle(Tile block1, double d2, double d4, double d6, double d8, double d10)
        {
            Tessellator tessellator12 = Tessellator.Instance;
            int i13 = block1.GetTexture(0);
            if (this.overrideBlockTexture >= 0)
            {
                i13 = this.overrideBlockTexture;
            }

            int i14 = (i13 & 15) << 4;
            int i15 = i13 & 240;
            float f16 = i14 / 256F;
            float f17 = (i14 + 15.99F) / 256F;
            float f18 = i15 / 256F;
            float f19 = (i15 + 15.99F) / 256F;
            double d20 = f16 + 7 / 256;
            double d22 = f18 + 6 / 256;
            double d24 = f16 + 9 / 256;
            double d26 = f18 + 8 / 256;
            d2 += 0.5;
            d6 += 0.5;
            double d28 = d2 - 0.5;
            double d30 = d2 + 0.5;
            double d32 = d6 - 0.5;
            double d34 = d6 + 0.5;
            double d36 = 0.0625;
            double d38 = 0.625;
            tessellator12.VertexUV(d2 + d8 * (1 - d38) - d36, d4 + d38, d6 + d10 * (1 - d38) - d36, d20, d22);
            tessellator12.VertexUV(d2 + d8 * (1 - d38) - d36, d4 + d38, d6 + d10 * (1 - d38) + d36, d20, d26);
            tessellator12.VertexUV(d2 + d8 * (1 - d38) + d36, d4 + d38, d6 + d10 * (1 - d38) + d36, d24, d26);
            tessellator12.VertexUV(d2 + d8 * (1 - d38) + d36, d4 + d38, d6 + d10 * (1 - d38) - d36, d24, d22);
            tessellator12.VertexUV(d2 - d36, d4 + 1, d32, f16, f18);
            tessellator12.VertexUV(d2 - d36 + d8, d4 + 0, d32 + d10, f16, f19);
            tessellator12.VertexUV(d2 - d36 + d8, d4 + 0, d34 + d10, f17, f19);
            tessellator12.VertexUV(d2 - d36, d4 + 1, d34, f17, f18);
            tessellator12.VertexUV(d2 + d36, d4 + 1, d34, f16, f18);
            tessellator12.VertexUV(d2 + d8 + d36, d4 + 0, d34 + d10, f16, f19);
            tessellator12.VertexUV(d2 + d8 + d36, d4 + 0, d32 + d10, f17, f19);
            tessellator12.VertexUV(d2 + d36, d4 + 1, d32, f17, f18);
            tessellator12.VertexUV(d28, d4 + 1, d6 + d36, f16, f18);
            tessellator12.VertexUV(d28 + d8, d4 + 0, d6 + d36 + d10, f16, f19);
            tessellator12.VertexUV(d30 + d8, d4 + 0, d6 + d36 + d10, f17, f19);
            tessellator12.VertexUV(d30, d4 + 1, d6 + d36, f17, f18);
            tessellator12.VertexUV(d30, d4 + 1, d6 - d36, f16, f18);
            tessellator12.VertexUV(d30 + d8, d4 + 0, d6 - d36 + d10, f16, f19);
            tessellator12.VertexUV(d28 + d8, d4 + 0, d6 - d36 + d10, f17, f19);
            tessellator12.VertexUV(d28, d4 + 1, d6 - d36, f17, f18);
        }

        public virtual void RenderCrossedSquares(Tile block1, int i2, double d3, double d5, double d7)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            int i10 = block1.GetTexture(0, i2);
            if (this.overrideBlockTexture >= 0)
            {
                i10 = this.overrideBlockTexture;
            }

            int i11 = (i10 & 15) << 4;
            int i12 = i10 & 240;
            double d13 = i11 / 256F;
            double d15 = (i11 + 15.99F) / 256F;
            double d17 = i12 / 256F;
            double d19 = (i12 + 15.99F) / 256F;
            double d21 = d3 + 0.5 - 0.45F;
            double d23 = d3 + 0.5 + 0.45F;
            double d25 = d7 + 0.5 - 0.45F;
            double d27 = d7 + 0.5 + 0.45F;
            tessellator9.VertexUV(d21, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d27, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d21, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d27, d15, d17);
        }

        public virtual void Func_1245_b(Tile block1, int i2, double d3, double d5, double d7)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            int i10 = block1.GetTexture(0, i2);
            if (this.overrideBlockTexture >= 0)
            {
                i10 = this.overrideBlockTexture;
            }

            int i11 = (i10 & 15) << 4;
            int i12 = i10 & 240;
            double d13 = i11 / 256F;
            double d15 = (i11 + 15.99F) / 256F;
            double d17 = i12 / 256F;
            double d19 = (i12 + 15.99F) / 256F;
            double d21 = d3 + 0.5 - 0.25;
            double d23 = d3 + 0.5 + 0.25;
            double d25 = d7 + 0.5 - 0.5;
            double d27 = d7 + 0.5 + 0.5;
            tessellator9.VertexUV(d21, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d27, d15, d17);
            tessellator9.VertexUV(d21, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d27, d15, d17);
            d21 = d3 + 0.5 - 0.5;
            d23 = d3 + 0.5 + 0.5;
            d25 = d7 + 0.5 - 0.25;
            d27 = d7 + 0.5 + 0.25;
            tessellator9.VertexUV(d21, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d25, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d25, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d25, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d25, d15, d17);
            tessellator9.VertexUV(d23, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d23, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d21, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d21, d5 + 1, d27, d15, d17);
            tessellator9.VertexUV(d21, d5 + 1, d27, d13, d17);
            tessellator9.VertexUV(d21, d5 + 0, d27, d13, d19);
            tessellator9.VertexUV(d23, d5 + 0, d27, d15, d19);
            tessellator9.VertexUV(d23, d5 + 1, d27, d15, d17);
        }

        public virtual bool RenderBlockFluids(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            int i6 = block1.GetColor(this.blockAccess, i2, i3, i4);
            float f7 = (i6 >> 16 & 255) / 255F;
            float f8 = (i6 >> 8 & 255) / 255F;
            float f9 = (i6 & 255) / 255F;
            bool z10 = block1.ShouldRenderFace(this.blockAccess, i2, i3 + 1, i4, 1);
            bool z11 = block1.ShouldRenderFace(this.blockAccess, i2, i3 - 1, i4, 0);
            bool[] z12 = new[]
            {
                block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 - 1, 2),
                block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 + 1, 3),
                block1.ShouldRenderFace(this.blockAccess, i2 - 1, i3, i4, 4),
                block1.ShouldRenderFace(this.blockAccess, i2 + 1, i3, i4, 5)
            };
            if (!z10 && !z11 && !z12[0] && !z12[1] && !z12[2] && !z12[3])
            {
                return false;
            }
            else
            {
                bool z13 = false;
                float f14 = 0.5F;
                float f15 = 1F;
                float f16 = 0.8F;
                float f17 = 0.6F;
                double d18 = 0;
                double d20 = 1;
                Material material22 = block1.material;
                int i23 = this.blockAccess.GetData(i2, i3, i4);
                float f24 = this.Func_1224_a(i2, i3, i4, material22);
                float f25 = this.Func_1224_a(i2, i3, i4 + 1, material22);
                float f26 = this.Func_1224_a(i2 + 1, i3, i4 + 1, material22);
                float f27 = this.Func_1224_a(i2 + 1, i3, i4, material22);
                int tex;
                int i31;
                float f36;
                float f37;
                float f38;
                if (this.renderAllFaces || z10)
                {
                    z13 = true;
                    tex = block1.GetTexture(TileFace.UP, i23);
                    float f29 = (float)LiquidTile.GetSlopeAngle(this.blockAccess, i2, i3, i4, material22);
                    if (f29 > -999F)
                    {
                        tex = block1.GetTexture(TileFace.NORTH, i23);
                    }

                    int i30 = (tex & 15) << 4;
                    i31 = tex & 240;
                    double d32 = ((double)i30 + 8D) / 256D;
                    double d34 = ((double)i31 + 8D) / 256D;
                    if (f29 < -999F)
                    {
                        f29 = 0F;
                    }
                    else
                    {
                        d32 = (i30 + 16) / 256F;
                        d34 = (i31 + 16) / 256F;
                    }

                    f36 = Mth.Sin(f29) * 8F / 256F;
                    f37 = Mth.Cos(f29) * 8F / 256F;
                    f38 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
                    tessellator5.Color(f15 * f38 * f7, f15 * f38 * f8, f15 * f38 * f9);
                    tessellator5.VertexUV(i2 + 0, i3 + f24, i4 + 0, d32 - f37 - f36, d34 - f37 + f36);
                    tessellator5.VertexUV(i2 + 0, i3 + f25, i4 + 1, d32 - f37 + f36, d34 + f37 + f36);
                    tessellator5.VertexUV(i2 + 1, i3 + f26, i4 + 1, d32 + f37 + f36, d34 + f37 - f36);
                    tessellator5.VertexUV(i2 + 1, i3 + f27, i4 + 0, d32 + f37 - f36, d34 - f37 - f36);
                }

                if (this.renderAllFaces || z11)
                {
                    float f52 = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                    tessellator5.Color(f14 * f52, f14 * f52, f14 * f52);
                    this.RenderBottomFace(block1, i2, i3, i4, block1.GetTexture(0));
                    z13 = true;
                }

                for (tex = 0; tex < 4; ++tex)
                {
                    int i53 = i2;
                    i31 = i4;
                    if (tex == 0)
                    {
                        i31 = i4 - 1;
                    }

                    if (tex == 1)
                    {
                        ++i31;
                    }

                    if (tex == 2)
                    {
                        i53 = i2 - 1;
                    }

                    if (tex == 3)
                    {
                        ++i53;
                    }

                    int i54 = block1.GetTexture((TileFace)tex + 2, i23);
                    int i33 = (i54 & 15) << 4;
                    int i55 = i54 & 240;
                    if (this.renderAllFaces || z12[tex])
                    {
                        float f35;
                        float f39;
                        float f40;
                        if (tex == 0)
                        {
                            f35 = f24;
                            f36 = f27;
                            f37 = i2;
                            f39 = i2 + 1;
                            f38 = i4;
                            f40 = i4;
                        }
                        else if (tex == 1)
                        {
                            f35 = f26;
                            f36 = f25;
                            f37 = i2 + 1;
                            f39 = i2;
                            f38 = i4 + 1;
                            f40 = i4 + 1;
                        }
                        else if (tex == 2)
                        {
                            f35 = f25;
                            f36 = f24;
                            f37 = i2;
                            f39 = i2;
                            f38 = i4 + 1;
                            f40 = i4;
                        }
                        else
                        {
                            f35 = f27;
                            f36 = f26;
                            f37 = i2 + 1;
                            f39 = i2 + 1;
                            f38 = i4;
                            f40 = i4 + 1;
                        }

                        z13 = true;
                        double d41 = (i33 + 0) / 256F;
                        double d43 = (i33 + 16 - 0.01) / 256;
                        double d45 = (i55 + (1F - f35) * 16F) / 256F;
                        double d47 = (i55 + (1F - f36) * 16F) / 256F;
                        double d49 = (i55 + 16 - 0.01) / 256;
                        float f51 = block1.GetBrightness(this.blockAccess, i53, i3, i31);
                        if (tex < 2)
                        {
                            f51 *= f16;
                        }
                        else
                        {
                            f51 *= f17;
                        }

                        tessellator5.Color(f15 * f51 * f7, f15 * f51 * f8, f15 * f51 * f9);
                        tessellator5.VertexUV(f37, i3 + f35, f38, d41, d45);
                        tessellator5.VertexUV(f39, i3 + f36, f40, d43, d47);
                        tessellator5.VertexUV(f39, i3 + 0, f40, d43, d49);
                        tessellator5.VertexUV(f37, i3 + 0, f38, d41, d49);
                    }
                }

                block1.minY = d18;
                block1.maxY = d20;
                return z13;
            }
        }

        private float Func_1224_a(int i1, int i2, int i3, Material material4)
        {
            int i5 = 0;
            float f6 = 0F;
            for (int i7 = 0; i7 < 4; ++i7)
            {
                int i8 = i1 - (i7 & 1);
                int i10 = i3 - (i7 >> 1 & 1);
                if (this.blockAccess.GetMaterial(i8, i2 + 1, i10) == material4)
                {
                    return 1F;
                }

                Material material11 = this.blockAccess.GetMaterial(i8, i2, i10);
                if (material11 != material4)
                {
                    if (!material11.IsSolid())
                    {
                        ++f6;
                        ++i5;
                    }
                }
                else
                {
                    int i12 = this.blockAccess.GetData(i8, i2, i10);
                    if (i12 >= 8 || i12 == 0)
                    {
                        f6 += LiquidTile.GetHeight(i12) * 10F;
                        i5 += 10;
                    }

                    f6 += LiquidTile.GetHeight(i12);
                    ++i5;
                }
            }

            return 1F - f6 / i5;
        }

        public virtual void RenderBlockFallingSand(Tile block1, Level world2, int i3, int i4, int i5)
        {
            float f6 = 0.5F;
            float f7 = 1F;
            float f8 = 0.8F;
            float f9 = 0.6F;
            Tessellator tessellator10 = Tessellator.Instance;
            tessellator10.Begin();
            float f11 = block1.GetBrightness(world2, i3, i4, i5);
            float f12 = block1.GetBrightness(world2, i3, i4 - 1, i5);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f6 * f12, f6 * f12, f6 * f12);
            this.RenderBottomFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.DOWN));
            f12 = block1.GetBrightness(world2, i3, i4 + 1, i5);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f7 * f12, f7 * f12, f7 * f12);
            this.RenderTopFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.UP));
            f12 = block1.GetBrightness(world2, i3, i4, i5 - 1);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f8 * f12, f8 * f12, f8 * f12);
            this.RenderEastFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.NORTH));
            f12 = block1.GetBrightness(world2, i3, i4, i5 + 1);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f8 * f12, f8 * f12, f8 * f12);
            this.RenderWestFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.SOUTH));
            f12 = block1.GetBrightness(world2, i3 - 1, i4, i5);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f9 * f12, f9 * f12, f9 * f12);
            this.RenderNorthFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.WEST));
            f12 = block1.GetBrightness(world2, i3 + 1, i4, i5);
            if (f12 < f11)
            {
                f12 = f11;
            }

            tessellator10.Color(f9 * f12, f9 * f12, f9 * f12);
            this.RenderSouthFace(block1, -0.5, -0.5, -0.5, block1.GetTexture(TileFace.EAST));
            tessellator10.End();
        }

        public virtual bool RenderStandardBlock(Tile block1, int i2, int i3, int i4)
        {
            int i5 = block1.GetColor(this.blockAccess, i2, i3, i4);
            float f6 = (i5 >> 16 & 255) / 255F;
            float f7 = (i5 >> 8 & 255) / 255F;
            float f8 = (i5 & 255) / 255F;
            if (GameRenderer.field_28135_a)
            {
                float f9 = (f6 * 30F + f7 * 59F + f8 * 11F) / 100F;
                float f10 = (f6 * 30F + f7 * 70F) / 100F;
                float f11 = (f6 * 30F + f8 * 70F) / 100F;
                f6 = f9;
                f7 = f10;
                f8 = f11;
            }

            return Client.IsAmbientOcclusionEnabled() ? this.RenderStandardBlockWithAmbientOcclusion(block1, i2, i3, i4, f6, f7, f8) : this.RenderStandardBlockWithColorMultiplier(block1, i2, i3, i4, f6, f7, f8);
        }

        public virtual bool RenderStandardBlockWithAmbientOcclusion(Tile block1, int i2, int i3, int i4, float f5, float f6, float f7)
        {
            this.enableAO = true;
            bool z8 = false;
            float f9 = this.lightValueOwn;
            float f10 = this.lightValueOwn;
            float f11 = this.lightValueOwn;
            float f12 = this.lightValueOwn;
            bool z13 = true;
            bool z14 = true;
            bool z15 = true;
            bool z16 = true;
            bool z17 = true;
            bool z18 = true;
            this.lightValueOwn = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            this.aoLightValueXNeg = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
            this.aoLightValueYNeg = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
            this.aoLightValueZNeg = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
            this.aoLightValueXPos = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
            this.aoLightValueYPos = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
            this.aoLightValueZPos = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
            this.field_22338_U = Tile.translucent[this.blockAccess.GetTile(i2 + 1, i3 + 1, i4)];
            this.field_22359_ac = Tile.translucent[this.blockAccess.GetTile(i2 + 1, i3 - 1, i4)];
            this.field_22334_Y = Tile.translucent[this.blockAccess.GetTile(i2 + 1, i3, i4 + 1)];
            this.field_22363_aa = Tile.translucent[this.blockAccess.GetTile(i2 + 1, i3, i4 - 1)];
            this.field_22337_V = Tile.translucent[this.blockAccess.GetTile(i2 - 1, i3 + 1, i4)];
            this.field_22357_ad = Tile.translucent[this.blockAccess.GetTile(i2 - 1, i3 - 1, i4)];
            this.field_22335_X = Tile.translucent[this.blockAccess.GetTile(i2 - 1, i3, i4 - 1)];
            this.field_22333_Z = Tile.translucent[this.blockAccess.GetTile(i2 - 1, i3, i4 + 1)];
            this.field_22336_W = Tile.translucent[this.blockAccess.GetTile(i2, i3 + 1, i4 + 1)];
            this.field_22339_T = Tile.translucent[this.blockAccess.GetTile(i2, i3 + 1, i4 - 1)];
            this.field_22355_ae = Tile.translucent[this.blockAccess.GetTile(i2, i3 - 1, i4 + 1)];
            this.field_22361_ab = Tile.translucent[this.blockAccess.GetTile(i2, i3 - 1, i4 - 1)];
            if (block1.texture == 3)
            {
                z18 = false;
                z17 = false;
                z16 = false;
                z15 = false;
                z13 = false;
            }

            if (this.overrideBlockTexture >= 0)
            {
                z18 = false;
                z17 = false;
                z16 = false;
                z15 = false;
                z13 = false;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 - 1, i4, 0))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueYNeg;
                    f11 = this.aoLightValueYNeg;
                    f10 = this.aoLightValueYNeg;
                    f9 = this.aoLightValueYNeg;
                }
                else
                {
                    --i3;
                    this.field_22376_n = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                    this.field_22374_p = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                    this.field_22373_q = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                    this.field_22371_s = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                    if (!this.field_22361_ab && !this.field_22357_ad)
                    {
                        this.field_22377_m = this.field_22376_n;
                    }
                    else
                    {
                        this.field_22377_m = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4 - 1);
                    }

                    if (!this.field_22355_ae && !this.field_22357_ad)
                    {
                        this.field_22375_o = this.field_22376_n;
                    }
                    else
                    {
                        this.field_22375_o = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4 + 1);
                    }

                    if (!this.field_22361_ab && !this.field_22359_ac)
                    {
                        this.field_22372_r = this.field_22371_s;
                    }
                    else
                    {
                        this.field_22372_r = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4 - 1);
                    }

                    if (!this.field_22355_ae && !this.field_22359_ac)
                    {
                        this.field_22370_t = this.field_22371_s;
                    }
                    else
                    {
                        this.field_22370_t = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4 + 1);
                    }

                    ++i3;
                    f9 = (this.field_22375_o + this.field_22376_n + this.field_22373_q + this.aoLightValueYNeg) / 4F;
                    f12 = (this.field_22373_q + this.aoLightValueYNeg + this.field_22370_t + this.field_22371_s) / 4F;
                    f11 = (this.aoLightValueYNeg + this.field_22374_p + this.field_22371_s + this.field_22372_r) / 4F;
                    f10 = (this.field_22376_n + this.field_22377_m + this.aoLightValueYNeg + this.field_22374_p) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = (z13 ? f5 : 1F) * 0.5F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = (z13 ? f6 : 1F) * 0.5F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = (z13 ? f7 : 1F) * 0.5F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                this.RenderBottomFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, 0));
                z8 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 + 1, i4, 1))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueYPos;
                    f11 = this.aoLightValueYPos;
                    f10 = this.aoLightValueYPos;
                    f9 = this.aoLightValueYPos;
                }
                else
                {
                    ++i3;
                    this.field_22368_v = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                    this.field_22364_z = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                    this.field_22366_x = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                    this.field_22362_A = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                    if (!this.field_22339_T && !this.field_22337_V)
                    {
                        this.field_22369_u = this.field_22368_v;
                    }
                    else
                    {
                        this.field_22369_u = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4 - 1);
                    }

                    if (!this.field_22339_T && !this.field_22338_U)
                    {
                        this.field_22365_y = this.field_22364_z;
                    }
                    else
                    {
                        this.field_22365_y = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4 - 1);
                    }

                    if (!this.field_22336_W && !this.field_22337_V)
                    {
                        this.field_22367_w = this.field_22368_v;
                    }
                    else
                    {
                        this.field_22367_w = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4 + 1);
                    }

                    if (!this.field_22336_W && !this.field_22338_U)
                    {
                        this.field_22360_B = this.field_22364_z;
                    }
                    else
                    {
                        this.field_22360_B = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4 + 1);
                    }

                    --i3;
                    f12 = (this.field_22367_w + this.field_22368_v + this.field_22362_A + this.aoLightValueYPos) / 4F;
                    f9 = (this.field_22362_A + this.aoLightValueYPos + this.field_22360_B + this.field_22364_z) / 4F;
                    f10 = (this.aoLightValueYPos + this.field_22366_x + this.field_22364_z + this.field_22365_y) / 4F;
                    f11 = (this.field_22368_v + this.field_22369_u + this.aoLightValueYPos + this.field_22366_x) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = z14 ? f5 : 1F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = z14 ? f6 : 1F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = z14 ? f7 : 1F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                this.RenderTopFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.UP));
                z8 = true;
            }

            int i19;
            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 - 1, 2))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueZNeg;
                    f11 = this.aoLightValueZNeg;
                    f10 = this.aoLightValueZNeg;
                    f9 = this.aoLightValueZNeg;
                }
                else
                {
                    --i4;
                    this.field_22358_C = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                    this.field_22374_p = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                    this.field_22366_x = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                    this.field_22356_D = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                    if (!this.field_22335_X && !this.field_22361_ab)
                    {
                        this.field_22377_m = this.field_22358_C;
                    }
                    else
                    {
                        this.field_22377_m = block1.GetBrightness(this.blockAccess, i2 - 1, i3 - 1, i4);
                    }

                    if (!this.field_22335_X && !this.field_22339_T)
                    {
                        this.field_22369_u = this.field_22358_C;
                    }
                    else
                    {
                        this.field_22369_u = block1.GetBrightness(this.blockAccess, i2 - 1, i3 + 1, i4);
                    }

                    if (!this.field_22363_aa && !this.field_22361_ab)
                    {
                        this.field_22372_r = this.field_22356_D;
                    }
                    else
                    {
                        this.field_22372_r = block1.GetBrightness(this.blockAccess, i2 + 1, i3 - 1, i4);
                    }

                    if (!this.field_22363_aa && !this.field_22339_T)
                    {
                        this.field_22365_y = this.field_22356_D;
                    }
                    else
                    {
                        this.field_22365_y = block1.GetBrightness(this.blockAccess, i2 + 1, i3 + 1, i4);
                    }

                    ++i4;
                    f9 = (this.field_22358_C + this.field_22369_u + this.aoLightValueZNeg + this.field_22366_x) / 4F;
                    f10 = (this.aoLightValueZNeg + this.field_22366_x + this.field_22356_D + this.field_22365_y) / 4F;
                    f11 = (this.field_22374_p + this.aoLightValueZNeg + this.field_22372_r + this.field_22356_D) / 4F;
                    f12 = (this.field_22377_m + this.field_22358_C + this.field_22374_p + this.aoLightValueZNeg) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = (z15 ? f5 : 1F) * 0.8F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = (z15 ? f6 : 1F) * 0.8F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = (z15 ? f7 : 1F) * 0.8F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                i19 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.NORTH);
                this.RenderEastFace(block1, i2, i3, i4, i19);
                if (fancyGrass && i19 == 3 && this.overrideBlockTexture < 0)
                {
                    this.colorRedTopLeft *= f5;
                    this.colorRedBottomLeft *= f5;
                    this.colorRedBottomRight *= f5;
                    this.colorRedTopRight *= f5;
                    this.colorGreenTopLeft *= f6;
                    this.colorGreenBottomLeft *= f6;
                    this.colorGreenBottomRight *= f6;
                    this.colorGreenTopRight *= f6;
                    this.colorBlueTopLeft *= f7;
                    this.colorBlueBottomLeft *= f7;
                    this.colorBlueBottomRight *= f7;
                    this.colorBlueTopRight *= f7;
                    this.RenderEastFace(block1, i2, i3, i4, 38);
                }

                z8 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 + 1, 3))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueZPos;
                    f11 = this.aoLightValueZPos;
                    f10 = this.aoLightValueZPos;
                    f9 = this.aoLightValueZPos;
                }
                else
                {
                    ++i4;
                    this.field_22354_E = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                    this.field_22353_F = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                    this.field_22373_q = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                    this.field_22362_A = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                    if (!this.field_22333_Z && !this.field_22355_ae)
                    {
                        this.field_22375_o = this.field_22354_E;
                    }
                    else
                    {
                        this.field_22375_o = block1.GetBrightness(this.blockAccess, i2 - 1, i3 - 1, i4);
                    }

                    if (!this.field_22333_Z && !this.field_22336_W)
                    {
                        this.field_22367_w = this.field_22354_E;
                    }
                    else
                    {
                        this.field_22367_w = block1.GetBrightness(this.blockAccess, i2 - 1, i3 + 1, i4);
                    }

                    if (!this.field_22334_Y && !this.field_22355_ae)
                    {
                        this.field_22370_t = this.field_22353_F;
                    }
                    else
                    {
                        this.field_22370_t = block1.GetBrightness(this.blockAccess, i2 + 1, i3 - 1, i4);
                    }

                    if (!this.field_22334_Y && !this.field_22336_W)
                    {
                        this.field_22360_B = this.field_22353_F;
                    }
                    else
                    {
                        this.field_22360_B = block1.GetBrightness(this.blockAccess, i2 + 1, i3 + 1, i4);
                    }

                    --i4;
                    f9 = (this.field_22354_E + this.field_22367_w + this.aoLightValueZPos + this.field_22362_A) / 4F;
                    f12 = (this.aoLightValueZPos + this.field_22362_A + this.field_22353_F + this.field_22360_B) / 4F;
                    f11 = (this.field_22373_q + this.aoLightValueZPos + this.field_22370_t + this.field_22353_F) / 4F;
                    f10 = (this.field_22375_o + this.field_22354_E + this.field_22373_q + this.aoLightValueZPos) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = (z16 ? f5 : 1F) * 0.8F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = (z16 ? f6 : 1F) * 0.8F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = (z16 ? f7 : 1F) * 0.8F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                i19 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH);
                this.RenderWestFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH));
                if (fancyGrass && i19 == 3 && this.overrideBlockTexture < 0)
                {
                    this.colorRedTopLeft *= f5;
                    this.colorRedBottomLeft *= f5;
                    this.colorRedBottomRight *= f5;
                    this.colorRedTopRight *= f5;
                    this.colorGreenTopLeft *= f6;
                    this.colorGreenBottomLeft *= f6;
                    this.colorGreenBottomRight *= f6;
                    this.colorGreenTopRight *= f6;
                    this.colorBlueTopLeft *= f7;
                    this.colorBlueBottomLeft *= f7;
                    this.colorBlueBottomRight *= f7;
                    this.colorBlueTopRight *= f7;
                    this.RenderWestFace(block1, i2, i3, i4, 38);
                }

                z8 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 - 1, i3, i4, 4))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueXNeg;
                    f11 = this.aoLightValueXNeg;
                    f10 = this.aoLightValueXNeg;
                    f9 = this.aoLightValueXNeg;
                }
                else
                {
                    --i2;
                    this.field_22376_n = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                    this.field_22358_C = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                    this.field_22354_E = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                    this.field_22368_v = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                    if (!this.field_22335_X && !this.field_22357_ad)
                    {
                        this.field_22377_m = this.field_22358_C;
                    }
                    else
                    {
                        this.field_22377_m = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4 - 1);
                    }

                    if (!this.field_22333_Z && !this.field_22357_ad)
                    {
                        this.field_22375_o = this.field_22354_E;
                    }
                    else
                    {
                        this.field_22375_o = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4 + 1);
                    }

                    if (!this.field_22335_X && !this.field_22337_V)
                    {
                        this.field_22369_u = this.field_22358_C;
                    }
                    else
                    {
                        this.field_22369_u = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4 - 1);
                    }

                    if (!this.field_22333_Z && !this.field_22337_V)
                    {
                        this.field_22367_w = this.field_22354_E;
                    }
                    else
                    {
                        this.field_22367_w = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4 + 1);
                    }

                    ++i2;
                    f12 = (this.field_22376_n + this.field_22375_o + this.aoLightValueXNeg + this.field_22354_E) / 4F;
                    f9 = (this.aoLightValueXNeg + this.field_22354_E + this.field_22368_v + this.field_22367_w) / 4F;
                    f10 = (this.field_22358_C + this.aoLightValueXNeg + this.field_22369_u + this.field_22368_v) / 4F;
                    f11 = (this.field_22377_m + this.field_22376_n + this.field_22358_C + this.aoLightValueXNeg) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = (z17 ? f5 : 1F) * 0.6F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = (z17 ? f6 : 1F) * 0.6F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = (z17 ? f7 : 1F) * 0.6F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                i19 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.WEST);
                this.RenderNorthFace(block1, i2, i3, i4, i19);
                if (fancyGrass && i19 == 3 && this.overrideBlockTexture < 0)
                {
                    this.colorRedTopLeft *= f5;
                    this.colorRedBottomLeft *= f5;
                    this.colorRedBottomRight *= f5;
                    this.colorRedTopRight *= f5;
                    this.colorGreenTopLeft *= f6;
                    this.colorGreenBottomLeft *= f6;
                    this.colorGreenBottomRight *= f6;
                    this.colorGreenTopRight *= f6;
                    this.colorBlueTopLeft *= f7;
                    this.colorBlueBottomLeft *= f7;
                    this.colorBlueBottomRight *= f7;
                    this.colorBlueTopRight *= f7;
                    this.RenderNorthFace(block1, i2, i3, i4, 38);
                }

                z8 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 + 1, i3, i4, 5))
            {
                if (this.field_22352_G <= 0)
                {
                    f12 = this.aoLightValueXPos;
                    f11 = this.aoLightValueXPos;
                    f10 = this.aoLightValueXPos;
                    f9 = this.aoLightValueXPos;
                }
                else
                {
                    ++i2;
                    this.field_22371_s = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                    this.field_22356_D = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                    this.field_22353_F = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                    this.field_22364_z = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                    if (!this.field_22359_ac && !this.field_22363_aa)
                    {
                        this.field_22372_r = this.field_22356_D;
                    }
                    else
                    {
                        this.field_22372_r = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4 - 1);
                    }

                    if (!this.field_22359_ac && !this.field_22334_Y)
                    {
                        this.field_22370_t = this.field_22353_F;
                    }
                    else
                    {
                        this.field_22370_t = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4 + 1);
                    }

                    if (!this.field_22338_U && !this.field_22363_aa)
                    {
                        this.field_22365_y = this.field_22356_D;
                    }
                    else
                    {
                        this.field_22365_y = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4 - 1);
                    }

                    if (!this.field_22338_U && !this.field_22334_Y)
                    {
                        this.field_22360_B = this.field_22353_F;
                    }
                    else
                    {
                        this.field_22360_B = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4 + 1);
                    }

                    --i2;
                    f9 = (this.field_22371_s + this.field_22370_t + this.aoLightValueXPos + this.field_22353_F) / 4F;
                    f12 = (this.aoLightValueXPos + this.field_22353_F + this.field_22364_z + this.field_22360_B) / 4F;
                    f11 = (this.field_22356_D + this.aoLightValueXPos + this.field_22365_y + this.field_22364_z) / 4F;
                    f10 = (this.field_22372_r + this.field_22371_s + this.field_22356_D + this.aoLightValueXPos) / 4F;
                }

                this.colorRedTopLeft = this.colorRedBottomLeft = this.colorRedBottomRight = this.colorRedTopRight = (z18 ? f5 : 1F) * 0.6F;
                this.colorGreenTopLeft = this.colorGreenBottomLeft = this.colorGreenBottomRight = this.colorGreenTopRight = (z18 ? f6 : 1F) * 0.6F;
                this.colorBlueTopLeft = this.colorBlueBottomLeft = this.colorBlueBottomRight = this.colorBlueTopRight = (z18 ? f7 : 1F) * 0.6F;
                this.colorRedTopLeft *= f9;
                this.colorGreenTopLeft *= f9;
                this.colorBlueTopLeft *= f9;
                this.colorRedBottomLeft *= f10;
                this.colorGreenBottomLeft *= f10;
                this.colorBlueBottomLeft *= f10;
                this.colorRedBottomRight *= f11;
                this.colorGreenBottomRight *= f11;
                this.colorBlueBottomRight *= f11;
                this.colorRedTopRight *= f12;
                this.colorGreenTopRight *= f12;
                this.colorBlueTopRight *= f12;
                i19 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.EAST);
                this.RenderSouthFace(block1, i2, i3, i4, i19);
                if (fancyGrass && i19 == 3 && this.overrideBlockTexture < 0)
                {
                    this.colorRedTopLeft *= f5;
                    this.colorRedBottomLeft *= f5;
                    this.colorRedBottomRight *= f5;
                    this.colorRedTopRight *= f5;
                    this.colorGreenTopLeft *= f6;
                    this.colorGreenBottomLeft *= f6;
                    this.colorGreenBottomRight *= f6;
                    this.colorGreenTopRight *= f6;
                    this.colorBlueTopLeft *= f7;
                    this.colorBlueBottomLeft *= f7;
                    this.colorBlueBottomRight *= f7;
                    this.colorBlueTopRight *= f7;
                    this.RenderSouthFace(block1, i2, i3, i4, 38);
                }

                z8 = true;
            }

            this.enableAO = false;
            return z8;
        }

        public virtual bool RenderStandardBlockWithColorMultiplier(Tile block1, int i2, int i3, int i4, float f5, float f6, float f7)
        {
            this.enableAO = false;
            Tessellator tessellator8 = Tessellator.Instance;
            bool z9 = false;
            float f10 = 0.5F;
            float f11 = 1F;
            float f12 = 0.8F;
            float f13 = 0.6F;
            float f14 = f11 * f5;
            float f15 = f11 * f6;
            float f16 = f11 * f7;
            float f17 = f10;
            float f18 = f12;
            float f19 = f13;
            float f20 = f10;
            float f21 = f12;
            float f22 = f13;
            float f23 = f10;
            float f24 = f12;
            float f25 = f13;
            if (block1 != Tile.grass)
            {
                f17 = f10 * f5;
                f18 = f12 * f5;
                f19 = f13 * f5;
                f20 = f10 * f6;
                f21 = f12 * f6;
                f22 = f13 * f6;
                f23 = f10 * f7;
                f24 = f12 * f7;
                f25 = f13 * f7;
            }

            float f26 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            float f27;
            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 - 1, i4, 0))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                tessellator8.Color(f17 * f27, f20 * f27, f23 * f27);
                this.RenderBottomFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.DOWN));
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 + 1, i4, 1))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                if (block1.maxY != 1 && !block1.material.IsLiquid())
                {
                    f27 = f26;
                }

                tessellator8.Color(f14 * f27, f15 * f27, f16 * f27);
                this.RenderTopFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.UP));
                z9 = true;
            }

            int i28;
            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 - 1, 2))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                if (block1.minZ > 0)
                {
                    f27 = f26;
                }

                tessellator8.Color(f18 * f27, f21 * f27, f24 * f27);
                i28 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.NORTH);
                this.RenderEastFace(block1, i2, i3, i4, i28);
                if (fancyGrass && i28 == 3 && this.overrideBlockTexture < 0)
                {
                    tessellator8.Color(f18 * f27 * f5, f21 * f27 * f6, f24 * f27 * f7);
                    this.RenderEastFace(block1, i2, i3, i4, 38);
                }

                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 + 1, 3))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                if (block1.maxZ < 1)
                {
                    f27 = f26;
                }

                tessellator8.Color(f18 * f27, f21 * f27, f24 * f27);
                i28 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH);
                this.RenderWestFace(block1, i2, i3, i4, i28);
                if (fancyGrass && i28 == 3 && this.overrideBlockTexture < 0)
                {
                    tessellator8.Color(f18 * f27 * f5, f21 * f27 * f6, f24 * f27 * f7);
                    this.RenderWestFace(block1, i2, i3, i4, 38);
                }

                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 - 1, i3, i4, 4))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                if (block1.minX > 0)
                {
                    f27 = f26;
                }

                tessellator8.Color(f19 * f27, f22 * f27, f25 * f27);
                i28 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.WEST);
                this.RenderNorthFace(block1, i2, i3, i4, i28);
                if (fancyGrass && i28 == 3 && this.overrideBlockTexture < 0)
                {
                    tessellator8.Color(f19 * f27 * f5, f22 * f27 * f6, f25 * f27 * f7);
                    this.RenderNorthFace(block1, i2, i3, i4, 38);
                }

                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 + 1, i3, i4, 5))
            {
                f27 = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                if (block1.maxX < 1)
                {
                    f27 = f26;
                }

                tessellator8.Color(f19 * f27, f22 * f27, f25 * f27);
                i28 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.EAST);
                this.RenderSouthFace(block1, i2, i3, i4, i28);
                if (fancyGrass && i28 == 3 && this.overrideBlockTexture < 0)
                {
                    tessellator8.Color(f19 * f27 * f5, f22 * f27 * f6, f25 * f27 * f7);
                    this.RenderSouthFace(block1, i2, i3, i4, 38);
                }

                z9 = true;
            }

            return z9;
        }

        public virtual bool RenderBlockCactus(Tile block1, int i2, int i3, int i4)
        {
            int i5 = block1.GetColor(this.blockAccess, i2, i3, i4);
            float f6 = (i5 >> 16 & 255) / 255F;
            float f7 = (i5 >> 8 & 255) / 255F;
            float f8 = (i5 & 255) / 255F;
            if (GameRenderer.field_28135_a)
            {
                float f9 = (f6 * 30F + f7 * 59F + f8 * 11F) / 100F;
                float f10 = (f6 * 30F + f7 * 70F) / 100F;
                float f11 = (f6 * 30F + f8 * 70F) / 100F;
                f6 = f9;
                f7 = f10;
                f8 = f11;
            }

            return this.Func_1230_b(block1, i2, i3, i4, f6, f7, f8);
        }

        public virtual bool Func_1230_b(Tile block1, int i2, int i3, int i4, float f5, float f6, float f7)
        {
            Tessellator tessellator8 = Tessellator.Instance;
            bool z9 = false;
            float f10 = 0.5F;
            float f11 = 1F;
            float f12 = 0.8F;
            float f13 = 0.6F;
            float f14 = f10 * f5;
            float f15 = f11 * f5;
            float f16 = f12 * f5;
            float f17 = f13 * f5;
            float f18 = f10 * f6;
            float f19 = f11 * f6;
            float f20 = f12 * f6;
            float f21 = f13 * f6;
            float f22 = f10 * f7;
            float f23 = f11 * f7;
            float f24 = f12 * f7;
            float f25 = f13 * f7;
            float f26 = 0.0625F;
            float f27 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            float f28;
            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 - 1, i4, 0))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
                tessellator8.Color(f14 * f28, f18 * f28, f22 * f28);
                this.RenderBottomFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.DOWN));
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3 + 1, i4, 1))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
                if (block1.maxY != 1 && !block1.material.IsLiquid())
                {
                    f28 = f27;
                }

                tessellator8.Color(f15 * f28, f19 * f28, f23 * f28);
                this.RenderTopFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.UP));
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 - 1, 2))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
                if (block1.minZ > 0)
                {
                    f28 = f27;
                }

                tessellator8.Color(f16 * f28, f20 * f28, f24 * f28);
                tessellator8.AddOffset(0F, 0F, f26);
                this.RenderEastFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.NORTH));
                tessellator8.AddOffset(0F, 0F, -f26);
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2, i3, i4 + 1, 3))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
                if (block1.maxZ < 1)
                {
                    f28 = f27;
                }

                tessellator8.Color(f16 * f28, f20 * f28, f24 * f28);
                tessellator8.AddOffset(0F, 0F, -f26);
                this.RenderWestFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH));
                tessellator8.AddOffset(0F, 0F, f26);
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 - 1, i3, i4, 4))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
                if (block1.minX > 0)
                {
                    f28 = f27;
                }

                tessellator8.Color(f17 * f28, f21 * f28, f25 * f28);
                tessellator8.AddOffset(f26, 0F, 0F);
                this.RenderNorthFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.WEST));
                tessellator8.AddOffset(-f26, 0F, 0F);
                z9 = true;
            }

            if (this.renderAllFaces || block1.ShouldRenderFace(this.blockAccess, i2 + 1, i3, i4, 5))
            {
                f28 = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
                if (block1.maxX < 1)
                {
                    f28 = f27;
                }

                tessellator8.Color(f17 * f28, f21 * f28, f25 * f28);
                tessellator8.AddOffset(-f26, 0F, 0F);
                this.RenderSouthFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.EAST));
                tessellator8.AddOffset(f26, 0F, 0F);
                z9 = true;
            }

            return z9;
        }

        public virtual bool RenderBlockFence(Tile block1, int i2, int i3, int i4)
        {
            bool z5 = false;
            float f6 = 0.375F;
            float f7 = 0.625F;
            block1.SetShape(f6, 0F, f6, f7, 1F, f7);
            this.RenderStandardBlock(block1, i2, i3, i4);
            z5 = true;
            bool z8 = false;
            bool z9 = false;
            if (this.blockAccess.GetTile(i2 - 1, i3, i4) == block1.id || this.blockAccess.GetTile(i2 + 1, i3, i4) == block1.id)
            {
                z8 = true;
            }

            if (this.blockAccess.GetTile(i2, i3, i4 - 1) == block1.id || this.blockAccess.GetTile(i2, i3, i4 + 1) == block1.id)
            {
                z9 = true;
            }

            bool z10 = this.blockAccess.GetTile(i2 - 1, i3, i4) == block1.id;
            bool z11 = this.blockAccess.GetTile(i2 + 1, i3, i4) == block1.id;
            bool z12 = this.blockAccess.GetTile(i2, i3, i4 - 1) == block1.id;
            bool z13 = this.blockAccess.GetTile(i2, i3, i4 + 1) == block1.id;
            if (!z8 && !z9)
            {
                z8 = true;
            }

            f6 = 0.4375F;
            f7 = 0.5625F;
            float f14 = 0.75F;
            float f15 = 0.9375F;
            float f16 = z10 ? 0F : f6;
            float f17 = z11 ? 1F : f7;
            float f18 = z12 ? 0F : f6;
            float f19 = z13 ? 1F : f7;
            if (z8)
            {
                block1.SetShape(f16, f14, f6, f17, f15, f7);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }

            if (z9)
            {
                block1.SetShape(f6, f14, f18, f7, f15, f19);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }

            f14 = 0.375F;
            f15 = 0.5625F;
            if (z8)
            {
                block1.SetShape(f16, f14, f6, f17, f15, f7);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }

            if (z9)
            {
                block1.SetShape(f6, f14, f18, f7, f15, f19);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }

            block1.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            return z5;
        }

        public virtual bool RenderBlockStairs(Tile block1, int i2, int i3, int i4)
        {
            bool z5 = false;
            int i6 = this.blockAccess.GetData(i2, i3, i4);
            if (i6 == 0)
            {
                block1.SetShape(0F, 0F, 0F, 0.5F, 0.5F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                block1.SetShape(0.5F, 0F, 0F, 1F, 1F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }
            else if (i6 == 1)
            {
                block1.SetShape(0F, 0F, 0F, 0.5F, 1F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                block1.SetShape(0.5F, 0F, 0F, 1F, 0.5F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }
            else if (i6 == 2)
            {
                block1.SetShape(0F, 0F, 0F, 1F, 0.5F, 0.5F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                block1.SetShape(0F, 0F, 0.5F, 1F, 1F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }
            else if (i6 == 3)
            {
                block1.SetShape(0F, 0F, 0F, 1F, 1F, 0.5F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                block1.SetShape(0F, 0F, 0.5F, 1F, 0.5F, 1F);
                this.RenderStandardBlock(block1, i2, i3, i4);
                z5 = true;
            }

            block1.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            return z5;
        }

        public virtual bool RenderBlockDoor(Tile block1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            DoorTile blockDoor6 = (DoorTile)block1;
            bool z7 = false;
            float f8 = 0.5F;
            float f9 = 1F;
            float f10 = 0.8F;
            float f11 = 0.6F;
            float f12 = block1.GetBrightness(this.blockAccess, i2, i3, i4);
            float f13 = block1.GetBrightness(this.blockAccess, i2, i3 - 1, i4);
            if (blockDoor6.minY > 0)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f8 * f13, f8 * f13, f8 * f13);
            this.RenderBottomFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.DOWN));
            z7 = true;
            f13 = block1.GetBrightness(this.blockAccess, i2, i3 + 1, i4);
            if (blockDoor6.maxY < 1)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f9 * f13, f9 * f13, f9 * f13);
            this.RenderTopFace(block1, i2, i3, i4, block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.UP));
            z7 = true;
            f13 = block1.GetBrightness(this.blockAccess, i2, i3, i4 - 1);
            if (blockDoor6.minZ > 0)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f10 * f13, f10 * f13, f10 * f13);
            int i14 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.NORTH);
            if (i14 < 0)
            {
                this.flipTexture = true;
                i14 = -i14;
            }

            this.RenderEastFace(block1, i2, i3, i4, i14);
            z7 = true;
            this.flipTexture = false;
            f13 = block1.GetBrightness(this.blockAccess, i2, i3, i4 + 1);
            if (blockDoor6.maxZ < 1)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f10 * f13, f10 * f13, f10 * f13);
            i14 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.SOUTH);
            if (i14 < 0)
            {
                this.flipTexture = true;
                i14 = -i14;
            }

            this.RenderWestFace(block1, i2, i3, i4, i14);
            z7 = true;
            this.flipTexture = false;
            f13 = block1.GetBrightness(this.blockAccess, i2 - 1, i3, i4);
            if (blockDoor6.minX > 0)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f11 * f13, f11 * f13, f11 * f13);
            i14 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.WEST);
            if (i14 < 0)
            {
                this.flipTexture = true;
                i14 = -i14;
            }

            this.RenderNorthFace(block1, i2, i3, i4, i14);
            z7 = true;
            this.flipTexture = false;
            f13 = block1.GetBrightness(this.blockAccess, i2 + 1, i3, i4);
            if (blockDoor6.maxX < 1)
            {
                f13 = f12;
            }

            if (Tile.lightEmission[block1.id] > 0)
            {
                f13 = 1F;
            }

            tessellator5.Color(f11 * f13, f11 * f13, f11 * f13);
            i14 = block1.GetBlockTexture(this.blockAccess, i2, i3, i4, TileFace.EAST);
            if (i14 < 0)
            {
                this.flipTexture = true;
                i14 = -i14;
            }

            this.RenderSouthFace(block1, i2, i3, i4, i14);
            z7 = true;
            this.flipTexture = false;
            return z7;
        }

        public virtual void RenderBottomFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minX * 16) / 256;
            double d14 = (i10 + block1.maxX * 16 - 0.01) / 256;
            double d16 = (i11 + block1.minZ * 16) / 256;
            double d18 = (i11 + block1.maxZ * 16 - 0.01) / 256;
            if (block1.minX < 0 || block1.maxX > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minZ < 0 || block1.maxZ > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            double d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31082_l == 2)
            {
                d12 = (i10 + block1.minZ * 16) / 256;
                d16 = (i11 + 16 - block1.maxX * 16) / 256;
                d14 = (i10 + block1.maxZ * 16) / 256;
                d18 = (i11 + 16 - block1.minX * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31082_l == 1)
            {
                d12 = (i10 + 16 - block1.maxZ * 16) / 256;
                d16 = (i11 + block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.minZ * 16) / 256;
                d18 = (i11 + block1.maxX * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31082_l == 3)
            {
                d12 = (i10 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.maxX * 16 - 0.01) / 256;
                d16 = (i11 + 16 - block1.minZ * 16) / 256;
                d18 = (i11 + 16 - block1.maxZ * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.minX;
            double d30 = d2 + block1.maxX;
            double d32 = d4 + block1.minY;
            double d34 = d6 + block1.minZ;
            double d36 = d6 + block1.maxZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d30, d32, d34, d20, d24);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
            }
            else
            {
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.VertexUV(d30, d32, d34, d20, d24);
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
            }
        }

        public virtual void RenderTopFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minX * 16) / 256;
            double d14 = (i10 + block1.maxX * 16 - 0.01) / 256;
            double d16 = (i11 + block1.minZ * 16) / 256;
            double d18 = (i11 + block1.maxZ * 16 - 0.01) / 256;
            if (block1.minX < 0 || block1.maxX > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minZ < 0 || block1.maxZ > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            double d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31083_k == 1)
            {
                d12 = (i10 + block1.minZ * 16) / 256;
                d16 = (i11 + 16 - block1.maxX * 16) / 256;
                d14 = (i10 + block1.maxZ * 16) / 256;
                d18 = (i11 + 16 - block1.minX * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31083_k == 2)
            {
                d12 = (i10 + 16 - block1.maxZ * 16) / 256;
                d16 = (i11 + block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.minZ * 16) / 256;
                d18 = (i11 + block1.maxX * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31083_k == 3)
            {
                d12 = (i10 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.maxX * 16 - 0.01) / 256;
                d16 = (i11 + 16 - block1.minZ * 16) / 256;
                d18 = (i11 + 16 - block1.maxZ * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.minX;
            double d30 = d2 + block1.maxX;
            double d32 = d4 + block1.maxY;
            double d34 = d6 + block1.minZ;
            double d36 = d6 + block1.maxZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d30, d32, d34, d20, d24);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
            }
            else
            {
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
                tessellator9.VertexUV(d30, d32, d34, d20, d24);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
            }
        }

        public virtual void RenderEastFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minX * 16) / 256;
            double d14 = (i10 + block1.maxX * 16 - 0.01) / 256;
            double d16 = (i11 + 16 - block1.maxY * 16) / 256;
            double d18 = (i11 + 16 - block1.minY * 16 - 0.01) / 256;
            double d20;
            if (this.flipTexture)
            {
                d20 = d12;
                d12 = d14;
                d14 = d20;
            }

            if (block1.minX < 0 || block1.maxX > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minY < 0 || block1.maxY > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31087_g == 2)
            {
                d12 = (i10 + block1.minY * 16) / 256;
                d16 = (i11 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + block1.maxY * 16) / 256;
                d18 = (i11 + 16 - block1.maxX * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31087_g == 1)
            {
                d12 = (i10 + 16 - block1.maxY * 16) / 256;
                d16 = (i11 + block1.maxX * 16) / 256;
                d14 = (i10 + 16 - block1.minY * 16) / 256;
                d18 = (i11 + block1.minX * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31087_g == 3)
            {
                d12 = (i10 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.maxX * 16 - 0.01) / 256;
                d16 = (i11 + block1.maxY * 16) / 256;
                d18 = (i11 + block1.minY * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.minX;
            double d30 = d2 + block1.maxX;
            double d32 = d4 + block1.minY;
            double d34 = d4 + block1.maxY;
            double d36 = d6 + block1.minZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d28, d34, d36, d20, d24);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d30, d34, d36, d12, d16);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d30, d32, d36, d22, d26);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d28, d32, d36, d14, d18);
            }
            else
            {
                tessellator9.VertexUV(d28, d34, d36, d20, d24);
                tessellator9.VertexUV(d30, d34, d36, d12, d16);
                tessellator9.VertexUV(d30, d32, d36, d22, d26);
                tessellator9.VertexUV(d28, d32, d36, d14, d18);
            }
        }

        public virtual void RenderWestFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minX * 16) / 256;
            double d14 = (i10 + block1.maxX * 16 - 0.01) / 256;
            double d16 = (i11 + 16 - block1.maxY * 16) / 256;
            double d18 = (i11 + 16 - block1.minY * 16 - 0.01) / 256;
            double d20;
            if (this.flipTexture)
            {
                d20 = d12;
                d12 = d14;
                d14 = d20;
            }

            if (block1.minX < 0 || block1.maxX > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minY < 0 || block1.maxY > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31086_h == 1)
            {
                d12 = (i10 + block1.minY * 16) / 256;
                d18 = (i11 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + block1.maxY * 16) / 256;
                d16 = (i11 + 16 - block1.maxX * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31086_h == 2)
            {
                d12 = (i10 + 16 - block1.maxY * 16) / 256;
                d16 = (i11 + block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.minY * 16) / 256;
                d18 = (i11 + block1.maxX * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31086_h == 3)
            {
                d12 = (i10 + 16 - block1.minX * 16) / 256;
                d14 = (i10 + 16 - block1.maxX * 16 - 0.01) / 256;
                d16 = (i11 + block1.maxY * 16) / 256;
                d18 = (i11 + block1.minY * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.minX;
            double d30 = d2 + block1.maxX;
            double d32 = d4 + block1.minY;
            double d34 = d4 + block1.maxY;
            double d36 = d6 + block1.maxZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d28, d34, d36, d12, d16);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d30, d34, d36, d20, d24);
            }
            else
            {
                tessellator9.VertexUV(d28, d34, d36, d12, d16);
                tessellator9.VertexUV(d28, d32, d36, d22, d26);
                tessellator9.VertexUV(d30, d32, d36, d14, d18);
                tessellator9.VertexUV(d30, d34, d36, d20, d24);
            }
        }

        public virtual void RenderNorthFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minZ * 16) / 256;
            double d14 = (i10 + block1.maxZ * 16 - 0.01) / 256;
            double d16 = (i11 + 16 - block1.maxY * 16) / 256;
            double d18 = (i11 + 16 - block1.minY * 16 - 0.01) / 256;
            double d20;
            if (this.flipTexture)
            {
                d20 = d12;
                d12 = d14;
                d14 = d20;
            }

            if (block1.minZ < 0 || block1.maxZ > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minY < 0 || block1.maxY > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31084_j == 1)
            {
                d12 = (i10 + block1.minY * 16) / 256;
                d16 = (i11 + 16 - block1.maxZ * 16) / 256;
                d14 = (i10 + block1.maxY * 16) / 256;
                d18 = (i11 + 16 - block1.minZ * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31084_j == 2)
            {
                d12 = (i10 + 16 - block1.maxY * 16) / 256;
                d16 = (i11 + block1.minZ * 16) / 256;
                d14 = (i10 + 16 - block1.minY * 16) / 256;
                d18 = (i11 + block1.maxZ * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31084_j == 3)
            {
                d12 = (i10 + 16 - block1.minZ * 16) / 256;
                d14 = (i10 + 16 - block1.maxZ * 16 - 0.01) / 256;
                d16 = (i11 + block1.maxY * 16) / 256;
                d18 = (i11 + block1.minY * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.minX;
            double d30 = d4 + block1.minY;
            double d32 = d4 + block1.maxY;
            double d34 = d6 + block1.minZ;
            double d36 = d6 + block1.maxZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d28, d32, d36, d20, d24);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d28, d30, d34, d22, d26);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d28, d30, d36, d14, d18);
            }
            else
            {
                tessellator9.VertexUV(d28, d32, d36, d20, d24);
                tessellator9.VertexUV(d28, d32, d34, d12, d16);
                tessellator9.VertexUV(d28, d30, d34, d22, d26);
                tessellator9.VertexUV(d28, d30, d36, d14, d18);
            }
        }

        public virtual void RenderSouthFace(Tile block1, double d2, double d4, double d6, int i8)
        {
            Tessellator tessellator9 = Tessellator.Instance;
            if (this.overrideBlockTexture >= 0)
            {
                i8 = this.overrideBlockTexture;
            }

            int i10 = (i8 & 15) << 4;
            int i11 = i8 & 240;
            double d12 = (i10 + block1.minZ * 16) / 256;
            double d14 = (i10 + block1.maxZ * 16 - 0.01) / 256;
            double d16 = (i11 + 16 - block1.maxY * 16) / 256;
            double d18 = (i11 + 16 - block1.minY * 16 - 0.01) / 256;
            double d20;
            if (this.flipTexture)
            {
                d20 = d12;
                d12 = d14;
                d14 = d20;
            }

            if (block1.minZ < 0 || block1.maxZ > 1)
            {
                d12 = (i10 + 0F) / 256F;
                d14 = (i10 + 15.99F) / 256F;
            }

            if (block1.minY < 0 || block1.maxY > 1)
            {
                d16 = (i11 + 0F) / 256F;
                d18 = (i11 + 15.99F) / 256F;
            }

            d20 = d14;
            double d22 = d12;
            double d24 = d16;
            double d26 = d18;
            if (this.field_31085_i == 2)
            {
                d12 = (i10 + block1.minY * 16) / 256;
                d16 = (i11 + 16 - block1.minZ * 16) / 256;
                d14 = (i10 + block1.maxY * 16) / 256;
                d18 = (i11 + 16 - block1.maxZ * 16) / 256;
                d24 = d16;
                d26 = d18;
                d20 = d12;
                d22 = d14;
                d16 = d18;
                d18 = d24;
            }
            else if (this.field_31085_i == 1)
            {
                d12 = (i10 + 16 - block1.maxY * 16) / 256;
                d16 = (i11 + block1.maxZ * 16) / 256;
                d14 = (i10 + 16 - block1.minY * 16) / 256;
                d18 = (i11 + block1.minZ * 16) / 256;
                d20 = d14;
                d22 = d12;
                d12 = d14;
                d14 = d22;
                d24 = d18;
                d26 = d16;
            }
            else if (this.field_31085_i == 3)
            {
                d12 = (i10 + 16 - block1.minZ * 16) / 256;
                d14 = (i10 + 16 - block1.maxZ * 16 - 0.01) / 256;
                d16 = (i11 + block1.maxY * 16) / 256;
                d18 = (i11 + block1.minY * 16 - 0.01) / 256;
                d20 = d14;
                d22 = d12;
                d24 = d16;
                d26 = d18;
            }

            double d28 = d2 + block1.maxX;
            double d30 = d4 + block1.minY;
            double d32 = d4 + block1.maxY;
            double d34 = d6 + block1.minZ;
            double d36 = d6 + block1.maxZ;
            if (this.enableAO)
            {
                tessellator9.Color(this.colorRedTopLeft, this.colorGreenTopLeft, this.colorBlueTopLeft);
                tessellator9.VertexUV(d28, d30, d36, d22, d26);
                tessellator9.Color(this.colorRedBottomLeft, this.colorGreenBottomLeft, this.colorBlueBottomLeft);
                tessellator9.VertexUV(d28, d30, d34, d14, d18);
                tessellator9.Color(this.colorRedBottomRight, this.colorGreenBottomRight, this.colorBlueBottomRight);
                tessellator9.VertexUV(d28, d32, d34, d20, d24);
                tessellator9.Color(this.colorRedTopRight, this.colorGreenTopRight, this.colorBlueTopRight);
                tessellator9.VertexUV(d28, d32, d36, d12, d16);
            }
            else
            {
                tessellator9.VertexUV(d28, d30, d36, d22, d26);
                tessellator9.VertexUV(d28, d30, d34, d14, d18);
                tessellator9.VertexUV(d28, d32, d34, d20, d24);
                tessellator9.VertexUV(d28, d32, d36, d12, d16);
            }
        }

        public virtual void RenderBlockOnInventory(Tile block1, int i2, float f3)
        {
            Tessellator tessellator4 = Tessellator.Instance;
            float f6;
            float f7;
            if (this.field_31088_b)
            {
                int i5_ = block1.GetColor(i2);
                f6 = (i5_ >> 16 & 255) / 255F;
                f7 = (i5_ >> 8 & 255) / 255F;
                float f8 = (i5_ & 255) / 255F;
                GL11.glColor4f(f6 * f3, f7 * f3, f8 * f3, 1F);
            }
            RenderShape i5 = block1.GetRenderShape();
            if (i5 != RenderShape.NORMAL && i5 != RenderShape.PISTON_BASE)
            {
                if (i5 == RenderShape.CROSS)
                {
                    tessellator4.Begin();
                    tessellator4.Normal(0F, -1F, 0F);
                    this.RenderCrossedSquares(block1, i2, -0.5, -0.5, -0.5);
                    tessellator4.End();
                }
                else if (i5 == RenderShape.CACTUS)
                {
                    block1.SetBlockBoundsForItemRender();
                    GL11.glTranslatef(-0.5F, -0.5F, -0.5F);
                    f6 = 0.0625F;
                    tessellator4.Begin();
                    tessellator4.Normal(0F, -1F, 0F);
                    this.RenderBottomFace(block1, 0, 0, 0, block1.GetTexture(TileFace.DOWN));
                    tessellator4.End();
                    tessellator4.Begin();
                    tessellator4.Normal(0F, 1F, 0F);
                    this.RenderTopFace(block1, 0, 0, 0, block1.GetTexture(TileFace.UP));
                    tessellator4.End();
                    tessellator4.Begin();
                    tessellator4.Normal(0F, 0F, -1F);
                    tessellator4.AddOffset(0F, 0F, f6);
                    this.RenderEastFace(block1, 0, 0, 0, block1.GetTexture(TileFace.NORTH));
                    tessellator4.AddOffset(0F, 0F, -f6);
                    tessellator4.End();
                    tessellator4.Begin();
                    tessellator4.Normal(0F, 0F, 1F);
                    tessellator4.AddOffset(0F, 0F, -f6);
                    this.RenderWestFace(block1, 0, 0, 0, block1.GetTexture(TileFace.SOUTH));
                    tessellator4.AddOffset(0F, 0F, f6);
                    tessellator4.End();
                    tessellator4.Begin();
                    tessellator4.Normal(-1F, 0F, 0F);
                    tessellator4.AddOffset(f6, 0F, 0F);
                    this.RenderNorthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.WEST));
                    tessellator4.AddOffset(-f6, 0F, 0F);
                    tessellator4.End();
                    tessellator4.Begin();
                    tessellator4.Normal(1F, 0F, 0F);
                    tessellator4.AddOffset(-f6, 0F, 0F);
                    this.RenderSouthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.EAST));
                    tessellator4.AddOffset(f6, 0F, 0F);
                    tessellator4.End();
                    GL11.glTranslatef(0.5F, 0.5F, 0.5F);
                }
                else if (i5 == RenderShape.CROPS)
                {
                    tessellator4.Begin();
                    tessellator4.Normal(0F, -1F, 0F);
                    this.Func_1245_b(block1, i2, -0.5, -0.5, -0.5);
                    tessellator4.End();
                }
                else if (i5 == RenderShape.TORCH)
                {
                    tessellator4.Begin();
                    tessellator4.Normal(0F, -1F, 0F);
                    this.RenderTorchAtAngle(block1, -0.5, -0.5, -0.5, 0, 0);
                    tessellator4.End();
                }
                else
                {
                    int i9;
                    if (i5 == RenderShape.STAIR)
                    {
                        for (i9 = 0; i9 < 2; ++i9)
                        {
                            if (i9 == 0)
                            {
                                block1.SetShape(0F, 0F, 0F, 1F, 1F, 0.5F);
                            }

                            if (i9 == 1)
                            {
                                block1.SetShape(0F, 0F, 0.5F, 1F, 0.5F, 1F);
                            }

                            GL11.glTranslatef(-0.5F, -0.5F, -0.5F);
                            tessellator4.Begin();
                            tessellator4.Normal(0F, -1F, 0F);
                            this.RenderBottomFace(block1, 0, 0, 0, block1.GetTexture(TileFace.DOWN));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 1F, 0F);
                            this.RenderTopFace(block1, 0, 0, 0, block1.GetTexture(TileFace.UP));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 0F, -1F);
                            this.RenderEastFace(block1, 0, 0, 0, block1.GetTexture(TileFace.NORTH));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 0F, 1F);
                            this.RenderWestFace(block1, 0, 0, 0, block1.GetTexture(TileFace.SOUTH));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(-1F, 0F, 0F);
                            this.RenderNorthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.WEST));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(1F, 0F, 0F);
                            this.RenderSouthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.EAST));
                            tessellator4.End();
                            GL11.glTranslatef(0.5F, 0.5F, 0.5F);
                        }
                    }
                    else if (i5 == RenderShape.FENCE)
                    {
                        for (i9 = 0; i9 < 4; ++i9)
                        {
                            f7 = 0.125F;
                            if (i9 == 0)
                            {
                                block1.SetShape(0.5F - f7, 0F, 0F, 0.5F + f7, 1F, f7 * 2F);
                            }

                            if (i9 == 1)
                            {
                                block1.SetShape(0.5F - f7, 0F, 1F - f7 * 2F, 0.5F + f7, 1F, 1F);
                            }

                            f7 = 0.0625F;
                            if (i9 == 2)
                            {
                                block1.SetShape(0.5F - f7, 1F - f7 * 3F, -f7 * 2F, 0.5F + f7, 1F - f7, 1F + f7 * 2F);
                            }

                            if (i9 == 3)
                            {
                                block1.SetShape(0.5F - f7, 0.5F - f7 * 3F, -f7 * 2F, 0.5F + f7, 0.5F - f7, 1F + f7 * 2F);
                            }

                            GL11.glTranslatef(-0.5F, -0.5F, -0.5F);
                            tessellator4.Begin();
                            tessellator4.Normal(0F, -1F, 0F);
                            this.RenderBottomFace(block1, 0, 0, 0, block1.GetTexture(TileFace.DOWN));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 1F, 0F);
                            this.RenderTopFace(block1, 0, 0, 0, block1.GetTexture(TileFace.UP));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 0F, -1F);
                            this.RenderEastFace(block1, 0, 0, 0, block1.GetTexture(TileFace.NORTH));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(0F, 0F, 1F);
                            this.RenderWestFace(block1, 0, 0, 0, block1.GetTexture(TileFace.SOUTH));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(-1F, 0F, 0F);
                            this.RenderNorthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.WEST));
                            tessellator4.End();
                            tessellator4.Begin();
                            tessellator4.Normal(1F, 0F, 0F);
                            this.RenderSouthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.EAST));
                            tessellator4.End();
                            GL11.glTranslatef(0.5F, 0.5F, 0.5F);
                        }

                        block1.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
                    }
                }
            }
            else
            {
                if (i5 == RenderShape.PISTON_BASE)
                {
                    i2 = 1;
                }

                block1.SetBlockBoundsForItemRender();
                GL11.glTranslatef(-0.5F, -0.5F, -0.5F);
                tessellator4.Begin();
                tessellator4.Normal(0F, -1F, 0F);
                this.RenderBottomFace(block1, 0, 0, 0, block1.GetTexture(TileFace.DOWN, i2));
                tessellator4.End();
                tessellator4.Begin();
                tessellator4.Normal(0F, 1F, 0F);
                this.RenderTopFace(block1, 0, 0, 0, block1.GetTexture(TileFace.UP, i2));
                tessellator4.End();
                tessellator4.Begin();
                tessellator4.Normal(0F, 0F, -1F);
                this.RenderEastFace(block1, 0, 0, 0, block1.GetTexture(TileFace.NORTH, i2));
                tessellator4.End();
                tessellator4.Begin();
                tessellator4.Normal(0F, 0F, 1F);
                this.RenderWestFace(block1, 0, 0, 0, block1.GetTexture(TileFace.SOUTH, i2));
                tessellator4.End();
                tessellator4.Begin();
                tessellator4.Normal(-1F, 0F, 0F);
                this.RenderNorthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.WEST, i2));
                tessellator4.End();
                tessellator4.Begin();
                tessellator4.Normal(1F, 0F, 0F);
                this.RenderSouthFace(block1, 0, 0, 0, block1.GetTexture(TileFace.EAST, i2));
                tessellator4.End();
                GL11.glTranslatef(0.5F, 0.5F, 0.5F);
            }
        }

        public static bool RenderItemIn3d(Tile.RenderShape i0)
        {
            return i0 == 0 ? true : (i0 == Tile.RenderShape.CACTUS ? true : (i0 == Tile.RenderShape.STAIR ? true : (i0 == Tile.RenderShape.FENCE ? true : i0 == Tile.RenderShape.PISTON_BASE)));
        }
    }
}
