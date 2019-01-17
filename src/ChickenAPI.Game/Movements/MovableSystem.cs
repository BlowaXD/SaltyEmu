using System;
using System.Linq;
using System.Linq.Expressions;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.IAs;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._ECS.Systems;
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

            if (!(entity is IMovableEntity mov))
            {
                return false;
            }

            return mov.Speed != 0;
        }

        protected override void Execute(IEntity entity)
        {
            ProcessMovement((IMovableEntity)entity);
        }

        private void Move(IEntity entity)
        {
            try
            {
                MvPacket packet = entity.GenerateMvPacket();
                entity.CurrentMap.BroadcastAsync(packet).ConfigureAwait(false).GetAwaiter().GetResult();

                if (entity is IPlayerEntity playerEntity)
                {
                    playerEntity.SendPacketAsync(playerEntity.GenerateCondPacket()).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Log.Error("Move()", e);
            }
        }

        private void ProcessMovement(IMovableEntity entity)
        {
            if (!(entity is IAiEntity ai))
            {
                return;
            }

            if (ai.Waypoints == null || ai.Waypoints.Length <= 0)
            {
                return;
            }

            byte speedIndex = (byte)(ai.Speed / 2 < 1 ? 1 : ai.Speed / 2);
            int maxindex = ai.Waypoints.Length > speedIndex ? speedIndex : ai.Waypoints.Length;
            Position<short> newPos = ai.Waypoints[maxindex - 1];

            if (!ai.CanMove(newPos))
            {
                return;
            }

            ai.Position = ai.Waypoints[maxindex - 1];
            ai.Waypoints = ai.Waypoints.Skip(maxindex).ToArray();
            Move(entity);
        }
    }
}