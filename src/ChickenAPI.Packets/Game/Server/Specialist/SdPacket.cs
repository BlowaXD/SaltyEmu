using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Specialist
{
    [PacketHeader("sd")]
    public class SdPacket : PacketBase
    {
        [PacketIndex(0)]
        public int CoolDown { get; set; }
    }
}