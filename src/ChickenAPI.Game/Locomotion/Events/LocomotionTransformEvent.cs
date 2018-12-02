using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Locomotion.Events
{
    public class LocomotionTransformEvent : ChickenEventArgs
    {
        public ItemInstanceDto Item { get; set; }
    }
}