using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StatPacketExtensions
    {
        public static StatPacket GenerateStatPacket(this IPlayerEntity player)
        {
            BattleComponent battle = player.Battle;
            return new StatPacket
            {
                Hp = battle.Hp,
                HpMax = battle.HpMax,
                Mp = battle.Mp,
                MpMax = battle.MpMax,
                Unknown = 0,
                CharacterOption = 0
            };
        }
    }
}