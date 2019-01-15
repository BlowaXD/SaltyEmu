using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Battle.Events
{
    /// <summary>
    ///     Fills the hit request in the event pipeline
    ///     Buffs
    ///     Damages
    ///     HitType...
    /// </summary>
    public class FillHitRequestEvent : GameEntityEvent
    {
        public HitRequest HitRequest { get; set; }
    }
}