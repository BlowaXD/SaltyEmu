using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Shops.Packets
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