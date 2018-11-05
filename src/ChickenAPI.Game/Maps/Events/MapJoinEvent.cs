using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Maps.Events
{
    public class MapJoinEvent : ChickenEventArgs
    {
        public IMapLayer Map { get; set; }
    }
}