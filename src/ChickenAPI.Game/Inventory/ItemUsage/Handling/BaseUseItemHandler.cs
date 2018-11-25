using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    public class BaseUseItemHandler : IItemUsageContainer
    {
        private static readonly Logger Log = Logger.GetLogger<BaseUseItemHandler>();
        protected readonly Dictionary<long, UseItemRequestHandler> HandlersByDialogId;

        public BaseUseItemHandler()
        {
            HandlersByDialogId = new Dictionary<long, UseItemRequestHandler>();
            Assembly currentAsm = Assembly.GetAssembly(typeof(BaseUseItemHandler));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<UseItemEffectAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<UseItemEffectAttribute>() != null))
                {
                    RegisterItemUsageCallback(new UseItemRequestHandler(method));
                }
            }
        }

        public void RegisterItemUsageCallback(UseItemRequestHandler handler)
        {
            if (HandlersByDialogId.ContainsKey(handler.Effect))
            {
                return;
            }

            Log.Info($"[REGISTER_HANDLER] UI_EFFECT : {handler.Effect} REGISTERED !");
            HandlersByDialogId.Add(handler.Effect, handler);
        }

        public void UseItem(IPlayerEntity player, InventoryUseItemEvent args)
        {
            if (player == null)
            {
                return;
            }

            if (!HandlersByDialogId.TryGetValue(args.Item.Item.Effect, out UseItemRequestHandler handler))
            {
                return;
            }

            handler.Handle(player, args);
        }
    }
}