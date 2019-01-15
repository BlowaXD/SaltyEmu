using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.NpcDialog.Handling;

namespace SaltyEmu.BasicPlugin.NpcDialogHandlers
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

        public void Execute(IPlayerEntity player, NpcDialogEvent @event)
        {
            if (!HandlersByDialogId.TryGetValue(@event.DialogId, out NpcDialogHandler handler))
            {
                return;
            }

            handler.Handle(player, @event);
        }
    }
}