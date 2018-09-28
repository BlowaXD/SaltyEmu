using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.ECS.Entities
{
    public abstract class EntityBase : IEntity
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityBase>();
        private static IEventManager _eventManager;
        protected Dictionary<Type, IComponent> Components;

        protected EntityBase(EntityType type, Dictionary<Type, IComponent> components)
        {
            Type = type;
            Components = components;
        }

        protected EntityBase(EntityType type) => Type = type;

        protected IEventManager EventManager => _eventManager ?? (_eventManager = ChickenContainer.Instance.Resolve<IEventManager>());


        public long Id { get; set; }

        public abstract void Dispose();

        public IEntityManager EntityManager { get; protected set; }

        public void NotifyEventHandler(ChickenEventArgs e)
        {
            EventManager.Notify(this, e);
        }

        public void NotifyEventHandler<T>(ChickenEventArgs e) where T : class, IEventHandler
        {
            EventManager.Notify<T>(this, e);
        }

        public EntityType Type { get; }

        public virtual void TransferEntity(IEntityManager manager)
        {
            if (EntityManager == null)
            {
                EntityManager = manager;
                EntityManager.RegisterEntity(this);
            }
            else
            {
                EntityManager.TransferEntity(this, manager);
            }
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