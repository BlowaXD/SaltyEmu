using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
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

            player.EmitEvent(new InventoryAddItemEvent
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