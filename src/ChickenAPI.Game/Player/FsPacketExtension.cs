using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Player
{
    public static class FsPacketExtension
    {
        public static FsPacket GenerateFsPacket(this IPlayerEntity player) => new FsPacket
        {
            Faction = player.Character.Faction
        };
    }
}