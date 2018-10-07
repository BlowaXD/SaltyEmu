using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using Foundatio.Caching;
using Foundatio.Serializer;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NosSharp.RedisSessionPlugin.Redis
{
    public class NewCacheClientBase<TObject> : IMappedRepository<TObject> where TObject : class, IMappedDto
    {
        protected readonly string Prefix;
        protected readonly ICacheClient CacheClient;

        protected string ToKey(long id)
        {
            return Prefix + "_" + id;
        }

        protected string ToKey(TObject obj) => ToKey(obj.Id);

        protected NewCacheClientBase(RedisConfiguration conf) : this(typeof(TObject).Name, conf)
        {
        }

        protected NewCacheClientBase(string prefix, RedisConfiguration conf)
        {
            Prefix = prefix;
            CacheClient = new RedisHybridCacheClient(new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = conf.Password,
                    EndPoints = { $"{conf.Host}:{conf.Port}" } // todo config
                }),
                Serializer = new JsonNetSerializer(new JsonSerializerSettings
                {
                })
            });
        }

        public IEnumerable<TObject> Get()
        {
            return CacheClient.GetAllAsync<TObject>(null).Result.Select(s => s.Value.Value);
        }

        public TObject GetById(long id)
        {
            return CacheClient.GetAsync<TObject>(ToKey(id)).Result.Value;
        }

        public IEnumerable<TObject> GetByIds(IEnumerable<long> ids)
        {
            return CacheClient.GetAllAsync<TObject>(ids.Select(ToKey)).Result.Select(s => s.Value.Value);
        }

        public TObject Save(TObject obj)
        {
            CacheClient.SetAsync(ToKey(obj), obj).Wait();
            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o)).Wait();
        }

        public void DeleteById(long id)
        {
            CacheClient.RemoveAsync(ToKey(id)).Wait();
        }

        public void DeleteByIds(IEnumerable<long> ids)
        {
            CacheClient.RemoveAllAsync(ids.Select(ToKey));
        }

        public async Task<IEnumerable<TObject>> GetAsync()
        {
            return (await CacheClient.GetAllAsync<TObject>(null)).Select(s => s.Value.Value);
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
            await CacheClient.SetAsync(ToKey(obj), obj);
            return obj;
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            await CacheClient.SetAllAsync(objs.ToDictionary(ToKey, o => o));
        }

        public async Task DeleteByIdAsync(long id)
        {
            await CacheClient.RemoveAsync(ToKey(id));
        }

        public async Task DeleteByIdsAsync(IEnumerable<long> ids)
        {
            await CacheClient.RemoveAllAsync(ids.Select(ToKey));
        }
    }
}