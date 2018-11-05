using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public delegate void NpcDialogDelegate(IPlayerEntity player, NpcDialogEventArgs npcDialogEvent);
}