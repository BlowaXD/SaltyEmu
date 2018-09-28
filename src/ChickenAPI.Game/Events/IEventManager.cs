﻿using System;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public interface IEventManager
    {
        void Register<T>(T handler) where T : IEventHandler;
        void Register(IEventHandler handler, Type type);
        void Unregister<T>(T handler) where T : IEventHandler;

        // notify only system with typeof(T)
        void Notify<T>(IEntity sender, ChickenEventArgs args);

        // notify everyone
        void Notify(IEntity sender, ChickenEventArgs args);
    }
}