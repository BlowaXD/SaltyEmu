using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("gold")]
    public class GoldPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Gold { get; set; }

        [PacketIndex(1)]
        public byte Unknown { get; set; } = 0;
    }
}