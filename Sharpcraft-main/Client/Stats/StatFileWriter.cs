using Newtonsoft.Json.Linq;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SharpCraft.Client.Stats
{
    public class StatFileWriter
    {
        private NullDictionary<Stat, int> field_25102_a = new();
        private NullDictionary<Stat, int> field_25101_b = new();
        private bool field_27189_c = false;
        private StatsSyncher statsSyncher;

        public StatFileWriter(User session1, JFile workDir)
        {
            JFile statDir = new JFile(workDir, "stats");
            if (!statDir.Exists())
            {
                statDir.Mkdir();
            }

            JFile[] files = workDir.ListFiles();
            int count = files.Length;

            for (int i = 0; i < count; ++i)
            {
                JFile file = files[i];
                if (file.GetName().StartsWith("stats_") && file.GetName().EndsWith(".dat"))
                {
                    JFile file8 = new JFile(statDir, file.GetName());
                    if (!file8.Exists())
                    {
                        Console.WriteLine("Relocating " + file.GetName());
                        file.RenameTo(file8);
                    }
                }
            }

            this.statsSyncher = new StatsSyncher(session1, this, statDir);
        }

        public virtual void ReadStat(Stat statBase1, int i2)
        {
            this.WriteStatToMap(this.field_25101_b, statBase1, i2);
            this.WriteStatToMap(this.field_25102_a, statBase1, i2);
            this.field_27189_c = true;
        }

        private void WriteStatToMap(NullDictionary<Stat, int> map1, Stat statBase2, int i3)
        {
            int i5 = map1[statBase2];
            map1[statBase2] = i5 + i3;
        }

        public virtual NullDictionary<Stat, int> Func_27176_a()
        {
            return new NullDictionary<Stat, int>(this.field_25101_b);
        }

        public virtual void Func_27179_a(NullDictionary<Stat, int> map1)
        {
            if (map1 != null)
            {
                this.field_27189_c = true;
                IEnumerator<Stat> iterator2 = map1.Keys.GetEnumerator();

                while (iterator2.MoveNext())
                {
                    Stat statBase3 = iterator2.Current;
                    this.WriteStatToMap(this.field_25101_b, statBase3, map1[statBase3]);
                    this.WriteStatToMap(this.field_25102_a, statBase3, map1[statBase3]);
                }
            }
        }

        public virtual void PutStats(NullDictionary<Stat, int> map1)
        {
            if (map1 != null)
            {
                IEnumerator<Stat> iterator2 = map1.Keys.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    Stat statBase3 = iterator2.Current;
                    int i5 = this.field_25101_b[statBase3];
                    this.field_25102_a[statBase3] = map1[statBase3] + i5;
                }
            }
        }

        public virtual void Write(NullDictionary<Stat, int> map1)
        {
            if (map1 != null)
            {
                this.field_27189_c = true;
                IEnumerator<Stat> iterator2 = map1.Keys.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    Stat statBase3 = iterator2.Current;
                    this.WriteStatToMap(this.field_25101_b, statBase3, map1[statBase3]);
                }
            }
        }

        public static NullDictionary<Stat, int> ReadStatsFromJson(string json)
        {
            NullDictionary<Stat, int> stats = new();
            try
            {
                string sessid = "local";
                StringBuilder sb = new StringBuilder();
                JObject root = JObject.Parse(json);
                JArray statschange = root["stats-change"].Value<JArray>();
                IEnumerator<JToken> itr = statschange.GetEnumerator();

                while (itr.MoveNext())
                {
                    JToken node = itr.Current;
                    NullDictionary<string, string> map = node.ToObject<NullDictionary<string, string>>();
                    KeyValuePair<string, string> entry = map.First();
                    int statId = int.Parse(entry.Key);
                    int statCount = int.Parse(entry.Value);
                    Stat stat = StatList.GetStat(statId);

                    if (stat == null)
                    {
                        Console.WriteLine(statId + " is not a valid stat");
                    }
                    else
                    {
                        sb.Append(StatList.GetStat(statId).guid).Append(",");
                        sb.Append(statCount).Append(",");
                        stats[stat] = statCount;
                    }
                }

                MD5Hash md5 = new MD5Hash(sessid);
                string checksum = md5.Hash(sb.ToString());
                if (!checksum.Equals(root["checksum"].Value<string>()))
                {
                    Console.WriteLine("CHECKSUM MISMATCH");
                    return null;
                }
            }
            catch (JsonException je)
            {
                je.PrintStackTrace();
            }

            return stats;
        }

        public static string WriteStatsToJson(string username, string sessionid, Dictionary<Stat, int> stats)
        {
            StringBuilder stringBuilder3 = new StringBuilder();
            StringBuilder stringBuilder4 = new StringBuilder();
            bool z5 = true;
            stringBuilder3.Append("{\r\n");
            if (username != null && sessionid != null)
            {
                stringBuilder3.Append("  \"user\":{\r\n");
                stringBuilder3.Append("    \"name\":\"").Append(username).Append("\",\r\n");
                stringBuilder3.Append("    \"sessionid\":\"").Append(sessionid).Append("\"\r\n");
                stringBuilder3.Append("  },\r\n");
            }

            stringBuilder3.Append("  \"stats-change\":[");
            IEnumerator<Stat> iterator6 = stats.Keys.GetEnumerator();
            while (iterator6.MoveNext())
            {
                Stat statBase7 = iterator6.Current;
                if (!z5)
                {
                    stringBuilder3.Append("},");
                }
                else
                {
                    z5 = false;
                }

                stringBuilder3.Append("\r\n    {\"").Append(statBase7.statId).Append("\":").Append(stats[statBase7]);
                stringBuilder4.Append(statBase7.guid).Append(",");
                stringBuilder4.Append(stats[statBase7]).Append(",");
            }

            if (!z5)
            {
                stringBuilder3.Append("}");
            }

            MD5Hash mD5String8 = new MD5Hash(sessionid);
            stringBuilder3.Append("\r\n  ],\r\n");
            stringBuilder3.Append("  \"checksum\":\"").Append(mD5String8.Hash(stringBuilder4.ToString())).Append("\"\r\n");
            stringBuilder3.Append("}");
            return stringBuilder3.ToString();
        }

        public virtual bool HasAchievementUnlocked(Achievement achievement1)
        {
            return this.field_25102_a.ContainsKey(achievement1);
        }

        public virtual bool Func_27181_b(Achievement achievement1)
        {
            return achievement1.parentAchievement == null || this.HasAchievementUnlocked(achievement1.parentAchievement);
        }

        public virtual int WriteStat(Stat statBase1)
        {
            return this.field_25102_a[statBase1];
        }

        public virtual void Dispose()
        {
        }

        public virtual void SyncStats()
        {
            this.statsSyncher.SyncStatsFileWithMap(this.Func_27176_a());
        }

        public virtual void Tick()
        {
            if (this.field_27189_c && this.statsSyncher.Func_27420_b())
            {
                this.statsSyncher.SaveStats(this.Func_27176_a());
            }

            this.statsSyncher.Tick();
        }
    }
}
