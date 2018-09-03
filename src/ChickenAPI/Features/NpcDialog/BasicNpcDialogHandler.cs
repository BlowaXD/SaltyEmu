using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class BasicNpcDialogHandler : INpcDialogHandler
    {
        protected Dictionary<long, NpcDialogHandlerAttribute> HandlersByDialogId = new Dictionary<long, NpcDialogHandlerAttribute>();

        public BasicNpcDialogHandler()
        {
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicNpcDialogHandler));
            foreach (Type handler in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => CustomAttributeExtensions.GetCustomAttribute<NpcDialogHandlerAttribute>((MemberInfo)m) != null)))
            {
                foreach (MethodInfo method in handler.GetMethods())
                {
                    Register(method.GetCustomAttribute<NpcDialogHandlerAttribute>());
                }
            }
        }

        public void Register(NpcDialogHandlerAttribute handlerAttribute)
        {
            if (HandlersByDialogId.ContainsKey(handlerAttribute.NpcDialogId))
            {
                return;
            }

            HandlersByDialogId.Add(handlerAttribute.NpcDialogId, handlerAttribute);
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
            if (!HandlersByDialogId.TryGetValue(eventArgs.DialogId, out NpcDialogHandlerAttribute handler))
            {
                return;
            }

            handler.Handle(player, eventArgs);
        }
    }
}