using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_ItemInfo_Handler : GenericEventPostProcessorBase<InventoryEqInfoEvent>
    {
        public Inventory_ItemInfo_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(InventoryEqInfoEvent eqInfo, CancellationToken cancellation)
        {
            if (!(eqInfo.Sender is IPlayerEntity playerEntity))
            {
                return;
            }

            InventoryComponent inventory = playerEntity.Inventory;

            ItemInstanceDto[] subInv;
            ItemInstanceDto itemInstance = null;

            switch (eqInfo.Type)
            {
                case 0:
                    subInv = inventory.GetSubInvFromInventoryType(PocketType.Wear);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 1:
                    subInv = inventory.GetSubInvFromInventoryType(PocketType.Equipment);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 7:
                case 10:
                    subInv = inventory.GetSubInvFromInventoryType(PocketType.Specialist);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 11:
                    subInv = inventory.GetSubInvFromInventoryType(PocketType.Costume);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    break;
            }

            if (itemInstance == null)
            {
                return;
            }

            if (itemInstance.Item.ItemType == ItemType.Specialist)
            {
                await playerEntity.SendPacketAsync(itemInstance.GenerateSlInfoPacket());
                return;
            }

            await playerEntity.SendPacketAsync(itemInstance.GenerateEInfoPacket());
        }
    }
}