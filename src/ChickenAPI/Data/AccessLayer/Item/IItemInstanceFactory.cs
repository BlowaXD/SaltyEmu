using ChickenAPI.Data.TransferObjects.Item;

namespace ChickenAPI.Data.AccessLayer.Item
{
    public interface IItemInstanceFactory
    {
        ItemInstanceDto CreateItem(ItemDto item, short quantity);
        ItemInstanceDto CreateItem(long itemId, short quantity);
    }
}