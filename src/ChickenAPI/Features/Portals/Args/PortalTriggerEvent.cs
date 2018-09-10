using ChickenAPI.Core.Events;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Features.Portals.Args
{
    public class PortalTriggerEvent : ChickenEventArgs
    {
        public PortalDto Portal { get; set; }
    }
}