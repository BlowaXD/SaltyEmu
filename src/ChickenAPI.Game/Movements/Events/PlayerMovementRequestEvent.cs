using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Movements.Events
{
    public class PlayerMovementRequestEvent : ChickenEventArgs
    {
        public short X { get; set; }
        public short Y { get; set; }
        public byte Speed { get; set; }
    }
}