using System;

namespace ChickenAPI.Events
{
    public interface IEventManager
    {
        /// <summary>
        ///     Register an EventHandler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        void Register<T>(EventHandler<ChickenEventArgs> callback) where T : ChickenEventArgs;

        /// <summary>
        ///     Unregister the event handler from the list of callbacks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        void Unregister<T>(EventHandler<ChickenEventArgs> callback) where T : ChickenEventArgs;

        /// <summary>
        ///     Invoke the event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="event"></param>
        void Invoke<T>(object sender, T @event) where T : ChickenEventArgs;
    }
}