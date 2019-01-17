using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Movements.DataObjects
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class MovableComponent : IComponent
    {
        private Position<short> _actual;
        private bool _isSitting;

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
            Speed = (byte)Algorithm.GetSpeed(entity.Character.Class, entity.Level);
        }

        private static IAlgorithmService Algorithm => new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;

        /// <summary>
        ///     Entity Walking Speed
        /// </summary>
        public byte Speed { get; set; }

        public bool IsSitting
        {
            get => _isSitting;
            set
            {
                _isSitting = value;
                Entity.CurrentMap.BroadcastAsync(Entity.GenerateRestPacket()).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

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
    }
}