using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Chat
{
    public class PlayerChatEventArg : SystemEventArgs
    {
        public IPlayerEntity Sender { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}