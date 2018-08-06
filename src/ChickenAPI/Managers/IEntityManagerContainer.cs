using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Managers
{
    public interface IEntityManagerContainer
    {
        void Register(IEntityManager manager);
        void Unregister(IEntityManager manager);

        IEnumerable<IEntityManager> Managers { get; }
    }
}