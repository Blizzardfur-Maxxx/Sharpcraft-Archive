using System;
using System.Diagnostics;

namespace SharpCraft.Core.Util
{
    public static class TimeUtil
    {
        private static readonly long StartTime = Stopwatch.GetTimestamp();

        public static double ElapsedTime => (Stopwatch.GetTimestamp() - StartTime) / (double)Stopwatch.Frequency;

        public static long NanoTime => Stopwatch.GetTimestamp() * TimeSpan.NanosecondsPerTick;

        public static long MilliTime => Stopwatch.GetTimestamp() / TimeSpan.TicksPerMillisecond;
    }
}
