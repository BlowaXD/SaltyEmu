using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Movement.Args
{
    public class PlayerMovementRequestEvent : ChickenEventArgs
    {
        public short X { get; set; }
        public short Y { get; set; }
        public byte Speed { get; set; }
    }
}