using System.Threading.Tasks;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Drops;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Drops
{
    public class GetPacketHandling : GenericGamePacketHandlerAsync<GetPacket>
    {
        protected override async Task Handle(GetPacket packet, IPlayerEntity player)
        {
            var mapItem = player.CurrentMap.GetEntity<IDropEntity>(packet.DropId, VisualType.MapObject);

            if (mapItem == null || PositionHelper.GetDistance(mapItem.Position, player.Position) > 8)
            {
                return;
            }

            if (mapItem.ItemVnum == 1046) // Gold
            {
                await player.GoldUp(mapItem.Quantity);
                await player.BroadcastAsync(player.GenerateGetPacket(mapItem.Id));
                mapItem.CurrentMap.UnregisterEntity(mapItem);
                mapItem.Dispose();
                return;
            }

            await player.EmitEventAsync(new InventoryPickUpEvent
            {
                Drop = mapItem
            });
        }
    }
}