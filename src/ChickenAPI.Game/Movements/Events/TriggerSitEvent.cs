using System.Collections.Generic;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Movements.Events
{
    public class TriggerSitEvent : ChickenEventArgs
    {
        public IEnumerable<long> ChildsId { get; set; }
    }
}