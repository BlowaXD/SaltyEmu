using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("info")]
    public class InfoPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public string Message { get; set; }
    }
}