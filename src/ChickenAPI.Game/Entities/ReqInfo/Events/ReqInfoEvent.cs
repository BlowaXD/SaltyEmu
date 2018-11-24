using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.ReqInfo.Events
{
    public class ReqInfoEvent : ChickenEventArgs
    {
        public ReqInfoType ReqType { get; set; }
        public long TargetVNum { get; set; }
        public int? MateVNum { get; set; }
    }
}