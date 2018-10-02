using System;
using System.Collections.Generic;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public class EventManager : IEventManager
    {
        private static readonly Logger Log = Logger.GetLogger<EventManager>();
        private readonly Dictionary<Type, List<IEventHandler>> _eventHandlersByType = new Dictionary<Type, List<IEventHandler>>();

        public void Register<T>(T handler) where T : IEventHandler
        {
            Register(handler, typeof(T));
        }

        public void Register(IEventHandler handler, Type type)
        {
            if (!_eventHandlersByType.TryGetValue(type, out List<IEventHandler> handlers))
            {
                handlers = new List<IEventHandler>();
                _eventHandlersByType[type] = handlers;
            }

            handlers.Add(handler);
        }

        public void Unregister<T>(T handler) where T : IEventHandler
        {
            if (!_eventHandlersByType.TryGetValue(typeof(T), out List<IEventHandler> handlers))
            {
                return;
            }

            handlers.Remove(handler);
        }

        public void Notify<T>(IEntity sender, ChickenEventArgs args)
        {
            if (!_eventHandlersByType.TryGetValue(typeof(T), out List<IEventHandler> handlers))
            {
                return;
            }

            foreach (IEventHandler handler in handlers)
            {
                try
                {
                    handler.Execute(sender, args);
                }
                catch (Exception e)
                {
                    Log.Error($"Notify<{typeof(T).Name}>()", e);
                }
            }
        }

        public void Notify(IEntity sender, ChickenEventArgs args)
        {
            foreach (List<IEventHandler> handlers in _eventHandlersByType.Values)
            {
                foreach (IEventHandler handler in handlers)
                {
                    try
                    {
                        handler.Execute(sender, args);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Notify()", e);
                    }
                }
            }
        }
    }
}