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
        protected readonly HashSet<IPlayerEntity> _players = new HashSet<IPlayerEntity>();
        protected readonly HashSet<IEntity> EntitiesSet = new HashSet<IEntity>();
        protected readonly Dictionary<VisualType, Dictionary<long, IEntity>> EntitiesByVisualType = new Dictionary<VisualType, Dictionary<long, IEntity>>();

        protected List<ISystem> _systems = new List<ISystem>();


        protected bool ShouldUpdate { get; set; }

        public void Dispose()
        {
            StopSystemUpdate();
        }

        public IEntityManager ParentEntityManager { get; protected set; }
        public IReadOnlyList<ISystem> Systems => _systems;

        public IEnumerable<IEntity> Entities => EntitiesSet;


        public IEnumerable<T> GetEntitiesByType<T>(VisualType type) where T : class, IEntity
        {
            return !EntitiesByVisualType.TryGetValue(type, out Dictionary<long, IEntity> entities) ? null : entities.Select(s => s.Value as T);
        }

        public IEntity GetEntity(long id, VisualType type) =>
            EntitiesByVisualType.TryGetValue(type, out Dictionary<long, IEntity> entities) && entities.TryGetValue(id, out IEntity entity) ? entity : null;

        public T GetEntity<T>(long id, VisualType type) where T : class, IEntity => GetEntity(id, type) as T;

        public void RegisterEntity<T>(T entity) where T : IEntity
        {
            if (entity.Type == VisualType.Character)
            {
                _players.Add(entity as IPlayerEntity);
                // player cache
                if (!ShouldUpdate)
                {
                    StartSystemUpdate();
                }
            }

            EntitiesSet.Add(entity);
            if (!EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities))
            {
                entities = new Dictionary<long, IEntity>();
                EntitiesByVisualType[entity.Type] = entities;
            }

            entities.Add(entity.Id, entity);
            UpdateCache();
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            if (entity.Type == VisualType.Character)
            {
                _players.Remove(entity as IPlayerEntity);
            }

            EntitiesSet.Remove(entity);
            if (EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities))
            {
                entities.Remove(entity.Id);
            }

            UpdateCache();
        }

        public bool HasEntity(IEntity entity) => HasEntity(entity.Id, entity.Type);

        public bool HasEntity(long id, VisualType type) => EntitiesByVisualType.TryGetValue(type, out Dictionary<long, IEntity> entities) && entities.ContainsKey(id);

        public bool DeleteEntity(IEntity entity) => EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities) && entities.Remove(entity.Id);

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