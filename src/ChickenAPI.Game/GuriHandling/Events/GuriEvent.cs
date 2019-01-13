using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.GuriHandling.Args
{
    public class GuriEvent : GameEntityEvent
    {
        public long EffectId { get; set; }

        public short Data { get; set; }

        public short InvSlot { get; set; }
    }
}