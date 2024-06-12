using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.PathFinding;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Reflection;
using static SharpCraft.Core.World.GameLevel.Biomes.Biome;

namespace SharpCraft.Core.World.GameLevel
{
    public sealed class MobSpawner
    {
        private static HashSet<ChunkPos> chunksToPoll = new HashSet<ChunkPos>();
        private static readonly Type[] nightSpawnEntities = new[]
        {
            typeof(Spider),
            typeof(Zombie),
            typeof(Skeleton)
        };

        private static TilePos GetRandomPosWithin(Level level, int x, int z)
        {
            int xo = x + level.rand.NextInt(16);
            int yo = level.rand.NextInt(128);
            int zo = z + level.rand.NextInt(16);
            return new TilePos(xo, yo, zo);
        }


        public static int Tick(Level level, bool hostile, bool passive)
        {
            if (!hostile && !passive)
            {
                return 0;
            }
            else
            {
                chunksToPoll.Clear();
                for (int i = 0; i < level.playerEntities.Count; ++i)
                {
                    Player player = level.playerEntities[i];
                    int xoff = Mth.Floor(player.x / 16d);
                    int zoff = Mth.Floor(player.z / 16d);
                    byte size = 8;
                    for (int cx = -size; cx <= size; ++cx)
                    {
                        for (int cz = -size; cz <= size; ++cz)
                        {
                            chunksToPoll.Add(new ChunkPos(cx + xoff, cz + zoff));
                        }
                    }
                }

                int ret = 0;
                Pos spawnpos = level.GetSpawnPos();
                foreach (MobCategory cat in MobCategory.Values())
                {
                    if ((!cat.IsPeacefulMob() || passive) && (cat.IsPeacefulMob() || hostile) && level.CountInstanceOf(cat.GetMobClass()) <= cat.GetMaxSpawnedCount() * chunksToPoll.Count / 256)
                    {
                        bool lab130 = false;

                        //label130:
                        foreach (ChunkPos chunkpos in chunksToPoll)
                        {
                            Biome biome = level.GetBiomeSource().GetBiomeGenAt(chunkpos);
                            IList<MobSpawnerData> mobs = biome.GetMobs(cat);
                            if (mobs != null && !(mobs.Count == 0))
                            {
                                int rarity = 0;
                                foreach (MobSpawnerData dat in mobs)
                                {
                                    rarity += dat.spawnRarityRate;
                                }

                                int rndint = level.rand.NextInt(rarity);
                                MobSpawnerData data = mobs[0];
                                foreach (MobSpawnerData mobdata in mobs)
                                {
                                    rndint -= mobdata.spawnRarityRate;
                                    if (rndint < 0)
                                    {
                                        data = mobdata;
                                        break;
                                    }
                                }

                                TilePos pos = GetRandomPosWithin(level, chunkpos.x * 16, chunkpos.z * 16);
                                int x = pos.x;
                                int y = pos.y;
                                int z = pos.z;
                                if (!level.IsSolidBlockingTile(x, y, z) && level.GetMaterial(x, y, z) == cat.GetMaterial())
                                {
                                    int count = 0;
                                    for (int i = 0; i < 3; ++i)
                                    {
                                        int xx = x;
                                        int yy = y;
                                        int zz = z;
                                        byte cnt = 6;
                                        for (int j = 0; j < 4; ++j)
                                        {
                                            xx += level.rand.NextInt(cnt) - level.rand.NextInt(cnt);
                                            yy += level.rand.NextInt(1) - level.rand.NextInt(1);
                                            zz += level.rand.NextInt(cnt) - level.rand.NextInt(cnt);
                                            if (IsSpawnPositionOk(cat, level, xx, yy, zz))
                                            {
                                                double xMob = xx + 0.5d;
                                                double yMob = yy;
                                                double zMob = zz + 0.5d;
                                                if (level.GetClosestPlayer(xMob, yMob, zMob, 24d) == null)
                                                {
                                                    double xdiff = xMob - spawnpos.x;
                                                    double ydiff = yMob - spawnpos.y;
                                                    double zdiff = zMob - spawnpos.z;
                                                    double dist = xdiff * xdiff + ydiff * ydiff + zdiff * zdiff;
                                                    if (!(dist < 576.0d))
                                                    {
                                                        Mob mob;
                                                        try
                                                        {
                                                            mob = data.entityClass.GetConstructor(new Type[] { typeof(Level) }).Invoke(new object[] { level }) as Mob;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ex.PrintStackTrace();
                                                            return ret;
                                                        }

                                                        mob.SetLocationAndAngles(xMob, yMob, zMob, level.rand.NextFloat() * 360F, 0F);
                                                        if (mob.GetCanSpawnHere())
                                                        {
                                                            ++count;
                                                            level.AddEntity(mob);
                                                            FinalizeMobSettings(mob, level, xMob, yMob, zMob);
                                                            if (count >= mob.GetMaxSpawnedInChunk())
                                                            {
                                                                //Console.WriteLine("blerk");
                                                                lab130 = true;
                                                                break; 
                                                                //continue label130;
                                                            }
                                                        }

                                                        ret += count;
                                                    }
                                                }
                                            }
                                        }

                                        if (lab130)
                                        {
                                            //Console.WriteLine("blerk 2");
                                            break;
                                        }
                                    }
                                }
                            }
                            if (lab130)
                            {
                                //Console.WriteLine("cont");
                                lab130 = false;
                                continue;
                            }
                        }
                    }
                }

                return ret;
            }
        }


        private static bool IsSpawnPositionOk(MobCategory cat, Level level, int x, int y, int z)
        {
            if (cat.GetMaterial() == Material.water)
            {
                return level.GetMaterial(x, y, z).IsLiquid() && !level.IsSolidBlockingTile(x, y + 1, z);
            }
            else
            {
                return level.IsSolidBlockingTile(x, y - 1, z) && !level.IsSolidBlockingTile(x, y, z) && !level.GetMaterial(x, y, z).IsLiquid() && !level.IsSolidBlockingTile(x, y + 1, z);
            }
        }

        private static void FinalizeMobSettings(Mob mob, Level level, double x, double y, double z)
        {
            if (mob is Spider && level.rand.NextInt(100) == 0)
            {
                Skeleton skeleton = new Skeleton(level);
                skeleton.SetLocationAndAngles(x, y, z, mob.yaw, 0F);
                level.AddEntity(skeleton);
                skeleton.MountEntity(mob);
            }
            else if (mob is Sheep)
            {
                ((Sheep)mob).SetFleeceColor(Sheep.GetRandomColor(level.rand));
            }
        }

        public static bool PerformSleepSpawning(Level level, IList<Player> players)
        {
            bool ret = false;
            PathFinder pathfinder = new PathFinder(level);
            foreach (Player player in players)
            {
                Type[] mobtypes = nightSpawnEntities;
                if (mobtypes != null && mobtypes.Length != 0)
                {
                    bool b = false;
                    for (int i = 0; i < 20 && !b; ++i)
                    {
                        int xx = Mth.Floor(player.x) + level.rand.NextInt(32) - level.rand.NextInt(32);
                        int zz = Mth.Floor(player.y) + level.rand.NextInt(32) - level.rand.NextInt(32);
                        int zz0 = Mth.Floor(player.z) + level.rand.NextInt(16) - level.rand.NextInt(16);
                        if (zz0 < 1)
                        {
                            zz0 = 1;
                        }
                        else if (zz0 > 128)
                        {
                            zz0 = 128;
                        }

                        int rndint = level.rand.NextInt(mobtypes.Length);
                        int yy = zz0;
                        while (yy > 2 && !level.IsSolidBlockingTile(xx, yy - 1, zz))
                        {
                            --yy;
                        }

                        while (!IsSpawnPositionOk(MobCategory.monster, level, xx, yy, zz) && yy < zz0 + 16 && yy < 128)
                        {
                            ++yy;
                        }

                        if (yy < zz0 + 16 && yy < 128)
                        {
                            double x = xx + 0.5d;
                            double y = yy;
                            double z = zz + 0.5d;
                            Mob mob;
                            try
                            {
                                mob = mobtypes[rndint].GetConstructor(new Type[] { typeof(Level) }).Invoke(new object[] { level }) as Mob;
                            }
                            catch (Exception ex)
                            {
                                ex.PrintStackTrace();
                                return ret;
                            }

                            mob.SetLocationAndAngles(x, y, z, level.rand.NextFloat() * 360F, 0F);
                            if (mob.GetCanSpawnHere())
                            {
                                PathFinding.Path path = pathfinder.CreateEntityPathTo(mob, player, 32F);
                                if (path != null && path.pathLength > 1)
                                {
                                    Node node = path.Last();
                                    if (Math.Abs((double)node.xCoord - player.x) < 1.5d && Math.Abs((double)node.zCoord - player.z) < 1.5d && Math.Abs((double)node.yCoord - player.y) < 1.5d)
                                    {
                                        Pos bedpos = BedTile.GetNearestEmptyPos(level, Mth.Floor(player.x), Mth.Floor(player.y), Mth.Floor(player.z), 1);
                                        if (bedpos == default)
                                        {
                                            bedpos = new Pos(xx, yy + 1, zz);
                                        }

                                        mob.SetLocationAndAngles(bedpos.x + 0.5d, (double)bedpos.y, bedpos.z + 0.5d, 0F, 0F);
                                        level.AddEntity(mob);
                                        FinalizeMobSettings(mob, level, bedpos.x + 0.5d, (double)bedpos.y, bedpos.z + 0.5d);
                                        player.WakeUpPlayer(true, false, false);
                                        mob.PlayLivingSound();
                                        ret = true;
                                        b = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }
    }
}