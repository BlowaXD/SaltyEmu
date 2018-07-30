using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Item;

namespace ChickenAPI.Data.AccessLayer.Item
{
    public interface IItemService : IMappedRepository<ItemDto>
    {
    }
}