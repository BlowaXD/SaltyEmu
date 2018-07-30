using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;
using ChickenAPI.Utils;

namespace ChickenAPI.ECS.Entities
{
    public abstract class EntityManagerBase : IEntityManager
    {
        protected static readonly Logger Log = Logger.GetLogger<EntityManagerBase>();
        protected bool Update;
        protected long LastEntityId;

        // entities
        protected readonly Dictionary<long, IEntity> EntitiesByEntityId = new Dictionary<long, IEntity>();

        // players
        protected readonly Dictionary<long, IPlayerEntity> PlayersBySessionId = new Dictionary<long, IPlayerEntity>();
        protected readonly Dictionary<long, IPlayerEntity> PlayersByEntityId = new Dictionary<long, IPlayerEntity>();

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
        public IEnumerable<IPlayerEntity> Players => PlayersBySessionId.Values;

        public IEnumerable<T> GetEntitiesByType<T>(EntityType type) where T : IEntity
        {
            // todo implementation
            return null;
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
            switch (entity.Type)
            {
                case EntityType.Player:
                    if (entity is IPlayerEntity sess)
                    {
                        PlayersByEntityId.Add(entity.Id, sess);
                        PlayersBySessionId.Add(sess.Session.SessionId, sess);
                    }

                    break;
                default:
                    break;
            }
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            EntitiesByEntityId.Remove(entity.Id);

            switch (entity.Type)
            {
                case EntityType.Player:
                    if (entity is IPlayerEntity sess)
                    {
                        PlayersByEntityId.Remove(entity.Id);
                        PlayersBySessionId.Remove(sess.Session.SessionId);
                    }

                    break;
                default:
                    break;
            }
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

        public void Broadcast<T>(T packet) where T : IPacket
        {
            foreach (IEntity entity in EntitiesByEntityId.Values.AsParallel().Where(s => s.Type == EntityType.Player))
            {
                if (!(entity is IPlayerEntity session))
                {
                    continue;
                }

                session.SendPacket(packet);
            }
        }

        public void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket
        {
            foreach (IPlayerEntity session in PlayersBySessionId.Values)
            {
                session.SendPacket(packet);
            }
        }
    }
}