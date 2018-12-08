using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade
{
    public delegate void ItemUpgradeDelegate(IPlayerEntity player, ItemUpgradeEventArgs npcDialogEvent);
}