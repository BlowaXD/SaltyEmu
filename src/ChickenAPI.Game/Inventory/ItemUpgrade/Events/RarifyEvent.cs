using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class RarifyEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }

        public RarifyProtection Protection { get; set; }

        public RarifyMode Mode { get; set; }

        public bool IsCommand { get; set; }
    }
}