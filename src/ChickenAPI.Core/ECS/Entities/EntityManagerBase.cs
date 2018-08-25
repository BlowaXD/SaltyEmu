using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.ECS.Entities
{
    public abstract class EntityManagerBase : IEntityManager
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityManagerBase>();

        // entities
        protected readonly Dictionary<long, IEntity> EntitiesByEntityId = new Dictionary<long, IEntity>();
        protected readonly Dictionary<EntityType, HashSet<IEntity>> EntitiesByEntityType = new Dictionary<EntityType, HashSet<IEntity>>();

        private IEntityManagerContainer _emContainer;
        protected List<ISystem> _systems = new List<ISystem>();

        protected List<IEntityManager> EntityManagers = new List<IEntityManager>();
        protected long LastEntityId;

        protected IEntityManagerContainer EmContainer
        {
            get => _emContainer ?? (_emContainer = ChickenContainer.Instance.Resolve<IEntityManagerContainer>());
            set => _emContainer = value;
        }

        protected bool ShouldUpdate { get; set; }

        public void Dispose()
        {
            StopSystemUpdate();
        }

        public IEntityManager ParentEntityManager { get; protected set; }
        public IReadOnlyList<ISystem> Systems => _systems;
        public IEnumerable<IEntityManager> ChildEntityManagers => EntityManagers;

        public virtual void AddChildEntityManager(IEntityManager entityManager)
        {
            EntityManagers.Add(entityManager);
        }

        public virtual void RemoveChildEntityManager(IEntityManager entityManager)
        {
            EntityManagers.Remove(entityManager);
        }

        public long NextEntityId => ++LastEntityId;
        public IEnumerable<IEntity> Entities => EntitiesByEntityId.Values.AsEnumerable();


        public IEnumerable<T> GetEntitiesByType<T>(EntityType type) where T : class, IEntity
        {
            return !EntitiesByEntityType.TryGetValue(type, out HashSet<IEntity> entities) ? new List<T>() : entities.Select(s => s as T);
        }

        public TEntity CreateEntity<TEntity>() where TEntity : class, IEntity, new()
        {
            var entity = new TEntity();
            RegisterEntity(entity);
            return entity;
        }

        public IEntity GetEntity(long id) => !EntitiesByEntityId.TryGetValue(id, out IEntity entity) ? null : entity;

        public T GetEntity<T>(long id) where T : class, IEntity => !EntitiesByEntityId.TryGetValue(id, out IEntity entity) ? null : entity as T;

        public void RegisterEntity<T>(T entity) where T : IEntity
        {
            if (!ShouldUpdate && entity.Type == EntityType.Player)
            {
                StartSystemUpdate();
            }

            entity.Id = NextEntityId;
            EntitiesByEntityId[entity.Id] = entity;
            if (!EntitiesByEntityType.TryGetValue(entity.Type, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                EntitiesByEntityType[entity.Type] = entities;
            }

            entities.Add(entity);
            UpdateCache();
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            EntitiesByEntityId.Remove(entity.Id);
            if (EntitiesByEntityType.TryGetValue(entity.Type, out HashSet<IEntity> entities))
            {
                entities.Remove(entity);
            }

            UpdateCache();
        }

        public bool HasEntity(IEntity entity) => HasEntity(entity.Id);

        public bool HasEntity(long id) => EntitiesByEntityId.ContainsKey(id);

        public bool DeleteEntity(IEntity entity) => EntitiesByEntityId.Remove(entity.Id);

        public void TransferEntity(long id, IEntityManager manager)
        {
            if (!EntitiesByEntityId.TryGetValue(id, out IEntity entity))
            {
                return;
            }

            TransferEntity(entity, manager);
        }

        public void TransferEntity(IEntity entity, IEntityManager manager)
        {
            UnregisterEntity(entity);
            manager.RegisterEntity(entity);
            entity.TransferEntity(manager);
        }

        public void Update(DateTime date)
        {
            if (!ShouldUpdate)
            {
                return;
            }

            foreach (ISystem i in Systems)
            {
                i.Update(date);
            }
        }

        public void StartSystemUpdate()
        {
            // todo tick system
            ShouldUpdate = true;
            EmContainer.Register(this);
        }

        public void StopSystemUpdate()
        {
            ShouldUpdate = false;
            EmContainer.Unregister(this);
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        private void UpdateCache()
        {
            foreach (ISystem system in _systems)
            {
                system.UpdateCache();
            }
        }
    }
}