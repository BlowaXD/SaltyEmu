using System;
using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;

namespace ChickenAPI.Game._ECS.Entities
{
    public interface IMapLayer : IEntityManager, IBroadcastable
    {
        Guid Id { get; }

        /// <summary>
        ///     Get the base map of the layer
        /// </summary>
        IMap Map { get; }

        bool IsPvpEnabled { get; }

        IEnumerable<IPlayerEntity> Players { get; }

        /// <summary>
        ///     Returns the Player Entity with the given CharacterId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IPlayerEntity GetPlayerById(long id);

        /// <summary>
        ///     Get all players in the area between X and Y
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEnumerable<IPlayerEntity> GetPlayersInRange(Position<short> pos, int range);

        /// <summary>
        ///     Get all entities in the area between X and Y
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range);

        /// <summary>
        ///     Get all specified entities in the area between X and Y
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntitiesInRange<T>(Position<short> pos, int range) where T : IEntity;

        /// <summary>
        ///     Get the current transportId and increment it
        /// </summary>
        /// <returns></returns>
        long GetNextId();
    }
}