using System;
using System.Text;
using ChickenAPI.Core.i18n;

namespace World.Utils
{
    public static class EncodingExtensions
    {
        public static Encoding GetEncoding(this LanguageKey key)
        {
            switch (key)
            {
                case LanguageKey.EN:
                    return Encoding.Default;
                case LanguageKey.FR:
                case LanguageKey.ES:
                    return Encoding.GetEncoding(1252);
                case LanguageKey.DE:
                case LanguageKey.PL:
                case LanguageKey.IT:
                case LanguageKey.CZ:
                    return Encoding.GetEncoding(1250);
                case LanguageKey.TR:
                    return Encoding.GetEncoding(1254);
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }
    }
}