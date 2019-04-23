using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
{
    [PacketHeader("info")]
    public class InfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public string Message { get; set; }
    }
}