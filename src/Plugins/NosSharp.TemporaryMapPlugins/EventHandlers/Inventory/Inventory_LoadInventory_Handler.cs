using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_LoadInventory_Handler : GenericEventPostProcessorBase<InventoryLoadEvent>
    {
        private readonly IItemInstanceService _characterItemService;

        public Inventory_LoadInventory_Handler(ILogger log, IItemInstanceService characterItemService) : base(log) => _characterItemService = characterItemService;

        protected override async Task Handle(InventoryLoadEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            InventoryComponent inventory = player.Inventory;

            IEnumerable<ItemInstanceDto> items = await _characterItemService.GetByCharacterIdAsync(player.Character.Id);
            if (items == null || !items.Any())
            {
                return;
            }

            foreach (ItemInstanceDto item in items)
            {
                switch (item.Type)
                {
                    case PocketType.Equipment:
                        inventory.Equipment[item.Slot] = item;
                        break;

                    case PocketType.Etc:
                        inventory.Etc[item.Slot] = item;
                        break;

                    case PocketType.Wear:
                        inventory.Wear[item.Slot] = item;
                        break;

                    case PocketType.Main:
                        inventory.Main[item.Slot] = item;
                        break;
                }
            }
        }
    }
}