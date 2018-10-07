using System;
using System.Linq.Expressions;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class BasicEventFilter<T> : IEventFilter where T : ChickenEventArgs
    {
        private Func<IEntity, T, bool> _filter;
        protected virtual Expression<Func<IEntity, T, bool>> CheckDelegate { get; }

        public bool Check(IEntity entity, T args)
        {
            _filter = _filter ?? CheckDelegate.Compile();

            return (bool)_filter?.Invoke(entity, args);
        }

        public bool Filter(IEntity entity, ChickenEventArgs e)
        {
            if (e is T args)
            {
                return Check(entity, args);
            }

            return false;
        }
    }
}