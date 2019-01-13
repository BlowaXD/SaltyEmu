using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_ItemInfo_Handler : GenericEventPostProcessorBase<InventoryEqInfoEvent>
    {
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
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Wear);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 1:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Equipment);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 7:
                case 10:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Specialist);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 11:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Costume);
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
                playerEntity.SendPacket(itemInstance.GenerateSlInfoPacket());
                return;
            }

            playerEntity.SendPacket(itemInstance.GenerateEInfoPacket());
        }
    }
}