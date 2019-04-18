using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class CModePacketExtension
    {
        public static CModePacket GenerateCModePacket(this IPlayerEntity player) => new CModePacket
        {
            VisualType = VisualType.Player,
            VisualId = player.Id,
            Morph = player.MorphId,
            MorphUpgrade = player.Sp?.Upgrade ?? 0,
            MorphDesign = player.Sp?.Design ?? 0,
            MorphBonus = (byte)(player.Character.ArenaWinner ? 1 : 0)
        };
    }
}