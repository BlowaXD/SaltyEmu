using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Entities.Drop
{
    public class DropItemEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}