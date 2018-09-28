using System;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Systems
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