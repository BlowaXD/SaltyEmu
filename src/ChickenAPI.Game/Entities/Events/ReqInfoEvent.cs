using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Entities.Events
{
    public class ReqInfoEvent : GameEntityEvent
    {
        public ReqInfoType ReqType { get; set; }
        public long TargetVNum { get; set; }
        public int? MateVNum { get; set; }
    }
}