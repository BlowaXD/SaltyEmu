using System;
using System.Linq.Expressions;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class BasicEventFilter<T> : IEventFilter where T : GameEntityEvent
    {
        private Func<IEntity, T, bool> _filter;
        protected abstract Expression<Func<IEntity, T, bool>> CheckDelegate { get; }

        public bool Filter(IEntity entity, GameEntityEvent e) => e is T args && Check(entity, args);

        public bool Check(IEntity entity, T args)
        {
            _filter = _filter ?? CheckDelegate.Compile();

            return (bool)_filter?.Invoke(entity, args);
        }
    }
}