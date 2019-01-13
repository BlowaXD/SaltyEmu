using ChickenAPI.Data.Map;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Portals.Args
{
    public class PortalTriggerEvent : GameEntityEvent
    {
        public PortalDto Portal { get; set; }
    }
}