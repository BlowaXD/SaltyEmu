using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.NpcDialog.Events;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers
{
    public class UseItemHandlerContainer : IItemUsageContainerAsync
    {
        private static readonly Logger Log = Logger.GetLogger<UseItemHandlerContainer>();

        private readonly Dictionary<(long, ItemType), UseItemRequestHandler> _handlers;

        public UseItemHandlerContainer()
        {
            _handlers = new Dictionary<(long, ItemType), UseItemRequestHandler>();

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

        public Task RegisterItemUsageCallback(UseItemRequestHandler handler)
        {
            _handlers.Add((handler.Effect, handler.IType), handler);
            Log.Info($"[REGISTER_HANDLER] UI_EFFECT : {handler.Effect} && ITYPE : {handler.IType} REGISTERED !");
            return Task.CompletedTask;
        }

        public Task UseItem(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (player == null)
            {
                return Task.CompletedTask;
            }

            if (!_handlers.TryGetValue((e.Item.Item.Effect, e.Item.Item.ItemType), out UseItemRequestHandler handler))
            {
                return Task.CompletedTask;
            }

            return handler.Handle(player, e);
        }
    }
}