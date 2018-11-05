using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class GetShopInformationEventArgs : ChickenEventArgs
    {
        public Shop Shop { get; set; }

        public byte Type { get; set; }
    }
}