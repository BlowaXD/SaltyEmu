using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.NpcDialog.Handling;

namespace ChickenAPI.Game.NpcDialog
{
    public interface INpcDialogHandler
    {
        void Register(NpcDialogHandler handler);

        void Unregister(long npcDialogId);
        void Unregister(NpcDialogHandlerAttribute handlerAttribute);

        void Execute(IPlayerEntity player, NpcDialogEvent @event);
    }
}