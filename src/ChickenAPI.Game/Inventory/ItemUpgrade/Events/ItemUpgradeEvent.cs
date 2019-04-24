using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class ItemUpgradeEvent : GameEntityEvent
    {
        public UpgradePacketType Type { get; set; }

        public ItemInstanceDto Item { get; set; }

        public ItemInstanceDto SecondItem { get; set; }

        public ItemInstanceDto CellonItem { get; set; }
    }
}