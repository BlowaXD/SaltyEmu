using System;
using System.Collections.Generic;
using ChickenAPI.Core.i18n;
using ChickenAPI.Game._i18n;
using SaltyEmu.Redis;
using SaltyEmu.RedisWrappers.Languages;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace SaltyEmu.RedisWrappers
{
    public class RedisGameLanguageService : IGameLanguageService
    {
        private readonly IRedisTypedClient<string> _client;

        public RedisGameLanguageService(RedisConfiguration configuration)
        {
            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<string>();
        }

        private static Dictionary<PlayerMessages, string> GetSetByLanguageKey(LanguageKey key)
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

        public string GetLanguage(PlayerMessages key, LanguageKey type)
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

        public void SetLanguage(PlayerMessages key, string value, LanguageKey type)
        {
            GetSetByLanguageKey(type)[key] = value;
        }
    }
}