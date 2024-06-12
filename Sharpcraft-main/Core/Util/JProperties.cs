using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.Util
{
    public class JProperties
    {
        private NullDictionary<string, string> data = new NullDictionary<string, string>();

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void SetProperty(string key, string value)
        {
            data[key] = value;
        }

        public string GetProperty(string key, string @default)
        {
            return ContainsKey(key) ? data[key] : @default;
        }

        public void Load(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                string[] lineSplit = line.Split('=');
                if (lineSplit.Length < 2) continue;
                data[lineSplit[0]] = lineSplit[1].Trim();
            }

            reader.Close();
        }

        public void Store(TextWriter writer, string comment)
        {
            writer.WriteLine($"# {comment}");
            writer.WriteLine($"# {DateTime.Now}");

            foreach (KeyValuePair<string, string> pair in data)
            {
                writer.WriteLine($"{pair.Key}={pair.Value}");
            }

            writer.Flush();
            writer.Close();
        }
    }
}
