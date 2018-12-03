using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Shop;

namespace ChickenAPI.Game.Shops.Extensions
{
    public static class ShopPacketExtensions
    {
        public static SMemoPacket GenerateShopMemoPacket(this IPlayerEntity player, SMemoPacketType type, string message)
        {
            return new SMemoPacket
            {
                SMemoPacketType = type,
                Message = message
            };
        }

        public static ShopPacket GenerateShopPacket(this INpcEntity npc)
        {
            if (!npc.HasShop)
            {
                return null;
            }

            return new ShopPacket
            {
                VisualType = npc.Type,
                EntityId = npc.Id,
                ShopId = npc.Shop.Id,
                MenuType = npc.Shop.MenuType,
                ShopType = npc.Shop.ShopType,
                Name = npc.Shop.Name
            };
        }
    }
}