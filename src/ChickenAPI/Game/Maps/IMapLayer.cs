using System.Collections.Generic;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Game.Game.Maps
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