using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Enums.Game.Items.Upgrade;
using ChickenAPI.Data.Item;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;
using ChickenAPI.Packets.Enumerations;
using EquipmentType = ChickenAPI.Data.Enums.Game.Items.EquipmentType;

namespace SaltyEmu.BasicPlugin.ItemUpgradeHandlers
{
    public class UpgradeItemHandler
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        [ItemUpgradeHandler(UpgradePacketType.UpgradeItem)]
        public static void UpgradeItem(IPlayerEntity player, ItemUpgradeEvent e)
        {
            UpgradeNpc(player, e);
        }

        [ItemUpgradeHandler(UpgradePacketType.UpgradeItemProtected)]
        public static void UpgradeItemProtected(IPlayerEntity player, ItemUpgradeEvent e)
        {
            UpgradeNpc(player, e);
        }

        [ItemUpgradeHandler(UpgradePacketType.UpgradeItem)]
        public static void UpgradeNpc(IPlayerEntity player, ItemUpgradeEvent e)
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

            var hasAmulet = FixedUpMode.None;
            ItemInstanceDto amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
            if (amulet?.Item.Effect == 793)
            {
                hasAmulet = FixedUpMode.HasAmulet;
            }

            player.EmitEvent(new UpgradeEquipmentEvent
            {
                Item = e.Item,
                Mode = e.Type == UpgradePacketType.UpgradeItemGoldScroll ? UpgradeMode.Reduced : UpgradeMode.Normal,
                Protection = e.Type == UpgradePacketType.UpgradeItem ? UpgradeProtection.None : UpgradeProtection.Protected,
                HasAmulet = hasAmulet,
                IsCommand = false
            });
        }
    }
}