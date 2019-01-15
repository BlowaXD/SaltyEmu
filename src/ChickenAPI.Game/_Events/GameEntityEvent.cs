using ChickenAPI.Core.Events;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game._Events
{
    public abstract class GameEntityEvent : ChickenEvent
    {
        public IEntity Sender { get; set; }
    }
}