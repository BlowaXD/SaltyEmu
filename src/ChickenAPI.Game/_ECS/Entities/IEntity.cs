using System;
using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game._ECS.Entities
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    ///     Defines an entity
    /// </summary>
    public interface IEntity : IDisposable
    {
        long Id { get; }

        /// <summary>
        ///     Gets the entity type
        /// </summary>
        VisualType Type { get; }

        /// <summary>
        ///     Gets the entityManager where the Entity is registered
        /// </summary>
        IMapLayer CurrentMap { get; }

        /// <summary>
        ///     Notify a system of the entity manager to be executed.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="e">Arguments</param>
        void EmitEvent<T>(T e) where T : GameEntityEvent;

        Task EmitEventAsync<T>(T e) where T : GameEntityEvent;

        /// <summary>
        ///     Will transfer the Entity to another entity manager
        /// </summary>
        /// <param name="map"></param>
        void TransferEntity(IMapLayer map);

        /// <summary>
        ///     Add the component to the actual <see cref="IEntity" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        void AddComponent<T>(T component) where T : IComponent;

        /// <summary>
        ///     Remove the component from the actual <see cref="IEntity" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        void RemoveComponent<T>(T component) where T : IComponent;


        /// <summary>
        ///     Returns if the entity contains the component of type <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool HasComponent<T>() where T : IComponent;

        /// <summary>
        ///     Get the component from the actual <see cref="IEntity" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>null in case entity does not contain the component</returns>
        T GetComponent<T>() where T : class, IComponent;
    }
}