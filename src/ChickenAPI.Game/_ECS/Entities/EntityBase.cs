using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game._ECS.Entities
{
    public abstract class EntityBase : IEntity
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityBase>();
        protected static readonly IEventPipeline EventPipelineAsync = new Lazy<IEventPipeline>(() => ChickenContainer.Instance.Resolve<IEventPipeline>()).Value;
        protected Dictionary<Type, IComponent> Components = new Dictionary<Type, IComponent>();

        protected EntityBase(VisualType type, long id)
        {
            Type = type;
            Id = id;
        }


        public long Id { get; }

        public VisualType Type { get; }

        public abstract void Dispose();

        public IMapLayer CurrentMap { get; protected set; }

        public void EmitEvent<T>(T e) where T : GameEntityEvent
        {
            e.Sender = this;
            EventPipelineAsync.Notify(e).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task EmitEventAsync<T>(T e) where T : GameEntityEvent
        {
            e.Sender = this;
            return EventPipelineAsync.Notify(e);
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
            CurrentMap = map;
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