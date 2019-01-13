using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Chat.Events
{
    public class ChatGeneralEvent : GameEntityEvent
    {
        public string Message { get; set; }
    }
}