using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public interface IItemUsageContainer
    {
        void RegisterItemUsageCallback(UseItemRequestHandler handler);

        void UseItem(IPlayerEntity player, InventoryUseItemEvent itemInstance);
    }
}