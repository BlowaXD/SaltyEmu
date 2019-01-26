using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog
{
    public interface INpcDialogAsyncHandler
    {
        long HandledId { get; }
        Task Execute(IPlayerEntity player, NpcDialogEvent e);
    }
}