using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data;
using Foundatio.Caching;
using Foundatio.Serializer;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NosSharp.RedisSessionPlugin.Redis
{
    public abstract class CacheClientBase<T> where T : IMappedDto
    {
        protected static string KeyPrefix = nameof(T) + '_';
        protected static string AllKeys = KeyPrefix + '*';
        private readonly ICacheClient _cache;

        protected CacheClientBase(RedisConfiguration configuration)
        {
            _cache = new RedisHybridCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(configuration.ToString()),
                Serializer = new JsonNetSerializer()
            });
        }

        protected string ToKey(long id) => KeyPrefix + id;

        protected string ToKey(T obj) => ToKey(obj.Id);

        protected IEnumerable<T> Get()
        {
            return _cache.GetAllAsync<T>(AllKeys).GetAwaiter().GetResult().Where(s => s.Value.HasValue).Select(s => s.Value.Value);
        }

        protected IEnumerable<T> Get(IEnumerable<long> ids)
        {
            return _cache.GetAllAsync<T>(ids.Select(ToKey)).GetAwaiter().GetResult().Where(s => s.Value.HasValue).Select(s => s.Value.Value);
        }

        protected T Get(long id) => _cache.GetAsync<T>(ToKey(id)).GetAwaiter().GetResult().Value;

        protected void Save(T obj)
        {
            _cache.SetAsync(ToKey(obj), obj);
        }
    }
}