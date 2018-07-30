using ChickenAPI.Enums.Packets;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("eff")]
    public class EffectPacket : PacketBase
    {
        [PacketIndex(0)]
        public EffectType EffectType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public int EffectId { get; set; }

    }
}