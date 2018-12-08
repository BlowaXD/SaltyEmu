using System.Collections.Generic;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities;

namespace ChickenAPI.Game.Inventory
{
    public class InventoryItem : ItemInstanceDto
    {
        public IInventoriedEntity Owner { get; }

        public IEnumerable<BCardDto> BCards { get; }
    }
}