using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("cond")]
    public class CondPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public bool CanAttack { get; set; }

        [PacketIndex(3)]
        public bool CanMove { get; set; }

        [PacketIndex(4)]
        public byte Speed { get; set; }
    }
}