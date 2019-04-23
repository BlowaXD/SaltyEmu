using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
{
    [PacketHeader("dlg")]
    public class DialogPacket : PacketBase
    {
        [PacketIndex(0, IsReturnPacket = true)]
        public PacketBase AcceptPacket { get; set; }

        [PacketIndex(1, IsReturnPacket = true)]
        public PacketBase RefusePacket { get; set; }

        [PacketIndex(2, SerializeToEnd = true)]
        public string Question { get; set; }
    }
}