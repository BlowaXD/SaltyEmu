using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Maps.Events
{
    public class MapJoinEvent : GameEntityEvent
    {
        public IMapLayer Map { get; set; }
    }
}