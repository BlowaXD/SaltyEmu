using System.Collections.Generic;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Maps
{
    public interface IMapLayer : ISynchronizedDto, IEntityManager
    {
        /// <summary>
        ///     Get the base map of the layer
        /// </summary>
        IMap Map { get; }

        /// <summary>
        ///     Get all entities in the area between X and Y
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range);
    }
}