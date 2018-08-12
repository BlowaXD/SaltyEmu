using System;
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

        public MovableComponent(IEntity entity, byte speed)
        {
            Entity = entity;
            Waypoints = null;
            Destination = new Position<short>();
            Actual = new Position<short>();
            Speed = speed;
        }

        public MovableComponent(IPlayerEntity entity)
        {
            Entity = entity;
            Waypoints = null;
            Destination = new Position<short>();
            Actual = new Position<short>();
            Speed = (byte)Container.Instance.Resolve<IAlgorithmService>().GetSpeed(entity.Character.Class, entity.Experience.Level);
        }

        /// <summary>
        ///     Entity Walking Speed
        /// </summary>
        public byte Speed { get; set; }

        public DirectionType DirectionType { get; set; }

        public Position<short>[] Waypoints { get; set; }

        public Position<short> Destination { get; set; }

        public Position<short> Actual
        {
            get => _actual;
            set
            {
                LastMove = DateTime.UtcNow;
                OnMove(Entity, new MoveEventArgs { Component = this, New = value, Old = _actual });
                _actual = value;
            }
        }

        public DateTime LastMove { get; set; }

        public IEntity Entity { get; set; }

        public static event TypedSenderEventHandler<IEntity, MoveEventArgs> Move;

        private static void OnMove(IEntity sender, MoveEventArgs e)
        {
            e.Component.LastMove = DateTime.UtcNow;
            Move?.Invoke(sender, e);
        }

        private static double Octile(int x, int y)
        {
            int min = Math.Min(x, y);
            int max = Math.Max(x, y);
            return min * Math.Sqrt(2) + max - min;
        }

        private static int GetDistance(Position<short> src, Position<short> dest) => (int)Octile(Math.Abs(src.X - dest.X), Math.Abs(src.Y - dest.Y));

        public bool CanMove(Position<short> newPos)
        {
            if (Speed == 0)
            {
                return false;
            }

            double waitingtime = GetDistance(newPos, Actual) / (double)Speed;
            return LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }
    }

    public class MoveEventArgs : EventArgs
    {
        public MovableComponent Component { get; set; }
        public Position<short> Old { get; set; }
        public Position<short> New { get; set; }
    }
}