using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Inventory
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