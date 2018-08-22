using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Data.TransferObjects.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace NosSharp.PacketHandler.Inventory.Commands
{
    public class AddItemCommandHandling
    {
        public static void OnAddItemCommand(AddItemCommandPacket packet, IPlayerEntity player)
        {
            // generate item
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceFactory>();
            ItemInstanceDto item = itemFactory.CreateItem(packet.ItemId, packet.Quantity);

            player.NotifyEventHandler<InventoryEventHandler>(new InventoryAddItemEventArgs
            {
                ItemInstance = item
            });
        }
    }

    [PacketHeader("$CreateItem", Authority = AuthorityType.GameMaster)]
    public class AddItemCommandPacket : PacketBase
    {
        [PacketIndex(0)]
        public long ItemId { get; set; }

        [PacketIndex(1)]
        public short Quantity { get; set; }
    }
}