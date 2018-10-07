using ChickenAPI.Data;
using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Data.AccessLayer.Item
{
    public interface IItemService : IMappedRepository<ItemDto>
    {
    }
}