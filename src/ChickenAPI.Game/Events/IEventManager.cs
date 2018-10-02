using System;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public interface IEventManager
    {
        void Register<T>(T handler) where T : IEventHandler;
        void Register(IEventHandler handler, Type type);
        void Unregister<T>(T handler) where T : IEventHandler;

        /// <summary>
        /// Notifies only event handlers that registered themselves for event of type <typeparam name="T"></typeparam> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Notify<T>(IEntity sender, ChickenEventArgs args);

        /// <summary>
        /// Notifies every registered event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Notify(IEntity sender, ChickenEventArgs args);
    }
}