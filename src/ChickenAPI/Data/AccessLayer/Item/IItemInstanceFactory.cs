using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Data.AccessLayer.Item
{
    public interface IItemInstanceFactory
    {
        ItemInstanceDto CreateItem(ItemDto item, short quantity);
        ItemInstanceDto CreateItem(long itemId, short quantity);
    }
}