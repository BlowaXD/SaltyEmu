using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.ECS.Entities
{
    public abstract class EntityManagerBase : IEntityManager
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityManagerBase>();
        protected bool Update;
        protected long LastEntityId;

        // entities
        protected readonly Dictionary<long, IEntity> EntitiesByEntityId = new Dictionary<long, IEntity>();
        protected readonly Dictionary<EntityType, HashSet<IEntity>> EntitiesByEntityType = new Dictionary<EntityType, HashSet<IEntity>>();

        protected List<IEntityManager> EntityManagers = new List<IEntityManager>();

        // systems
        protected Dictionary<Type, INotifiableSystem> NotifiableSystems = new Dictionary<Type, INotifiableSystem>();
        protected List<ISystem> _systems = new List<ISystem>();

        public void Dispose()
        {
            throw new NotImplementedException();
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

        public IEntity GetEntity(long id)
        {
            return !EntitiesByEntityId.TryGetValue(id, out IEntity entity) ? null : entity;
        }

        public T GetEntity<T>(long id) where T : class, IEntity
        {
            return !EntitiesByEntityId.TryGetValue(id, out IEntity entity) ? null : entity as T;
        }

        public void RegisterEntity<T>(T entity) where T : IEntity
        {
            entity.Id = NextEntityId;
            EntitiesByEntityId[entity.Id] = entity;
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            EntitiesByEntityId.Remove(entity.Id);

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

        public void StartSystemUpdate(int delay)
        {
            // todo tick system
            Update = true;
        }

        public void StopSystemUpdate()
        {
            Update = false;
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        public void NotifySystem<T>(IEntity entity, SystemEventArgs e) where T : class, INotifiableSystem
        {
            try
            {
                NotifiableSystems[typeof(T)].Execute(entity, e);
            }
            catch (Exception exception)
            {
                Log.Error("[NOTIFY_SYSTEM]", exception);
                Console.WriteLine(exception);
            }
        }

        public void NotifySystems(IEntity entity, SystemEventArgs e)
        {
            foreach (INotifiableSystem system in NotifiableSystems.Values)
            {
                try
                {
                    system.Execute(entity, e);
                }
                catch (Exception exception)
                {
                    Log.Error("[NOTIFY_SYSTEM]", exception);
                    Console.WriteLine(exception);
                }
            }
        }
    }
}