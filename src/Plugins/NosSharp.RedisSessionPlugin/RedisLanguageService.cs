using System;
using ChickenAPI.Core.i18n;
using ChickenAPI.Enums;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace NosSharp.RedisSessionPlugin
{
    public class RedisLanguageService : ILanguageService
    {
        private readonly IRedisSet<string> _set;
        private readonly IRedisTypedClient<string> _client;

        public RedisLanguageService(RedisConfiguration configuration)
        {

            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<string>();
            _set = _client.Sets["WorldServer"];
        }

        public string GetLanguage(string key, LanguageKey language) => throw new NotImplementedException();

        public string GetLanguage(ChickenI18NKey key, LanguageKey type) => throw new NotImplementedException();

        public void SetLanguage(string key, string value, LanguageKey type)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(ChickenI18NKey key, string value, LanguageKey type)
        {
            throw new NotImplementedException();
        }
    }
}