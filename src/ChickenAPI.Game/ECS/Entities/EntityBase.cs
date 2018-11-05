using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.ECS.Entities
{
    public abstract class EntityBase : IEntity
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityBase>();
        protected static readonly IEventManager EventManager = new Lazy<IEventManager>(() => ChickenContainer.Instance.Resolve<IEventManager>()).Value;
        protected Dictionary<Type, IComponent> Components;

        protected EntityBase(VisualType type, long id)
        {
            Type = type;
            Id = id;
        }


        public long Id { get; }

        public VisualType Type { get; }

        public abstract void Dispose();

        public IMapLayer CurrentMap { get; protected set; }

        public void EmitEvent<T>(T e) where T : ChickenEventArgs
        {
            e.Sender = this;
            EventManager.Notify(this, e);
        }

        public virtual void TransferEntity(IMapLayer map)
        {
            if (CurrentMap == map)
            {
                return;
            }

            if (CurrentMap == null)
            {
                CurrentMap = map;
            }

            CurrentMap.TransferEntity(this, map);
        }

        public virtual void AddComponent<T>(T component) where T : IComponent
        {
            if (!Components.TryGetValue(typeof(T), out IComponent comp))
            {
                Components[typeof(T)] = component;
            }
        }

        public virtual void RemoveComponent<T>(T component) where T : IComponent
        {
            Components.Remove(typeof(T));
        }

        public virtual bool HasComponent<T>() where T : IComponent => Components.ContainsKey(typeof(T));

        public virtual T GetComponent<T>() where T : class, IComponent => !Components.TryGetValue(typeof(T), out IComponent component) ? null : component as T;
    }
}