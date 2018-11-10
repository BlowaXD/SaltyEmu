using System;
using System.Collections.Generic;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Visibility.Events;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityEventHandler : EventHandlerBase
    {
        private readonly Logger _log = Logger.GetLogger<VisibilityEventHandler>();

        public override ISet<Type> HandledTypes => new HashSet<Type>();

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            if (!(entity is IVisibleCapacity visible))
            {
                return;
            }

            switch (e)
            {
                case VisibilitySetInvisibleEventArgs invisibleEvent:
                    visible.Visibility = VisibilityType.Invisible;
                    SetInvisible(entity, invisibleEvent);
                    break;

                case VisibilitySetVisibleEventArgs visibleEvent:
                    visible.Visibility = VisibilityType.Visible;
                    SetVisible(entity, visibleEvent);
                    break;
            }
        }

        /// <summary>
        /// To review
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        private static void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            if (!args.Broadcast)
            {
                return;
            }

            if (entity is IPlayerEntity player)
            {
                player.BroadcastExceptSender(player.GenerateInPacket());
            }
            else
            {
                entity.CurrentMap.Broadcast(entity.GenerateInPacket());
            }
        }

        private static void SetInvisible(IEntity entity, VisibilitySetInvisibleEventArgs args)
        {
            if (!args.Broadcast)
            {
                return;
            }

            if (entity is IPlayerEntity player)
            {
                player.BroadcastExceptSender(player.GenerateOutPacket());
            }
        }
    }
}