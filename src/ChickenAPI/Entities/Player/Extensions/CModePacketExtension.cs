using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Entities.Player
{
    public static class CModePacketExtension
    {
        public static CModePacketBase GenerateCModePacket(this IPlayerEntity player)
        {
            return new CModePacketBase
            {
                VisualType = VisualType.Character,
                CharacterId = player.Character.Id,
                Morph = 0,
                SpUpgrade = player.Sp?.Upgrade ?? 0,
                SpDesign = player.Sp?.Design ?? 0,
                ArenaWinner = player.Character.ArenaWinner,
            };
        }
    }
}