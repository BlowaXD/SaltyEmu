using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class SummingEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }

        public ItemInstanceDto SecondItem { get; set; }
    }
}