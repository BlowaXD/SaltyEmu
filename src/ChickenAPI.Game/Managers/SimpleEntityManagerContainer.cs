using System.Collections.Generic;
using ChickenAPI.Game._ECS;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Managers
{
    public class SimpleEntityManagerContainer : IEntityManagerContainer
    {
        private readonly HashSet<IEntityManager> _managers = new HashSet<IEntityManager>();

        public void Register(IEntityManager manager)
        {
            if (!_managers.Contains(manager))
            {
                _managers.Add(manager);
            }
        }

        public void Unregister(IEntityManager manager)
        {
            if (_managers.Contains(manager))
            {
                _managers.Remove(manager);
            }
        }

        public IEnumerable<IEntityManager> Managers => _managers;
    }
}