using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class CModePacketExtension
    {
        public static CModePacketBase GenerateCModePacket(this IPlayerEntity player) => new CModePacketBase
        {
            VisualType = VisualType.Character,
            CharacterId = player.Character.Id,
            Morph = 0,
            SpUpgrade = player.Sp?.Upgrade ?? 0,
            SpDesign = player.Sp?.Design ?? 0,
            ArenaWinner = player.Character.ArenaWinner
        };
    }
}