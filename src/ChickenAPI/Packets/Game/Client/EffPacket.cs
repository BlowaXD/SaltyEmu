using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Packets.Game.Client
{
    [PacketHeader("eff")]
    public class EffectPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType EffectType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public long EffectId { get; set; }
    }
}