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
        protected readonly string DataPrefix;
        protected readonly string KeySetKey;
        protected readonly RedisHybridCacheClient CacheClient;

        protected Task<CacheValue<ICollection<string>>> KeySet => CacheClient.GetSetAsync<string>(KeySetKey);

        protected async Task<ICollection<string>> GetAllKeysAsync()
        {
            return (await KeySet).Value;
        }

        protected Task RegisterKeyAsync(IEnumerable<Guid> keys)
        {
            return CacheClient.SetAddAsync(KeySetKey, keys.Select(ToKey));
        }

        protected Task RemoveKeyAsync(IEnumerable<Guid> keys)
        {
            return CacheClient.SetRemoveAsync(KeySetKey, keys.Select(ToKey));
        }


        protected string ToKey(Guid id)
        {
            return DataPrefix + id;
        }

        protected string ToKey(TObject obj) => ToKey(obj.Id);

        protected GenericRedisCacheClient(RedisConfiguration conf) : this(typeof(TObject).Name.ToLower(), conf)
        {
        }

        private GenericRedisCacheClient(string basePrefix, RedisConfiguration conf)
        {
            DataPrefix = basePrefix + ":data:";
            KeySetKey = basePrefix + ":keys:set";
            var tmp = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = conf.Password,
                    EndPoints = { $"{conf.Host}:{conf.Port}" },
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
            return CacheClient.GetAllAsync<TObject>(GetAllKeysAsync().ConfigureAwait(false).GetAwaiter().GetResult()).ConfigureAwait(false).GetAwaiter().GetResult().Values.Select(s => s.Value);
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
            RegisterKeyAsync(new[] { obj.Id }).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.SetAsync(ToKey(obj), obj).ConfigureAwait(false).GetAwaiter().GetResult();
            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            RegisterKeyAsync(objs.Select(obj => obj.Id)).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(Guid id)
        {
            RemoveKeyAsync(new[] { id }).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.RemoveAsync(ToKey(id)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            RemoveKeyAsync(ids).ConfigureAwait(false).GetAwaiter().GetResult();
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
            await RegisterKeyAsync(new[] { obj.Id });
            await CacheClient.SetAsync(ToKey(obj), obj);
            return obj;
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            await RegisterKeyAsync(objs.Select(s => s.Id));
            await CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o));
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await RemoveKeyAsync(new[] { id });
            await CacheClient.RemoveAsync(ToKey(id));
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            await RemoveKeyAsync(ids);
            await CacheClient.RemoveAllAsync(ids.Select(ToKey));
        }
    }
}