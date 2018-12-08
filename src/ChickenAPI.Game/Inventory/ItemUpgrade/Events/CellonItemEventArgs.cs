using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class CellonItemEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto Jewelry { get; set; }
        public ItemInstanceDto Cellon { get; set; }
        public int GoldAmount { get; set; }
    }
}
