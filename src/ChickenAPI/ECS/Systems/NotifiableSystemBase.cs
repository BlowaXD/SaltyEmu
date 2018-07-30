using System;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.ECS.Systems
{
    public abstract class NotifiableSystemBase : SystemBase, INotifiableSystem
    {
        protected NotifiableSystemBase(IEntityManager entityManager) : base(entityManager)
        {
        }

        public abstract void Execute(IEntity entity, SystemEventArgs e);
    }
}