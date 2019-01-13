using System;
using System.Text;
using ChickenAPI.Core.i18n;

namespace SaltyEmu.Core
{
    public static class EncodingExtensions
    {
        public static string GetKeyForLangFiles(this LanguageKey key)
        {
            switch (key)
            {
                case LanguageKey.FR:
                    return "fr";
                case LanguageKey.EN:
                    return "en";
                case LanguageKey.DE:
                    return "de";
                case LanguageKey.PL:
                    return "pl";
                case LanguageKey.IT:
                    return "it";
                case LanguageKey.ES:
                    return "es";
                case LanguageKey.CZ:
                    return "cz";
                case LanguageKey.TR:
                    return "tr";
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }

        public static Encoding GetEncoding(this LanguageKey key)
        {
            switch (key)
            {
                case LanguageKey.FR:
                case LanguageKey.EN:
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