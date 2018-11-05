using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class BasicNpcDialogHandler : INpcDialogHandler
    {
        private static readonly Logger Log = Logger.GetLogger<BasicNpcDialogHandler>();
        protected readonly Dictionary<long, NpcDialogHandler> HandlersByDialogId;

        public BasicNpcDialogHandler()
        {
            HandlersByDialogId = new Dictionary<long, NpcDialogHandler>();
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicNpcDialogHandler));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<NpcDialogHandlerAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<NpcDialogHandlerAttribute>() != null))
                {
                    Register(new NpcDialogHandler(method));
                }
            }
        }

        public void Register(NpcDialogHandler handlerAttribute)
        {
            if (HandlersByDialogId.ContainsKey(handlerAttribute.DialogId))
            {
                return;
            }

            Log.Info($"[REGISTER_HANDLER] NPC_DIALOG_ID : {handlerAttribute.DialogId} REGISTERED !");
            HandlersByDialogId.Add(handlerAttribute.DialogId, handlerAttribute);
        }

        public void Unregister(long npcDialogId)
        {
            HandlersByDialogId.Remove(npcDialogId);
        }

        public void Unregister(NpcDialogHandlerAttribute handlerAttribute)
        {
            Unregister(handlerAttribute.NpcDialogId);
        }

        public void Execute(IPlayerEntity player, NpcDialogEventArgs eventArgs)
        {
            if (!HandlersByDialogId.TryGetValue(eventArgs.DialogId, out NpcDialogHandler handler))
            {
                return;
            }

            handler.Handle(player, eventArgs);
        }
    }
}