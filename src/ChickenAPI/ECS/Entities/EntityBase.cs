using System;
using System.Collections.Generic;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Utils;

namespace ChickenAPI.ECS.Entities
{
    public abstract class EntityBase : IEntity
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityBase>();
        protected Dictionary<Type, IComponent> Components;

        protected EntityBase(EntityType type, Dictionary<Type, IComponent> components)
        {
            Type = type;
            Components = components;
        }

        protected EntityBase(EntityType type) => Type = type;


        public long Id { get; set; }

        public abstract void Dispose();

        public IEntityManager EntityManager { get; protected set; }

        public void NotifySystem<T>(SystemEventArgs e) where T : class, INotifiableSystem
        {
            EntityManager.NotifySystem<T>(this, e);
        }

        public EntityType Type { get; }

        public virtual void TransferEntity(IEntityManager manager)
        {
            if (EntityManager == null)
            {
                EntityManager = manager;
                EntityManager.RegisterEntity(this);
                Log.Info($"[ENTITY:{Id}] Initializing EntityManager");
            }
            else
            {
                EntityManager.TransferEntity(this, manager);
            }
        }

        public virtual void AddComponent<T>(T component) where T : IComponent
        {
        }

        public virtual void RemoveComponent<T>(T component) where T : IComponent
        {
            throw new NotImplementedException();
        }

        public virtual bool HasComponent<T>() where T : IComponent
        {
            return Components.ContainsKey(typeof(T));
        }

        public virtual T GetComponent<T>() where T : class, IComponent
        {
            return !Components.TryGetValue(typeof(T), out IComponent component) ? null : component as T;
        }
    }
}