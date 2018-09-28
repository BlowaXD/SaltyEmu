﻿using System;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.ECS.Entities
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
        /// Notifies all event handlers with the given event
        /// </summary>
        /// <param name="e"></param>
        void NotifyEventHandler(ChickenEventArgs e);

        /// <summary>
        ///     Notify a system of the entity manager to be executed.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="e">Arguments</param>
        void NotifyEventHandler<T>(ChickenEventArgs e) where T : class, IEventHandler;

        /// <summary>
        ///     Will transfer the Entity to another entity manager
        /// </summary>
        /// <param name="manager"></param>
        void TransferEntity(IMapLayer manager);

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