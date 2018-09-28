using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS
{
    public interface IEntityManagerContainer
    {
        IEnumerable<IEntityManager> Managers { get; }
        void Register(IEntityManager manager);
        void Unregister(IEntityManager manager);
    }
}