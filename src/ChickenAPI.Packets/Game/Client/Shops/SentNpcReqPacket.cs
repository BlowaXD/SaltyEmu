using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Shops
{
    [PacketHeader("npc_req")]
    public class SentNpcReqPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2, IsOptional = true)]
        public long? Dialog { get; set; }
    }
}