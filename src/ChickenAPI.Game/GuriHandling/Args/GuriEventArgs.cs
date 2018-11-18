using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.GuriHandling.Args
{
    public class GuriEventArgs : ChickenEventArgs
    {
        public long EffectId { get; set; }

        public short Data { get; set; }
    }
}