using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class GoldPacketExtension
    {
        public static GoldPacket GenerateGoldPacket(this IPlayerEntity player) => new GoldPacket
        {
            Gold = player.Character.Gold
        };
    }
}