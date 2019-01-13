using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers
{
    public class BaseUseItemHandler : IItemUsageContainer
    {
        private static readonly Logger Log = Logger.GetLogger<BaseUseItemHandler>();

        protected readonly Dictionary<(long, ItemType), UseItemRequestHandler> Handlers = new Dictionary<(long, ItemType), UseItemRequestHandler>();

        public BaseUseItemHandler()
        {
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicPlugin));
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
            if (Handlers.ContainsKey((handler.Effect, handler.IType)))
            {
                return;
            }

            Handlers.Add((handler.Effect, handler.IType), handler);
            Log.Info($"[REGISTER_HANDLER] UI_EFFECT : {handler.Effect} && ITYPE : {handler.IType} REGISTERED !");
        }

        public void UseItem(IPlayerEntity player, InventoryUseItemEvent args)
        {
            if (player == null)
            {
                return;
            }

            if (!Handlers.TryGetValue((args.Item.Item.Effect, args.Item.Item.ItemType), out UseItemRequestHandler handler))
            {
                return;
            }

            handler.Handle(player, args);
        }
    }
}