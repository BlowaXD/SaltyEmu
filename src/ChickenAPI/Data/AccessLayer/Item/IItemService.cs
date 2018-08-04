using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Data.AccessLayer.Item
{
    public interface IItemService : IMappedRepository<ItemDto>
    {
    }
}