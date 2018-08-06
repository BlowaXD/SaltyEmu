using System;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Movement
{
    public class MovableSystem : NotifiableSystemBase
    {
        public MovableSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.HasComponent<MovableComponent>();

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IMovableEntity movableEntity))
            {
                return;
            }

            MovableComponent movable = movableEntity.Movable;

            if (movable.Waypoints.Count <= 0 || !movable.CanMove())
            {
                return;
            }

            movable.Actual = movable.Waypoints.Dequeue();
            Move(entity);
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
            foreach (IEntity entityy in entity.EntityManager.Entities)
            {
                if (Match(entityy) && entityy is IPlayerEntity player)
                {
                    player.SendPacket(new MvPacket(entityy));
                }
            }

            if (entity is IPlayerEntity playerEntity)
            {
                playerEntity.SendPacket(new CondPacketBase(playerEntity));
            }
        }
    }
}