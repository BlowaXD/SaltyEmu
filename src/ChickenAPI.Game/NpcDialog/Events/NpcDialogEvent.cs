using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.NpcDialog.Events
{
    public class NpcDialogEvent : GameEntityEvent
    {
        public long DialogId { get; set; }

        public long Type { get; set; }

        public long Value { get; set; }

        public long NpcId { get; set; }
    }
}