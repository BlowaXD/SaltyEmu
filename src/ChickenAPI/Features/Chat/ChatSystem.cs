using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Chat.Args;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Chat
{
    public class ChatEventHandler : EventHandlerBase
    {
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

            if (entity.EntityManager is IBroadcastable broadcastable)
            {
                broadcastable.Broadcast((IPlayerEntity)entity, sayPacket);
            }
        }
    }
}