using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class GetShopInformationEventArgs : SystemEventArgs
    {
        public Shop Shop { get; set; }

        public byte Type { get; set; }
    }
}