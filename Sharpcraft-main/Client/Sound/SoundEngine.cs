using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Sound
{
    public class SoundEngine
    {
        private static SoundSystem sndSystem;
        private SoundRepository soundPoolSounds = new SoundRepository();
        private SoundRepository soundPoolStreaming = new SoundRepository();
        private SoundRepository soundPoolMusic = new SoundRepository();
        private int soundCounter = 0;
        private Options options;
        private static bool loaded = false;
        private JRandom rand = new JRandom();
        private int ticksBeforeMusic;
        public virtual void Init(Options gameSettings1)
        {
            this.soundPoolStreaming.useDigitCheck = false;
            this.options = gameSettings1;
            if (!loaded && (gameSettings1 == null || gameSettings1.soundVolume != 0F || gameSettings1.musicVolume != 0F))
            {
                this.Init();
            }
        }

        private void Init()
        {
            ticksBeforeMusic = this.rand.NextInt(12000);
            try
            {
                float f1 = this.options.soundVolume;
                float f2 = this.options.musicVolume;
                this.options.soundVolume = 0F;
                this.options.musicVolume = 0F;
                this.options.SaveOptions();
                sndSystem = new SoundSystem();
                this.options.soundVolume = f1;
                this.options.musicVolume = f2;
                this.options.SaveOptions();
            }
            catch (Exception throwable3)
            {
                throwable3.PrintStackTrace();
                Console.WriteLine("error loading sound system");
            }

            loaded = true;
            //disable for now
            //loaded = false;
        }

        public virtual void OnSoundOptionsChanged()
        {
            if (!loaded && (this.options.soundVolume != 0F || this.options.musicVolume != 0F))
            {
                this.Init();
            }

            if (loaded)
            {
                if (this.options.musicVolume == 0F)
                {
                    sndSystem.Stop("BgMusic");
                }
                else
                {
                    sndSystem.SetVolume("BgMusic", this.options.musicVolume);
                }
            }
        }

        public virtual void Destroy()
        {
            if (loaded)
            {
                sndSystem.Cleanup();
            }
        }

        public virtual void AddSound(string string1, JFile file2)
        {
            this.soundPoolSounds.AddSound(string1, file2);
        }

        public virtual void AddStreaming(string string1, JFile file2)
        {
            this.soundPoolStreaming.AddSound(string1, file2);
        }

        public virtual void AddMusic(string string1, JFile file2)
        {
            this.soundPoolMusic.AddSound(string1, file2);
        }

        public virtual void PlayRandomMusicIfReady()
        {
            if (loaded && this.options.musicVolume != 0F)
            {
                if (!sndSystem.Playing("BgMusic") && !sndSystem.Playing("streaming"))
                {
                    if (this.ticksBeforeMusic > 0)
                    {
                        --this.ticksBeforeMusic;
                        return;
                    }

                    SoundData soundPoolEntry1 = this.soundPoolMusic.GetRandomSound();
                    if (soundPoolEntry1 != null)
                    {
                        this.ticksBeforeMusic = this.rand.NextInt(12000) + 12000;
                        sndSystem.BackgroundMusic("BgMusic", soundPoolEntry1.filePath, soundPoolEntry1.name, false);
                        sndSystem.SetVolume("BgMusic", this.options.musicVolume);
                        sndSystem.Play("BgMusic");
                    }
                }
            }
        }

        public virtual void Update(Mob entityLiving1, float f2)
        {
            if (loaded && this.options.soundVolume != 0F)
            {
                if (entityLiving1 != null)
                {
                    float f3 = entityLiving1.prevYaw + (entityLiving1.yaw - entityLiving1.prevYaw) * f2;
                    double d4 = entityLiving1.prevX + (entityLiving1.x - entityLiving1.prevX) * f2;
                    double d6 = entityLiving1.prevY + (entityLiving1.y - entityLiving1.prevY) * f2;
                    double d8 = entityLiving1.prevZ + (entityLiving1.z - entityLiving1.prevZ) * f2;
                    float f10 = Mth.Cos(-f3 * 0.017453292F - Mth.PI);
                    float f11 = Mth.Sin(-f3 * 0.017453292F - Mth.PI);
                    float f12 = -f11;
                    float f13 = 0F;
                    float f14 = -f10;
                    float f15 = 0F;
                    float f16 = 1F;
                    float f17 = 0F;
                    sndSystem.SetListenerPosition((float)d4, (float)d6, (float)d8);
                    sndSystem.SetListenerOrientation(f12, f13, f14, f15, f16, f17);
                }
            }
        }

        public virtual void PlayStreaming(string string1, float f2, float f3, float f4, float f5, float f6)
        {
            if (loaded && this.options.soundVolume != 0F)
            {
                string string7 = "streaming";
                if (sndSystem.Playing("streaming"))
                {
                    sndSystem.Stop("streaming");
                }

                if (string1 != null)
                {
                    SoundData soundPoolEntry8 = this.soundPoolStreaming.GetRandomSoundFromSoundPool(string1);
                    if (soundPoolEntry8 != null && f5 > 0F)
                    {
                        if (sndSystem.Playing("BgMusic"))
                        {
                            sndSystem.Stop("BgMusic");
                        }

                        float f9 = 16F;
                        sndSystem.NewStreamingSource(true, string7, soundPoolEntry8.filePath, soundPoolEntry8.name, false, f2, f3, f4, 2, f9 * 4F);
                        sndSystem.SetVolume(string7, 0.5F * this.options.soundVolume);
                        sndSystem.Play(string7);
                    }
                }
            }
        }

        public virtual void PlaySound(string string1, float f2, float f3, float f4, float f5, float f6)
        {
            if (loaded && this.options.soundVolume != 0F)
            {
                SoundData soundPoolEntry7 = this.soundPoolSounds.GetRandomSoundFromSoundPool(string1);
                if (soundPoolEntry7 != null && f5 > 0F)
                {
                    this.soundCounter = (this.soundCounter + 1) % 256;
                    string string8 = "sound_" + this.soundCounter;
                    float f9 = 16F;
                    if (f5 > 1F)
                    {
                        f9 *= f5;
                    }

                    sndSystem.NewSource(f5 > 1F, string8, soundPoolEntry7.filePath, soundPoolEntry7.name, false, f2, f3, f4, 2, f9);
                    sndSystem.SetPitch(string8, f6);
                    if (f5 > 1F)
                    {
                        f5 = 1F;
                    }

                    sndSystem.SetVolume(string8, f5 * this.options.soundVolume);
                    sndSystem.Play(string8);
                }
            }
        }

        public virtual void PlaySoundFX(string string1, float f2, float f3)
        {
            if (loaded && this.options.soundVolume != 0F)
            {
                SoundData soundPoolEntry4 = this.soundPoolSounds.GetRandomSoundFromSoundPool(string1);
                if (soundPoolEntry4 != null)
                {
                    this.soundCounter = (this.soundCounter + 1) % 256;
                    string string5 = "sound_" + this.soundCounter;
                    sndSystem.NewSource(false, string5, soundPoolEntry4.filePath, soundPoolEntry4.name, false, 0F, 0F, 0F, 0, 0F);
                    if (f2 > 1F)
                    {
                        f2 = 1F;
                    }

                    f2 *= 0.25F;
                    sndSystem.SetPitch(string5, f3);
                    sndSystem.SetVolume(string5, f2 * this.options.soundVolume);
                    sndSystem.Play(string5);
                }
            }
        }
    }
}
