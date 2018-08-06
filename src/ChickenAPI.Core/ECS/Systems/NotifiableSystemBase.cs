using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Core.ECS.Systems
{
    public abstract class NotifiableSystemBase : SystemBase, INotifiableSystem
    {
        protected NotifiableSystemBase(IEntityManager entityManager) : base(entityManager)
        {
        }

        public abstract void Execute(IEntity entity, SystemEventArgs e);
    }
}