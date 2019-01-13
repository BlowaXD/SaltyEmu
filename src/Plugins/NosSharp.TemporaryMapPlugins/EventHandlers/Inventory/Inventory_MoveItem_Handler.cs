using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_MoveItem_Handler : GenericEventPostProcessorBase<InventoryMoveEvent>
    {
        private IGameConfiguration _gameConfiguration;

        public Inventory_MoveItem_Handler(IGameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration;
        }

        protected override async Task Handle(InventoryMoveEvent args, CancellationToken cancellation)
        {
            if (!(args.Sender is IPlayerEntity player))
            {
                return;
            }
            InventoryComponent inv = player.Inventory;

            ItemInstanceDto source = inv.GetSubInvFromInventoryType(args.InventoryType)[args.SourceSlot];
            ItemInstanceDto dest = inv.GetSubInvFromInventoryType(args.InventoryType)[args.DestinationSlot];

            if (source == null)
            {
                return;
            }

            if (dest != null && (args.InventoryType == InventoryType.Main || args.InventoryType == InventoryType.Etc) && dest.ItemId == source.ItemId &&
                dest.Amount + source.Amount > _gameConfiguration.Inventory.MaxItemPerSlot)
            {
                // if both source & dest are stackable && slots combined are > max slots
                // should provide a "fill" possibility
                return;
            }

            if (dest == null)
            {
                inv.MoveItem(source, args);
            }
            else
            {
                inv.MoveItems(source, dest);
            }
        }
    }
}