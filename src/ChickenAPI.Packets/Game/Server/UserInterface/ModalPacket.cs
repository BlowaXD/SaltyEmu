using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
{
    [PacketHeader("modal")]
    public class ModalPacket : PacketBase
    {
        [PacketIndex(0)]
        public ModalPacketType Type { get; set; } //  TODO : Create An enum

        [PacketIndex(1)]
        public string Message { get; set; }
    }
}