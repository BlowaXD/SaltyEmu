using System;
using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Chat.Args;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Chat
{
    public class ChatEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(PlayerChatEventArg)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case PlayerChatEventArg playerChatEvent:
                    PlayerChat(entity, playerChatEvent);
                    break;
            }
        }

        private static void PlayerChat(IEntity entity, PlayerChatEventArg args)
        {
            var sayPacket = new SayPacket
            {
                Type = SayColorType.White,
                Message = args.Message,
                VisualType = VisualType.Character,
                VisualId = args.SenderId
            };
            ((IPlayerEntity)entity).BroadcastExceptSender(sayPacket);
        }
    }
}