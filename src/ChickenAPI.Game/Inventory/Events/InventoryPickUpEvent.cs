using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryPickUpEvent : GameEntityEvent
    {
        public IDropEntity Drop { get; set; }
    }
}