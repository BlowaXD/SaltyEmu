using System;
using System.Linq.Expressions;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class BasicEventFilter<T> : IEventFilter where T : ChickenEventArgs
    {
        private Func<IEntity, T, bool> _filter;
        protected abstract Expression<Func<IEntity, T, bool>> CheckDelegate { get; }

        public bool Check(IEntity entity, T args)
        {
            _filter = _filter ?? CheckDelegate.Compile();

            return (bool)_filter?.Invoke(entity, args);
        }

        public bool Filter(IEntity entity, ChickenEventArgs e)
        {
            return e is T args && Check(entity, args);
        }
    }
}