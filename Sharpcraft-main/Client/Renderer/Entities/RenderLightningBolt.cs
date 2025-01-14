﻿
using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Weather;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderLightningBolt : Render<LightningBolt>
    {
        public override void DoRender(LightningBolt entityLightningBolt1, double d2, double d4, double d6, float f8, float f9)
        {
            Tessellator tessellator10 = Tessellator.Instance;
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE);
            double[] d11 = new double[8];
            double[] d12 = new double[8];
            double d13 = 0.0D;
            double d15 = 0.0D;
            JRandom random17 = new JRandom(entityLightningBolt1.randomSeed);

            for (int i18 = 7; i18 >= 0; --i18)
            {
                d11[i18] = d13;
                d12[i18] = d15;
                d13 += random17.NextInt(11) - 5;
                d15 += random17.NextInt(11) - 5;
            }

            for (int i45 = 0; i45 < 4; ++i45)
            {
                JRandom random46 = new JRandom(entityLightningBolt1.randomSeed);

                for (int i19 = 0; i19 < 3; ++i19)
                {
                    int i20 = 7;
                    int i21 = 0;
                    if (i19 > 0)
                    {
                        i20 = 7 - i19;
                    }

                    if (i19 > 0)
                    {
                        i21 = i20 - 2;
                    }

                    double d22 = d11[i20] - d13;
                    double d24 = d12[i20] - d15;

                    for (int i26 = i20; i26 >= i21; --i26)
                    {
                        double d27 = d22;
                        double d29 = d24;
                        if (i19 == 0)
                        {
                            d22 += random46.NextInt(11) - 5;
                            d24 += random46.NextInt(11) - 5;
                        }
                        else
                        {
                            d22 += random46.NextInt(31) - 15;
                            d24 += random46.NextInt(31) - 15;
                        }

                        tessellator10.Begin(5);
                        float f31 = 0.5F;
                        tessellator10.Color(0.9F * f31, 0.9F * f31, 1.0F * f31, 0.3F);
                        double d32 = 0.1D + i45 * 0.2D;
                        if (i19 == 0)
                        {
                            d32 *= i26 * 0.1D + 1.0D;
                        }

                        double d34 = 0.1D + i45 * 0.2D;
                        if (i19 == 0)
                        {
                            d34 *= (i26 - 1) * 0.1D + 1.0D;
                        }

                        for (int i36 = 0; i36 < 5; ++i36)
                        {
                            double d37 = d2 + 0.5D - d32;
                            double d39 = d6 + 0.5D - d32;
                            if (i36 == 1 || i36 == 2)
                            {
                                d37 += d32 * 2.0D;
                            }

                            if (i36 == 2 || i36 == 3)
                            {
                                d39 += d32 * 2.0D;
                            }

                            double d41 = d2 + 0.5D - d34;
                            double d43 = d6 + 0.5D - d34;
                            if (i36 == 1 || i36 == 2)
                            {
                                d41 += d34 * 2.0D;
                            }

                            if (i36 == 2 || i36 == 3)
                            {
                                d43 += d34 * 2.0D;
                            }

                            tessellator10.Vertex(d41 + d22, d4 + i26 * 16, d43 + d24);
                            tessellator10.Vertex(d37 + d27, d4 + (i26 + 1) * 16, d39 + d29);
                        }

                        tessellator10.End();
                    }
                }
            }

            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glEnable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
        }
    }

}