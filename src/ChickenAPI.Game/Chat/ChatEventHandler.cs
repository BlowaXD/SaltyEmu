using System;
using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Chat.Events;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Chat
{
    public class ChatEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ChatGeneralEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ChatGeneralEvent playerChatEvent:
                    PlayerChat(entity, playerChatEvent);
                    break;
            }
        }

        private static void PlayerChat(IEntity entity, ChatGeneralEvent args)
        {
            var sayPacket = new SayPacket
            {
                Type = SayColorType.White,
                Message = args.Message,
                VisualType = entity.Type,
                VisualId = entity.Id
            };
            ((IPlayerEntity)entity).BroadcastExceptSender(sayPacket);
        }
    }
}