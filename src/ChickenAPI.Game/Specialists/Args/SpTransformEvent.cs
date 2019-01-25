using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Specialists.Args
{
    public class SpTransformEvent : GameEntityEvent
    {
        public bool Wait { get; set; }
    }
}