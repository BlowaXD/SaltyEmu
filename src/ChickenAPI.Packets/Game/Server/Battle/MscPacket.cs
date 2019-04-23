using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Battle
{
    [PacketHeader("ms_c")]
    public class MscPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Unknown { get; set; } = 0;
    }
}