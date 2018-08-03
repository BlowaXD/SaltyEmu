using ChickenAPI.Data.TransferObjects.Shop;
using ChickenAPI.ECS.Systems;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class GetShopInformationEventArgs : SystemEventArgs
    {
        public Shop Shop { get; set; }
    }
}