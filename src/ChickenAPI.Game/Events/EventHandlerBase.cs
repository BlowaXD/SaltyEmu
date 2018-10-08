using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class EventHandlerBase : IEventHandler
    {
        public abstract ISet<Type> HandledTypes { get; }
        public abstract void Execute(IEntity entity, ChickenEventArgs e);
    }
}