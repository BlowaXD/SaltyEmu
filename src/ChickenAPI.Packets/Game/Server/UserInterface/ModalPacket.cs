using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("modal")]
    public class ModalPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Type { get; set; } //  TODO : Create An enum

        [PacketIndex(1)]
        public string message { get; set; }
    }
}