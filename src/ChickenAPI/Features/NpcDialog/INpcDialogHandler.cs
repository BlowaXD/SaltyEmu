using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public interface INpcDialogHandler
    {
        void Register(NpcDialogHandler handler);

        void Unregister(long npcDialogId);
        void Unregister(NpcDialogHandlerAttribute handlerAttribute);

        void Execute(IPlayerEntity player, NpcDialogEventArgs eventArgs);
    }
}