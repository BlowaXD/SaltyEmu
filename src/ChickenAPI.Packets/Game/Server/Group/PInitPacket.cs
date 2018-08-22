using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Group
{
    [PacketHeader("pinit")]
    public class PInitPacket : PacketBase
    {
        public PInitPacket()
        {
            PartySize = 0;
            PartyString = string.Empty;
        }

        [PacketIndex(0)]
        public long PartySize { get; set; }

        [PacketIndex(1)]
        public string PartyString { get; set; }
    }
}