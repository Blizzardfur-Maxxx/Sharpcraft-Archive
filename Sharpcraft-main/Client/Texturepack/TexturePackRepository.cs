using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Texturepack
{
    public class TexturePackRepository
    {
        private IList<AbstractTexturePack> availableTexturePacks = new List<AbstractTexturePack>();
        private AbstractTexturePack defaultTexturePack = new DefaultTexturePack();
        public AbstractTexturePack selectedTexturePack;
        private NullDictionary<string, AbstractTexturePack> field_6538_d = new ();
        private Client mc;
        private JFile texturePackDir;
        private string currentTexturePack;

        public TexturePackRepository(Client instance, JFile file2)
        {
            mc = instance;
            selectedTexturePack = defaultTexturePack;
            texturePackDir = new JFile(file2, "texturepacks");

            if (!texturePackDir.Exists())
            {
                texturePackDir.Mkdir();
            }

            currentTexturePack = instance.options.skin;
            UpdateAvaliableTexturePacks();
            selectedTexturePack.OpenTexturePackFile();
        }

        public virtual bool SetTexturePack(AbstractTexturePack texturePackBase1)
        {
            if (texturePackBase1 == selectedTexturePack)
            {
                return false;
            }
            else
            {
                selectedTexturePack.CloseTexturePackFile();
                currentTexturePack = texturePackBase1.texturePackFileName;
                selectedTexturePack = texturePackBase1;
                mc.options.skin = currentTexturePack;
                mc.options.SaveOptions();
                selectedTexturePack.OpenTexturePackFile();
                return true;
            }
        }

        public virtual void UpdateAvaliableTexturePacks()
        {
            List<AbstractTexturePack> arrayList1 = new List<AbstractTexturePack>();
            this.selectedTexturePack = null;
            arrayList1.Add(this.defaultTexturePack);

            if (texturePackDir.Exists() && this.texturePackDir.IsDirectory())
            {
                JFile[] file2 = this.texturePackDir.ListFiles();
                JFile[] file3 = file2;
                int i4 = file2.Length;
                for (int i5 = 0; i5 < i4; ++i5)
                {
                    JFile file6 = file3[i5];
                    if (file6.IsFile() && file6.GetName().ToLower().EndsWith(".zip"))
                    {
                        string string7 = file6.GetName() + ":" + file6.Length() + ":" + file6.LastModified();
                        try
                        {
                            if (!this.field_6538_d.ContainsKey(string7))
                            {
                                Console.WriteLine($"Loading texture pack {file6}...");
                                FileTexturePack texturePackCustom8 = new FileTexturePack(file6);
                                texturePackCustom8.field_6488_d = string7;
                                this.field_6538_d[string7] = texturePackCustom8;
                                texturePackCustom8.Prepare(this.mc);
                            }

                            AbstractTexturePack texturePackBase12 = this.field_6538_d[string7];
                            if (texturePackBase12.texturePackFileName.Equals(this.currentTexturePack))
                            {
                                this.selectedTexturePack = texturePackBase12;
                            }

                            arrayList1.Add(texturePackBase12);
                        }
                        catch (IOException iOException9)
                        {
                            iOException9.PrintStackTrace();
                        }
                    }
                }
            }

            if (this.selectedTexturePack == null)
            {
                this.selectedTexturePack = this.defaultTexturePack;
            }

            foreach (AbstractTexturePack tex in arrayList1) availableTexturePacks.Remove(tex);
            IEnumerator<AbstractTexturePack> iterator10 = this.availableTexturePacks.GetEnumerator();
            while (iterator10.MoveNext())
            {
                AbstractTexturePack texturePackBase11 = iterator10.Current;
                texturePackBase11.UnbindThumbnailTexture(this.mc);
                this.field_6538_d.Remove(texturePackBase11.field_6488_d);
            }

            this.availableTexturePacks = arrayList1;
        }

        public virtual IList<AbstractTexturePack> AvailableTexturePacks()
        {
            return new List<AbstractTexturePack>(this.availableTexturePacks);
        }
    }
}
