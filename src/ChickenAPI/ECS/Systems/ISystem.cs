using System;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.ECS.Systems
{
    public interface ISystem
    {
        IEntityManager EntityManager { get; }
        
        void Update(DateTime time);

        /// <summary>
        ///     Check if the entity matches the system filter predicate.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        bool Match(IEntity entity);
    }
}