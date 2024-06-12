using SharpCraft.Core.Util;
using System.IO;

namespace SharpCraft.Core.i18n
{
    public class I18N
    {
        public static I18N Instance { get; private set; } = new I18N();
        private JProperties translateTable = new JProperties();

        private I18N()
        {
            translateTable.Load(new StreamReader($"{SharedConstants.ASSETS_CORE_PATH}/lang/en_US.lang"));
            translateTable.Load(new StreamReader($"{SharedConstants.ASSETS_CORE_PATH}/lang/stats_US.lang"));
        }

        public string TranslateKey(string key)
            => translateTable.GetProperty(key, key);

        public string TranslateKeyFormat(string key, params object[] args)
            => string.Format(translateTable.GetProperty(key, key), args);

        public string TranslateNamedKey(string key)
            => translateTable.GetProperty(key + ".name", "");
    }
}
