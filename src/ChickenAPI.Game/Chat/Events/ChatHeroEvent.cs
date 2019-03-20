using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Chat.Events
{
    public class ChatHeroEvent : GameEntityEvent
    {
        public string Message { get; set; }
    }
}