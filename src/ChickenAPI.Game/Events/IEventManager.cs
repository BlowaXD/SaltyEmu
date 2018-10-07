using System;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public interface IEventManager
    {
        /// <summary>
        /// Register a filter in filters pipeline for events of Type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        void Register<T>(IEventFilter filter) where T : ChickenEventArgs;

        /// <summary>
        /// Register a filter in filters in piepline for event
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        void Register(IEventFilter filter, Type type);

        /// <summary>
        /// Registers an handler that takes type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void Register<T>(IEventHandler handler) where T : ChickenEventArgs;

        void Register(IEventHandler handler, Type type);
        void Unregister<T>(T handler) where T : IEventHandler;

        /// <summary>
        /// Notifies only event handlers that registered themselves for event of type <typeparam name="T"></typeparam> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Notify<T>(IEntity sender, T args) where T : ChickenEventArgs;

        /// <summary>
        /// Notifies every registered event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Notify(IEntity sender, ChickenEventArgs args);
    }
}