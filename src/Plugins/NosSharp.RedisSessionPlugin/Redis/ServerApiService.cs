using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using ChickenAPI.Data.Server;
using Foundatio.Caching;
using Foundatio.Serializer;
using Newtonsoft.Json;
using SaltyEmu.Redis;
using StackExchange.Redis;

namespace SaltyEmu.RedisWrappers.Redis
{
    public class ServerApiService : IServerApiService
    {
        private static readonly string Prefix = nameof(WorldServerDto).ToLower() + ":";
        private static readonly string AllKeys = Prefix + "*";
        private static readonly Logger Log = Logger.GetLogger<ServerApiService>();
        private readonly ICacheClient _cache;
        private RedisConfiguration _configuration;

        public ServerApiService(RedisConfiguration config)
        {
            _configuration = config;

            var options = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(config.Host),
                Serializer = new JsonNetSerializer(new JsonSerializerSettings
                {
                    
                })
            };
            _cache = new RedisCacheClient(options);
        }


        public WorldServerDto GetRunningServer { get; private set; }

        public bool RegisterServer(WorldServerDto dto)
        {
            dto.Id = Guid.NewGuid();
            IDictionary<string, CacheValue<WorldServerDto>> servers = _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult();
            if (servers.Values.Any(s => s?.Value?.Id == dto.Id))
            {
                Log.Warn("Server with the same Guid is already registered");
                return true;
            }

            GetRunningServer = dto;
            _cache.AddAsync(ToKey(dto), dto);
            return false;
        }

        public void UnregisterServer(Guid id)
        {
            _cache.RemoveAllAsync(new[] { ToKey(id) });
        }

        public IEnumerable<WorldServerDto> GetServers()
        {
            return _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult().Where(s => s.Value.HasValue).Select(s => s.Value.Value).ToList();
        }

        private static string ToKey(ISynchronizedDto dto) => ToKey(dto.Id);

        private static string ToKey(Guid id) => Prefix + id;
    }
}