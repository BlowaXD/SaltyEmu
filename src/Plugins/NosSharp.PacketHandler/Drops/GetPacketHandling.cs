using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Packets.Game.Client.Drops;

namespace NosSharp.PacketHandler.Drops
{
    public class GetPacketHandling
    {
        public static void OnGetPacket(GetPacket packet, IPlayerEntity player)
        {
            var mapItem = player.CurrentMap.GetEntity<IDropEntity>(packet.DropId, VisualType.MapObject);

            if (mapItem == null || PositionHelper.GetDistance(mapItem.Position, player.Position) > 8)
            {
                return;
            }

            if (mapItem.ItemVnum == 1046) // Gold
            {
                player.GoldUp(mapItem.Quantity);
                player.Broadcast(player.GenerateGetPacket(mapItem.Id));
                mapItem.CurrentMap.UnregisterEntity(mapItem);
                mapItem.Dispose();
                return;
            }
            player.EmitEvent(new InventoryPickUpEvent
            {
                Drop = mapItem
            });
        }
    }
}