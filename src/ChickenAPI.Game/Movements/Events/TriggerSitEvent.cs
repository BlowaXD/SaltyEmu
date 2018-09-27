using System.Collections.Generic;
using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Movement.Args
{
    public class TriggerSitEvent : ChickenEventArgs
    {
        public IEnumerable<long> ChildsId { get; set; }
    }
}