using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using Foundatio.Caching;
using Foundatio.Serializer;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace SaltyEmu.Redis
{
    public abstract class GenericRedisCacheClient<TObject> : ISynchronizedRepository<TObject> where TObject : class, ISynchronizedDto
    {
        protected readonly Logger Log = Logger.GetLogger($"RedisCache({typeof(TObject).Name})");
        protected readonly string Prefix;
        protected readonly RedisHybridCacheClient CacheClient;

        protected string ToKey(Guid id)
        {
            return Prefix + ":" + id;
        }

        protected string ToKey(TObject obj) => ToKey(obj.Id);

        protected GenericRedisCacheClient(RedisConfiguration conf) : this(typeof(TObject).Name, conf)
        {
        }

        private GenericRedisCacheClient(string prefix, RedisConfiguration conf)
        {
            Prefix = prefix;
            var tmp = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = conf.Password,
                    EndPoints = { $"{conf.Host}:{conf.Port}" },
                    ChannelPrefix = prefix,
                }),
                Serializer = new JsonNetSerializer(new JsonSerializerSettings
                {
                })
            };
            CacheClient = new RedisHybridCacheClient(tmp);
            Log.Info($"Connected to redis : {conf.Host}");
        }

        public IEnumerable<TObject> Get()
        {
            return CacheClient.GetAllAsync<TObject>().ConfigureAwait(false).GetAwaiter().GetResult().Values.Select(s => s.Value);
        }

        public TObject GetById(Guid id)
        {
            return CacheClient.GetAsync<TObject>(ToKey(id)).ConfigureAwait(false).GetAwaiter().GetResult().Value;
        }

        public IEnumerable<TObject> GetByIds(IEnumerable<Guid> ids)
        {
            return CacheClient.GetAllAsync<TObject>(ids.Select(ToKey)).ConfigureAwait(false).GetAwaiter().GetResult().Values.Select(s => s.Value);
        }

        public TObject Save(TObject obj)
        {
            CacheClient.SetAsync(ToKey(obj), obj).ConfigureAwait(false).GetAwaiter().GetResult();
            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(Guid id)
        {
            CacheClient.RemoveAsync(ToKey(id)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            CacheClient.RemoveAllAsync(ids.Select(ToKey)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<TObject>> GetAsync()
        {
            return (await CacheClient.GetAllAsync<TObject>(CacheClient.LocalCache.Keys)).Values.Select(s => s.Value);
        }

        public async Task<TObject> GetByIdAsync(Guid id)
        {
            return (await CacheClient.GetAsync<TObject>(ToKey(id))).Value;
        }

        public async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return (await CacheClient.GetAllAsync<TObject>(ids.Select(ToKey))).Values.Select(s => s.Value);
        }

        public async Task<TObject> SaveAsync(TObject obj)
        {
            await CacheClient.SetAsync(ToKey(obj), obj);
            return obj;
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            await CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o));
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await CacheClient.RemoveAsync(ToKey(id));
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            await CacheClient.RemoveAllAsync(ids.Select(ToKey));
        }
    }
}