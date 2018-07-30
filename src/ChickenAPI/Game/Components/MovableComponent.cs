using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Data.AccessLayer.Character;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Components
{
    /// <summary>
    /// </summary>
    public class MovableComponent : IComponent
    {
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

        private Position<short> _actual;

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

        public DateTime NextMove { get; set; }

        public IEntity Entity { get; set; }

        public static event TypedSenderEventHandler<IEntity, MoveEventArgs> Move;

        private static void OnMove(IEntity sender, MoveEventArgs e)
        {
            e.Component.LastMove = DateTime.Now;
            // todo tick the systems update
            e.Component.NextMove = DateTime.Now;
            Move?.Invoke(sender, e);
        }

        public bool CanMove() => (DateTime.Now - LastMove).TotalMilliseconds > 2000 / Speed;
    }

    public class MoveEventArgs : EventArgs
    {
        public MovableComponent Component { get; set; }
        public Position<short> Old { get; set; }
        public Position<short> New { get; set; }
    }
}