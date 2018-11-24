using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class GetPacketExtension
    {
        public static GetPacket GenerateGetPacket(this IPlayerEntity player, long itemId) => new GetPacket
        {
            CharacterId = player.Id,
            ItemId = itemId
        };
    }
}