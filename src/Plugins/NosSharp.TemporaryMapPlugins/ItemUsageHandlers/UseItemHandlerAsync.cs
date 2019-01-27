using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers
{
    public class UseItemHandlerContainer : IItemUsageContainerAsync
    {
        private readonly Logger _log = Logger.GetLogger<UseItemHandlerContainer>();

        private readonly Dictionary<(long, ItemType), IUseItemRequestHandlerAsync> _handlers = new Dictionary<(long, ItemType), IUseItemRequestHandlerAsync>();

        public Task RegisterItemUsageCallback(IUseItemRequestHandlerAsync handler)
        {
            _handlers.Add((handler.EffectId, handler.Type), handler);
            _log.Info($"[REGISTER_HANDLER] UI_EFFECT : {handler.EffectId} && ITYPE : {handler.Type} REGISTERED !");
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(IUseItemRequestHandlerAsync handler)
        {
            if (!_handlers.ContainsKey((handler.EffectId, handler.Type)))
            {
                return Task.CompletedTask;
            }

            _handlers.Remove((handler.EffectId, handler.Type));
            return Task.CompletedTask;
        }

        public Task UseItem(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (player == null)
            {
                return Task.CompletedTask;
            }

            if (!_handlers.TryGetValue((e.Item.Item.Effect, e.Item.Item.ItemType), out IUseItemRequestHandlerAsync handler))
            {
                return Task.CompletedTask;
            }

            return handler.Handle(player, e);
        }
    }
}