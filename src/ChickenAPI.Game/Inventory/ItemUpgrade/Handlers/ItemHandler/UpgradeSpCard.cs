using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using System;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.ItemHandler
{
    public class UpgradeSpCard
    {
        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpNoProtection)]
        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpProtected)]
        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpProtected2)]
        public static void UpgradeNpc(IPlayerEntity player, ItemUpgradeEventArgs e)
        {
            if (e.Item.Rarity == -2)
            {
                player.SendChatMessage("CANT_UPGRADE_DESTROYED_SP", SayColorType.Yellow);
                return;
            }

            if (e.Item.Item.EquipmentSlot != EquipmentType.Sp)
            {
                return;
            }

            if (e.Item.Upgrade >= 15)
            {
                return;
            }

            player.EmitEvent(new UpgradeEventArgs
            {
                Item = e.Item,
                Protection = e.Type == UpgradePacketType.UpgradeSpNoProtection ? UpgradeProtection.None : UpgradeProtection.Protected,
                Type = UpgradeTypeEvent.Specialist,
                IsCommand = false
            });
        }
    }
}
