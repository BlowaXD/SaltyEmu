using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Packets.Game.Client.Specialists;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class SpPacketExtension
    {
        public static SpPacket GenerateSpPacket(this IPlayerEntity player) => new SpPacket
        {
            AdditionalPoints = player.Character.SpAdditionPoint,
            MaxAdditionalPoints = SpecialistComponent.MAX_SP_ADDITIONAL_POINTS,
            Points = player.Character.SpPoint,
            MaxPoints = SpecialistComponent.MAX_SP_POINTS
        };
    }
}