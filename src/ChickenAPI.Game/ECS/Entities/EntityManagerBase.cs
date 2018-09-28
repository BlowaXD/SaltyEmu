using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Systems;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.ECS.Entities
{
    public abstract class EntityManagerBase : IEntityManager
    {
        protected static IEntityManagerContainer EmContainer = new Lazy<IEntityManagerContainer>(() => ChickenContainer.Instance.Resolve<IEntityManagerContainer>()).Value;
        protected static readonly Logger Log = Logger.GetLogger<EntityManagerBase>();

        // entities
        protected readonly Dictionary<long, IEntity> EntitiesByEntityId = new Dictionary<long, IEntity>();
        protected readonly Dictionary<VisualType, Dictionary<long, IEntity>> EntitiesByVisualType = new Dictionary<VisualType, Dictionary<long, IEntity>>();

        protected List<ISystem> _systems = new List<ISystem>();
        protected List<IEntityManager> EntityManagers = new List<IEntityManager>();

        protected long LastEntityId;


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


        public IEnumerable<T> GetEntitiesByType<T>(VisualType type) where T : class, IEntity
        {
            return !EntitiesByVisualType.TryGetValue(type, out Dictionary<long, IEntity> entities) ? null : entities.Select(s => s as T);
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
            if (!ShouldUpdate && entity.Type == VisualType.Character)
            {
                StartSystemUpdate();
            }

            EntitiesByEntityId[entity.Id] = entity;
            if (!EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities))
            {
                entities = new Dictionary<long, IEntity>
                {
                    { entity.Id, entity }
                };
            }

            entities.Add(entity.Id, entity);
            UpdateCache();
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            EntitiesByEntityId.Remove(entity.Id);
            if (EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities))
            {
                entities.Remove(entity.Id);
            }

            UpdateCache();
        }

        public bool HasEntity(IEntity entity) => HasEntity(entity.Id);

        public bool HasEntity(long id) => EntitiesByEntityId.ContainsKey(id);

        public bool DeleteEntity(IEntity entity) => EntitiesByEntityId.Remove(entity.Id);

        public void TransferEntity(long id, IMapLayer manager)
        {
            if (!EntitiesByEntityId.TryGetValue(id, out IEntity entity))
            {
                return;
            }

            TransferEntity(entity, manager);
        }

        public void TransferEntity(IEntity entity, IMapLayer manager)
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