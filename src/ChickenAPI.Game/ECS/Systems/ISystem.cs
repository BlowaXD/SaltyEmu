using System;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS.Systems
{
    public interface ISystem
    {
        IEntityManager EntityManager { get; }

        void UpdateCache();


        void Update(DateTime time);

        /// <summary>
        ///     Check if the entity matches the system filter predicate.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        bool Match(IEntity entity);
    }
}