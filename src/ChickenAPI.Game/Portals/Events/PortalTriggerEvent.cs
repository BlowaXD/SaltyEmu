using ChickenAPI.Data.Map;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Portals.Events
{
    public class PortalTriggerEvent : GameEntityEvent
    {
        public PortalDto Portal { get; set; }
    }
}