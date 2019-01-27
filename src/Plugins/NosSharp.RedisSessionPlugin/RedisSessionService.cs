using System;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Server;
using ChickenAPI.Enums.Game;
using SaltyEmu.Redis;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace SaltyEmu.RedisWrappers
{
    public class RedisSessionService : ISessionService
    {
        private readonly Logger _log = Logger.GetLogger<RedisPlugin>();
        private readonly IRedisTypedClient<PlayerSessionDto> _client;

        public RedisSessionService(RedisConfiguration configuration)
        {
            _client = new RedisClient(new RedisEndpoint
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Password = configuration.Password
            }).As<PlayerSessionDto>();
            _log.Info($"Redis session connected to {configuration.Host}");
        }

        public void RegisterSession(PlayerSessionDto dto)
        {
            if (GetBySessionId(dto.Id) != null)
            {
                _log.Info($"Session {dto.Id} already registered");
                return;
            }

            dto.Id = (int)_client.GetNextSequence();
            _client.Store(dto);
        }

        public void UpdateSession(PlayerSessionDto dto)
        {
            PlayerSessionDto session = GetBySessionId(dto.Id);
            if (session == null)
            {
                _log.Info($"Session {dto.Id} not found");
                return;
            }

            session.State = dto.State;
            session.WorldServerId = dto.WorldServerId;
            _client.Store(session);
        }

        public PlayerSessionDto GetByAccountName(string accountName)
        {
            return _client.GetAll().FirstOrDefault(s => string.Equals(s.Username, accountName, StringComparison.CurrentCultureIgnoreCase));
        }

        public PlayerSessionDto GetBySessionId(int id) => _client.GetById(id);

        public void UnregisterSession(int sessionId)
        {
            PlayerSessionDto session = GetBySessionId(sessionId);
            session.State = PlayerSessionState.Unauthed;
            _client.Store(session);
        }

        public void UnregisterSessions(Guid serverId)
        {
            foreach (PlayerSessionDto i in _client.GetAll().Where(s => s.WorldServerId == serverId))
            {
                i.State = PlayerSessionState.Unauthed;
                _client.Store(i);
            }
        }
    }
}