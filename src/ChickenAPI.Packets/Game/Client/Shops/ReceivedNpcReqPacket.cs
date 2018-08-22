using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Shops.Packets
{
    [PacketHeader("npc_req")]
    public class ReceivedNpcReqPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }
    }
}