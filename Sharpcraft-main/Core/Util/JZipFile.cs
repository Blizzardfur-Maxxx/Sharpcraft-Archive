using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    public class JZipFile : IDisposable
    {
        private JFile tempDir;
        private List<FileStream> toCleanup = new List<FileStream>();

        public JZipFile(JFile file) 
        {
            if (!file.Exists() || file.IsDirectory())
                throw new ArgumentException("Invalid ZIP file!");
            string tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            tempDir = new JFile(Path.GetTempPath(), $"zip_{Path.GetFileName(tempFile)}");
            using FileStream stream = file.GetReadStream();
            ZipFile.ExtractToDirectory(stream, tempDir.GetAbsolutePath());
        }

        public Stream GetInputStream(string path) 
        {
            JFile file = new JFile(tempDir, path);
            FileStream stream = file.GetReadStream();
            if (stream == null) throw new IOException($"ZIP doesn't contain {path}");
            toCleanup.Add(stream);
            return stream;
        }

        public void Dispose()
        {
            foreach (FileStream stream in toCleanup) 
            {
                stream.Close();
                stream.Dispose();
            }
            // Probably shouldn't do that - vlOd
            //try
            //{
            //    Directory.Delete(tempDir.GetAbsolutePath(), true);
            //}
            //catch { }
            GC.Collect();
        }
    }
}
