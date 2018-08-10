using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Movement
{
    /// <summary>
    /// </summary>
    public class MovableComponent : IComponent
    {
        private static readonly Logger Log = Logger.GetLogger<MovableComponent>();
        private Position<short> _actual;

        public MovableComponent(IEntity entity)
        {
            Entity = entity;
            Waypoints = new Queue<Position<short>>();
            Destination = new Position<short>();
            Actual = new Position<short>();
            Speed = 11;
        }

        public MovableComponent(IPlayerEntity entity)
        {
            Entity = entity;
            Waypoints = new Queue<Position<short>>();
            Destination = new Position<short>();
            Actual = new Position<short>();
            Speed = (byte)Container.Instance.Resolve<IAlgorithmService>().GetSpeed(entity.Character.Class, entity.Experience.Level);
        }

        /// <summary>
        ///     Entity Walking Speed
        /// </summary>
        public byte Speed { get; set; }

        public DirectionType DirectionType { get; set; }

        public Queue<Position<short>> Waypoints { get; set; }

        public Position<short> Destination { get; set; }

        public Position<short> Actual
        {
            get => _actual;
            set
            {
                OnMove(Entity, new MoveEventArgs { Component = this, New = value, Old = _actual });
                _actual = value;
            }
        }

        public DateTime LastMove { get; private set; }

        public IEntity Entity { get; set; }

        public static event TypedSenderEventHandler<IEntity, MoveEventArgs> Move;

        private static void OnMove(IEntity sender, MoveEventArgs e)
        {
            e.Component.LastMove = DateTime.UtcNow;
            Move?.Invoke(sender, e);
        }

        public bool CanMove()
        {
            if (Entity.Type != EntityType.Monster)
            {
                return (DateTime.Now - LastMove).TotalMilliseconds > 2000 / Speed;
            }

            Log.Info($"Monster CanMove()");
            Speed = 20;

            return (DateTime.Now - LastMove).TotalMilliseconds > 2000 / Speed;
        }
    }

    public class MoveEventArgs : EventArgs
    {
        public MovableComponent Component { get; set; }
        public Position<short> Old { get; set; }
        public Position<short> New { get; set; }
    }
}