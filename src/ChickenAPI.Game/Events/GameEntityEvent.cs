using ChickenAPI.Core.Events;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class GameEntityEvent : ChickenEvent
    {
        public IEntity Sender { get; set; }
    }
}