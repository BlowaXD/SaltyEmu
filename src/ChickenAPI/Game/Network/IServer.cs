using System;
using System.Collections.Generic;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Network
{
    public interface IServer : IBroadcastable
    {
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

        IEnumerable<IPlayerEntity> Players { get; }
    }
}