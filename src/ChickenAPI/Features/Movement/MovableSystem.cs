using System;
using System.Linq;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Movement
{
    public class MovableSystem : NotifiableSystemBase
    {
        public MovableSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override double RefreshRate => 3;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == EntityType.Player)
            {
                return false;
            }

            var movable = entity.GetComponent<MovableComponent>();
            if (movable == null)
            {
                return false;
            }

            return movable.Speed != 0;
        }

        protected override void Execute(IEntity entity)
        {
            var movableComponent = entity.GetComponent<MovableComponent>();
            ProcessMovement(entity, movableComponent);
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case UpdateCacheEventArgs update:
                    UpdateCacheRequest = true;
                    break;
            }
        }

        private void Move(IEntity entity)
        {
            try
            {
                var packet = new MvPacket(entity);
                if (EntityManager is IMapLayer mapLayer) // wtf ?
                {
                    mapLayer.Broadcast(packet);
                }

                if (entity is IPlayerEntity playerEntity)
                {
                    playerEntity.SendPacket(new CondPacketBase(playerEntity));
                }
            }
            catch (Exception e)
            {
                Log.Error("Move()", e);
            }
        }

        private void ProcessMovement(IEntity entity, MovableComponent movableComponent)
        {
            if (movableComponent.Waypoints == null || movableComponent.Waypoints.Length <= 0)
            {
                return;
            }

            byte speedIndex = (byte)(movableComponent.Speed / 2 < 1 ? 1 : movableComponent.Speed / 2);
            int maxindex = movableComponent.Waypoints.Length > speedIndex ? speedIndex : movableComponent.Waypoints.Length;
            Position<short> newPos = movableComponent.Waypoints[maxindex - 1];

            if (!movableComponent.CanMove(newPos))
            {
                return;
            }

            movableComponent.Actual = movableComponent.Waypoints[maxindex - 1];
            movableComponent.Waypoints = movableComponent.Waypoints.Skip(maxindex).ToArray();
            Move(entity);
        }
    }
}