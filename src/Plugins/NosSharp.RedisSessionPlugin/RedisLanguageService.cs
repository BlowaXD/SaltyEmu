using System;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Server;
using ChickenAPI.Game.Data.Language;
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

    public string GetLanguage(string key, RegionType type) => throw new NotImplementedException();
        public string GetLanguage(LanguageKeys key, RegionType type) => throw new NotImplementedException();

        public void SetLanguage(string key, string value, RegionType type)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(LanguageKeys key, string value, RegionType type)
        {
            throw new NotImplementedException();
        }
    }
}