using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Xml;
using SharpCraft.Core;
using SharpCraft.Core.Util;

namespace SharpCraft.Client
{
    public class ResourceDownloadThread : IDisposable
    {
        private const int BufferSize = 8192;
        
        private readonly Client mc;
        private bool closing;
        private readonly JFile resourceFolder;
        private readonly byte[] buffer;

        public ResourceDownloadThread(Client mc, JFile workingDirectory)
        {
            this.mc = mc;
            closing = false;
            resourceFolder = new JFile(workingDirectory, "sounds");
            buffer = new byte[BufferSize];
            if (!resourceFolder.Exists()) resourceFolder.Mkdir();
        }

        public bool IsAlive()
        {
            return false;
        }

        public void Start()
        {
            Thread t = new Thread(ThreadMain);
            t.Name = "Resource download thread";
            t.Start();
        }

        private void ThreadMain()
        {
            try
            {
                using HttpResponseMessage docRes = SharedConstants.HTTP_CLIENT.GetAsync(SharedConstants.RESOURCE_URL).Result;
                using Stream docStream = docRes.Content.ReadAsStream();
                
                XmlDocument doc = new XmlDocument();
                doc.Load(docStream);
                XmlNodeList contents = doc.GetElementsByTagName("Contents");

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < contents.Count; j++)
                    {
                        XmlNode node = contents[j];
                        if (node == null) continue;
                        if (node.NodeType != XmlNodeType.Element) continue;

                        string key = ((XmlElement)node).GetElementsByTagName("Key")[0].FirstChild.Value;
                        long size = long.Parse(((XmlElement)node).GetElementsByTagName("Size")[0].FirstChild.Value);
                        if (size < 1L) continue;

                        DownloadAndInstallResource(SharedConstants.RESOURCE_URL, key, size, i);
                        
                        if (this.closing) return;
                    }
                }
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
        }

        private void DownloadAndInstallResource(string baseUrl, string key, long size, int mode)
        {
            try
            {
                int slashIdx = key.IndexOf('/');
                string dirName = key.Substring(0, slashIdx);
                if (dirName != "sound" && dirName != "newsound")
                {
                    if (mode != 1) return;
                } else if (mode != 0) return;

                JFile resourcePath = new JFile(resourceFolder, key);
                if (!resourcePath.Exists() || resourcePath.Length() != size)
                {
                    // Download
                    resourcePath.Mkdir2();
                    string url = baseUrl + key.Replace(" ", "%20");
                    Console.WriteLine($"Downloading '{baseUrl}{key}'");
                    DownloadResource(url, resourcePath);
                
                    if (this.closing) return;
                }
                
                // Install
                mc.InstallResource(key, resourcePath);
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
        }

        private void DownloadResource(string url, JFile path)
        {
            using HttpResponseMessage res = SharedConstants.HTTP_CLIENT.GetAsync(url).Result;
            res.EnsureSuccessStatusCode();
            using Stream contentStream = res.Content.ReadAsStream();
            using FileStream outStream = path.GetWriteStream();

            do
            {
                int n = contentStream.Read(this.buffer, 0, BufferSize);
                if (n == 0) return;
                outStream.Write(this.buffer, 0, n);
            } while (!this.closing);
        }

        public void Dispose()
        {
            this.closing = true;
        }
    }
}