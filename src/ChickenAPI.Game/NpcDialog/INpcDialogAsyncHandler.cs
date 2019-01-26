using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog
{
    public interface INpcDialogAsyncHandler
    {
        Task<bool> Match(IPlayerEntity player, NpcDialogEvent e);
        Task Execute(IPlayerEntity player, NpcDialogEvent e);
    }
}