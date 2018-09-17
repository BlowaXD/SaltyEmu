using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Movement.Args;
using ChickenAPI.Game.Features.Movement.Extensions;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.Movement
{
    public class MovementEventHandler : EventHandlerBase
    {
        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case TriggerSitEvent triggerSit:
                    TriggerSit(entity, triggerSit);
                    break;
                case PlayerMovementRequestEvent playerMovementRequest:
                    TreatPlayerMovementRequest(entity as IPlayerEntity, playerMovementRequest);
                    break;
            }
        }

        private static bool PreMovementChecks(IPlayerEntity player, PlayerMovementRequestEvent e)
        {
            // check for player' diseases
            if (player.Character.Hp == 0)
            {
                return false;
            }

            if (!player.Movable.CanMove(e.X, e.Y))
            {
                return false;
            }

            if (player.Movable.Actual.X == e.X || player.Movable.Actual.Y == e.Y)
            {
                return false;
            }

            if (player.Movable.Speed < e.Speed)
            {
                return false;
            }

            return true;
        }

        private void TreatPlayerMovementRequest(IPlayerEntity player, PlayerMovementRequestEvent e)
        {
            if (!player.HasPermission(PermissionType.MOVEMENT_MOVE_SELF))
            {
                return;
            }

            if (!PreMovementChecks(player, e))
            {
                return;
            }

            player.Movable.Actual.X = e.X;
            player.Movable.Actual.Y = e.Y;

            player.SendPacket(player.GenerateCondPacket());
            if (player.EntityManager is IBroadcastable broadcastable)
            {
                broadcastable.Broadcast(player.GenerateMvPacket());
            }
        }

        private void TriggerSit(IEntity entity, TriggerSitEvent triggerSit)
        {
            switch (entity)
            {
                case IPlayerEntity player:
                    if (!player.HasPermission(PermissionType.MOVEMENT_MOVE_SELF))
                    {
                        return;
                    }

                    player.Movable.IsSitting = !player.Movable.IsSitting;

                    // pets should sit
                    break;
                case IMonsterEntity monster:
                    monster.Movable.IsSitting = !monster.Movable.IsSitting;
                    break;
                case INpcEntity npc:
                    npc.Movable.IsSitting = !npc.Movable.IsSitting;
                    break;
            }
        }
    }
}