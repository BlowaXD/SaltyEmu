using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS
{
    /// <summary>
    ///     It's supposed to contain all Entity containers that are actually running
    /// </summary>
    public interface IEntityManagerContainer
    {
        /// <summary>
        ///     Registered & running <see cref="IEntityManager" />
        /// </summary>
        IEnumerable<IEntityManager> Managers { get; }

        void Register(IEntityManager manager);
        void Unregister(IEntityManager manager);
    }
}