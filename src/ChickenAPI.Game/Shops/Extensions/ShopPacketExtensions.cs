using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Shop;
using System.Threading.Tasks;

namespace ChickenAPI.Game.Shops.Extensions
{
    public static class ShopPacketExtensions
    {
        public static ShopEndPacket GenerateShopEndPacket(this IPlayerEntity player, ShopEndPacketType type) =>
            new ShopEndPacket
            {
                Type = type
            };

        public static SMemoPacket GenerateShopMemoPacket(this IPlayerEntity player, SMemoPacketType type, string message) =>
            new SMemoPacket
            {
                SMemoPacketType = type,
                Message = message
            };

        public static PFlagPacket GeneratePFlagPacket(this IPlayerEntity player) =>
            new PFlagPacket
            {
                EntityType = player.Type,
                EntityId = player.Id,
                ShopId = player.Shop.Id
            };

        public static ShopPacket GenerateShopPacket(this IPlayerEntity player)
        {
            if (!player.HasShop)
            {
                return null;
            }

            return new ShopPacket
            {
                VisualType = player.Type,
                EntityId = player.Id,
                ShopId = 1,
                MenuType = 3,
                ShopType = 0,
                Name = player.Shop.ShopName
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
        public static ShopPacket GenerateEmptyShopPacket(this IPlayerEntity player)
        {
            return new ShopPacket
            {
                VisualType = player.Type,
                EntityId = player.Id,
                ShopId = 0,
                MenuType = 0,
                ShopType = 0,
            };
        }

        public static async Task ClosePersonalShopAsync(this IPlayerEntity player, bool closeShopWindow = false)
        {
            await player.BroadcastAsync(player.GenerateEmptyShopPacket());
            if (closeShopWindow)
            {
                await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.PersonalShop));
            }
        }
    }
}