using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer
{
    public class MemTexture
    {
        private string url;
        private IMemTextureProcessor processor;
        public Bitmap image;
        public int referenceCount = 1;
        public uint textureName = uint.MaxValue;
        public bool textureSetupComplete = false;

        public MemTexture(string url, IMemTextureProcessor mtp)
        {
            this.url = url;
            processor = mtp;
            Thread thread = new Thread(new ThreadStart(() => 
            {
                try
                {
                    using HttpResponseMessage response = SharedConstants.HTTP_CLIENT.GetAsync(url).Result;

                    // For some reason, Betacraft returns empty images for invalid usernames
                    if (!response.IsSuccessStatusCode || response.Content.Headers.ContentLength == 0)
                        return;

                    Stream stream = response.Content.ReadAsStream();
                    if (processor == null)
                        image = new Bitmap(Image.FromStream(stream));
                    else
                        image = processor.Process(new Bitmap(Image.FromStream(stream)));
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
