using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.AccessLayer.Item;
using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Systems.Inventory;
using ChickenAPI.Game.Systems.Inventory.Args;
using ChickenAPI.Packets;

namespace NosSharp.PacketHandler.Inventory.Commands
{
    public class AddItemCommandHandling
    {
        public static void OnAddItemCommand(AddItemCommandPacket packet, IPlayerEntity player)
        {
            // generate item
            var itemFactory = Container.Instance.Resolve<IItemInstanceFactory>();
            ItemInstanceDto item = itemFactory.CreateItem(packet.ItemId, packet.Quantity);

            player.NotifySystem<InventorySystem>(new InventoryAddItemEventArgs
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