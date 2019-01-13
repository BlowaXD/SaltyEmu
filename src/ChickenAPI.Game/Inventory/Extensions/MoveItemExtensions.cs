using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class MoveItemExtensions
    {
        public static void MoveItem(this InventoryComponent inv, ItemInstanceDto source, InventoryMoveEvent args)
        {
            ItemInstanceDto[] subInv = inv.GetSubInvFromItemInstance(source);
            subInv[args.DestinationSlot] = source;
            subInv[args.SourceSlot] = null;

            source.Slot = args.DestinationSlot;
            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(player.GenerateEmptyIvnPacket(args.InventoryType, args.SourceSlot));
            player.SendPacket(source.GenerateIvnPacket());
        }

        public static void MoveItems(this InventoryComponent inv, ItemInstanceDto source, ItemInstanceDto dest)
        {
            ItemInstanceDto[] subInv = inv.GetSubInvFromItemInstance(source);
            subInv[dest.Slot] = source;
            subInv[source.Slot] = dest;

            short tmp = dest.Slot;
            dest.Slot = source.Slot;
            source.Slot = tmp;

            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(source.GenerateIvnPacket());
            player.SendPacket(dest.GenerateIvnPacket());
        }
    }
}