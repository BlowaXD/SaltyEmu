using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Systems;

namespace ChickenAPI.Core.ECS.Entities
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    ///     Contains entities and sub <see cref="T:ChickenAPI.Core.ECS.Entities.IEntityManager" />
    /// </summary>
    public interface IEntityManager : IDisposable
    {
        #region EntityManager

        /// <summary>
        ///     Gets the parent context, returns null if none
        /// </summary>
        IEntityManager ParentEntityManager { get; }


        IReadOnlyList<ISystem> Systems { get; }


        /// <summary>
        ///     Gets the child contexts, returns null if none
        /// </summary>
        IEnumerable<IEntityManager> ChildEntityManagers { get; }

        /// <summary>
        ///     Add a child context to the actual context
        /// </summary>
        /// <param name="entityManager"></param>
        void AddChildEntityManager(IEntityManager entityManager);

        /// <summary>
        ///     Remove the child context from the actual context
        /// </summary>
        /// <param name="entityManager"></param>
        void RemoveChildEntityManager(IEntityManager entityManager);

        #endregion

        #region Entities

        long NextEntityId { get; }

        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        ///     Get all entities with the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntitiesByType<T>(EntityType type) where T : class, IEntity;

        /// <summary>
        ///     Creates a new entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity concrete type.</typeparam>
        /// <returns>New entity</returns>
        TEntity CreateEntity<TEntity>() where TEntity : class, IEntity, new();

        /// <summary>
        ///     Get the entity from entity manager by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEntity GetEntity(long id);

        /// <summary>
        ///     Returns the entity as a <see cref="T" /> from entity manager by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntity<T>(long id) where T : class, IEntity;

        /// <summary>
        ///     Register an entity to the entity container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void RegisterEntity<T>(T entity) where T : IEntity;

        /// <summary>
        ///     Unregister the entity from the entity manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void UnregisterEntity<T>(T entity) where T : IEntity;

        /// <summary>
        ///     Returns if the entity is contained in the entity manager
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool HasEntity(IEntity entity);

        /// <summary>
        ///     Returns if the entity is contained in the entity manager by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HasEntity(long id);

        /// <summary>
        ///     Deletes an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Deleted state</returns>
        bool DeleteEntity(IEntity entity);

        /// <summary>
        ///     Transfer the entity contained in the entity manger to another one by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="manager"></param>
        void TransferEntity(long id, IEntityManager manager);

        /// <summary>
        ///     Transfer the entity contained in the entity manger to another one
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="manager"></param>
        void TransferEntity(IEntity entity, IEntityManager manager);

        #endregion

        #region System

        void Update(DateTime date);

        void StartSystemUpdate();

        /// <summary>
        ///     Stops the context update.
        /// </summary>
        void StopSystemUpdate();

        /// <summary>
        ///     Adds a new system to the context.
        /// </summary>
        /// <param name="system">System</param>
        void AddSystem(ISystem system);

        /// <summary>
        ///     Removes a system from the context.
        /// </summary>
        /// <param name="system"></param>
        void RemoveSystem(ISystem system);

        #endregion
    }
}