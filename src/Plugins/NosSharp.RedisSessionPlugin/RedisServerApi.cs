﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.AccessLayer.Server;
using ChickenAPI.Game.Data.TransferObjects.Server;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace NosSharp.RedisSessionPlugin
{
    public class RedisServerApi : IServerApiService
    {
        private static readonly Logger Log = Logger.GetLogger<RedisServerApi>();
        private readonly IRedisSet<WorldServerDto> _set;
        private readonly IRedisTypedClient<WorldServerDto> _client;

        public RedisServerApi(RedisConfiguration configuration)
        {
            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<WorldServerDto>();
            _set = _client.Sets["WorldServer"];
        }

        public bool RegisterServer(WorldServerDto dto)
        {
            dto.ChannelId = (short)_set.Count;
            dto.Id = Guid.NewGuid();
            _set.Add(dto);
            return false;
        }

        public void UnregisterServer(Guid id)
        {
            IEnumerable<WorldServerDto> servers = _set.GetAll().Where(s => s.Id != id);
            _set.Clear();
            foreach (WorldServerDto server in servers)
            {
                _set.Add(server);
            }
        }

        public IEnumerable<WorldServerDto> GetServers()
        {
            return _client.Sets["WorldServer"].GetAll();
        }
    }
}