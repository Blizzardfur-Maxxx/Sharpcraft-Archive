using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Sound
{
    public class SoundRepository
    {
        private JRandom rand = new JRandom();
        //mapping:
        // soundname -> [soundname1, soundname2, soundname3]
        private NullDictionary<string, List<SoundData>> soundAliases = new ();
        private List<SoundData> entries = new ();
        public int nEntries = 0;
        public bool useDigitCheck = true;

        public virtual SoundData AddSound(string string1, JFile file2)
        {
            try
            {
                string string3 = string1;
                string1 = string1.Substring(0, string1.IndexOf("."));
                if (this.useDigitCheck)
                {

                    while (char.IsDigit(string1[string1.Length - 1]))
                    {
                        string1 = string1.Substring(0, string1.Length - 1);
                    }
                }

                string1 = string1.Replace("/", ".");
                if (!this.soundAliases.ContainsKey(string1))
                {
                    this.soundAliases[string1]=  new List<SoundData>();
                }

                SoundData soundPoolEntry4 = new SoundData(string3, file2.GetAbsolutePath());
                this.soundAliases[string1].Add(soundPoolEntry4);
                this.entries.Add(soundPoolEntry4);
                ++this.nEntries;
                return soundPoolEntry4;
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
                throw;
            }
        }

        public virtual SoundData GetRandomSoundFromSoundPool(string string1)
        {
            List<SoundData> list2 = this.soundAliases[string1];
            return list2 == null ? null : list2[this.rand.NextInt(list2.Count)];
        }

        public virtual SoundData GetRandomSound()
        {
            return this.entries.Count == 0 ? null : this.entries[this.rand.NextInt(this.entries.Count)];
        }
    }
}
