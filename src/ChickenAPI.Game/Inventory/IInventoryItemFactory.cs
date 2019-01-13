using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities;

namespace ChickenAPI.Game.Inventory
{
    public interface IInventoryItemFactory
    {
        Task<InventoryItem> GetInventoryItemAsync(ItemInstanceDto dto, IInventoriedEntity owner);
    }
}