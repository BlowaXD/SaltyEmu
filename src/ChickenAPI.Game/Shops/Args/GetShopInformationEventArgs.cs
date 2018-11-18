using ChickenAPI.Game.Events;
using ChickenAPI.Game.Shops;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class GetShopInformationEventArgs : ChickenEventArgs
    {
        public Shop Shop { get; set; }

        public byte Type { get; set; }
    }
}