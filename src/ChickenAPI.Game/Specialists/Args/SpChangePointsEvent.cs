using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Specialists.Args
{
    public class SpChangePointsEvent : GameEntityEvent
    {
        public short Attack { get; set; }
        public short Defense { get; set; }
        public short Element { get; set; }
        public short HpMp { get; set; }
    }
}