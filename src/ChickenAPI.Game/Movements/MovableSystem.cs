using System;
using System.Linq;
using System.Linq.Expressions;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.ECS.Systems;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Movements
{
    public class MovableSystem : SystemBase
    {
        public MovableSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override double RefreshRate => 3;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == VisualType.Character)
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

        private void Move(IEntity entity)
        {
            try
            {
                MvPacket packet = entity.GenerateMvPacket();
                if (EntityManager is IMapLayer mapLayer) // wtf ?
                {
                    mapLayer.Broadcast(packet);
                }

                if (entity is IPlayerEntity playerEntity)
                {
                    playerEntity.SendPacket(playerEntity.GenerateCondPacket());
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