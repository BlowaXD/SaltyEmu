using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Quicklist.Events
{
    public class QuicklistSwapElementEvent : GameEntityEvent
    {
        public short Q1 { get; set; }

        public short Q2 { get; set; }

        public short Data1 { get; set; }
        public short Data2 { get; set; }
    }
}