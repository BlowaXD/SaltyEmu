using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;
using ChickenAPI.Game.Shops.Extensions;

namespace SaltyEmu.BasicPlugin.ItemUpgradeHandlers
{
    public class RarifyItemHandler
    {
        private static readonly Logger Log = Logger.GetLogger<RarifyItemHandler>();

        [ItemUpgradeHandler(UpgradePacketType.RarifyItem)]
        public static void RarifyNpc(IPlayerEntity player, ItemUpgradeEvent e)
        {
            if (e.Item.Item.EquipmentSlot != EquipmentType.Armor && e.Item.Item.EquipmentSlot != EquipmentType.MainWeapon &&
                e.Item.Item.EquipmentSlot != EquipmentType.SecondaryWeapon)
            {
                return;
            }

            var mode = RarifyMode.Normal;
            var protection = RarifyProtection.None;
            var amulet = player.Inventory.GetItemFromSlotAndType((short)EquipmentType.Amulet, PocketType.Wear);
            if (amulet != null)
            {
                switch (amulet.Item.Effect)
                {
                    case 791:
                        protection = RarifyProtection.RedAmulet;
                        break;

                    case 792:
                        protection = RarifyProtection.BlueAmulet;
                        break;

                    case 794:
                        protection = RarifyProtection.HeroicAmulet;
                        break;

                    case 795:
                        protection = RarifyProtection.RandomHeroicAmulet;
                        break;

                    case 796:
                    case 798:
                        if (e.Item.Item.IsHeroic)
                        {
                            mode = RarifyMode.Success;
                            protection = RarifyProtection.RandomHeroicAmulet;
                        }

                        break;

                    case 797:
                        mode = RarifyMode.Reduce;
                        protection = RarifyProtection.RandomHeroicAmulet;
                        break;
                }
            }

            player.EmitEvent(new RarifyEvent
            {
                Item = e.Item,
                Mode = mode,
                Protection = protection,
                IsCommand = false
            });

            player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseWindow));
        }

        [ItemUpgradeHandler(UpgradePacketType.RarifyItemProtected)]
        public static void RarifyScroll(IPlayerEntity player, ItemUpgradeEvent e)
        {
            if (e.Item.Item.EquipmentSlot != EquipmentType.Armor && e.Item.Item.EquipmentSlot != EquipmentType.MainWeapon &&
                e.Item.Item.EquipmentSlot != EquipmentType.SecondaryWeapon)
            {
                return;
            }

            player.EmitEvent(new RarifyEvent
            {
                Item = e.Item,
                Mode = RarifyMode.Normal,
                Protection = RarifyProtection.Scroll,
                IsCommand = false
            });
        }
    }
}