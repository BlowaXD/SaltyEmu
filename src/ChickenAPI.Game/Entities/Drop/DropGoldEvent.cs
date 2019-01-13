using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Drop
{
    public class DropGoldEvent : GameEntityEvent
    {
        public long GoldAmount { get; set; }
    }
}