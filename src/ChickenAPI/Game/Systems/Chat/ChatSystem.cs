using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Game.Systems.Chat
{
    public class ChatSystem : NotifiableSystemBase
    {
        public ChatSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
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