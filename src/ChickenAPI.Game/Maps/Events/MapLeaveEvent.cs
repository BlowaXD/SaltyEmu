using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Maps.Events
{
    public class MapLeaveEvent : GameEntityEvent
    {
        public IMapLayer Map { get; set; }
    }
}