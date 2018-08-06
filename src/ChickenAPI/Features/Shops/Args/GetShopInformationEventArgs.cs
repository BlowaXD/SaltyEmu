using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Packets.Game.Client;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class GetShopInformationEventArgs : SystemEventArgs
    {
        public Shop Shop { get; set; }

        public ShoppingPacket Packet { get; set; }
    }
}