using System.Collections.Generic;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Movements.Events
{
    public class TriggerSitEvent : GameEntityEvent
    {
        public IEnumerable<long> ChildsId { get; set; }
    }
}