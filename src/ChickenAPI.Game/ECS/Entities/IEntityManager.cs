using System;
using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Systems;

namespace ChickenAPI.Game.ECS.Entities
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    ///     Contains entities and sub <see cref="T:ChickenAPI.Game.ECS.Entities.IEntityManager" />
    /// </summary>
    public interface IEntityManager : IDisposable
    {
        #region EntityManager

        /// <summary>
        ///     Gets the parent context, returns null if none
        /// </summary>
        IEntityManager ParentEntityManager { get; }


        IReadOnlyList<ISystem> Systems { get; }

        #endregion

        #region Entities

        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        ///     Get all entities with the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntitiesByType<T>(VisualType type) where T : class, IEntity;

        /// <summary>
        ///     Get the entity from entity manager by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEntity GetEntity(long id, VisualType type);

        /// <summary>
        ///     Returns the entity as a <see cref="T" /> from entity manager by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        T GetEntity<T>(long id, VisualType type) where T : class, IEntity;

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
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasEntity(long id, VisualType type);

        /// <summary>
        ///     Deletes an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Deleted state</returns>
        bool DeleteEntity(IEntity entity);

        /// <summary>
        ///     Transfer the entity contained in the entity manger to another one
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="manager"></param>
        void TransferEntity(IEntity entity, IMapLayer manager);

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