using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Network
{
    public interface IServer : IBroadcastable
    {
        IEnumerable<IPlayerEntity> Players { get; }
        void RegiterEntityManager(IEntityManager entityManager);
        void UnregisterEntityManager(IEntityManager entityManager);

        void Update(DateTime time);
        void Update(long tick);

        IPlayerEntity GetPlayerBySessionId(long sessionId);
        IPlayerEntity GetPlayerByAccountId(long accountId);
        IPlayerEntity GetPlayerByCharacterId(long characterId);

        void Register(IPlayerEntity entity);
        void Unregister(IPlayerEntity entity);

        void UnregisterBySessionId(long sessionId);
        void UnregisterByAccountId(long accontId);
        void UnregisterByCharacterId(long characterId);
    }
}