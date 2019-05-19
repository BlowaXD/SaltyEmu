using System;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Drop.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Maps;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_RemoveItem_Event : GenericEventPostProcessorBase<InventoryRemoveItemEvent>
    {
        private readonly IPathfinder _pathFinder;
        private readonly IItemInstanceDtoFactory _itemFactory;
        private readonly IRandomGenerator _random;


        public Inventory_RemoveItem_Event(ILogger log, IPathfinder pathFinder, IItemInstanceDtoFactory itemFactory, IRandomGenerator random) : base(log)
        {
            _pathFinder = pathFinder;
            _itemFactory = itemFactory;
            _random = random;
        }

        protected override async Task Handle(InventoryRemoveItemEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player) || e.ItemInstance?.Item.IsDroppable == false)
            {
                return;
            }


            ItemInstanceDto[] subinv = player.Inventory.GetSubInvFromItem(e.ItemInstance.Item);

            Position<short>[] pos = _pathFinder.GetNeighbors(player.Position, player.CurrentMap.Map);

            // create a new item
            if (e.Amount < e.ItemInstance.Amount)
            {
                ItemInstanceDto tmp = _itemFactory.Duplicate(e.ItemInstance);
                e.ItemInstance.Amount -= e.Amount;
                tmp.Amount = e.Amount;
                await player.ActualizeUiInventorySlot(e.ItemInstance.Type, e.ItemInstance.Slot);
                e.ItemInstance = tmp;
            }
            else
            {
                subinv[e.ItemInstance.Slot] = null;
                await player.ActualizeUiInventorySlot(e.ItemInstance.Type, e.ItemInstance.Slot);
            }

            IDropEntity drop = new ItemDropEntity(player.CurrentMap.GetNextId())
            {
                ItemVnum = e.ItemInstance.ItemId,
                Item = e.ItemInstance.Item,
                ItemInstance = e.ItemInstance,
                DroppedTimeUtc = DateTime.Now,
                Position = pos.Length > 1 ? pos[_random.Next(pos.Length)] : player.Position,
                Quantity = e.Amount < e.ItemInstance.Amount ? e.Amount : e.ItemInstance.Amount
            };
            drop.TransferEntity(player.CurrentMap);
            await player.CurrentMap.BroadcastAsync(drop.GenerateDropPacket());
        }
    }
}