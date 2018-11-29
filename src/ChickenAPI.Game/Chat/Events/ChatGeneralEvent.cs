using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Chat.Events
{
    public class ChatGeneralEvent : ChickenEventArgs
    {
        public string Message { get; set; }
    }
}