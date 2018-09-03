using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.NpcDialog.Events
{
    public class NpcDialogEventArgs : ChickenEventArgs
    {
        public long DialogId { get; set; }
    }
}