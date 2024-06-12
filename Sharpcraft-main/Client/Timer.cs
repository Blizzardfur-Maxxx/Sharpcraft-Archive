using SharpCraft.Core.Util;
using System;
using System.Diagnostics;

namespace SharpCraft.Client
{
    public class Timer
    {
        public float tps;
        private double lastHRTime;
        public int elapsedTicks;
        public float renderPartialTicks;
        public float timerSpeed = 1.0F;
        public float elapsedPartialTicks = 0.0F;
        private long lastSyncSysClock;
        private long lastSyncHRClock;
        private long field_28132_i;
        private double timeSyncAdjustment = 1.0D;

        public Timer(float var1) {
            this.tps = var1;
            this.lastSyncSysClock = TimeUtil.MilliTime;
            this.lastSyncHRClock = TimeUtil.NanoTime / 1000000L;
        }

        public void UpdateTimer() {
            long var1 = TimeUtil.MilliTime;
            long var3 = var1 - this.lastSyncSysClock;
            long var5 = TimeUtil.NanoTime / 1000000L;
            double var7 = (double)var5 / 1000.0D;
            if(var3 > 1000L) {
                this.lastHRTime = var7;
            } else if(var3 < 0L) {
                this.lastHRTime = var7;
            } else {
                this.field_28132_i += var3;
                if(this.field_28132_i > 1000L) {
                    long var9 = var5 - this.lastSyncHRClock;
                    double var11 = (double)this.field_28132_i / (double)var9;
                    this.timeSyncAdjustment += (var11 - this.timeSyncAdjustment) * (double)0.2F;
                    this.lastSyncHRClock = var5;
                    this.field_28132_i = 0L;
                }

                if(this.field_28132_i < 0L) {
                    this.lastSyncHRClock = var5;
                }
            }

            this.lastSyncSysClock = var1;
            double var13 = (var7 - this.lastHRTime) * this.timeSyncAdjustment;
            this.lastHRTime = var7;
            if(var13 < 0.0D) {
                var13 = 0.0D;
            }

            if(var13 > 1.0D) {
                var13 = 1.0D;
            }

            this.elapsedPartialTicks = (float)((double)this.elapsedPartialTicks + var13 * (double)this.timerSpeed * (double)this.tps);
            this.elapsedTicks = (int)this.elapsedPartialTicks;
            this.elapsedPartialTicks -= (float)this.elapsedTicks;
            if(this.elapsedTicks > 10) {
                this.elapsedTicks = 10;
            }

            this.renderPartialTicks = this.elapsedPartialTicks;
        }
    }
}
