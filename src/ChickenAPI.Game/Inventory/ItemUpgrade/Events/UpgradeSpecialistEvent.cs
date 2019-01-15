using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class UpgradeSpecialistEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }

        public UpgradeProtection Protection { get; set; }

        public UpgradeMode Mode { get; set; }

        public FixedUpMode HasAmulet { get; set; }

        public bool IsCommand { get; set; }
    }
}