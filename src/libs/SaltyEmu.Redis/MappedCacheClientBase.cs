using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using Foundatio.Caching;
using Foundatio.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace SaltyEmu.Redis
{
    public class MappedCacheClientBase<TObject> : IMappedRepository<TObject> where TObject : class, IMappedDto
    {
        protected readonly string DataPrefix;
        protected readonly string KeySetKey;
        protected readonly ICacheClient CacheClient;
        protected Task<CacheValue<ICollection<string>>> KeySet => CacheClient.GetSetAsync<string>(KeySetKey);

        protected async Task<ICollection<string>> GetAllKeysAsync()
        {
            return (await KeySet).Value;
        }

        protected Task RegisterKeyAsync(IEnumerable<long> keys)
        {
            return CacheClient.SetAddAsync(KeySetKey, keys.Select(ToKey));
        }

        protected Task RemoveKeyAsync(IEnumerable<long> keys)
        {
            return CacheClient.SetRemoveAsync(KeySetKey, keys.Select(ToKey));
        }

        protected string ToKey(long id)
        {
            return DataPrefix + id;
        }

        protected string ToKey(TObject obj) => ToKey(obj.Id);

        protected MappedCacheClientBase(RedisConfiguration conf) : this(typeof(TObject).Name.ToLower(), conf)
        {
        }

        private MappedCacheClientBase(string basePrefix, RedisConfiguration conf)
        {
            DataPrefix = "data:" + basePrefix + ':';
            KeySetKey = "keys:" + basePrefix;
            CacheClient = new RedisHybridCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = conf.Password,
                    EndPoints = { $"{conf.Host}:{conf.Port}" } // todo config
                }),
                Serializer = new JsonNetSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }),
            });
        }

        public IEnumerable<TObject> Get()
        {
            return CacheClient.GetAllAsync<TObject>(GetAllKeysAsync().ConfigureAwait(false).GetAwaiter().GetResult()).Result.Select(s => s.Value.Value);
        }

        public TObject GetById(long id)
        {
            return CacheClient.GetAsync<TObject>(ToKey(id)).ConfigureAwait(false).GetAwaiter().GetResult().Value;
        }

        public IEnumerable<TObject> GetByIds(IEnumerable<long> ids)
        {
            return CacheClient.GetAllAsync<TObject>(ids.Select(ToKey)).ConfigureAwait(false).GetAwaiter().GetResult().Select(s => s.Value.Value);
        }

        public TObject Save(TObject obj)
        {
            RegisterKeyAsync(new[] { obj.Id }).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.SetAsync(ToKey(obj), obj).ConfigureAwait(false).GetAwaiter().GetResult();
            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            RegisterKeyAsync(objs.Select(s => s.Id)).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(long id)
        {
            RemoveKeyAsync(new[] { id }).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.RemoveAsync(ToKey(id)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<long> ids)
        {
            RemoveKeyAsync(ids).ConfigureAwait(false).GetAwaiter().GetResult();
            CacheClient.RemoveAllAsync(ids.Select(ToKey)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<TObject>> GetAsync()
        {
            return (await CacheClient.GetAllAsync<TObject>(await GetAllKeysAsync())).Select(s => s.Value.Value);
        }

        public async Task<TObject> GetByIdAsync(long id)
        {
            return (await CacheClient.GetAsync<TObject>(ToKey(id))).Value;
        }

        public async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return (await CacheClient.GetAllAsync<TObject>(ids.Select(ToKey))).Select(s => s.Value.Value);
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

        public async Task DeleteByIdAsync(long id)
        {
            await RemoveKeyAsync(new[] { id });
            await CacheClient.RemoveAsync(ToKey(id));
        }

        public async Task DeleteByIdsAsync(IEnumerable<long> ids)
        {
            await RemoveKeyAsync(ids);
            await CacheClient.RemoveAllAsync(ids.Select(ToKey));
        }
    }
}