using ChickenAPI.Game.Specialists;
using ChickenAPI.Packets.Game.Client.Specialists;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class SpPacketExtension
    {
        public const int MAX_SP_POINTS = 1000000;
        public const int MAX_SP_ADDITIONAL_POINTS = 10000000;

        public static SpPacket GenerateSpPacket(this IPlayerEntity player) => new SpPacket
        {
            AdditionalPoints = player.Character.SpAdditionPoint,
            MaxAdditionalPoints = MAX_SP_ADDITIONAL_POINTS,
            Points = player.Character.SpPoint,
            MaxPoints = MAX_SP_POINTS
        };
    }
}