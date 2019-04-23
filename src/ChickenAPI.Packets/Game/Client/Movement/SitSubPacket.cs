using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Movement
{
    [PacketHeader("sit_sub_packet")] // header will be ignored for serializing just sub list packets
    public class SitSubPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte UserType { get; set; }

        [PacketIndex(1)]
        public long UserId { get; set; }
    }
}