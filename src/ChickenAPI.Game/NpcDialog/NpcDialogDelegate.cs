using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog
{
    public delegate void NpcDialogDelegate(IPlayerEntity player, NpcDialogEventArgs npcDialogEvent);
}