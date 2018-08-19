using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Player.Extensions
{
    public static class FsPacketExtension
    {
        public static FsPacket GenerateFsPacket(this IPlayerEntity player)
        {
            return new FsPacket
            {
                Faction = player.Character.Faction
            };
        }
    }
}