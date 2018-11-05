using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Maps.Events
{
    public class MapLeaveEvent : ChickenEventArgs
    {
        public IMapLayer Map { get; set; }
    }
}