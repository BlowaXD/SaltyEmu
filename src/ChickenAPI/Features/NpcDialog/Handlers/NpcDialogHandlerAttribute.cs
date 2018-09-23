using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

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