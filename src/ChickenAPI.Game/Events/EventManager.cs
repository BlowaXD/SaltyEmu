using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public class EventManager : IEventManager
    {
        private static readonly Logger Log = Logger.GetLogger<EventManager>();
        private readonly Dictionary<Type, List<IEventFilter>> _eventFiltersByType = new Dictionary<Type, List<IEventFilter>>();
        private readonly Dictionary<Type, List<IEventHandler>> _eventHandlersByType = new Dictionary<Type, List<IEventHandler>>();

        public void Register<T>(IEventFilter filter) where T : ChickenEventArgs
        {
            Register(filter, typeof(T));
        }

        public void Register(IEventFilter filter, Type type)
        {
            if (!_eventFiltersByType.TryGetValue(type, out List<IEventFilter> filters))
            {
                filters = new List<IEventFilter>();
                _eventFiltersByType[type] = filters;
            }

            filters.Add(filter);
        }

        public void Register<T>(IEventHandler handler) where T : ChickenEventArgs
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

        public void Notify<T>(IEntity sender, T args) where T : ChickenEventArgs
        {
            if (!_eventHandlersByType.TryGetValue(typeof(T), out List<IEventHandler> handlers))
            {
                return;
            }

            if (args.Sender == null)
            {
                args.Sender = sender;
            }


            if (!CanSendEvent(sender, args, typeof(T)))
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

        private bool CanSendEvent(IEntity entity, ChickenEventArgs e, Type type)
        {
            return !_eventFiltersByType.TryGetValue(type, out List<IEventFilter> filters) || filters.All(filter => filter.Filter(entity, e));
        }

        public void Notify(IEntity sender, ChickenEventArgs args)
        {
            if (args.Sender == null)
            {
                args.Sender = sender;
            }

            foreach (KeyValuePair<Type, List<IEventHandler>> events in _eventHandlersByType)
            {
                if (!CanSendEvent(sender, args, events.Key))
                {
                    continue;
                }

                foreach (IEventHandler handler in events.Value)
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