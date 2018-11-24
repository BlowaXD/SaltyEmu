using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("req_info")]
    public class ReqInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public ReqInfoType ReqType { get; set; }

        [PacketIndex(1)]
        public long TargetVNum { get; set; }

        [PacketIndex(2)]
        public int? MateVNum { get; set; }
    }
}