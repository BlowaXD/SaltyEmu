using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Movements.Events
{
    public class PlayerMovementRequestEvent : GameEntityEvent
    {
        public short X { get; set; }
        public short Y { get; set; }
        public byte Speed { get; set; }
    }
}