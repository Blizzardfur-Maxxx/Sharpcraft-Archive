using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core
{
    public static class Profiler
    {
        public static bool profilingEnabled = false;
        private static List<string> sectionList = new List<string>();
        private static List<long> timestampList = new List<long>();
        private static string profilingSection = "";
        private static NullDictionary<string, long> profilingMap = new NullDictionary<string, long>();

        public static void ClearProfiling()
        {
            profilingMap.Clear();
        }

        public static void StartSection(string string0)
        {
            if (profilingEnabled)
            {
                if (profilingSection.Length > 0)
                {
                    profilingSection += '.';
                }

                profilingSection += string0;

                sectionList.Add(profilingSection);
                timestampList.Add(TimeUtil.NanoTime);
            }
        }

        public static void EndSection()
        {
            if (profilingEnabled)
            {
                long j0 = TimeUtil.NanoTime;
                int i = timestampList.Count - 1;
                long j2 = timestampList[i];
                timestampList.RemoveAt(i);
                sectionList.RemoveAt(sectionList.Count - 1);
                long j4 = j0 - j2;
                if (profilingMap.ContainsKey(profilingSection))
                {
                    profilingMap[profilingSection] = profilingMap[profilingSection] + j4;
                }
                else
                {
                    profilingMap[profilingSection] = j4;
                }

                profilingSection = sectionList.Count > 0 ? sectionList[sectionList.Count - 1] : "";
                if (j4 > 100000000)
                {
                    Console.WriteLine(profilingSection + ' ' + j4);
                }
            }
        }

        public static List<ProfilerResult> GetProfilingData(string string0)
        {
            if (!profilingEnabled)
            {
                return null;
            }
            else
            {
                long j2 = profilingMap.ContainsKey("root") ? profilingMap["root"] : 0;
                long j4 = profilingMap.ContainsKey(string0) ? profilingMap[string0] : -1;
                List<ProfilerResult> arrayList6 = new List<ProfilerResult>();
                if (string0.Length > 0)
                {
                    string0 += '.';
                }

                long j7 = 0;
                foreach (string string10 in profilingMap.Keys)
                {
                    if (string10.Length > string0.Length && string10.StartsWith(string0) && string10.IndexOf('.', string0.Length + 1) < 0)
                    {
                        j7 += profilingMap[string10];
                    }
                }

                double f19 = j7;
                if (j7 < j4)
                {
                    j7 = j4;
                }

                if (j2 < j7)
                {
                    j2 = j7;
                }

                foreach (string string11 in profilingMap.Keys)
                {
                    if (string11.Length > string0.Length && string11.StartsWith(string0) && string11.IndexOf('.', string0.Length + 1) < 0)
                    {
                        long j12 = profilingMap[string11];
                        double d14 = (double)j12 * 100 / j7;
                        double d16 = (double)j12 * 100 / j2;
                        string string18 = string11.Substring(string0.Length);
                        arrayList6.Add(new ProfilerResult(string18, d14, d16));
                    }
                }

                foreach (string string11 in profilingMap.Keys)
                {
                    profilingMap[string11] = profilingMap[string11] * 999 / 1000;
                }

                if (j7 > f19)
                {
                    arrayList6.Add(new ProfilerResult("unspecified", (double)(j7 - f19) * 100 / j7, (double)(j7 - f19) * 100 / j2));
                }

                arrayList6.Sort();
                arrayList6.Insert(0, new ProfilerResult(string0, 100, (double)j7 * 100 / j2));
                return arrayList6;
            }
        }

        public static void EndStartSection(string string0)
        {
            EndSection();
            StartSection(string0);
        }
    }
}
