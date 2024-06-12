using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpCraft.Core.Util;
using StbVorbisSharp;
using static LWCSGL.OpenAL.AL10;
using static LWCSGL.OpenAL.AL10C;
using static LWCSGL.OpenAL.ALC10;
using static LWCSGL.OpenAL.ALC10C;

namespace SharpCraft.Client.Sound
{
    public unsafe class SoundSystem
    {
        private const int MaxSoundSources = 32;
        private const int MaxStreamingSources = 4;
        
        private readonly IntPtr device;

        private readonly Dictionary<string, Source> sources;
        private readonly Dictionary<uint, Source> channelSources;
        
        private readonly Dictionary<string, uint> normalAudio;
        private readonly Dictionary<string, StreamingAudio> streamingAudio; //fname->aud

        private readonly uint[] soundChannels;
        private readonly uint[] streamingChannels;
        
        public SoundSystem()
        {
            Console.WriteLine("Starting up SoundSystem...");
            Console.WriteLine("Initializing LWCSGL OpenAL\n    (The LWCSGL binding of OpenAL. For more information, see http://www.github.com/vlOd2/LWCSGL)");
            
            this.device = alcOpenDevice("");
            if (this.device == IntPtr.Zero)
                throw new Exception("Failed to open OpenAL device.");

            void* ctx = alcCreateContext((void*)this.device, (uint*)0);
            if (!alcMakeContextCurrent(ctx))
                throw new Exception("Failed to make OpenAL context current.");
            
            CheckALError();
            
            //create maps
            this.sources = new Dictionary<string, Source>();
            this.channelSources = new Dictionary<uint, Source>();
            this.normalAudio = new Dictionary<string, uint>();
            this.streamingAudio = new Dictionary<string, StreamingAudio>();
            
            //allocate sources
            this.soundChannels = new uint[MaxSoundSources];
            this.streamingChannels = new uint[MaxStreamingSources];
            fixed (uint* sndPtr = &this.soundChannels[0]) alGenSources(MaxSoundSources, sndPtr);
            fixed (uint* strPtr = &this.streamingChannels[0]) alGenSources(MaxStreamingSources, strPtr);
            CheckALError();
            
            Console.WriteLine("OpenAL initialized.");
        }

        private bool TryFindEmptyChannel(SourceType t, out uint channel)
        {
            uint[] arr = t == SourceType.Sound ? this.soundChannels : this.streamingChannels;
            for (int i = 0; i < arr.Length; i++)
            {
                uint chan = arr[i]; //openal-chan uwu

                uint sta;
                alGetSourcei(chan, AL_SOURCE_STATE, &sta);
                if (sta != AL_PLAYING)
                {
                    if (this.channelSources.Remove(chan, out Source lastSrc))
                        lastSrc.DetachChannel();
                    channel = chan;
                    return true;
                }
            }

            channel = 0;
            return false;
        }
        
        private bool EnsureSourceChannel(Source s)
        {
            if (s.HasChannel) throw new Exception("???");//return true;
            
            if (!TryFindEmptyChannel(s.Type, out uint chan))
                return false;
            s.AttachChannel(chan);
            this.channelSources.Add(chan, s);
            return true;
        }

        private StreamingAudio LoadStreamingAudio(string fname)
        {
            StreamingAudio aud;
            if (!this.streamingAudio.TryGetValue(fname, out aud))
            {
                //this shit throws... sad.
                Vorbis dec = Vorbis.FromMemory(LoadAudioFile(fname)); //loads entire file into memory, but it's compressed so it should be fine
                uint buf;
                alGenBuffers(1, &buf);
                aud = new StreamingAudio(dec, buf);
                this.streamingAudio.Add(fname, aud);
            }
            return aud;
        }

        private uint LoadNormalAudio(string fname)
        {
            uint buf;
            if (!this.normalAudio.TryGetValue(fname, out buf))
            {
                alGenBuffers(1, &buf);
                CheckALError();

                byte[] d = LoadAudioFile(fname);
                int len;
                int channels;
                int sampleRate;
                short* samples = null;
                fixed (byte* mem = &d[0])
                {
                    len = StbVorbis.stb_vorbis_decode_memory(mem, d.Length, &channels, &sampleRate, ref samples);
                }

                int size = len * channels * sizeof(short);

                /*using (FileStream fs = File.Create($"{Path.GetFileNameWithoutExtension(fname)}.raw"))
                {
                    byte[] sd = new byte[size];
                    Marshal.Copy((IntPtr)samples, sd, 0, size);
                    fs.Write(sd, 0, sd.Length);
                }*/
                
                //upload
                alBufferData(buf, GetBufFormat(channels), samples, (uint)size, (uint)sampleRate);
                CheckALError();
                Marshal.FreeHGlobal((IntPtr)samples);
                this.normalAudio.Add(fname, buf);
            }
            return buf;
        }

        private byte[] LoadAudioFile(string fname)
        {
            byte[] d = File.ReadAllBytes(fname);
            if (fname.EndsWith(".mus")) UnMus(d, Mth.GetJHashCode(Path.GetFileName(fname)));
            return d;
        }

        private static void UnMus(byte[] data, int hash) //hash=fname.jhashcode()
        {
            for (int i = 0; i < data.Length; i++)
            {
                byte b = (byte)((sbyte)data[i] ^ hash >> 8); //maybe remove sbyte cast
                hash = hash * 498729871 + 85731 * b;
                data[i] = b;
            }
        }

        private static uint GetBufFormat(int channels)
        {
            switch (channels)
            {
                case 1: return AL_FORMAT_MONO16;
                case 2: return AL_FORMAT_STEREO16;
                default: throw new Exception($"unsupported channel count {channels}");
            }
        }

        public void BackgroundMusic(string sourceName, string filePath, string identifier, bool loop)
        {
            //perm source, temp
            NewStreamingSource(true, sourceName, filePath, identifier, loop, 0.0f, 0.0f, 0.0f, 0, 0.0f);
        }
        
        public void NewSource(bool priority, string sourceName, string filePath, string identifier, bool loop, float x, float y, float z, int attModel, float distOrRoll)
        {
            Source src = new Source(SourceType.Sound);
            if (!this.sources.TryAdd(sourceName, src))
                src = this.sources[sourceName];//throw new Exception($"a source called '{sourceName}' already exists");
            src.Looping = loop;
            src.Position = new Vector3(x, y, z);
            src.AttenuationModel = (AttenuationModel)attModel;
            src.DistOrRoll = distOrRoll;
            src.Buffer = LoadNormalAudio(filePath);
        }

        public void NewStreamingSource(bool priority, string sourceName, string filePath, string identifier, bool loop, float x, float y, float z, int attModel, float distOrRoll)
        {
            Source src = new Source(SourceType.Streaming);
            if (!this.sources.TryAdd(sourceName, src))
                src = this.sources[sourceName];//throw new Exception($"a source called '{sourceName}' already exists");
            src.Looping = loop;
            src.Position = new Vector3(x, y, z);
            src.AttenuationModel = (AttenuationModel)attModel;
            src.DistOrRoll = distOrRoll;
            src.StreamingAudio = LoadStreamingAudio(filePath);
            src.Buffer = src.StreamingAudio.Buffer;
        }

        public void Play(string sourceName)
        {
            if (!this.sources.TryGetValue(sourceName, out Source src))
                return;//throw new Exception("no such source");
            if (src.StreamingAudio != null) return;//TEMP

            if (!src.HasChannel)
            {
                if (!EnsureSourceChannel(src))
                    return;
                
                //configure source
                alSourcei(src.Channel, AL_LOOPING, src.Looping ? AL_TRUE : AL_FALSE);
                CheckALError();
                alSourcei(src.Channel, AL_BUFFER, src.Buffer);
                CheckALError();
                alSource3f(src.Channel, AL_POSITION, src.Position.X, src.Position.Y, src.Position.Z);
                CheckALError();
                alSourcef(src.Channel, AL_ROLLOFF_FACTOR, src.AttenuationModel == AttenuationModel.Rolloff ? src.DistOrRoll : 0.0f);
                CheckALError();
                alSourcef(src.Channel, AL_PITCH, src.Pitch);
                CheckALError();
                alSourcef(src.Channel, AL_GAIN, src.Gain);
                CheckALError();
                
                //Console.WriteLine($"{src.Pitch}, {src.Gain}");
                //alSourcei(src.Channel, AL_SOURCE_RELATIVE, AL_TRUE);
            }
            
            //Console.WriteLine($"splay {src.Channel}");
            alSourcePlay(src.Channel);
            CheckALError();
        }

        public bool Playing(string sourceName)
        {
            if (!this.sources.TryGetValue(sourceName, out Source src))
                return false;//throw new Exception("no such source");
            if (!src.HasChannel) return false;
            uint sta;
            alGetSourcei(src.Channel, AL_SOURCE_STATE, &sta);
            return sta == AL_PLAYING;
        }

        public void SetListenerOrientation(float lookX, float lookY, float lookZ, float upX, float upY, float upZ)
        {
            Span<float> o = stackalloc float[6]
            {
                lookX, lookY, lookZ,
                upX, upY, upZ
            };
            fixed (float* op = o)
            {
                alListenerfv(AL_ORIENTATION, op);
            }
        }

        public void SetListenerPosition(float x, float y, float z)
        {
            alListener3f(AL_POSITION, x, y, z);
        }

        public void SetPitch(string sourceName, float pitch)
        {
            if (!this.sources.TryGetValue(sourceName, out Source src))
                return;//throw new Exception("no such source");
            src.Pitch = pitch;
        }

        public void SetVolume(string sourceName, float volume)
        {
            if (!this.sources.TryGetValue(sourceName, out Source src))
                return;//throw new Exception("no such source");
            src.Gain = volume; //*maxgain
        }

        public void Stop(string sourceName)
        {
            if (!this.sources.TryGetValue(sourceName, out Source src))
                return;//throw new Exception("no such source");
            if (!src.HasChannel) return;
            
            alSourceStop(src.Channel);
            src.DetachChannel();
            this.channelSources.Remove(src.Channel);
            //todo: rewind to beginning
        }

        private void CheckALError()
        {
            uint err = alGetError();
            if (err != AL_NO_ERROR)
            {
                throw new Exception($"AL Error #{err}!");
            }
        }
        
        public void Cleanup()
        {
            Console.WriteLine("SoundSystem shutting down...\n    Author: Dart, www.dartscode.com");
            //clean up sources
            fixed (uint* sndPtr = &this.soundChannels[0])
            {
                alSourceStopv(MaxSoundSources, sndPtr);
                alDeleteSources(MaxSoundSources, sndPtr);
            }
            fixed (uint* strPtr = &this.streamingChannels[0])
            {
                alSourceStopv(MaxSoundSources, strPtr);
                alDeleteSources(MaxStreamingSources, strPtr);
            }
        }
    }
}
