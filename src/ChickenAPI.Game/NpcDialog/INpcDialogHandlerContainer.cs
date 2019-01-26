using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog
{
    public interface INpcDialogHandlerContainer
    {
        Task RegisterAsync(INpcDialogAsyncHandler handler);
        Task UnregisterAsync(INpcDialogAsyncHandler handler);

        Task Execute(IPlayerEntity player, NpcDialogEvent e);
    }
}