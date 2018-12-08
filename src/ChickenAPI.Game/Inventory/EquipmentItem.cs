using System.Collections.Generic;
using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Inventory
{
    public class EquipmentItem : InventoryItem
    {
        public List<EquipmentOptionDto> EquipmentOptions { get; }
    }
}