using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Packets.Game.Client.Shops;

namespace ChickenAPI.Game.Shops.Extensions
{
    public static class ShopPacketExtensions
    {
        public static ShopPacket GenerateShopPacket(this INpcEntity npc)
        {
            if (npc.Shop == null)
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