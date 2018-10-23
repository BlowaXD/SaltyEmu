using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    /// <summary>
    /// Fills the hit request in the event pipeline
    /// Buffs
    /// Damages
    /// HitType...
    /// </summary>
    public class FillHitRequestEvent : ChickenEventArgs
    {
        public HitRequest HitRequest { get; set; }
    }
}