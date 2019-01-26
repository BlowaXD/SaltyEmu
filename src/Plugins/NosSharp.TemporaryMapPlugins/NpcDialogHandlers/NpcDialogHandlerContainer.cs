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
        private readonly HashSet<INpcDialogAsyncHandler> _handlers;

        public NpcDialogHandlerContainer()
        {
            _handlers = new HashSet<INpcDialogAsyncHandler>();
        }

        public Task RegisterAsync(INpcDialogAsyncHandler handler)
        {
            _handlers.Add(handler);
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(INpcDialogAsyncHandler handler)
        {
            _handlers.Remove(handler);
            return Task.CompletedTask;
        }

        public async Task Execute(IPlayerEntity player, NpcDialogEvent e)
        {
            foreach (INpcDialogAsyncHandler npcDialogAsyncHandler in _handlers)
            {
                if (await npcDialogAsyncHandler.Match(player, e))
                {
                    await npcDialogAsyncHandler.Execute(player, e);
                }
            }
        }
    }
}