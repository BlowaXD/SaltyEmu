using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using System.Threading.Tasks;
using ChickenAPI.Game.Inventory.Events;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public interface IUseItemRequestHandlerAsync
    {
        /// <summary>
        ///     ItemType for the handler
        /// </summary>
        ItemType Type { get; }

        long EffectId { get; }

        Task Handle(IPlayerEntity player, InventoryUseItemEvent e);
    }
}