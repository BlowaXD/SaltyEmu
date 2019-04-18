using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Families;

namespace ChickenAPI.Game.Families.Extensions
{
    public static class GidxPacketExtensions
    {
        public static GidxPacket GenerateGidxPacket(this IPlayerEntity player)
        {
            if (!player.HasFamily)
            {
                return null;
            }

            return new GidxPacket
            {
                VisualType = player.Type,
                VisualId = player.Id,
                FamilyId = player.Family.Id,
                FamilyName = player.Family.Name,
                FamilyLevel = player.Family.FamilyLevel,
                FamilyCustomRank = "(i18nRANKTODO)"
            };
        }
    }
}