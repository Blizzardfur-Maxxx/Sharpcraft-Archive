using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace SharpCraft.Core
{
    public class SharedConstants
    {
        //logger namespace
        public const string LOGGER_NS = "global";
        public const string ASSETS_CLIENT_PATH = "./assets";
        public const string ASSETS_CORE_PATH = "./assets";
        public const string VER = "Beta 1.73.0016.0";
        public const string VERSION_STRING = "SharpCraft " + VER;
        public const int PROTOCOL_VERSION = 14;
        public static readonly string VALID_TEXT_CHARACTERS = GetAllowedCharacters();
        public static readonly char[] ILLEGAL_FILENAME_CHARACTERS = { '/', '\n', '\r', '\t', '\u0000', '\f', '`', '?', '*', '\\', '<', '>', '|', '\"', ':' };
        public const string PROXY_URL = "http://betacraft.uk:11705";
        public static readonly HttpClient HTTP_CLIENT = new HttpClient();
        public const string RESOURCE_URL = $"{PROXY_URL}/MinecraftResources/";
        public const string SKIN_URL = $"{PROXY_URL}/MinecraftSkins/";
        public const string CLOAK_URL = $"{PROXY_URL}/MinecraftCloaks/";
        public const string CHECKSERVER = "http://session.minecraft.net/game/checkserver.jsp?user={0}&severId={1}";
        public const string JOINSERVER = "http://session.minecraft.net/game/joinserver.jsp?user={0}&sessionId={1}&serverId={2}";
		public const string LIC_CHECK = "https://login.minecraft.net/session?name={0}&session={1}";
		
        private static string GetAllowedCharacters()
        {
            string allowedChars = "";
            StreamReader textReader = new StreamReader($"{ASSETS_CORE_PATH}/font.txt");
            string line;

            while ((line = textReader.ReadLine()) != null)
            {
                if (!line.StartsWith('#'))
                    allowedChars += line;
            }

            textReader.Close();
            return allowedChars;
        }
    }
}
