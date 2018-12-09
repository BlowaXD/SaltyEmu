using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryPickUpEvent : ChickenEventArgs
    {
        public IDropEntity Drop { get; set; }
    }
}