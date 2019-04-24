using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Entities.Events
{
    public class ReqInfoEvent : GameEntityEvent
    {
        public ReqInfoType ReqType { get; set; }
        public long TargetId { get; set; }
        public int? MateTransportId { get; set; }
    }
}