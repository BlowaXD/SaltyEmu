using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Specialist
{
    [PacketHeader("sd")]
    public class SdPacket : PacketBase
    {
        [PacketIndex(0)]
        public int CoolDown { get; set; }
    }
}