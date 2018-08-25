using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Chat.Args
{
    public class PlayerChatEventArg : ChickenEventArgs
    {
        public IPlayerEntity Sender { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}