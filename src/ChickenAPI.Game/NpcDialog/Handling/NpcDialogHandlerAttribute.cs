using System;

namespace ChickenAPI.Game.NpcDialog.Handling
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NpcDialogHandlerAttribute : Attribute
    {
        public NpcDialogHandlerAttribute(long npcDialogId) => NpcDialogId = npcDialogId;

        public long NpcDialogId { get; }
    }
}