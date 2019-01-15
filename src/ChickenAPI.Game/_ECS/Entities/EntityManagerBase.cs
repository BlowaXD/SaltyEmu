using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Systems;

namespace ChickenAPI.Game._ECS.Entities
{
    public abstract class EntityManagerBase : IEntityManager
    {
        protected static IEntityManagerContainer EmContainer = new Lazy<IEntityManagerContainer>(() => ChickenContainer.Instance.Resolve<IEntityManagerContainer>()).Value;
        protected static readonly Logger Log = Logger.GetLogger<EntityManagerBase>();

        // entities
        protected readonly HashSet<IPlayerEntity> _players = new HashSet<IPlayerEntity>();
        protected readonly Dictionary<VisualType, Dictionary<long, IEntity>> EntitiesByVisualType = new Dictionary<VisualType, Dictionary<long, IEntity>>();
        protected readonly HashSet<IEntity> EntitiesSet = new HashSet<IEntity>();
        protected readonly object LockObj = new object();

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
            lock(LockObj)
            {
                if (entity.Type == VisualType.Character)
                {
                    _players.Add(entity as IPlayerEntity);
                }

                EntitiesSet.Add(entity);
                if (!EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities))
                {
                    entities = new Dictionary<long, IEntity>();
                    EntitiesByVisualType[entity.Type] = entities;
                }

                entities.Add(entity.Id, entity);
                UpdateCache();
                // player cache
                if (!ShouldUpdate && entity.Type == VisualType.Character)
                {
                    StartSystemUpdate();
                }
            }
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            lock(LockObj)
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
        }

        public bool HasEntity(IEntity entity) => HasEntity(entity.Id, entity.Type);

        public bool HasEntity(long id, VisualType type) => EntitiesByVisualType.TryGetValue(type, out Dictionary<long, IEntity> entities) && entities.ContainsKey(id);

        public bool DeleteEntity(IEntity entity) => EntitiesByVisualType.TryGetValue(entity.Type, out Dictionary<long, IEntity> entities) && entities.Remove(entity.Id);

        public void TransferEntity(IEntity entity, IMapLayer manager)
        {
            UnregisterEntity(entity);
            manager.RegisterEntity(entity);
        }

        public void Update(DateTime date)
        {
            if (!ShouldUpdate)
            {
                return;
            }

            lock(LockObj)
            {
                foreach (ISystem i in Systems)
                {
                    i.Update(date);
                }
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
            lock(LockObj)
            {
                _systems.Add(system);
            }
        }

        public void RemoveSystem(ISystem system)
        {
            lock(LockObj)
            {
                _systems.Remove(system);
            }
        }

        private void UpdateCache()
        {
            lock(LockObj)
            {
                foreach (ISystem system in _systems)
                {
                    system.UpdateCache();
                }
            }
        }
    }
}