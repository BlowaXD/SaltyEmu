using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;
using ChickenAPI.Game.GuriHandling.Handling;

namespace SaltyEmu.BasicPlugin.EventHandlers.Guri
{
    public class BaseGuriHandler : IGuriHandler
    {
        private static readonly Logger Log = Logger.GetLogger<BaseGuriHandler>();
        protected readonly Dictionary<long, GuriRequestHandler> HandlersByDialogId;

        public BaseGuriHandler()
        {
            HandlersByDialogId = new Dictionary<long, GuriRequestHandler>();
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicPlugin));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<GuriEffectAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<GuriEffectAttribute>() != null))
                {
                    Register(new GuriRequestHandler(method));
                }
            }
        }

        public void Register(GuriRequestHandler handler)
        {
            if (HandlersByDialogId.ContainsKey(handler.GuriEffectId))
            {
                return;
            }

            Log.Info($"[REGISTER_HANDLER] GURI_EFFECT : {handler.GuriEffectId} REGISTERED !");
            HandlersByDialogId.Add(handler.GuriEffectId, handler);
        }

        public void Unregister(long guriEffectId)
        {
            HandlersByDialogId.Remove(guriEffectId);
        }

        public void Handle(IPlayerEntity player, GuriEvent args)
        {
            if (player == null)
            {
                return;
            }

            if (!HandlersByDialogId.TryGetValue(args.EffectId, out GuriRequestHandler handler))
            {
                return;
            }

            handler.Handle(player, args);
        }
    }
}