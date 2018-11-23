using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class EventHandlerBase : IEventHandler
    {
        protected static readonly ILanguageService LanguageService = new Lazy<ILanguageService>(() => ChickenContainer.Instance.Resolve<ILanguageService>()).Value;
        public abstract ISet<Type> HandledTypes { get; }
        public abstract void Execute(IEntity entity, ChickenEventArgs e);
    }
}