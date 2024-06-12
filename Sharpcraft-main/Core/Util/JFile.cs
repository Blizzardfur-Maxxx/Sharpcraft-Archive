using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.Util
{
    // Don't touch or I bomb, k thx bye - vlOd
    public class JFile
    {
        private string path;
        private string absolutePath;
        private bool isDirectory;
        private FileAttributes attributes;

        public JFile(string path)
        {
            this.path = path;
            if (!Path.IsPathRooted(path))
                absolutePath = Path.GetFullPath(path);
            UpdateDetails();
        }

        public JFile(string parent, string path) : this(Path.Combine(parent, path))
        {
        }

        public JFile(JFile parent, string path) : this(Path.Combine(parent.path, path))
        {
        }
        
        private void UpdateDetails() 
        {
            try 
            {
                attributes = File.GetAttributes(GetAbsolutePath());
                isDirectory = attributes.HasFlag(FileAttributes.Directory);
            }
            catch 
            {
                attributes = FileAttributes.None;
                isDirectory = false;
            }
        }

        public string GetPath() => path;
        public string GetAbsolutePath() => absolutePath != null ? absolutePath : path;

        public bool IsFile() => !IsDirectory();

        public bool IsDirectory() 
        {
            UpdateDetails();
            return isDirectory;
        }

        public bool Exists() 
        {
            UpdateDetails();
            return isDirectory ? Directory.Exists(GetAbsolutePath()) : 
                File.Exists(GetAbsolutePath());
        }

        public bool Mkdir() 
        {
            bool created = false;

            if (!Exists()) 
            {
                try 
                {
                    Directory.CreateDirectory(GetAbsolutePath());
                    created = true;
                }
                catch
                {
                }
            }

            return created;
        }

        public bool Mkdir2()
        {
            bool created = false;

            if (!Exists())
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(GetAbsolutePath()));
                    created = true;
                }
                catch
                {
                }
            }

            return created;
        }

        public JFile[] ListFiles() 
        {
            if (!IsDirectory()) return null;
            List<JFile> files = new List<JFile>();

            foreach (string file in Directory.GetFileSystemEntries(GetAbsolutePath())) 
                files.Add(new JFile(file));

            return files.ToArray();
        }

        public JFile[] ListFiles(Func<string, bool> accept)
        {
            if (!IsDirectory()) return null;
            List<JFile> files = new List<JFile>();

            foreach (string file in Directory.GetFileSystemEntries(GetAbsolutePath()))
            {
                if (accept == null || accept.Invoke(file))
                    files.Add(new JFile(file));
            }

            return files.ToArray();
        }

        public JFile[] ListFiles(Func<JFile, bool> accept)
        {
            if (!IsDirectory()) return null;
            List<JFile> files = new List<JFile>();

            foreach (string file in Directory.GetFileSystemEntries(GetAbsolutePath()))
            {
                JFile fileObj = new JFile(file);
                if (accept == null || accept.Invoke(fileObj))
                    files.Add(fileObj);
            }

            return files.ToArray();
        }

        public string GetName()
        {
            return Path.GetFileName(path) ?? "";
        }

        public bool RenameTo(JFile file)
        {
            bool renamed = false;
            try 
            {
                File.Move(GetAbsolutePath(), file.GetAbsolutePath());
                renamed = true;
            }
            catch { }
            return renamed;
        }

        public FileStream GetReadStream() 
        {
            if (!Exists() || isDirectory) return null;
            return File.OpenRead(GetAbsolutePath());
        }

        public FileStream GetWriteStream()
        {
            if (isDirectory) return null;
            return File.OpenWrite(GetAbsolutePath());
        }

        public long LastModified()
        {
            if (!Exists()) return 0;
            return File.GetLastWriteTime(GetAbsolutePath()).Ticks;
        }

        public long Length()
        {
            if (!Exists()) return 0;
            using FileStream stream = GetReadStream();
            long length = stream.Length;
            stream.Close();
            return length;
        }

        public bool Delete()
        {
            if (!Exists()) return false;
            string absPath = GetAbsolutePath();

            if (isDirectory) 
            {
                try 
                {
                    Directory.Delete(absPath);
                    return true;
                }
                catch { return false; }
            }
            else 
            {
                try
                {
                    File.Delete(absPath);
                    return true;
                }
                catch { return false; }
            }
        }

        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }
}
