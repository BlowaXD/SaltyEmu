using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.Events
{
    public class EventManager : IEventManager
    {
        private static readonly Logger Log = Logger.GetLogger<EventManager>();
        private readonly Dictionary<Type, IEventHandler> _eventHandlersByType = new Dictionary<Type, IEventHandler>();

        public void Register<T>(T handler) where T : IEventHandler
        {
            Register(handler, typeof(T));
        }

        public void Register(IEventHandler handler, Type type)
        {
            if (!_eventHandlersByType.ContainsKey(type))
            {
                _eventHandlersByType.Add(type, handler);
            }
        }

        public void Unregister<T>(T handler) where T : IEventHandler
        {
            _eventHandlersByType.Remove(typeof(T));
        }

        public void Notify<T>(IEntity sender, ChickenEventArgs args)
        {
            try
            {
                _eventHandlersByType[typeof(T)].Execute(sender, args);
            }
            catch (Exception e)
            {
                Log.Error($"Notify<{typeof(T).Name}>()", e);
            }
        }

        public void Notify(IEntity sender, ChickenEventArgs args)
        {
            foreach (IEventHandler i in _eventHandlersByType.Values)
            {
                try
                {
                    i.Execute(sender, args);
                }
                catch (Exception e)
                {
                    Log.Error("Notify()", e);
                }
            }
        }
    }
}