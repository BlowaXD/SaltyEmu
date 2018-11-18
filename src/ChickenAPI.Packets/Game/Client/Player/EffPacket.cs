using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
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