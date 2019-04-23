using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Inventory
{
    [PacketHeader("wear")]
    public class WearPacket : PacketBase
    {
        [PacketIndex(0)]
        public short ItemSlot { get; set; }

        [PacketIndex(1)]
        public byte WearPacketType { get; set; }
    }
}