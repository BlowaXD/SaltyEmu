using System;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class NpcDialogAttribute : Attribute
    {
        public NpcDialogAttribute(long dialogId)
        {
            DialogId = dialogId;
        }

        public long DialogId { get; set; }
    }
}