using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("stat")]
    public class StatPacket : PacketBase
    {
        public StatPacket(IPlayerEntity player)
        {
            var battle = player.GetComponent<BattleComponent>();

            Hp = battle.Hp;
            HpMax = battle.HpMax;
            Mp = battle.Mp;
            MpMax = battle.MpMax;
            Unknown = 0;
            CharacterOption = 0;
        }

        [PacketIndex(0)]
        public long Hp { get; set; }

        [PacketIndex(1)]
        public long HpMax { get; set; }

        [PacketIndex(2)]
        public long Mp { get; set; }

        [PacketIndex(3)]
        public long MpMax { get; set; }

        [PacketIndex(4)]
        public long Unknown { get; set; } // seems to be 0

        [PacketIndex(5)]
        public double CharacterOption { get; set; }
    }
}