using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class ProcessHitRequestEvent : ChickenEventArgs
    {
      public HitRequest HitRequest { get; set; }  
    }
}