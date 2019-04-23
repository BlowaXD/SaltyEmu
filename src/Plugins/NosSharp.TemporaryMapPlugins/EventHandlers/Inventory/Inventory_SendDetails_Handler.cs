using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_SendDetails_Handler : GenericEventPostProcessorBase<InventoryRequestDetailsEvent>
    {
        protected override async Task Handle(InventoryRequestDetailsEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }


            await player.SendPacketAsync(player.GenerateInventoryPacket(PocketType.Equipment));
            await player.SendPacketAsync(player.GenerateInventoryPacket(PocketType.Main));
            await player.SendPacketAsync(player.GenerateInventoryPacket(PocketType.Etc));
        }
    }
}