using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StatPacketExtensions
    {
        public static StatPacket GenerateStatPacket(this IPlayerEntity player) =>
            new StatPacket
            {
                Hp = player.Hp,
                HpMaximum = player.HpMax,
                Mp = player.Mp,
                MpMaximum = player.MpMax,
                Unknown = 0,
                Option = 0,
            };
    }
}