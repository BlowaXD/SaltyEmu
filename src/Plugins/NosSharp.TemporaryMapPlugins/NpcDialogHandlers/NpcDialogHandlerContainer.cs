using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.NpcDialog.Events;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers
{
    public class NpcDialogHandlerContainer : INpcDialogHandlerContainer
    {
        private readonly Dictionary<long, INpcDialogAsyncHandler> _handlers;

        public NpcDialogHandlerContainer()
        {
            _handlers = new Dictionary<long, INpcDialogAsyncHandler>();
        }

        public Task RegisterAsync(INpcDialogAsyncHandler handler)
        {
            _handlers.Add(handler.HandledId, handler);
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(INpcDialogAsyncHandler handler)
        {
            _handlers.Remove(handler.HandledId);
            return Task.CompletedTask;
        }

        public Task Execute(IPlayerEntity player, NpcDialogEvent e)
        {
            if (!_handlers.TryGetValue(e.DialogId, out INpcDialogAsyncHandler handler))
            {
                return Task.CompletedTask;
            }

            return handler.Execute(player, e);
        }
    }
}