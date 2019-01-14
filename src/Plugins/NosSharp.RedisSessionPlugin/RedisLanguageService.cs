using System;
using System.Collections.Generic;
using ChickenAPI.Core.i18n;
using SaltyEmu.Redis;
using SaltyEmu.RedisWrappers.Languages;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace SaltyEmu.RedisWrappers
{
    public class RedisLanguageService : ILanguageService
    {
        private readonly IRedisTypedClient<string> _client;

        public RedisLanguageService(RedisConfiguration configuration)
        {
            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<string>();
        }

        private static Dictionary<ChickenI18NKey, string> GetSetByLanguageKey(LanguageKey key)
        {
            switch (key)
            {
                case LanguageKey.EN:
                    return EnglishI18n.Languages;
                case LanguageKey.FR:
                case LanguageKey.DE:
                case LanguageKey.PL:
                case LanguageKey.IT:
                case LanguageKey.ES:
                case LanguageKey.CZ:
                case LanguageKey.TR:
                default:
                    return EnglishI18n.Languages;
            }
        }

        public string GetLanguage(string key, LanguageKey language)
        {
            return null;
        }

        public string GetLanguage(ChickenI18NKey key, LanguageKey type)
        {
            string value = "";
            if (GetSetByLanguageKey(type)?.TryGetValue(key, out value) == false)
            {
                value = type + "_" + key;
            }

            return value;
        }

        public void SetLanguage(string key, string value, LanguageKey type)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(ChickenI18NKey key, string value, LanguageKey type)
        {
            GetSetByLanguageKey(type)[key] = value;
        }
    }
}