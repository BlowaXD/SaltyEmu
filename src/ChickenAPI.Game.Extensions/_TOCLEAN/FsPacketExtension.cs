using System.Threading.Tasks;
using ChickenAPI.Packets.Game.Client.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class FsPacketExtension
    {
        public static ScrPacket GenerateScrPacket(this IPlayerEntity player)
        {
            return new ScrPacket { Unknow1 = 0, Unknow2 = 0, Unknow3 = 0, Unknow4 = 0, Unknow5 = 0, Unknow6 = 0 };
        }

        public static Task ActualizeUiFaction(this IPlayerEntity player)
        {
            return player.SendPacketAsync(player.GenerateFsPacket());
        }

        public static FsPacket GenerateFsPacket(this IPlayerEntity player) => new FsPacket
        {
            Faction = player.Character.Faction
        };
    }
}