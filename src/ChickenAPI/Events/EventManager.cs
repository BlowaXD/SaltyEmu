using System;
using System.Collections.Generic;

namespace ChickenAPI.Events
{
    /// <summary>
    ///     A simple Event Manager
    /// </summary>
    public class SimpleEventManager : IEventManager
    {
        private readonly Dictionary<Type, Dictionary<int, EventHandler<ChickenEventArgs>>> _eventHandlers = new Dictionary<Type, Dictionary<int, EventHandler<ChickenEventArgs>>>();

        public void Register<T>(EventHandler<ChickenEventArgs> callback) where T : ChickenEventArgs
        {
            if (!_eventHandlers.TryGetValue(typeof(T), out Dictionary<int, EventHandler<ChickenEventArgs>> handlers))
            {
                handlers = new Dictionary<int, EventHandler<ChickenEventArgs>>();
                _eventHandlers.Add(typeof(T), handlers);
            }

            handlers.Add(callback.GetHashCode(), callback);
        }

        public void Unregister<T>(EventHandler<ChickenEventArgs> callback) where T : ChickenEventArgs
        {
            if (!_eventHandlers.TryGetValue(typeof(T), out Dictionary<int, EventHandler<ChickenEventArgs>> handlers))
            {
                return;
            }

            handlers.Remove(callback.GetHashCode());
        }

        public void Invoke<T>(object sender, T @event) where T : ChickenEventArgs
        {
            if (!_eventHandlers.TryGetValue(typeof(T), out Dictionary<int, EventHandler<ChickenEventArgs>> handlers))
            {
                return;
            }

            foreach (EventHandler<ChickenEventArgs> handler in handlers.Values)
            {
                handler?.Invoke(sender, @event);
            }
        }
    }
}