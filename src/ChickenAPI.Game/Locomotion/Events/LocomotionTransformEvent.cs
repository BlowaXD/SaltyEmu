using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Locomotion.Events
{
    public class LocomotionTransformEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }
    }
}