using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StatPacketExtensions
    {
        public static StatPacket GenerateStatPacket(this IPlayerEntity player)
        {
            return new StatPacket
            {
                Hp = player.Hp,
                HpMax = player.HpMax,
                Mp = player.Mp,
                MpMax = player.MpMax,
                Unknown = 0,
                CharacterOption = 0
            };
        }
    }
}