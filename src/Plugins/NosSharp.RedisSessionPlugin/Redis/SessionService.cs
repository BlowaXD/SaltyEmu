using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Server;
using ChickenAPI.Enums.Game;
using Foundatio.Caching;
using SaltyEmu.Redis;
using StackExchange.Redis;

namespace SaltyEmu.RedisWrappers.Redis
{
    public class SessionService : ISessionService
    {
        private const string KeyPrefix = nameof(PlayerSessionDto) + ":";
        private const string AllKeys = KeyPrefix + "*";
        private readonly ICacheClient _cache;

        public SessionService(RedisConfiguration config)
        {
            var options = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(config.Host)
            };
            _cache = new RedisCacheClient(options);
        }

        public void RegisterSession(PlayerSessionDto dto)
        {
            _cache.AddAsync(ToKey(dto), dto).GetAwaiter().GetResult();
        }

        public void UpdateSession(PlayerSessionDto dto)
        {
            if (!_cache.ExistsAsync(ToKey(dto)).GetAwaiter().GetResult())
            {
                return;
            }

            _cache.ReplaceAsync(ToKey(dto), dto).GetAwaiter().GetResult();
        }

        public PlayerSessionDto GetByAccountName(string accountName)
        {
            IDictionary<string, CacheValue<PlayerSessionDto>> tmp = _cache.GetAllAsync<PlayerSessionDto>(AllKeys).GetAwaiter().GetResult();
            return tmp.Values.FirstOrDefault(s => s.Value.Username == accountName)?.Value;
        }

        public PlayerSessionDto GetBySessionId(int id) => _cache.GetAsync<PlayerSessionDto>(ToKey(id)).GetAwaiter().GetResult().Value;

        public void UnregisterSession(int sessionId)
        {
            IDictionary<string, CacheValue<PlayerSessionDto>> tmp = _cache.GetAllAsync<PlayerSessionDto>(AllKeys).GetAwaiter().GetResult();
            KeyValuePair<string, CacheValue<PlayerSessionDto>> toDelete = tmp.FirstOrDefault(s => s.Value.Value.Id == sessionId);
            toDelete.Value.Value.State = PlayerSessionState.Unauthed;
            _cache.ReplaceAsync(toDelete.Key, toDelete.Value);
        }

        public void UnregisterSessions(Guid serverId)
        {
            IDictionary<string, CacheValue<PlayerSessionDto>> tmp = _cache.GetAllAsync<PlayerSessionDto>(AllKeys).GetAwaiter().GetResult();
            IEnumerable<KeyValuePair<string, CacheValue<PlayerSessionDto>>> toDelete = tmp.Where(s => s.Value.Value.WorldServerId == serverId);
            foreach (KeyValuePair<string, CacheValue<PlayerSessionDto>> pair in toDelete)
            {
                pair.Value.Value.State = PlayerSessionState.Unauthed;
                _cache.ReplaceAsync(pair.Key, pair.Value);
            }
        }

        private static string ToKey(int sessionId) => KeyPrefix + sessionId;

        private static string ToKey(PlayerSessionDto obj) => ToKey(obj.Id);
    }
}