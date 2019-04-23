using ChickenAPI.Packets.ClientPackets.Drops;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class GetPacketExtension
    {
        public static GetPacket GenerateGetPacket(this IPlayerEntity player, long itemId) => new GetPacket
        {
            PickerId = player.Id,
            PickerType = player.Type,
            VisualId = itemId
        };
    }
}