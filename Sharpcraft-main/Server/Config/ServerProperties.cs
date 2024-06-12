using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core;

namespace SharpCraft.Server.Config
{
    public class ServerProperties
    {
        public static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        private JProperties serverProperties = new JProperties();
        private JFile serverPropertiesFile;
        public ServerProperties(JFile file1)
        {
            this.serverPropertiesFile = file1;
            if (file1.Exists())
            {
                try
                {
                    StreamReader reader = new StreamReader(file1.GetAbsolutePath());
                    this.serverProperties.Load(reader);
                    reader.Dispose();
                }
                catch (Exception exception3)
                {
                    logger.Log(LogLevel.WARNING, "Failed to load " + file1, exception3);
                    this.GenerateNewProperties();
                }
            }
            else
            {
                logger.Log(LogLevel.WARNING, file1 + " does not exist");
                this.GenerateNewProperties();
            }
        }

        public virtual void GenerateNewProperties()
        {
            logger.Log(LogLevel.INFO, "Generating new properties file");
            this.SaveProperties();
        }

        public virtual void SaveProperties()
        {
            try
            {
                StreamWriter writer = new StreamWriter(this.serverPropertiesFile.GetAbsolutePath());
                this.serverProperties.Store(writer, "Server properties");
                writer.Dispose();
            }
            catch (Exception exception2)
            {
                logger.Log(LogLevel.WARNING, "Failed to save " + this.serverPropertiesFile, exception2);
                this.GenerateNewProperties();
            }
        }

        public virtual string GetStringProperty(string string1, string string2)
        {
            if (!this.serverProperties.ContainsKey(string1))
            {
                this.serverProperties.SetProperty(string1, string2);
                this.SaveProperties();
            }

            return this.serverProperties.GetProperty(string1, string2);
        }

        public virtual int GetIntProperty(string string1, int i2)
        {
            try
            {
                return int.Parse(this.GetStringProperty(string1, "" + i2));
            }
            catch (Exception)
            {
                this.serverProperties.SetProperty(string1, "" + i2);
                return i2;
            }
        }

        private static bool ParseBool(string str)
        {
            return str.ToLower().Equals("true");
        }

        public virtual bool GetBooleanProperty(string string1, bool z2)
        {
            string str = z2.ToString().ToLower();
            try
            {
                return ParseBool(this.GetStringProperty(string1, "" + str));
            }
            catch (Exception)
            {
                this.serverProperties.SetProperty(string1, "" + str);
                return z2;
            }
        }

        public virtual void SetProperty(string string1, bool z2)
        {
            string str = z2.ToString().ToLower();
            this.serverProperties.SetProperty(string1, "" + str);
            this.SaveProperties();
        }
    }
}
