using System;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NpcDialogHandlerAttribute : Attribute
    {
        public NpcDialogHandlerAttribute(long npcDialogId)
        {
            NpcDialogId = npcDialogId;
        }

        public long NpcDialogId { get; }
    }
}