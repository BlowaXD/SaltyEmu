using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.GuriHandling.Args
{
    public class GuriEventArgs : ChickenEventArgs
    {
        public long EffectId { get; set; }

        public short Data { get; set; }
    }
}