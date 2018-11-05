using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Chat.Args
{
    public class PlayerChatEventArg : ChickenEventArgs
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}