using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using System.Threading.Tasks;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public interface UseItemRequestHandlerAsync
    {
        /// <summary>
        ///     ItemType for the handler
        /// </summary>
        ItemType Type { get; }

        long EffectId { get; }

        Task Handle(IPlayerEntity player, ItemInstanceDto itemInstance);
    }
}