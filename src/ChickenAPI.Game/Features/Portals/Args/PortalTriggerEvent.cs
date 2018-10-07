using ChickenAPI.Data.Map;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Portals.Args
{
    public class PortalTriggerEvent : ChickenEventArgs
    {
        public PortalDto Portal { get; set; }
    }
}