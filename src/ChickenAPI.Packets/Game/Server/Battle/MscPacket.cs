using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Battle
{
    [PacketHeader("ms_c")]
    public class MscPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Unknown { get; set; } = 0;
    }
}