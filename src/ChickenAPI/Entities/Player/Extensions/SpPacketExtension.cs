using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Game.Features.Specialists.Packets;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class SpPacketExtension
    {
        public static SpPacket GenerateSpPacket(this IPlayerEntity player)
        {
            return new SpPacket
            {
                AdditionalPoints = player.Character.SpAdditionPoint,
                MaxAdditionalPoints = SpecialistComponent.MAX_SP_ADDITIONAL_POINTS,
                Points = player.Character.SpPoint,
                MaxPoints = SpecialistComponent.MAX_SP_POINTS,
            };
        }
    }
}