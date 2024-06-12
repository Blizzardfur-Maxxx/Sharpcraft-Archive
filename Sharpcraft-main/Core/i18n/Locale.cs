namespace SharpCraft.Core.i18n
{
    public class Locale
    {
        private static I18N localizedName = I18N.Instance;

        public static string TranslateKey(string key)
            => localizedName.TranslateKey(key);

        public static string TranslateKeyFormat(string key, params object[] args)
            => localizedName.TranslateKeyFormat(key, args);
    }
}
