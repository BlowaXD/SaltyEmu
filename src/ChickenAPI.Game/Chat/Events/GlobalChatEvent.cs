using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Chat.Events
{
    public class GlobalChatEvent : GameEntityEvent
    {
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ItemInstanceDto LinkedItem { get; set; }

        public bool HasItemLinked => LinkedItem != null;
    }
}