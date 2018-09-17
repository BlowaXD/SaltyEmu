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
using ChickenAPI.Game.Maps;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Game.Features.Movement.Extensions;

namespace ChickenAPI.Game.Features.Movement
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class MovableComponent : IComponent
    {
        private static IAlgorithmService _algorithmService;
        private static readonly Logger Log = Logger.GetLogger<MovableComponent>();
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
            Speed = (byte)Algorithm.GetSpeed(entity.Character.Class, entity.Experience.Level);
        }

        private static IAlgorithmService Algorithm => _algorithmService ?? (_algorithmService = ChickenContainer.Instance.Resolve<IAlgorithmService>());

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
                if (Entity.EntityManager is IMapLayer mapLayer)
                {
                    mapLayer.Broadcast(Entity.GenerateRestPacket());
                }
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

    public class MoveEventArgs : EventArgs
    {
        public MovableComponent Component { get; set; }
        public Position<short> Old { get; set; }
        public Position<short> New { get; set; }
    }
}