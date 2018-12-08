using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class ItemUpgradeEventArgs : ChickenEventArgs
    {
        public UpgradePacketType Type { get; set; }

        public ItemInstanceDto Item { get; set; }

        public ItemInstanceDto SecondItem { get; set; }

        public ItemInstanceDto CellonItem { get; set; }
    }
}