using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Game.Systems.Chat
{
    public class PlayerChatEventArg : SystemEventArgs
    {
        public IPlayerEntity Sender { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}