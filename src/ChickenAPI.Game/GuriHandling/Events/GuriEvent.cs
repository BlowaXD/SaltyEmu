using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.GuriHandling.Events
{
    public class GuriEvent : GameEntityEvent
    {
        public long EffectId { get; set; }

        public short Data { get; set; }

        public short InvSlot { get; set; }
    }
}