using System;
using System.Linq.Expressions;
using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server;

namespace ChickenAPI.Game.Systems.Movable
{
    public class MovableSystem : NotifiableSystemBase
    {
        public MovableSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.HasComponent<MovableComponent>();

        public override void Execute(IEntity entity)
        {
            if (!Match(entity))
            {
                return;
            }
            var movable = entity.GetComponent<MovableComponent>();

            if (movable.Waypoints.Count <= 0 || !movable.CanMove())
            {
                return;
            }

            movable.Actual = movable.Waypoints.Dequeue();
            Move(entity);
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            throw new NotImplementedException();
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