using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS
{
    public interface IEntityManagerContainer
    {
        IEnumerable<IEntityManager> Managers { get; }
        void Register(IEntityManager manager);
        void Unregister(IEntityManager manager);
    }
}