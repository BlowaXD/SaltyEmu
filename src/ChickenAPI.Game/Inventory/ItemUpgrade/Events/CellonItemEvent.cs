using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class CellonItemEvent : GameEntityEvent
    {
        public ItemInstanceDto Jewelry { get; set; }
        public ItemInstanceDto Cellon { get; set; }
        public int GoldAmount { get; set; }
    }
}