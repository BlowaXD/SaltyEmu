using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public interface IItemUsageContainerAsync
    {
        Task RegisterItemUsageCallback(UseItemRequestHandler handler);

        Task UseItem(IPlayerEntity player, InventoryUseItemEvent itemInstance);
    }
}