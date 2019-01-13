using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Server;
using Foundatio.Caching;
using SaltyEmu.Redis;
using StackExchange.Redis;

namespace SaltyEmu.RedisWrappers.Redis
{
    public class ServerApiService : IServerApiService
    {
        private const string Prefix = nameof(WorldServerDto) + "_";
        private const string AllKeys = Prefix + "*";
        private static readonly Logger Log = Logger.GetLogger<ServerApiService>();
        private readonly ICacheClient _cache;
        private RedisConfiguration _configuration;

        public ServerApiService(RedisConfiguration config)
        {
            _configuration = config;

            var options = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(config.Host)
            };
            _cache = new RedisCacheClient(options);
        }


        private WorldServerDto _worldInstance;
        public WorldServerDto GetRunningServer => _worldInstance;

        public bool RegisterServer(WorldServerDto dto)
        {
            IDictionary<string, CacheValue<WorldServerDto>> servers = _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult();
            if (servers.Values.Any(s => s.Value.Id == dto.Id))
            {
                Log.Warn("Server with the same Guid is already registered");
                return true;
            }

            _worldInstance = dto;
            _cache.AddAsync(ToKey(dto), dto);
            return false;
        }

        public void UnregisterServer(Guid id)
        {
            _cache.RemoveAllAsync(new[] { ToKey(id) });
        }

        public IEnumerable<WorldServerDto> GetServers()
        {
            return _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult().Where(s => s.Value.HasValue).Select(s => s.Value.Value);
        }

        private static string ToKey(WorldServerDto dto) => ToKey(dto.Id);

        private static string ToKey(Guid id) => Prefix + id;
    }
}