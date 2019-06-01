using ChickenAPI.Data.Enums.Game.Items.Upgrade;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;
using ChickenAPI.Packets.Enumerations;
using EquipmentType = ChickenAPI.Data.Enums.Game.Items.EquipmentType;

namespace SaltyEmu.BasicPlugin.ItemUpgradeHandlers
{
    public class UpgradeSpCard
    {
        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpNoProtection)]
        public static void UpgradeSpNoProtection(IPlayerEntity player, ItemUpgradeEvent e)
        {
            UpgradeNpc(player, e);
        }

        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpProtected)]
        public static void UpgradeSpProtected(IPlayerEntity player, ItemUpgradeEvent e)
        {
            UpgradeNpc(player, e);
        }

        [ItemUpgradeHandler(UpgradePacketType.UpgradeSpProtected2)]
        public static void UpgradeNpc(IPlayerEntity player, ItemUpgradeEvent e)
        {
            if (e.Item.Rarity == -2)
            {
                player.SendChatMessageAsync("CANT_UPGRADE_DESTROYED_SP", SayColorType.Yellow);
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

            player.EmitEvent(new UpgradeSpecialistEvent()
            {
                Item = e.Item,
                Protection = e.Type == UpgradePacketType.UpgradeSpNoProtection ? UpgradeProtection.None : UpgradeProtection.Protected,
                IsCommand = false
            });
        }
    }
}