using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using System;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.ItemHandler
{
    public class UpgradeItemHandler
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        [ItemUpgradeHandler(UpgradePacketType.UpgradeItem)]
        [ItemUpgradeHandler(UpgradePacketType.UpgradeItemProtected)]
        [ItemUpgradeHandler(UpgradePacketType.UpgradeItemGoldScroll)]
        public static void UpgradeNpc(IPlayerEntity player, ItemUpgradeEventArgs e)
        {
            if (e.Item.Upgrade >= Configuration.UpgradeItem.MaximumUpgrade)
            {
                return;
            }

            if (e.Item.Item.EquipmentSlot != EquipmentType.Armor && e.Item.Item.EquipmentSlot != EquipmentType.MainWeapon &&
                e.Item.Item.EquipmentSlot != EquipmentType.SecondaryWeapon)
            {
                return;
            }

            FixedUpMode HasAmulet = FixedUpMode.None;
            ItemInstanceDto amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
            if (amulet?.Item.Effect == 793)
            {
                HasAmulet = FixedUpMode.HasAmulet;
            }

            player.EmitEvent(new UpgradeEventArgs
            {
                Item = e.Item,
                Mode = e.Type == UpgradePacketType.UpgradeItemGoldScroll ? UpgradeMode.Reduced : UpgradeMode.Normal,
                Protection = e.Type == UpgradePacketType.UpgradeItem ? UpgradeProtection.None : UpgradeProtection.Protected,
                HasAmulet = HasAmulet,
                Type = UpgradeTypeEvent.Item,
                IsCommand = false
            });
        }
    }
}