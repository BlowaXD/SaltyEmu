using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class FsPacketExtension
    {
        public static FsPacket GenerateFsPacket(this IPlayerEntity player) => new FsPacket
        {
            Faction = player.Character.Faction
        };
    }
}